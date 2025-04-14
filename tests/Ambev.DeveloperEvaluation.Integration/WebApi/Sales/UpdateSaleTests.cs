using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using Ambev.DeveloperEvaluation.Integration.Fixtures;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.WebApi.Sales
{
    public class UpdateSaleTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public UpdateSaleTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
         

        }
        public void Dispose()
        {
            // Limpa headers para evitar herança em testes seguintes
            _client.DefaultRequestHeaders.Clear();

            // Se estiver usando autenticação baseada em cookie, pode limpar também:
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Remove("Cookie");
            _client.DefaultRequestHeaders.Remove("Idempotency-Key");

            // Qualquer cleanup adicional
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
            await CriarUsuarioPadraoAsync();

            var (email, password) = await CriarUsuarioPadraoAsync();
            var getToken = await ObterTokenAutenticacaoAsync(email, password);  
           
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken);

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

            var (email, password) = await CriarUsuarioPadraoAsync();
            var getToken = await ObterTokenAutenticacaoAsync(email, password);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken);

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

            await CriarUsuarioPadraoAsync();

            var (email, password) = await CriarUsuarioPadraoAsync();
            var getToken = await ObterTokenAutenticacaoAsync(email, password);

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken);

            GerarIdempotencykeyHeader();
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
            _client.DefaultRequestHeaders.Clear();

            GerarIdempotencykeyHeader();

            var response = await _client.PostAsJsonAsync("/api/sales", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateSale_Should_Return_NotFound_When_Sale_Does_Not_Exist()
        {
            // Arrange
            var updateCommand = CreateSaleHandlerTestData.GenerateValidSaleCommand();

            await CriarUsuarioPadraoAsync();

            var (email, password) = await CriarUsuarioPadraoAsync();
            var getToken = await ObterTokenAutenticacaoAsync(email, password);

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken);

            GerarIdempotencykeyHeader();
            // Act
            var response = await _client.PutAsJsonAsync("/api/sales", updateCommand);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteSale_Should_Return_NotFound_When_Sale_Does_Not_Exist()
        {  // Arrange
            var (email, password) = await CriarUsuarioPadraoAsync();

            var getToken = await ObterTokenAutenticacaoAsync(email, password);

            _client.DefaultRequestHeaders.Clear();       
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken);

            GerarIdempotencykeyHeader();
            // Act
            var response = await _client.DeleteAsync($"/api/sales/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSaleById_Should_Return_NotFound_When_Sale_Does_Not_Exist()
        {
            var (email, password) = await CriarUsuarioPadraoAsync();

            var getToken = await ObterTokenAutenticacaoAsync(email, password);

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getToken);

            GerarIdempotencykeyHeader();

            var response = await _client.GetAsync($"/api/sales/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private void GerarIdempotencykeyHeader()
        {  
            _client.DefaultRequestHeaders.Remove("Idempotency-Key");
            _client.DefaultRequestHeaders.Add("Idempotency-Key", Guid.NewGuid().ToString());
        }

        private async Task<(string Email, string Password)> CriarUsuarioPadraoAsync()
        {
            var email = $"user_{Guid.NewGuid()}@test.com";
            var password = "@n@listA2024";

            var userFake = new
            {
                username = "usuario teste",
                password = password,
                phone = "62991046911",
                email = email,
                status = 1,
                role = 3
            };

            var response = await _client.PostAsJsonAsync("/api/users", userFake);
            response.EnsureSuccessStatusCode();

            return (email, password);
        }

        private async Task<string> ObterTokenAutenticacaoAsync(string email ,string password)
        {         

            var loginDataFake = new
            {
                Email = email,
                Password = password
            };
            var authResponse = await _client.PostAsJsonAsync("/api/auth", loginDataFake);
            authResponse.EnsureSuccessStatusCode();

            var jsonResponse = await authResponse.Content.ReadFromJsonAsync<ApiResponseWithData<AuthenticateUserResponse?>?>();
            return jsonResponse?.Data?.Token ?? throw new Exception("Token não retornado");
        }
    }
}
