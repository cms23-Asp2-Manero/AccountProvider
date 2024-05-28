using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;
using AccountProvider.Interfaces;
using AccountProvider.Repositories;
using Microsoft.EntityFrameworkCore;
using ProviderTests.Seeds;


namespace ProviderTests
{
    public class AccountRepositoryTests
    {

        internal Context GetContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            return new Context(options);
        }

        #region Create
        [Fact]
        internal async Task CreateAsync_Should_ReturnCreatedAccountEntity()
        {
            // Arrange
            var context = GetContext();
            var repo = new AccountRepository(context);
            var manager = new ContextManager(context);
            manager.Cleanup();

            AccountEntity entity = new()
            {
                UserId = "4",
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
                PhoneNumber = "404",
            };

            // Act
            var account = await repo.CreateAsync(entity);

            // Assert
            Assert.NotNull(account);
            Assert.IsType<AccountEntity>(account);
            Assert.Equal("4", account.UserId);
            Assert.Equal("Test", account.FirstName);
            Assert.Equal("Test", account.LastName);
            Assert.Equal("Test", account.Email);
            Assert.Equal("404", account.PhoneNumber);

            // Cleanup
            manager.Cleanup();
        }
        #endregion

        #region GetAll
        [Fact]
        internal async Task GetAllAsync_Should_ReturnAllAccounts()
        {
            // Arrange
            var context = GetContext();
            var repo = new AccountRepository(context);
            var manager = new ContextManager(context);
            manager.Cleanup();

            int expectedCount = 3; 
            var expectedAccounts = manager.Seed(expectedCount);

            // Act
            var actualAccounts = await repo.GetAllAsync();

            // Assert
            Assert.NotEmpty(actualAccounts);

            Assert.IsType<List<AccountEntity>>(actualAccounts);
            Assert.Equal(expectedCount, actualAccounts.Count());

            // Cleanup
            manager.Cleanup();
        }
        #endregion

        #region GetOne
        [Fact]
        internal async Task GetOneAsync_Should_ReturnAccountWithId()
        {
            // Arrange
            var context = GetContext();
            var repo = new AccountRepository(context);
            var manager = new ContextManager(context);
            manager.Cleanup();
            manager.Seed(3);

            // Act
            var account = await repo.GetOneAsync("2");

            // Assert
            Assert.NotNull(account);
            Assert.IsType<AccountEntity>(account);
            Assert.Equal("2", account.UserId);

            // Cleanup
            manager.Cleanup();
        }
        #endregion

        #region Update
        [Fact]
        internal async Task UpdateAsync_Should_ReturnUpdatedAccount()
        {
            // Arrange
            var context = GetContext();
            var repo = new AccountRepository(context);
            var manager = new ContextManager(context);
            manager.Cleanup();
            manager.Seed(3);

            AccountEntity entity = new()
            {
                UserId = "2",
                FirstName = "Updated",
                LastName = "Updated",
                Email = "Updated",
                PhoneNumber = "0101010101",
            };

            // Act
            var account = await repo.UpdateAsync(entity);

            // Assert
            Assert.NotNull(account);
            Assert.IsType<AccountEntity>(account);
            Assert.Equal("Updated", account.FirstName);
            Assert.Equal("2", account.UserId);
            Assert.Equal("Updated", account.LastName);
            Assert.Equal("Updated", account.Email);
            Assert.Equal("0101010101", account.PhoneNumber);

            // Cleanup
            manager.Cleanup();
        }
        #endregion

        #region Delete
        [Fact]
        internal async Task DeleteAsync_Should_ReturnTrue()
        {
            // Arrange
            var context = GetContext();
            var repo = new AccountRepository(context);
            var manager = new ContextManager(context);
            manager.Cleanup();
            manager.Seed(3);



            // Act
            bool successfull = await repo.DeleteAsync("1");

            // Assert
            var entities = manager.GetAll();

            Assert.True(successfull);
            Assert.Null(entities.FirstOrDefault(x => x.UserId == "1"));

            // Cleanup
            manager.Cleanup();
        }
        [Fact]
        internal async Task DeleteAsync_Should_ReturnFalse()
        {
            // Arrange
            var context = GetContext();
            var repo = new AccountRepository(context);
            var manager = new ContextManager(context);
            manager.Cleanup();
            manager.Seed(3);

            // Act
            bool successfull = await repo.DeleteAsync("404");

            // Assert
            Assert.False(successfull);

            // Cleanup
            manager.Cleanup();
        }
        #endregion

        #region Exists
        [Fact]
        internal async Task ExistsAsync_Should_ReturnTrue()
        {
            // Arrange
            var context = GetContext();
            var repo = new AccountRepository(context);
            var manager = new ContextManager(context);
            manager.Cleanup();
            manager.Seed(3);

            // Act
            bool exists = await repo.ExistsAsync(x => x.UserId == "2");

            // Assert
            Assert.True(exists);

            // Cleanup
            manager.Cleanup();
        }
        [Fact]
        internal async Task ExistsAsync_Should_ReturnFalse()
        {
            // Arrange
            var context = GetContext();
            var repo = new AccountRepository(context);
            var manager = new ContextManager(context);
            manager.Cleanup();
            manager.Seed(3);

            // Act
            bool exists = await repo.ExistsAsync(x => x.UserId == "404");

            // Assert
            Assert.False(exists);

            // Cleanup
            manager.Cleanup();
        }
        #endregion
    }
}
