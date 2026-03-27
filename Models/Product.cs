namespace InventoryHub.Models;

public class Product
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int QuantidadeEmEstoque { get; set; }
    public int QuantidadeMinima { get; set; }
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public List<StockMovement> Movimentacoes { get; set; } = new();
}