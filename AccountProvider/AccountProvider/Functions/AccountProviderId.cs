using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountProvider.Functions
{
    public class AccountProviderId(ILogger<AccountProviderId> logger, Context context)
    {
        private readonly ILogger<AccountProviderId> _logger = logger;
        private readonly Context _context = context;

        [Function("AccountProviderId")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "delete", Route = "AccountProvider/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (req.Method == HttpMethods.Get)
            {
                try
                {
                    AccountEntity? account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == id);
                    if (account != null)
                    {
                        return new OkObjectResult(account);
                    }
                    else
                    {
                        return new NotFoundObjectResult("No Object found with id: " + id);
                    }
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex);
                }
                
            }

            if (req.Method == HttpMethods.Delete)
            {
                try
                {
                    AccountEntity? account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == id);
                    if (account != null)
                    {
                        _context.Accounts.Remove(account);
                        await _context.SaveChangesAsync();

                    }
                    return new NoContentResult();
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex);
                }
            }

            return new BadRequestResult();
        }
    }
}
