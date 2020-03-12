using System.Threading.Tasks;
using BetterRead.Shared.Services.Abstractions;
using Xunit;

namespace BetterRead.Shared.Services.Tests
{
    public class SearchServiceTests
    {
        [Fact]
        public async Task GetBook_WhenValidUrl_ShouldNotBeNull()
        {
            //Assign
            var sut = GetSut();
            
            //Act
            var book = await sut.SearchBooksAsync("Тест");
            
            //Assert
            Assert.NotNull(book);
        }
        private static SearchService GetSut()
        {
            return new SearchService(new FetchService());
        }
    }
}