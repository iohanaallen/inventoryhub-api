namespace InventoryHub.DTOs.Product;

public class UpdateProductDto
{
    public string Nome { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int QuantidadeMinima { get; set; }
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = string.Empty;
}