﻿using GetnetProvider.Commands.CommandResponses;
using GetnetProvider.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GetnetProvider.Services
{
    public interface IGetnetAuthenticationService
    {
        Task<CreateAccessTokenResponse> GetAccessToken();
    }

    public class GetnetAuthenticationService : IGetnetAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly GetnetSettings _settings;

        public GetnetAuthenticationService(IOptions<GetnetSettings> settingsOptions)
        {
            _httpClient = new HttpClient();
            _settings = settingsOptions.Value;
        }
        public async Task<CreateAccessTokenResponse> GetAccessToken()
        {
            try
            {
                var authString = GenerateAuthString();

                var stringContent = new StringContent("scope=oob&grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                var response = await _httpClient.PostAsync($"{_settings.ApiUrl}/auth/oauth/v2/token", stringContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var createAccessTokenResponse = JsonSerializer.Deserialize<CreateAccessTokenResponse>(await response.Content.ReadAsStringAsync());
                    return createAccessTokenResponse;
                }
                else
                {
                    var createAccessTokenErrorResponse = JsonSerializer.Deserialize<CreateAccessTokenErrorResponse>(await response.Content.ReadAsStringAsync());
                    throw new Exception(createAccessTokenErrorResponse.Error);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateAuthString() => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}"));
    }
}
