using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;
using AccountProvider.Interfaces;
using AccountProvider.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountProvider.Functions
{
    public class Delete(ILogger<Delete> logger, IAccountRepository accountRepository)
    {
        private readonly ILogger<Delete> _logger = logger;
        private readonly IAccountRepository _accountRepository = accountRepository;

        [Function("Delete")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "AccountProvider/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                bool result = await _accountRepository.DeleteAsync(id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was raised");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
