using Azure.Data.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TableStorage.CrudApi.Services.Dates;
using TableStorage.CrudApi.Services.GameReviews;
using TableStorage.CrudApi.Services.Games;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IFakeReviewGenerator, FakeReviewGenerator>();
builder.Services.AddSingleton<IFakeSummaryGenerator, FakeSummaryGenerator>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IGameReviewTableEntityFactory, GameReviewTableEntityFactory>();
var client = new TableClient(builder.Configuration.GetConnectionString("ReviewData"), "GameReviews");
await client.CreateIfNotExistsAsync();
builder.Services.AddScoped<TableClient>(provider => client);
builder.Services.AddScoped<GameReviewsService>();
builder.Services.AddScoped<GameReviewSummaryService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TableStorage.CrudApi", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TableStorage.CrudApi v1"));
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
