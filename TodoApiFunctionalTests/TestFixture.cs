using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TodoApi;
using TodoApi.Models;

namespace TodoApiFunctionalTests
{
    public class TestFixture : IDisposable
    {
        private TodoContext _db;
        
        public HttpClient Client { get; }
        
        public TestFixture()
        {
            var factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                var testConfig = new ConfigurationBuilder()
                    .AddUserSecrets<TestFixture>().Build();
            
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<TodoContext>));
                    services.Remove(descriptor);
                    services.AddDbContext<TodoContext>(opt =>
                        opt.UseCosmos(testConfig["AzureCosmosDB:ReadWriteKey"],
                            databaseName: "Todos"));
                    
                    var sp = services.BuildServiceProvider();
                    
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    _db = scopedServices.GetRequiredService<TodoContext>();
                });
            });

            Client = factory.CreateClient();
        }
        
        public void Dispose()
        {
            _db?.TodoItems.RemoveRange(_db.TodoItems);
        }
    }
}