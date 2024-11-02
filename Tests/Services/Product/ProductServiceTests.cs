namespace Tests.Services.Product;

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Services.Implementations;
using EcommerceAPI.DTOs;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact(DisplayName = "CreateProductAsync - Deve criar um novo produto com sucesso")]
    public async Task CreateProductAsync_ShouldCreateNewProductSuccessfully()
    {
        // Arrange
        var productDto = new ProductDTO { Name = "Produto Teste", Price = 100 };
        var product = new EcommerceAPI.Model.Product { ProductId = 1, Name = "Produto Teste", Price = 100 };
        
        _productRepositoryMock.Setup(repo => repo.GetProductByNameAsync(productDto.Name))!
            .ReturnsAsync((EcommerceAPI.Model.Product)null!);

        _mapperMock.Setup(mapper => mapper.Map<EcommerceAPI.Model.Product>(productDto)).Returns(product);
        _productRepositoryMock.Setup(repo => repo.CreateProductAsync(product)).ReturnsAsync(product);
        _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(product)).Returns(productDto);

        // Act
        var result = await _productService.CreateProductAsync(productDto);

        // Assert
        Assert.Equal(productDto.Name, result.Name);
        _productRepositoryMock.Verify(repo => repo.CreateProductAsync(It.IsAny<EcommerceAPI.Model.Product>()), Times.Once);
    }

    [Fact(DisplayName = "DeleteProductAsync - Deve excluir um produto existente com sucesso")]
    public async Task DeleteProductAsync_ShouldDeleteExistingProductSuccessfully()
    {
        // Arrange
        int productId = 1;
        var product = new EcommerceAPI.Model.Product { ProductId = productId, Name = "Produto Existente" };

        _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId)).ReturnsAsync(product);

        // Act
        await _productService.DeleteProductAsync(productId);

        // Assert
        _productRepositoryMock.Verify(repo => repo.DeleteProductAsync(productId), Times.Once);
    }

    [Fact(DisplayName = "GetAllProductsAsync - Deve retornar todos os produtos")]
    public async Task GetAllProductsAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<EcommerceAPI.Model.Product> { new EcommerceAPI.Model.Product { ProductId = 1, Name = "Produto1" } };
        var productDtos = new List<ProductDTO> { new ProductDTO { Name = "Produto1" } };

        _productRepositoryMock.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(products);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ProductDTO>>(products)).Returns(productDtos);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.Single(result);
    }

    [Fact(DisplayName = "GetProductByIdAsync - Deve retornar um produto pelo ID")]
    public async Task GetProductByIdAsync_ShouldReturnProductById()
    {
        // Arrange
        int productId = 1;
        var product = new EcommerceAPI.Model.Product { ProductId = productId, Name = "Produto Teste" };
        var productDto = new ProductDTO { Name = "Produto Teste" };

        _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId)).ReturnsAsync(product);
        _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(product)).Returns(productDto);

        // Act
        var result = await _productService.GetProductByIdAsync(productId);

        // Assert
        Assert.Equal(productDto.Name, result.Name);
    }

    [Fact(DisplayName = "UpdateProductAsync - Deve atualizar um produto existente com sucesso")]
    public async Task UpdateProductAsync_ShouldUpdateExistingProductSuccessfully()
    {
        // Arrange
        int productId = 1;
        var productDto = new ProductDTO { Name = "Produto Atualizado", Price = 150 };
        var product = new EcommerceAPI.Model.Product { ProductId = productId, Name = "Produto Atualizado", Price = 150 };

        _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId)).ReturnsAsync(product);
        _mapperMock.Setup(mapper => mapper.Map<EcommerceAPI.Model.Product>(productDto)).Returns(product);
        _productRepositoryMock.Setup(repo => repo.UpdateProductAsync(productId, product)).ReturnsAsync(product);
        _mapperMock.Setup(mapper => mapper.Map<ProductDTO>(product)).Returns(productDto);

        // Act
        var result = await _productService.UpdateProductAsync(productId, productDto);

        // Assert
        Assert.Equal(productDto.Name, result.Name);
        _productRepositoryMock.Verify(repo => repo.UpdateProductAsync(productId, product), Times.Once);
    }
}
