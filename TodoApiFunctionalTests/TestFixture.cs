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
        public HttpClient Client { get; }

        private readonly WebApplicationFactory<Startup> _factory;
        
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
                });
            });

            _factory = factory;
            Client = factory.CreateClient();
        }
        
        public void Dispose()
        {
            var testConfig = new ConfigurationBuilder()
                .AddUserSecrets<TestFixture>().Build();
            var contextOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseCosmos(testConfig["AzureCosmosDB:ReadWriteKey"],
                    databaseName: "Todos").Options;
            var context = new TodoContext(contextOptions);
            context.TodoItems.RemoveRange(context.TodoItems);
            context.SaveChanges();
            _factory.Dispose();
        }
    }
}