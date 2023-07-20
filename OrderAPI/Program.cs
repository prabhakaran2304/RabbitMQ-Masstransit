using MassTransit;
using OrderAPI.Consumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderStatusConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://user:mypass@localhost:5672");

        cfg.ReceiveEndpoint("Order-Status", c =>
        {
            c.ConfigureConsumer<OrderStatusConsumer>(ctx);
        });
    });
});

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
