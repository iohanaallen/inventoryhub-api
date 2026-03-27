using InventoryHub.Data;
using InventoryHub.DTOs.StockMovement;
using InventoryHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryHub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockMovementsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StockMovementsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("entry")]
    public async Task<IActionResult> Entry(CreateStockMovementDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);

        if (product is null)
            return NotFound("Produto não encontrado.");

        if (dto.Quantidade <= 0)
            return BadRequest("A quantidade deve ser maior que zero.");

        product.QuantidadeEmEstoque += dto.Quantidade;

        var movement = new StockMovement
        {
            ProductId = dto.ProductId,
            Quantidade = dto.Quantidade,
            Observacao = dto.Observacao,
            TipoMovimentacao = "Entrada"
        };

        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Entrada registrada com sucesso.",
            estoqueAtual = product.QuantidadeEmEstoque
        });
    }

    [HttpPost("exit")]
    public async Task<IActionResult> Exit(CreateStockMovementDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);

        if (product is null)
            return NotFound("Produto não encontrado.");

        if (dto.Quantidade <= 0)
            return BadRequest("A quantidade deve ser maior que zero.");

        if (product.QuantidadeEmEstoque < dto.Quantidade)
            return BadRequest("Estoque insuficiente para realizar a saída.");

        product.QuantidadeEmEstoque -= dto.Quantidade;

        var movement = new StockMovement
        {
            ProductId = dto.ProductId,
            Quantidade = dto.Quantidade,
            Observacao = dto.Observacao,
            TipoMovimentacao = "Saida"
        };

        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Saída registrada com sucesso.",
            estoqueAtual = product.QuantidadeEmEstoque
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movements = await _context.StockMovements
            .Include(m => m.Product)
            .OrderByDescending(m => m.DataMovimentacao)
            .Select(m => new
            {
                m.Id,
                m.ProductId,
                Produto = m.Product.Nome,
                m.TipoMovimentacao,
                m.Quantidade,
                m.Observacao,
                m.DataMovimentacao
            })
            .ToListAsync();

        return Ok(movements);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetByProductId(int productId)
    {
        var exists = await _context.Products.AnyAsync(p => p.Id == productId);

        if (!exists)
            return NotFound("Produto não encontrado.");

        var movements = await _context.StockMovements
            .Where(m => m.ProductId == productId)
            .OrderByDescending(m => m.DataMovimentacao)
            .ToListAsync();

        return Ok(movements);
    }
}