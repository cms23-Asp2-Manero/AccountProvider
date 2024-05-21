using AccountProvider.Data.Entities;
using ProviderTests.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderTests
{
    public class AccountRepositoryTests : IClassFixture<AccountSeedDataFixture>
    {
        public AccountSeedDataFixture fixture;

        public AccountRepositoryTests(AccountSeedDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void GetOneAsync_Should_ReturnAllAccountsWithId()
        {
            //Arrange

            fixture.Seed();

            //Act
            var accounts = fixture.Context.Accounts.ToList(); 


            //Assert
            Assert.IsType<List<AccountEntity>>(accounts);
            
        }
    }
}
