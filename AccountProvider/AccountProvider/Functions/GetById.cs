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
    public class GetById(ILogger<GetById> logger, IAccountRepository accountRepository)
    {
        private readonly ILogger<GetById> _logger = logger;
        private readonly IAccountRepository _accountRepository = accountRepository;

        [Function("GetById")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "AccountProvider/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("Proccessing GET request to get one Account object based on id");

            try
            {
                AccountEntity account = await _accountRepository.GetOneAsync(id);
                if (account != null)
                    return new OkObjectResult(account);
                else
                    return new NotFoundObjectResult(new { error = "No object found" });

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
