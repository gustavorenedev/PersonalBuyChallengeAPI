using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Model;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Services.Implementations;
using Moq;

namespace Tests.Services.Cart;

public class CartServiceTests
{
    private readonly CartService _cartService;
    private readonly Mock<ICartRepository> _mockCartRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IMapper> _mockMapper;

    public CartServiceTests()
    {
        _mockCartRepository = new Mock<ICartRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();

        _cartService = new CartService(
            _mockCartRepository.Object,
            _mockProductRepository.Object,
            _mockMapper.Object
        );
    }

    [Fact(DisplayName = "GetCartByClientIdAsync - Retorna carrinho do cliente existente")]
    public async Task GetCartByClientIdAsync_ShouldReturnCart_ForExistingClient()
    {
        // Arrange
        int clientId = 1;
        var cart = new EcommerceAPI.Model.Cart { ClientId = clientId, Items = new List<ItemCart>() };
        _mockCartRepository.Setup(r => r.GetCartByClientIdAsync(clientId)).ReturnsAsync(cart);

        // Act
        var result = await _cartService.GetCartByClientIdAsync(clientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientId, result.ClientId);
    }

    [Fact(DisplayName = "CreateCartForClientAsync - Cria novo carrinho para cliente")]
    public async Task CreateCartForClientAsync_ShouldCreateCart_WhenNoneExists()
    {
        // Arrange
        int clientId = 2;
        _mockCartRepository.Setup(r => r.GetCartByClientIdAsync(clientId))!.ReturnsAsync((EcommerceAPI.Model.Cart)null!);

        // Act
        var result = await _cartService.CreateCartForClientAsync(clientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientId, result.ClientId);
    }

    [Fact(DisplayName = "AddProductToCartAsync - Adiciona produto ao carrinho existente")]
    public async Task AddProductToCartAsync_ShouldAddProduct_WhenCartExists()
    {
        // Arrange
        int clientId = 1;
        int productId = 1;
        var cart = new EcommerceAPI.Model.Cart
        {
            ClientId = clientId,
            Items = new List<ItemCart>()
        };

        var product = new EcommerceAPI.Model.Product { ProductId = productId, Name = "Produto Teste" };

        _mockCartRepository.Setup(r => r.GetCartByClientIdAsync(clientId)).ReturnsAsync(cart);
        _mockProductRepository.Setup(p => p.GetProductByIdAsync(productId)).ReturnsAsync(product);

        var dto = new AddProductoToCartDTO { ItemId = productId, Quantity = 2 };

        // Act
        var result = await _cartService.AddProductToCartAsync(clientId, dto);

        // Assert
        Assert.NotNull(result);
        _mockCartRepository.Verify(r => r.UpdateCartAsync(cart), Times.Once);
    }


    [Fact(DisplayName = "RemoveProductFromCartAsync - Remove produto do carrinho")]
    public async Task RemoveProductFromCartAsync_ShouldRemoveProduct_WhenProductExistsInCart()
    {
        // Arrange
        int clientId = 1;
        int productId = 1;
        var item = new ItemCart { ProductId = productId, Quantity = 1 };
        var cart = new EcommerceAPI.Model.Cart { ClientId = clientId, Items = new List<ItemCart> { item } };
        _mockCartRepository.Setup(r => r.GetCartByClientIdAsync(clientId)).ReturnsAsync(cart);

        // Act
        var result = await _cartService.RemoveProductFromCartAsync(clientId, productId);

        // Assert
        Assert.DoesNotContain(result.Items, i => i.ProductId == productId);
    }

    [Fact(DisplayName = "UpdateProductQuantityInCartAsync - Atualiza quantidade de produto no carrinho")]
    public async Task UpdateProductQuantityInCartAsync_ShouldUpdateQuantity_WhenProductExistsInCart()
    {
        // Arrange
        int clientId = 1;
        int productId = 1;
        int newQuantity = 5;
        var item = new ItemCart
        {
            ProductId = productId,
            Quantity = 2
        };

        var cart = new EcommerceAPI.Model.Cart
        {
            ClientId = clientId,
            Items = new List<ItemCart> { item }
        };

        _mockCartRepository.Setup(r => r.GetCartByClientIdAsync(clientId)).ReturnsAsync(cart);

        // Act
        var result = await _cartService.UpdateProductQuantityInCartAsync(clientId, productId, newQuantity);

        // Assert
        Assert.NotNull(result);
    }


    [Fact(DisplayName = "DeleteCartAsync - Deleta o carrinho do cliente")]
    public async Task DeleteCartAsync_ShouldDeleteCart_WhenCartExists()
    {
        // Arrange
        int clientId = 1;
        var cart = new EcommerceAPI.Model.Cart 
        { 
            ClientId = clientId, 
            Items = new List<ItemCart>() 
        };

        _mockCartRepository.Setup(r => r.GetCartByClientIdAsync(clientId)).ReturnsAsync(cart);

        // Act
        await _cartService.DeleteCartAsync(clientId);

        // Assert
        _mockCartRepository.Verify(r => r.DeleteCartAsync(clientId), Times.Once);
    }
}
