using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.ExternalAPIs;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Tests.IntergrationTests
{
    public class CountryApiServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockedHttpMessageHandler;

        public CountryApiServiceTests()
        {
            _mockedHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        }

        private void MockHttpMessageHandlerResponse(Mock<HttpMessageHandler> mockedHttpMessageHandler, string response)
        {
            mockedHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response),
                })
                .Verifiable();
        }

        private void MockHttpMessageHandlerThrowsException(Mock<HttpMessageHandler> mockedHttpMessageHandler)
        {
            mockedHttpMessageHandler
                .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Mocked HTTP request exception"))
            .Verifiable();
        }


        [Fact]
        public async Task GetCountries_ShouldReturnCountries_WhenApiResponseIsSuccessful()
        {
            String expectedResponse = "[{\"name\": {\"common\": \"Country1\"}, \"capital\": [\"Capital1\"], \"borders\": [\"Country2\"]}," +
                "{\"name\": {\"common\": \"Country2\"}, \"capital\": [\"Capital2\"], \"borders\": [\"Country1\"]}]";
            MockHttpMessageHandlerResponse(_mockedHttpMessageHandler, expectedResponse);
            HttpClient httpClient = new HttpClient(_mockedHttpMessageHandler.Object);
            CountryApiService countryApiService = new CountryApiService(httpClient);


            // Act
            List<Country> result = (List<Country>)await countryApiService.GetAllCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.CommonName == "Country1");
            Assert.Contains(result, c => c.CommonName == "Country2");
        }

        [Fact]
        public async Task GetCountries_ShouldThrowExternalApiException_WhenApiFails()
        {
            // Arrange
            MockHttpMessageHandlerThrowsException(_mockedHttpMessageHandler);
            HttpClient httpClient = new HttpClient(_mockedHttpMessageHandler.Object);
            CountryApiService countryApiService = new CountryApiService(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<CountriesExternalApiException>(() => countryApiService.GetAllCountriesAsync());
        }
    }
}
