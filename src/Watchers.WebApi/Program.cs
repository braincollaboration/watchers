using Telegram.Bot;
using Watchers.WebApi.Bots;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITelegramBotClient>(x =>
    new TelegramBotClient("5676246626:AAFn1RHQ6zXGzjrcD2Hi_9839FiRbbFxQ_4"));
builder.Services.AddSingleton(x =>
    new ExternalService(new TelegramBotClient("5676246626:AAFn1RHQ6zXGzjrcD2Hi_9839FiRbbFxQ_4")));

builder.Services.AddHostedService<XdDesignCheckBot>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();