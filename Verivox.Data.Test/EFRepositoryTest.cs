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
            DbContextOptionsBuilder<Context> builder = new DbContextOptionsBuilder<Context>();
            builder.UseInMemoryDatabase(databaseName: "Verivox");
            DbContextOptions<Context> options = builder.Options;

            using (Context context = new Context(options))
            {
                context.AddRange(GetAllData());
                context.SaveChanges();
            }

            using (Context context = new Context(options))
            {
                EfRepository<Product> repository = new EfRepository<Product>(context);
                List<Product> found = repository.Table.OrderBy(o => o.Id).ToList();
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
