using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Data;
using src.Models;
using src.Repositories.GenericRepository;

namespace src.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }


}