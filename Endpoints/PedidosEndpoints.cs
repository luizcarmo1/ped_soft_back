using Api_Pedidos.Models;
using Api_Pedidos.Data;
using Api_Pedidos.DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Pedidos.Endpoints
{
    public static class PedidosEndpoints
    {
        public static void RegistrarEndpointsPedidos(this IEndpointRouteBuilder rotas)
        {
            // Grupamento de rotas para /api/pedidos
            var rotaPedidos = rotas.MapGroup("/api/pedidos");

            // GET /api/pedidos
            rotaPedidos.MapGet("/", async (PedidosDbContext db) =>
            {
                var pedidos = await db.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.Produtos)
                        .ThenInclude(pp => pp.Produto)
                    .ToListAsync();

                return Results.Ok(pedidos);
            }).Produces<List<Pedido>>();

            // GET /api/pedidos/{id}
            rotaPedidos.MapGet("/{id}", async (int id, PedidosDbContext db) =>
            {
                var pedido = await db.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.Produtos)
                        .ThenInclude(pp => pp.Produto)
                    .FirstOrDefaultAsync(p => p.PedidoID == id);

                if (pedido != null)
                {
                    return Results.Ok(pedido);
                }
                else
                {
                    return Results.NotFound();
                }
            }).Produces<Pedido>();

            // POST /api/pedidos/clientes
            rotaPedidos.MapPost("/clientes", async (PedidosDbContext db, Cliente cliente) =>
            {
                try
                {
                    await db.Clientes.AddAsync(cliente);
                    await db.SaveChangesAsync();

                    return Results.Created($"/api/pedidos/clientes/{cliente.ClienteID}", cliente);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Erro ao adicionar o cliente: {ex.Message}");
                }
            }).Produces<Cliente>();

            // POST /api/pedidos/produtos
            rotaPedidos.MapPost("/produtos", async (PedidosDbContext db, Produto produto) =>
            {
                try
                {
                    await db.Produtos.AddAsync(produto);
                    await db.SaveChangesAsync();

                    return Results.Created($"/api/pedidos/produtos/{produto.ProdutoID}", produto);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Erro ao adicionar o produto: {ex.Message}");
                }
            }).Produces<Produto>();

            // POST /api/pedidos
            rotaPedidos.MapPost("/", async (PedidosDbContext db, PedidosDTO pedidoDTO) =>
            {
                var cliente = await db.Clientes.FindAsync(pedidoDTO.ClienteID);

                if (cliente == null)
                {
                    return Results.NotFound("Cliente não encontrado.");
                }

                List<PedidoProduto> pedidoProdutos = new List<PedidoProduto>();

                foreach (var produtoId in pedidoDTO.ProductsId)
                {
                    var produtoEncontrado = await db.Produtos.FindAsync(produtoId);

                    if (produtoEncontrado == null)
                    {
                        return Results.NotFound($"Produto com ID {produtoId} não encontrado.");
                    }

                    var produto = new PedidoProduto
                    {
                        Produto = produtoEncontrado,
                        Quantidade = pedidoDTO.QtdProduto
                    };

                    pedidoProdutos.Add(produto);
                }

                var pedido = new Pedido
                {
                    Data_entrada_pedido = pedidoDTO.Data_entrada_pedido,
                    StatusPedido = pedidoDTO.StatusPedido,
                    QtdProduto = pedidoDTO.QtdProduto,
                    Cliente = cliente,
                    Produtos = pedidoProdutos,
                    FormaPagamento = pedidoDTO.FormaPagamento
                };

                try
                {
                    await db.Pedidos.AddAsync(pedido);
                    await db.SaveChangesAsync();

                    return Results.Created($"/api/pedidos/{pedido.PedidoID}", pedido);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Erro ao adicionar o pedido: {ex.Message}");
                }
            }).Produces<Pedido>();

            // PUT /api/pedidos/{id}
            rotaPedidos.MapPut("/{id}", async (int id, Pedido pedido, PedidosDbContext db) =>
            {
                var pedidoExistente = await db.Pedidos.FindAsync(id);
                if (pedidoExistente == null)
                {
                    return Results.NotFound();
                }

                db.Entry(pedidoExistente).CurrentValues.SetValues(pedido);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE /api/pedidos/{id}
            rotaPedidos.MapDelete("/{id}", async (int id, PedidosDbContext db) =>
            {
                var pedido = await db.Pedidos.FindAsync(id);
                if (pedido == null)
                {
                    return Results.NotFound();
                }
                db.Pedidos.Remove(pedido);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
