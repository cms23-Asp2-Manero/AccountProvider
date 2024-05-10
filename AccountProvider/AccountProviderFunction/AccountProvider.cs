using AccountProvider.Data.contexts;
using AccountProvider.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountProvider
{
    public class AccountProvider(ILogger<AccountProvider> logger, Context context)
    {
        private readonly ILogger<AccountProvider> _logger = logger;
        private readonly Context _context = context;

        [Function("AccountProvider")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            IEnumerable<AccountEntity> accounts = await _context.Accounts.ToListAsync();

            return new OkObjectResult(accounts);
        }
    }
}
