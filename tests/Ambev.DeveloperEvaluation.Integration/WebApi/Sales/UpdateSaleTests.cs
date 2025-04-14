using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Fixtures;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Sales
{
    public class UpdateSaleTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UpdateSaleTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateSale_Should_Return_Created()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();

            GerarIdempotencykeyHeader();

            // Ação: cria a requisição POST valida
            var response = await _client.PostAsJsonAsync("/api/sales", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task UpdateSale_Should_Return_OK()
        {

            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();

            GerarIdempotencykeyHeader();
            var createResponse = await _client.PostAsJsonAsync("/api/sales", command);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // Recupera o ID da venda criada
            var createdContent = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>(); // ajusta conforme o retorno real
            createdContent.Should().NotBeNull();
            var saleId = createdContent?.Data?.Id;

            // Act: envia comando de update com novo cliente e item
            var updateCommand = new UpdateSaleRequest()
            {
                Id = (Guid)saleId,
                Customer = "Cliente Atualizado",
                Branch = "Filial Atualizada"               
            };

            GerarIdempotencykeyHeader();
            var update = await _client.PutAsJsonAsync("/api/sales", updateCommand);

            // Assert
            update.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteSale_Should_Return_OK()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            GerarIdempotencykeyHeader();

            var createResponse = await _client.PostAsJsonAsync("/api/sales", command);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // Recupera o ID da venda criada
            var createdContent = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>(); // ajusta conforme o retorno real
            createdContent.Should().NotBeNull();
            var saleId = createdContent.Data!.Id;

            GerarIdempotencykeyHeader();
            var delete = await _client.DeleteAsync($"/api/sales/{saleId}");

            // Assert
            delete.StatusCode.Should().Be(HttpStatusCode.OK);
        }
      
        [Fact]
        public async Task GetSaleById_Should_Return_Sale_When_Exists()
        {
            // Arrange 
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            GerarIdempotencykeyHeader();
            var createResponse = await _client.PostAsJsonAsync("/api/sales", command);
            createResponse.EnsureSuccessStatusCode();

            var createResult = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetSaleResponse>>();
            var saleId = createResult?.Data?.Id;

            // Act - faz o GET da venda criada
            var getResponse = await _client.GetAsync($"/api/sales/{saleId}");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResult = await getResponse.Content.ReadFromJsonAsync<ApiResponseWithData<GetSaleResponse>>();
            getResult?.Data?.Id.Should().Be(saleId);
        }

        [Fact]
        public async Task CreateSale_Should_Return_BadRequest_When_Invalid()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            command.Items = new List<SaleItemDTO>                                               {
                new SaleItemDTO
                {
                    Quantities = 21,
                    UnitPrices = 100
                }
            };

            var response = await _client.PostAsJsonAsync("/api/sales", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateSale_Should_Return_NotFound_When_Sale_Does_Not_Exist()
        {
            var updateCommand = CreateSaleHandlerTestData.GenerateValidSaleCommand();
            GerarIdempotencykeyHeader();
            var response = await _client.PutAsJsonAsync("/api/sales", updateCommand);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteSale_Should_Return_NotFound_When_Sale_Does_Not_Exist()
        {
    
            GerarIdempotencykeyHeader();

            var response = await _client.DeleteAsync($"/api/sales/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSaleById_Should_Return_NotFound_When_Sale_Does_Not_Exist()
        {
            var response = await _client.GetAsync($"/api/sales/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private void GerarIdempotencykeyHeader()
        {
            var idempotencyKey = Guid.NewGuid().ToString();
            _client.DefaultRequestHeaders.Remove("Idempotency-Key");
            _client.DefaultRequestHeaders.Add("Idempotency-Key", idempotencyKey);
        }
    }
}
