using InventoryAPI.Consumer;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderConsumer>();
    x.AddConsumer<OrderTypeConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://user:mypass@localhost:5672");

        cfg.ReceiveEndpoint("Order-Queue", c =>
        {
            c.ConfigureConsumer<OrderConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("order-type", c =>
        {
            c.ConfigureConsumer<OrderTypeConsumer>(ctx);
        });
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
