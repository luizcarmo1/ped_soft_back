using Api_Pedidos.Models;
using API_Pedidos.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Api_Pedidos.Data.PedidosDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodasOrigens",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirTodasOrigens");

// Mapeamento dos endpoints
app.MapGet("/", () => "");

// Registrar endpoints de pedidos
app.RegistrarEndpointsPedidos();

app.Run();
