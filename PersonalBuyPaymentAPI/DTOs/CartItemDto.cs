namespace PersonalBuyPaymentAPI.DTOs;

public class CartItemDto{
    public ProductDto Product { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}