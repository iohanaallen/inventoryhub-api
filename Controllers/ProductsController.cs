using InventoryHub.Data;
using InventoryHub.DTOs.Product;
using InventoryHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryHub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products
            .OrderByDescending(p => p.DataCriacao)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                SKU = p.SKU,
                Descricao = p.Descricao,
                QuantidadeEmEstoque = p.QuantidadeEmEstoque,
                QuantidadeMinima = p.QuantidadeMinima,
                Preco = p.Preco,
                Categoria = p.Categoria,
                DataCriacao = p.DataCriacao
            })
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return NotFound("Produto não encontrado.");

        return Ok(new ProductResponseDto
        {
            Id = product.Id,
            Nome = product.Nome,
            SKU = product.SKU,
            Descricao = product.Descricao,
            QuantidadeEmEstoque = product.QuantidadeEmEstoque,
            QuantidadeMinima = product.QuantidadeMinima,
            Preco = product.Preco,
            Categoria = product.Categoria,
            DataCriacao = product.DataCriacao
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        if (await _context.Products.AnyAsync(p => p.SKU == dto.SKU))
            return BadRequest("Já existe um produto com este SKU.");

        var product = new Product
        {
            Nome = dto.Nome,
            SKU = dto.SKU,
            Descricao = dto.Descricao,
            QuantidadeEmEstoque = dto.QuantidadeEmEstoque,
            QuantidadeMinima = dto.QuantidadeMinima,
            Preco = dto.Preco,
            Categoria = dto.Categoria
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return NotFound("Produto não encontrado.");

        var skuEmUso = await _context.Products.AnyAsync(p => p.SKU == dto.SKU && p.Id != id);
        if (skuEmUso)
            return BadRequest("Já existe outro produto com este SKU.");

        product.Nome = dto.Nome;
        product.SKU = dto.SKU;
        product.Descricao = dto.Descricao;
        product.QuantidadeMinima = dto.QuantidadeMinima;
        product.Preco = dto.Preco;
        product.Categoria = dto.Categoria;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Produto atualizado com sucesso."
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return NotFound("Produto não encontrado.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Produto removido com sucesso."
        });
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var products = await _context.Products
            .Where(p => p.QuantidadeEmEstoque <= p.QuantidadeMinima)
            .OrderBy(p => p.QuantidadeEmEstoque)
            .ToListAsync();

        return Ok(products);
    }
}