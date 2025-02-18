using Flow.Application.UseCases.Transaction;
using Flow.Core.Repositories;
using Flow.Core.Security.Tokens;
using Flow.Infra.Data;
using Flow.Infra.MessageBrokers;
using Flow.Infra.Migrations;
using Flow.Infra.Repositories;
using Flow.Infra.Tokens;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RelationalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RelationalDb"),
        sqlOptions => sqlOptions.MigrationsAssembly("Flow.Infra")));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();
builder.Services.AddScoped<IExecuteTransactionUseCase, ExecuteTransactionUseCase>();
builder.Services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();

builder.Services.AddScoped<ITokenGenerate, TokenGenerate>();
builder.Services.AddScoped<ITokenValidate, TokenValidate>();


var kafkaSection = builder.Configuration.GetSection("Kafka");

builder.Services.AddSingleton(new KafkaProducer(
        kafkaSection["BootstrapServers"],
        kafkaSection["Topic"]
    ));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddFluentMigratorCore().ConfigureRunner(rb =>
{
    rb.AddSqlServer()
      .AddAllDatabases()
      .WithGlobalConnectionString(builder.Configuration.GetConnectionString("RelationalDb"))
      .ScanIn(Assembly.Load("Flow.Infra"))
      .For.All();
});

builder.Services.Configure<SelectingProcessorAccessorOptions>(options =>
{
    options.ProcessorId = "sqlserver";
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

UpdateDatabase();

app.Run();

void UpdateDatabase()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<RelationalDbContext>();

    var connection = builder.Configuration.GetConnectionString("RelationalDb");

    Database.CreateDatabase(connection);
    app.DatabaseMigrate();
}
