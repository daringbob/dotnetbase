using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using src.Data;
using src.Repositories.Auth;
using src.Repositories.Store;
using src.Repositories.Email;
using src.Utilities;
using Hangfire;
using Hangfire.SqlServer;
using src.Utilities.Filebase;
using src.Repositories.UnitOfWork;
using src.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR().AddJsonProtocol(options =>
{

    options.PayloadSerializerOptions.PropertyNamingPolicy = null; // Không thay đổi tên
    options.PayloadSerializerOptions.DictionaryKeyPolicy = null; // Không thay đổi tên key trong dictionary
});

// sqlserver
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// odata
builder
    .Services.AddControllers()
    .AddOData(opt =>
        opt.AddRouteComponents("odata", OdataHelper.GetEdmModel()).EnableQueryFeatures(2000)
    );

// JWT
var issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
var signingKey = builder.Configuration.GetValue<string>("JWT:Key") ?? "sample-key";
byte[] signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);
builder
    .Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters =
            new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
            };
    });

// HttpClient
builder.Services.AddHttpClient();

// Services
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IFirebaseService, FirebaseService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n"
                + "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n"
                + "Example: 'Bearer 12345abcef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        }
    );
});

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
    );
});


// Thêm dịch vụ EmailRepository vào container
builder.Services.AddScoped<IEmailRepository, EmailRepository>();

// Cấu hình Hangfire với SQL Server
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Thêm Hangfire Server vào DI
builder.Services.AddHangfireServer();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

// Thêm trang Dashboard của Hangfire
app.UseHangfireDashboard();

// Cấu hình công việc gửi email hàng ngày
// Cấu hình múi giờ lại vì HangFire mặc định xài giờ UTC
var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
var jobId = "SendScheduledEmailJob";

//Xóa tác vụ cũ nếu tồn tại để tránh gửi bù khi bỏ lỡ do tắt server
RecurringJob.RemoveIfExists(jobId);

// Tạo tác vụ mới
RecurringJob.AddOrUpdate<SendScheduledEmailJob>(
    jobId,
    service => service.SendScheduledEmailAsync(),
    //Cron.Daily(15, 0),//Gửi hàng ngày vào lúc 15:00 UTC+7
    Cron.Yearly(12, 31, 0, 0), // Gửi hàng năm vào 0:00 UTC+7 ngày 31/12
                               //Cron.MinuteInterval(2), // Gửi mỗi 2 phút
    new RecurringJobOptions
    {
        TimeZone = timeZone // Múi giờ cho công việc     
    }
);


app.Run();
