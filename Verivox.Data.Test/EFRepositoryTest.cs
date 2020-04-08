using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Verivox.Domain;
using Xunit;

namespace Verivox.Data.Test
{
    public class EFRepositoryTest
    {
        [Fact]
        public void Check_GetAll()
        {
            var builder = new DbContextOptionsBuilder<Context>();
            builder.UseInMemoryDatabase(databaseName: "Verivox");
            var options = builder.Options;

            using (var context = new Context(options))
            {
                context.AddRange(GetAllData());
                context.SaveChanges();
            }

            using (var context = new Context(options))
            {
                var repository = new EfRepository<Product>(context);
                var found = repository.Table.OrderBy(o => o.Id).ToList();
                Assert.Equal(4, found.Count);
                Assert.Equal(30, found.First().BaseAmount);
            }
        }

        private List<Product> GetAllData()
        {
            return new List<Product>
            {
                new Product{Id=2, Name="Product A",BaseAmount=15,ProductTypeId=1 },
                new Product{Id=3, Name="Product B",BaseAmount=10,ProductTypeId=2 },
                new Product{Id=4, Name="Product C",BaseAmount=20,ProductTypeId=3 },
                new Product{Id=1, Name="Product D",BaseAmount=30,ProductTypeId=4 }
            };
        }
    }
}
