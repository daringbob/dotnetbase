using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using src.Data;

namespace src.Utilities
{
    public static class OdataHelper
    {
        public static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            var dbContextType = typeof(AppDbContext);

            foreach (var property in dbContextType.GetProperties())
            {
                if (
                    property.PropertyType.IsGenericType
                    && property.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)
                )
                {
                    var entityType = property.PropertyType.GetGenericArguments()[0];
                    var entitySetName = property.Name;
                    odataBuilder
                        ?.GetType()
                        ?.GetMethod(nameof(ODataConventionModelBuilder.EntitySet))
                        ?.MakeGenericMethod(entityType)
                        .Invoke(odataBuilder, new object[] { entitySetName });
                }
            }

            return odataBuilder?.GetEdmModel()!;
        }
    }
}
