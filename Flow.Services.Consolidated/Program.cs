using Flow.Application.UseCases.Consolidated;
using Flow.Infra.Data;
using Flow.Services.Consolidated.Consumers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var mongoSection = builder.Configuration.GetSection("MongoDb");
builder.Services.AddSingleton(new MongoDbContext(
        mongoSection["ConnectionString"],
        mongoSection["Database"]
    ));

builder.Services.AddHostedService<KafkaBackgroundConsumer>();
builder.Services.AddScoped<IGetDailyBalanceUseCase, GetDailyBalanceUseCase>();
// Add services to the container.

builder.Services.AddControllers();
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

app.Run();
