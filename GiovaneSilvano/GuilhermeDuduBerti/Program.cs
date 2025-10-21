using GuilhermeDuduBerti;
using Microsoft.AspNetCore.Mvc;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapPost("/GuilhermeDuduBerti/cadastrar", ([FromBody] Cliente cliente, [FromServices] AppDataContext ctx) =>
{
    Cliente? clienteDaVez = ctx.Clientes.FirstOrDefault(x => x.cpf == cliente.cpf);

    if (clienteDaVez.ano == cliente.ano && clienteDaVez.mes == cliente.mes)
    {
        return Results.Conflict("Essa leitura já existe!");
    }

    if (cliente.ano <= 2000)
    {
        return Results.Conflict("Ano inválido");
    }

    if (cliente.mes < 1 || cliente.mes > 12)
    {
        return Results.Conflict("Mês inválido");
    }

    if (cliente.m3Consumidos < 0)
    {
        return Results.Conflict("Consumo menor do que zero!");
    }

    ctx.Clientes.Add(cliente);
    ctx.SaveChanges();
    return Results.Created("", cliente);
});

app.MapGet("/GuilhermeDuduBerti/listar",
    ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Clientes.Any())
    {
        return Results.Ok(ctx.Clientes.ToList());
    }
    return Results.NotFound("Lista vazia!");
});

app.MapGet("/GuilhermeDuduBerti/buscar/{cpf}/{mes}/{ano}", ([FromRoute] int cpf, [FromRoute] int mes, [FromRoute] int ano, [FromServices] AppDataContext ctx) =>
{
    Cliente? cliente = ctx.Clientes.FirstOrDefault(x => x.cpf == cpf);
    if (cliente.cpf == cpf && cliente.mes == mes && cliente.ano == ano)
    {
        return Results.Ok(cliente);
    }
        return Results.NotFound("Consumo não encontrado!");
    
});

app.MapDelete("/GuilhermeDuduBerti/deletar/{cpf}/{mes}/{ano}", ([FromRoute] int cpf, [FromRoute] int mes, [FromRoute] int ano, [FromServices] AppDataContext ctx) =>
{
    Cliente? clienteRemovido = ctx.Clientes.FirstOrDefault(x => x.cpf == cpf);
    if (clienteRemovido.cpf == cpf && clienteRemovido.mes == mes && clienteRemovido.ano == ano)
    {
        ctx.Clientes.Remove(clienteRemovido);
        ctx.SaveChanges();
        return Results.Ok(clienteRemovido);
    }
    return Results.NotFound("Consumo não encotrado!");
});

RouteHandlerBuilder routeHandlerBuilder = app.MapGet("/GuilhermeDuduBerti/total-geral",
    ([FromServices] AppDataContext ctx) =>
{
Cliente cliente = new Cliente();
double consumoTotal = 0;

foreach (Cliente c in ctx.Clientes)
{
    if (c.m3Consumidos < 11)
    {
        if (c.bandeira.Contains('Amarela')) {
            consumoTotal += c.m3Consumidos * 2.5 * 1.1;
        } else if (c.bandeira.Contains('Vermelha')) {
            consumoTotal += c.m3Consumidos * 2.5 * 1.2;
        } else {
            consumoTotal += c.m3Consumidos * 2.5;
        }
    }

    if (10 < c.m3Consumidos && c.m3Consumidos < 21)
    {
        if (c.bandeira.Contains('Amarela')) {
            consumoTotal += c.m3Consumidos * 3.5 * 1.1;
        } else if (c.bandeira.Contains('Vermelha')) {
            consumoTotal += c.m3Consumidos * 3.5 * 1.2;
        } else {
            consumoTotal += c.m3Consumidos * 3.5;
        }
    }

    if (21 < c.m3Consumidos && c.m3Consumidos < 51)
    {
        if (c.bandeira.Contains('Amarela')) {
            consumoTotal += c.m3Consumidos * 5 * 1.1;
        } else if (c.bandeira.Contains('Vermelha')) {
            consumoTotal += c.m3Consumidos * 5 * 1.2;
        } else {
            consumoTotal += c.m3Consumidos * 5;
        }
    }

    if (50 < c.m3Consumidos)
    {
        if (c.bandeira.Contains('Amarela')) {
            consumoTotal += c.m3Consumidos * 6.5 * 1.1;
        } else if (c.bandeira.Contains('Vermelha')) {
            consumoTotal += c.m3Consumidos * 6.5 * 1.2;
        } else {
            consumoTotal += c.m3Consumidos * 6.5;
        }
    }

    if (consumoTotal == 0) {
        return Results.NotFound("Consumo não encotrado!");
    }

    return Results.Ok(consumoTotal)};

app.Run();

