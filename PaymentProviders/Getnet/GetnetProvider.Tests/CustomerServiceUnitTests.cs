using GetnetProvider.Commands.CommandRequests;
using GetnetProvider.Models;
using GetnetProvider.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentMS.Domain.Entities;
using PaymentMS.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace GetnetProvider.Tests
{
    public class CustomerServiceUnitTests
    {
        private readonly ILogger<GetnetCustomerService> _logger;
        private readonly GetnetCustomerService _getnetCustomerService;
        private readonly IConfigurationRoot _configuration;
        private readonly IOptions<GetnetSettings> _getnetSettingsOptions;
        private readonly GetnetAuthenticationService _getnetAuthenticationService;

        public CustomerServiceUnitTests()
        {
            //_configuration = new ConfigurationBuilder().AddUserSecrets<CustomerServiceUnitTests>().Build();
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            _getnetSettingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["SellerId"],
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"],
            });

            _getnetAuthenticationService = new GetnetAuthenticationService(_getnetSettingsOptions);

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _logger = factory.CreateLogger<GetnetCustomerService>();

            _getnetCustomerService = new GetnetCustomerService(_getnetSettingsOptions, _getnetAuthenticationService, _logger);
        }

        #region CreateCustomer
        [Fact]
        public async Task CreateCustomerWithSuccess()
        {
            //// Arrange
            var sellerId = _getnetSettingsOptions.Value.SellerId;
            var observation = "cliente deseja plano anual.";
            var contacts = new Contact[]
            {
                new Contact(ContactType.Phone, "32768700"),
                new Contact(ContactType.CellPhone, "987878787"),
                new Contact(ContactType.Email, "cliente@email.com"),
            };

            var addresses = new Address[]
            {
                new Address("Casa", "Rua Oscar Leitão", "200A", "Regional VI", "Messejana", "Fortaleza", "Ceará", "Brasil", "60871550"),
            };

            var customer = new Customer(PersonType.Fisic, "Igor Lucas Silva", DocumentType.CPF, "46199345002", new DateTime(1992, 12, 14), contacts, addresses);
            var createCustomerRequest = new CreateCustomerRequest(sellerId, observation, customer);

            //// Act    
            var createCustomerResponse = await _getnetCustomerService.CreateAsync(createCustomerRequest);

            //// Assert
            var result = !Object.ReferenceEquals(createCustomerResponse, null);
            Assert.True(result);
            Assert.True((int)HttpStatusCode.OK == createCustomerResponse.StatusCode);
        }

        [Fact]
        public async Task CreateCustomerWithStatusCode400()
        {
            //// Arrange
            var sellerId = _getnetSettingsOptions.Value.SellerId;
            var observation = "cliente deseja plano anual.";
            var contacts = new Contact[]
            {
                new Contact(ContactType.Phone, "32768700"),
                new Contact(ContactType.CellPhone, "987878787"),
                new Contact(ContactType.Email, "cliente@email.com"),
            };

            var addresses = new Address[]
            {
                new Address("Casa", "Rua Oscar Leitão", "200A", "Regional VI", "Messejana", "Fortaleza", "Ceará", "Brasil", "60871550"),
            };
            //"46199345002"
            var customer = new Customer(PersonType.Fisic, "Igor Lucas Silva", DocumentType.CPF, string.Empty, new DateTime(1992, 12, 14), contacts, addresses);
            var createCustomerRequest = new CreateCustomerRequest(sellerId, observation, customer);

            //// Act    
            var createCustomerResponse = await _getnetCustomerService.CreateAsync(createCustomerRequest);

            //// Assert
            var result = !Object.ReferenceEquals(createCustomerResponse, null);
            Assert.True((int)HttpStatusCode.BadRequest == createCustomerResponse.StatusCode);
            Assert.True(result);
        }

        [Fact]
        public async Task CreateCustomerWithStatusCode401()
        {
            //// Arrange
            var sellerId = Guid.NewGuid().ToString();
            var observation = "cliente deseja plano anual.";
            var contacts = new Contact[]
            {
                new Contact(ContactType.Phone, "32768700"),
                new Contact(ContactType.CellPhone, "987878787"),
                new Contact(ContactType.Email, "cliente@email.com"),
            };

            var addresses = new Address[]
            {
                new Address("Casa", "Rua Oscar Leitão", "200A", "Regional VI", "Messejana", "Fortaleza", "Ceará", "Brasil", "60871550"),
            };

            var customer = new Customer(PersonType.Fisic, "Igor Lucas Silva", DocumentType.CPF, "46199345002", new DateTime(1992, 12, 14), contacts, addresses);
            var createCustomerRequest = new CreateCustomerRequest(sellerId, observation, customer);

            //// Act    
            var createCustomerResponse = await _getnetCustomerService.CreateAsync(createCustomerRequest);

            //// Assert
            var result = !Object.ReferenceEquals(createCustomerResponse, null);
            Assert.True((int)HttpStatusCode.Unauthorized == createCustomerResponse.StatusCode);
            Assert.True(result);
        }
        #endregion CreateCustomer

        #region ReadCustomers
        [Fact]
        public async Task ReadCustomersWithStatusCode200()
        {
            //// Arrange
            int page = 1, limit = 10;
            var queryString = $"?page={page}&limit={limit}";

            //// Act            
            var readCustomersResponse = await _getnetCustomerService.ReadAsync(queryString);

            //// Assert
            var result = !Object.ReferenceEquals(readCustomersResponse, null) && readCustomersResponse.Result.Customers.Any();
            Assert.True(((int)HttpStatusCode.OK) == readCustomersResponse.StatusCode);
            Assert.True(result);
        }

        [Fact]
        public async Task ReadCustomersWithStatusCode400()
        {
            //// Arrange
            int page = 0, limit = 10;
            var queryString = $"?page={page}&limit={limit}";

            //// Act            
            var readCustomersResponse = await _getnetCustomerService.ReadAsync(queryString);

            //// Assert
            var result = !Object.ReferenceEquals(readCustomersResponse, null) && readCustomersResponse.Error.Items.Any();
            Assert.True(((int)HttpStatusCode.BadRequest) == readCustomersResponse.StatusCode);
            Assert.True(result);
        }

        [Fact]
        public async Task ReadCustomersWithStatusCode401()
        {
            //// Arrange
            var getnetSettingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = Guid.NewGuid().ToString(),
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"],
            });

            var getnetAuthenticationService = new GetnetAuthenticationService(getnetSettingsOptions);

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<GetnetCustomerService>();

            var getnetCustomerService = new GetnetCustomerService(getnetSettingsOptions, getnetAuthenticationService, logger);

            int page = 1, limit = 10;

            var queryString = $"?page={page}&limit={limit}";

            //// Act            
            var readCustomersResponse = await getnetCustomerService.ReadAsync(queryString);

            //// Assert
            var result = !Object.ReferenceEquals(readCustomersResponse, null) && readCustomersResponse.Error.Items.Any();
            Assert.True(((int)HttpStatusCode.Unauthorized) == readCustomersResponse.StatusCode);
            Assert.True(result);
        }

        //Em sandbox não funciona o 404 (Vem 2 registros padrões)
        //Em homolog não funciona o 404 (Vem o status 200 com array vazio)
        //Em prod não funciona o 404 (Vem o status 200 com array vazio) 
        //[Fact]
        //public async Task ReadCustomersWithStatusCode404()
        //{
        //    //// Arrange
        //    int page = 1, limit = 10;
        //    var customer_id = Guid.NewGuid().ToString();
        //    var queryString = $"?page={page}&limit={limit}&customer_id={customer_id}";

        //    //// Act            
        //    var readCustomersResponse = await _getnetCustomerService.ReadAsync(queryString);

        //    //// Assert
        //    var result = !Object.ReferenceEquals(readCustomersResponse, null) && readCustomersResponse.Error.Items.Any();
        //    Assert.True(((int)HttpStatusCode.NotFound) == readCustomersResponse.StatusCode);
        //    Assert.True(result);
        //}

        #endregion ReadCustomers

        #region ReadCustomerById
        [Fact]
        public async Task ReadCustomerByIdWithStatusCode200()
        {
            //// Arrange
            var customerId = "customer_21081826";

            //// Act            
            var readCustomerResponse = await _getnetCustomerService.ReadByCustomerIdAsync(customerId);

            //// Assert
            var result = !Object.ReferenceEquals(readCustomerResponse, null);
            Assert.True(result);
            Assert.True((int)HttpStatusCode.OK == readCustomerResponse.StatusCode);
        }

        //TODO: ReadCustomerByIdWithStatusCode400()

        [Fact]
        public async Task ReadCustomerByIdWithStatusCode401()
        {
            //// Arrange
            var getnetSettingsOptions = Options.Create(new GetnetSettings()
            {
                SellerId = Guid.NewGuid().ToString(),
                ClientId = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientId"],
                ClientSecret = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ClientSecret"],
                ApiUrl = _configuration.GetSection("PaymentProviders").GetSection("Getnet")["ApiUrl"],
            });

            var getnetAuthenticationService = new GetnetAuthenticationService(getnetSettingsOptions);

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<GetnetCustomerService>();

            var getnetCustomerService = new GetnetCustomerService(getnetSettingsOptions, getnetAuthenticationService, logger);

            var customer_id = $"123";

            //// Act            
            var readCustomerResponse = await getnetCustomerService.ReadByCustomerIdAsync(customer_id);

            //// Assert
            var result = !Object.ReferenceEquals(readCustomerResponse, null) && readCustomerResponse.Error.Items.Any();
            Assert.True(((int)HttpStatusCode.Unauthorized) == readCustomerResponse.StatusCode);
            Assert.True(result);
        }

        [Fact]
        public async Task ReadCustomerByIdWithStatusCode404()
        {
            //// Arrange
            var customerId = "123";

            //// Act            
            var readCustomerResponse = await _getnetCustomerService.ReadByCustomerIdAsync(customerId);

            //// Assert
            var result = !Object.ReferenceEquals(readCustomerResponse, null);
            Assert.True(result);
            Assert.True((int)HttpStatusCode.NotFound == readCustomerResponse.StatusCode);
        }

        #endregion ReadCustomerById
    }
}