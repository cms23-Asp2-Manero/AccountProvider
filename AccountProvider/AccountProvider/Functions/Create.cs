using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;
using AccountProvider.Interfaces;
using AccountProvider.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccountProvider.Functions
{
    public class Create(ILogger<Create> logger, IAccountRepository accountRepository)
    {
        private readonly ILogger<Create> _logger = logger;
        private readonly IAccountRepository _accountRepository = accountRepository;

        [Function("Create")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "AccountProvider")] HttpRequest req)
        {
            _logger.LogInformation("Processing POST request to Create an Account object");

            try
            {
                string message = await new StreamReader(req.Body).ReadToEndAsync();
                AccountEntity? account = JsonConvert.DeserializeObject<AccountEntity>(message);
                if (account != null && account.UserId != null && account.FirstName != null && account.LastName != null && account.Email != null)
                {
                    AccountEntity entity = await _accountRepository.CreateAsync(account);
                    return new OkObjectResult(entity);
                }
                else
                {
                    return new BadRequestObjectResult(new { error = "Post request failed due to invalid data" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception was raised");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
