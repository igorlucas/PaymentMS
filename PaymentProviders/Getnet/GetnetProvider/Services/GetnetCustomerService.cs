using GetnetProvider.Commands;
using GetnetProvider.Commands.CommandRequests;
using GetnetProvider.Commands.CommandResponses;
using GetnetProvider.Getnet.Models;
using GetnetProvider.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GetnetProvider.Services
{
    public interface IGetnetCustomerService
    {
        Task<ServiceCommandResponse<CreateCustomerResponse>> CreateAsync(CreateCustomerRequest createCustomerRequest);
        Task<ServiceCommandResponse<ReadCustomersResponse>> ReadAsync(string queryString);
        Task<ServiceCommandResponse<ReadCustomerResponse>> ReadByCustomerIdAsync(string customerId);
    }

    public class GetnetCustomerService : IGetnetCustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly GetnetSettings _settings;
        private readonly IGetnetAuthenticationService _authenticationService;

        public GetnetCustomerService(
            IOptions<GetnetSettings> settingsOptions,
            IGetnetAuthenticationService authenticationService,
            ILogger<GetnetCustomerService> logger
            )
        {
            var httpHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(httpHandler);
            _settings = settingsOptions.Value;
            _authenticationService = authenticationService;
        }
        public async Task<ServiceCommandResponse<CreateCustomerResponse>> CreateAsync(CreateCustomerRequest createCustomerRequest)
        {
            try
            {
                var accessToken = await _authenticationService.GetAccessToken();
                var json = JsonSerializer.Serialize(createCustomerRequest, new JsonSerializerOptions() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);

                var response = await _httpClient.PostAsync($"{_settings.ApiUrl}/v1/customers", stringContent);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var createCustomerResponse = JsonSerializer.Deserialize<CreateCustomerResponse>((await response.Content.ReadAsStringAsync()));
                            return new ServiceCommandResponse<CreateCustomerResponse>(createCustomerResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Validation.Keys.Select(k => k);
                            return new ServiceCommandResponse<CreateCustomerResponse>((int)response.StatusCode, message, items);
                        };
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.InternalServerError:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            return new ServiceCommandResponse<CreateCustomerResponse>((int)response.StatusCode, message, items);
                        };
                    default: throw new Exception();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceCommandResponse<ReadCustomersResponse>> ReadAsync(string queryString)
        {
            try
            {
                var accessToken = await _authenticationService.GetAccessToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);
                _httpClient.DefaultRequestHeaders.Add("seller_id", _settings.SellerId);

                var response = await _httpClient.GetAsync($"{_settings.ApiUrl}/v1/customers{queryString}");

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var listCustomersResponse = JsonSerializer.Deserialize<ReadCustomersResponse>((await response.Content.ReadAsStringAsync()));
                            return new ServiceCommandResponse<ReadCustomersResponse>(listCustomersResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}. Source: {errorScheme.Validation.Source}";
                            return new ServiceCommandResponse<ReadCustomersResponse>((int)response.StatusCode, message, errorScheme.Validation.Keys);
                        };
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.InternalServerError:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            return new ServiceCommandResponse<ReadCustomersResponse>((int)response.StatusCode, message, items);
                        };
                    default: throw new Exception();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceCommandResponse<ReadCustomerResponse>> ReadByCustomerIdAsync(string customerId)
        {
            try
            {
                var accessToken = await _authenticationService.GetAccessToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.Token);
                _httpClient.DefaultRequestHeaders.Add("seller_id", _settings.SellerId);

                var response = await _httpClient.GetAsync($"{_settings.ApiUrl}/v1/customers/{customerId}");

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            var message = "Requisição tratada com sucesso.";
                            var listCustomersResponse = JsonSerializer.Deserialize<ReadCustomerResponse>((await response.Content.ReadAsStringAsync()));
                            return new ServiceCommandResponse<ReadCustomerResponse>(listCustomersResponse, (int)response.StatusCode, message);
                        };
                    case HttpStatusCode.BadRequest:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseError400Scheme>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}. Source: {errorScheme.Validation.Source}";
                            return new ServiceCommandResponse<ReadCustomerResponse>((int)response.StatusCode, message, errorScheme.Validation.Keys);
                        };
                    case HttpStatusCode.NotFound:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<string>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e);
                            return new ServiceCommandResponse<ReadCustomerResponse>((int)response.StatusCode, message, items);
                        };
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.InternalServerError:
                        {
                            var errorScheme = (JsonSerializer.Deserialize<ResponseErrorScheme<DetailsRequestErrorScheme>>((await response.Content.ReadAsStringAsync())));
                            var message = $"Erro ao tratar requisição. Message: {errorScheme.Message}.";
                            var items = errorScheme.Details.Select(e => e.DescriptionDetail);
                            return new ServiceCommandResponse<ReadCustomerResponse>((int)response.StatusCode, message, items);
                        };
                    default: throw new Exception();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}