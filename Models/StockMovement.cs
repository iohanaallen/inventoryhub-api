namespace InventoryHub.Models;

public class StockMovement
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string TipoMovimentacao { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public string Observacao { get; set; } = string.Empty;
    public DateTime DataMovimentacao { get; set; } = DateTime.UtcNow;
}