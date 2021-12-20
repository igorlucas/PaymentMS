using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentMS.API.Controllers;
using PaymentMS.API.Services;
using PaymentMS.Data;
using PaymentMS.Domain.Contracts.Services;
using PaymentMS.Domain.Entities;
using PaymentMS.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Priority;

namespace PaymentGateway.Tests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class CustomerServiceUnitTests
    {
        private readonly ICustomerService _customerService;
        public CustomerServiceUnitTests()
        {
            var optionsDbContext = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("InMemoryDatabase1").Options;

            var dbContext = new AppDbContext(optionsDbContext);

            _customerService = new CustomerService(dbContext);

        }

        [Fact, Priority(0)]
        public void CreateCustomer()
        {
            //// Arrange
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

            //// Act    
            var newCustomer = _customerService.Create(customer);

            //// Assert
            var result = !Object.ReferenceEquals(newCustomer, null);
            Assert.True(result);
        }

        [Fact, Priority(1)]
        public void ReadAllCustomers()
        {
            //// Arrange
            //// Act    
            var customers = _customerService.ReadAll();

            //// Assert
            var result = !Object.ReferenceEquals(customers, null) && customers.Any();
            Assert.True(result);
        }

        [Fact, Priority(2)]
        public void ReadCustomerById()
        {
            //// Arrange
            var customerId = _customerService.ReadAll().FirstOrDefault()?.Id;

            //// Act    
            var customer = _customerService.ReadById(customerId.Value);

            //// Assert
            var result = !Object.ReferenceEquals(customer, null);
            Assert.True(result);
        }

        [Fact, Priority(2)]
        public void UpdateCustomer()
        {
            //// Arrange
            var customer = _customerService.ReadAll().FirstOrDefault();
            customer.PersonType = PersonType.Juridic;
            customer.DocumentType = DocumentType.CNPJ;
            customer.DocumentValue = "80525028000181";

            //// Act    
            var customerUpdated = _customerService.Update(customer);

            //// Assert
            var result = !Object.ReferenceEquals(customerUpdated, null) && Object.ReferenceEquals(customerUpdated, customer);
            Assert.True(result);
        }

        [Fact, Priority(3)]
        public void DeleteCustomer()
        {
            //// Arrange
            var customer = _customerService.ReadAll().FirstOrDefault();

            //// Act    
            _customerService.Delete(customer);

            //// Assert
            var customerCheck = _customerService.ReadById(customer.Id);
            var result = Object.ReferenceEquals(customerCheck, null);
            Assert.True(result);
        }
    }

    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class CustomerControllerUnitTests
    {
        private readonly CustomerService _customerService;

        public CustomerControllerUnitTests()
        {
            var optionsDbContext = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("InMemoryDatabase2").Options;

            var dbContext = new AppDbContext(optionsDbContext);

            _customerService = new CustomerService(dbContext);
        }

        [Fact, Priority(0)]
        public void PostCustomerWithSuccess()
        {
            // Arrange
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

            var controller = new CustomersController(_customerService);

            // Act  
            var response = controller.PostCustomer(customer);

            // Assert
            var objectResult = Assert.IsType<CreatedAtActionResult>(response.Result);
            var createdCustomer = Assert.IsType<Customer>(objectResult.Value);
            Assert.NotNull(createdCustomer);
        }

        [Fact, Priority(1)]
        public void GetCustomersWithSuccess()
        {
            // Arrange
            var controller = new CustomersController(_customerService);

            // Act  
            var response = controller.GetCustomers();

            // Assert   
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var customers = Assert.IsType<List<Customer>>(objectResult.Value);
            Assert.NotNull(customers);
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact, Priority(2)]
        public void GetCustomerWithSuccess()
        {
            // Arrange
            var controller = new CustomersController(_customerService);

            // Act      
            var responseCustomers = controller.GetCustomers();
            var customerResult = Assert.IsType<OkObjectResult>(responseCustomers.Result);
            var customerId = Assert.IsType<List<Customer>>(customerResult.Value).First().Id;
            var response = controller.GetCustomer(customerId);

            // Assert   
            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            var customer = Assert.IsType<Customer>(objectResult.Value);
            Assert.NotNull(customer);
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact, Priority(3)]
        public void PutCustomerWithSuccess()
        {
            // Arrange
            var controller = new CustomersController(_customerService);

            // Act      
            var responseCustomers = controller.GetCustomers();
            var customerResult = Assert.IsType<OkObjectResult>(responseCustomers.Result);
            var customer = (Assert.IsType<List<Customer>>(customerResult.Value)).First();
            customer.FullName = "Igor Epfanio Silva";
            var response = controller.PutCustomer(customer.Id, customer);

            // Assert   
            var noContentResult = Assert.IsType<NoContentResult>(response);
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact, Priority(4)]
        public void DeleteCustomerWithSuccess()
        {
            // Arrange
            var controller = new CustomersController(_customerService);

            // Act      
            var responseCustomers = controller.GetCustomers();
            var customerResult = Assert.IsType<OkObjectResult>(responseCustomers.Result);
            var customer = (Assert.IsType<List<Customer>>(customerResult.Value)).First();
            customer.FullName = "Igor Epfanio Silva";
            var response = controller.DeleteCustomer(customer.Id);

            // Assert   
            var noContentResult = Assert.IsType<NoContentResult>(response);
            Assert.IsType<NoContentResult>(noContentResult);
        }
    }
}