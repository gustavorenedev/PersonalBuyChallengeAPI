using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Services.Implementations;
using Moq;

namespace Tests.Services.Client;

public class ClientServiceTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _mapperMock = new Mock<IMapper>();
        _clientService = new ClientService(_clientRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact(DisplayName = "CreateClientAsync deve criar um novo cliente com sucesso")]
    public async Task CreateClientAsync_ShouldCreateClientSuccessfully()
    {
        var clientDto = new ClientDTO { Name = "Teste", Email = "teste@exemplo.com", Password = "123456" };
        var client = new EcommerceAPI.Model.Client();
        var createdClient = new EcommerceAPI.Model.Client();

        _clientRepositoryMock.Setup(repo => repo.GetClientByEmailAsync(clientDto.Email)).ReturnsAsync((EcommerceAPI.Model.Client)null);
        _mapperMock.Setup(m => m.Map<EcommerceAPI.Model.Client>(clientDto)).Returns(client);
        _clientRepositoryMock.Setup(repo => repo.CreateClientAsync(client)).ReturnsAsync(createdClient);
        _mapperMock.Setup(m => m.Map<ClientDTO>(createdClient)).Returns(clientDto);

        var result = await _clientService.CreateClientAsync(clientDto);

        Assert.Equal(clientDto, result);
    }

    [Fact(DisplayName = "CreateClientAsync deve lançar ArgumentNullException se clientDto for nulo")]
    public async Task CreateClientAsync_ShouldThrowArgumentNullException_WhenClientDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _clientService.CreateClientAsync(null));
    }

    [Fact(DisplayName = "DeleteClientAsync deve deletar o cliente com sucesso")]
    public async Task DeleteClientAsync_ShouldDeleteClientSuccessfully()
    {
        var clientId = 1;
        var client = new EcommerceAPI.Model.Client();

        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(clientId)).ReturnsAsync(client);

        await _clientService.DeleteClientAsync(clientId);

        _clientRepositoryMock.Verify(repo => repo.DeleteClientAsync(clientId), Times.Once);
    }

    [Fact(DisplayName = "DeleteClientAsync deve lançar KeyNotFoundException se cliente não existir")]
    public async Task DeleteClientAsync_ShouldThrowKeyNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = 1;

        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(clientId)).ReturnsAsync((EcommerceAPI.Model.Client)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _clientService.DeleteClientAsync(clientId));
    }

    [Fact(DisplayName = "GetAllClientsAsync deve retornar todos os clientes com sucesso")]
    public async Task GetAllClientsAsync_ShouldReturnAllClientsSuccessfully()
    {
        var clients = new List<EcommerceAPI.Model.Client> { new EcommerceAPI.Model.Client(), new EcommerceAPI.Model.Client() };
        var clientDtos = new List<ClientDTO> { new ClientDTO(), new ClientDTO() };

        _clientRepositoryMock.Setup(repo => repo.GetClientsAsync()).ReturnsAsync(clients);
        _mapperMock.Setup(m => m.Map<IEnumerable<ClientDTO>>(clients)).Returns(clientDtos);

        var result = await _clientService.GetAllClientsAsync();

        Assert.Equal(clientDtos, result);
    }

    [Fact(DisplayName = "GetClientByIdAsync deve retornar o cliente por ID com sucesso")]
    public async Task GetClientByIdAsync_ShouldReturnClientByIdSuccessfully()
    {
        var clientId = 1;
        var client = new EcommerceAPI.Model.Client();
        var clientDto = new ClientDTO();

        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(clientId)).ReturnsAsync(client);
        _mapperMock.Setup(m => m.Map<ClientDTO>(client)).Returns(clientDto);

        var result = await _clientService.GetClientByIdAsync(clientId);

        Assert.Equal(clientDto, result);
    }

    [Fact(DisplayName = "GetClientByIdAsync deve lançar KeyNotFoundException se cliente não existir")]
    public async Task GetClientByIdAsync_ShouldThrowKeyNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = 1;

        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(clientId)).ReturnsAsync((EcommerceAPI.Model.Client)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _clientService.GetClientByIdAsync(clientId));
    }

    [Fact(DisplayName = "UpdateClientAsync deve atualizar o cliente com sucesso")]
    public async Task UpdateClientAsync_ShouldUpdateClientSuccessfully()
    {
        var clientId = 1;
        var clientDto = new ClientDTO { Name = "Atualizado", Email = "atualizado@exemplo.com", Password = "novaSenha" };
        var client = new EcommerceAPI.Model.Client();
        var updatedClient = new EcommerceAPI.Model.Client();

        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(clientId)).ReturnsAsync(client);
        _mapperMock.Setup(m => m.Map<EcommerceAPI.Model.Client>(clientDto)).Returns(client);
        _clientRepositoryMock.Setup(repo => repo.UpdateClientAsync(clientId, client)).ReturnsAsync(updatedClient);
        _mapperMock.Setup(m => m.Map<ClientDTO>(updatedClient)).Returns(clientDto);

        var result = await _clientService.UpdateClientAsync(clientId, clientDto);

        Assert.Equal(clientDto, result);
    }

    [Fact(DisplayName = "UpdateClientAsync deve lançar KeyNotFoundException se cliente não existir")]
    public async Task UpdateClientAsync_ShouldThrowKeyNotFoundException_WhenClientDoesNotExist()
    {
        var clientId = 1;
        var clientDto = new ClientDTO { Name = "Teste", Email = "teste@exemplo.com", Password = "123456" };

        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(clientId)).ReturnsAsync((EcommerceAPI.Model.Client)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _clientService.UpdateClientAsync(clientId, clientDto));
    }
}
