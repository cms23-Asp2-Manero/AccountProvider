using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;
using AccountProvider.Interfaces;
using AccountProvider.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccountProvider.Functions
{
    public class Update(ILogger<Update> logger, IAccountRepository accountRepository)
    {
        private readonly ILogger<Update> _logger = logger;
        private readonly IAccountRepository _accountRepository = accountRepository;

        [Function("Update")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "AccountProvider")] HttpRequest req)
        {
            _logger.LogInformation("Processing PUT request to Update an Account object");

            try
            {
                string message = await new StreamReader(req.Body).ReadToEndAsync();
                AccountEntity? account = JsonConvert.DeserializeObject<AccountEntity>(message);
                if (account != null && account.UserId != null && account.FirstName != null && account.LastName != null && account.Email != null)
                {
                    if (await _accountRepository.ExistsAsync(x => x.UserId == account.UserId))
                    {
                        AccountEntity modifiedEntity = await _accountRepository.UpdateAsync(account);
                        return new OkObjectResult(modifiedEntity);
                    }
                    return new NotFoundObjectResult(new { error = "No object found" });
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
