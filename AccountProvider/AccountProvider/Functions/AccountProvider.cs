using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;
using AccountProvider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccountProvider.Functions
{
    public class AccountProvider(ILogger<AccountProvider> logger, Context context)
    {
        private readonly ILogger<AccountProvider> _logger = logger;
        private readonly Context _context = context;

        [Function("AccountProvider")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (req.Method == HttpMethods.Post)
            {
                try
                {
                    string message = await new StreamReader(req.Body).ReadToEndAsync();
                    AccountEntity? account = JsonConvert.DeserializeObject<AccountEntity>(message);
                    if (account != null) 
                    {
                        AccountEntity entity = _context.Accounts.Add(account).Entity;
                        await _context.SaveChangesAsync();
                        return new OkObjectResult(entity);
                    }
                    else
                    {
                        return new BadRequestResult();
                    }
                    
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex);
                }
                
            }
            
            IEnumerable<AccountEntity> accounts = await _context.Accounts.ToListAsync();

            return new OkObjectResult(accounts);

        }
    }
}
