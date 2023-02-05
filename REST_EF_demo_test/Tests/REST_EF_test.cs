using Microsoft.EntityFrameworkCore;
using REST_EF_demo.Data;
using REST_EF_demo.Models;
using REST_EF_demo.Services;
using FluentAssertions;

namespace REST_EF_demo_test.Repository
{
    public class REST_EF_test
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();
            for (int i = 0;i < 10; i++)
            {
                databaseContext.Authors.Add(
                    new Author
                    {
                        Name = "J. R. R. Tolkien",
                        DateOfBirth = new DateTime(1892, 1, 3)
                    });
            }
            await databaseContext.SaveChangesAsync();
            return databaseContext;
        }
        [Fact]
        public async void LibraryService_GetAuthor_ReturnsAuthor()
        {
            // Arrange
            var name = "J. R. R. Tolkien";
            var dbContext = await GetDatabaseContext();
            var LibraryService = new LibraryService(dbContext);

            // Act
            var result = await LibraryService.GetAuthorAsync(name, false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Author>();
        }
        [Fact]
        public async void LibraryService_DeleteAuthor_ReturnsAuthor()
        {
            // Arrange
            var name = "J. R. R. Tolkien";
            var dbContext = await GetDatabaseContext();
            var LibraryService = new LibraryService(dbContext);

            // Act
            var authorResult = await LibraryService.GetAuthorAsync(name, false);
            var result = await LibraryService.DeleteAuthorAsync(authorResult);

            // Assert
            result.Should().Be((true, "Author got deleted."));
        }
        [Fact]
        public async void LibraryService_UpdateAuthor_ReturnsAuthor()
        {
            // Arrange
            var name = "J. R. R. Tolkien";
            var dbContext = await GetDatabaseContext();
            var LibraryService = new LibraryService(dbContext);

            // Act
            var authorResult = await LibraryService.GetAuthorAsync(name, false);
            var authorBeforeUpdate = new Author { 
                Id = authorResult.Id,
                Name = authorResult.Name,
                DateOfBirth = authorResult.DateOfBirth,
                Books = authorResult.Books
            };
            authorResult.DateOfBirth = new DateTime(1913, 5, 12);
            var result = await LibraryService.UpdateAuthorAsync(authorResult);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Author>();
            result.Should().NotBe(authorBeforeUpdate);
            result.DateOfBirth.Should().Be(new DateTime(1913, 5, 12));
        }
    }
}
