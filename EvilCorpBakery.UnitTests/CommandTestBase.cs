using EvilCorpBakery.API.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace EvilCorpBakery.UnitTests
{
    public class CommandTestBase : IDisposable
    {
        protected readonly EvilCorpBakeryAppDbContext _context;

        public CommandTestBase()
        {
            var options = new DbContextOptionsBuilder<EvilCorpBakeryAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new EvilCorpBakeryAppDbContext(options);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}