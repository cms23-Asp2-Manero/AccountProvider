using AccountProvider.Data.Entities;
using AccountProvider.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AccountProvider.Functions
{
    public class GetAll(ILogger<GetAll> logger, IAccountRepository accountRepository)
    {
        private readonly ILogger<GetAll> _logger = logger;
        private readonly IAccountRepository _accountRepository = accountRepository;

        [Function("GetAll")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "AccountProvider")] HttpRequest req)
        {
            _logger.LogInformation("Processing GET request to get all Account objects");
            try
            {
                IEnumerable<AccountEntity> accounts = await _accountRepository.GetAllAsync();
                return new OkObjectResult(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was raised");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
