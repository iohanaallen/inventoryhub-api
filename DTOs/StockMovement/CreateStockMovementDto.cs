namespace InventoryHub.DTOs.StockMovement;

public class CreateStockMovementDto
{
    public int ProductId { get; set; }
    public int Quantidade { get; set; }
    public string Observacao { get; set; } = string.Empty;
}