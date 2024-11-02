namespace RecommendationProductAPI.Model;

public class Product
{
    public float UserId { get; set; }    // Pode ser uma categoria ou marca representada numericamente
    public float ProductId { get; set; } // ID único do produto
    public float Rating { get; set; }    // Relevância percebida, pode ser um valor fictício se necessário
}
