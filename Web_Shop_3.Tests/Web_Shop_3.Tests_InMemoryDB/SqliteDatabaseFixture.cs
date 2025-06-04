using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Persistence.MySQL.Context;
using Web_Shop_3.Persistence.MySQL.Model;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop_3.Tests_InMemoryDB
{
    public class SqliteDatabaseFixture : IDisposable
    {
        private readonly SqliteConnection connection;
        public SqliteDatabaseFixture()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
        }
        public void Dispose() => this.connection.Dispose();
        public WwsishopContext CreateContext()
        {
            var context = new WwsishopContext(new DbContextOptionsBuilder<WwsishopContext>()
                .UseSqlite(this.connection)
                .Options);


            if (context.Database.EnsureCreated())
            {
                using var viewCommand = context.Database.GetDbConnection().CreateCommand();
                viewCommand.CommandText = @"
                CREATE VIEW AllCustomers AS
                SELECT Name
                FROM Customer;";
                viewCommand.ExecuteNonQuery();

                viewCommand.CommandText = @"
                CREATE VIEW AllProducts AS
                SELECT Name
                FROM Product;";
                viewCommand.ExecuteNonQuery();
            }
            context.AddRange(
                new Customer { IdCustomer = 1, Name = "Mateusz", Surname = "Behr", Email = "ma.be@gmail.com", PasswordHash = BC.HashPassword("Test111") },
                new Customer { IdCustomer = 2, Name = "Jan", Surname = "Kowalski", Email = "jan.kowalski@o2.pl", PasswordHash = BC.HashPassword("Test222") });
            context.SaveChanges();

            context.AddRange(
                new Product { IdProduct = 1, Name = "Buty Asics Beyond FF MT", Description = "Model BEYOND FF MT został zaprojektowany z myślą o zapewnieniu zaawansowanego wsparcia i amortyzacji, co umożliwia większą swobodę ruchów i pewność siebie podczas poruszania się na boisku", Price = 589.00M, Sku = "ASICS-BEYOND-FFMT01" },
                new Product { IdProduct = 2, Name = "Wertykulator elektryczny", Description = "Aerator wertykulator elektryczny do trawy trawnika 2w1 kosz 45l 38cm.", Price = 501.99M, Sku = "AER-WERT-G83002" });
            context.SaveChanges();

            return context;
        }
    }
}
