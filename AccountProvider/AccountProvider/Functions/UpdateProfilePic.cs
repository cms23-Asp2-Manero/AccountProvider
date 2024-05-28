using AccountProvider.Data.Contexts;
using AccountProvider.Interfaces;
using AccountProvider.Models;
using AccountProvider.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccountProvider.Functions;

public class UpdateProfilePic
{
    private readonly ILogger<UpdateProfilePic> _logger;
    private readonly IAccountRepository _accountRepository;

    public UpdateProfilePic(ILogger<UpdateProfilePic> logger, IAccountRepository accountRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
    }

    [Function("UpdateProfilePic")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            var model = await UnpackUpdateProfilePicModel(req);
            if(model != null)
            {
                var user = await _accountRepository.GetByEmailAsync(model.Email);
                if(user != null)
                {
                    user.ImageUrl = model.ImgUrl;

                    await _accountRepository.UpdateAsync(user);

                    return new OkResult();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UpdateProfilePic.Run() :: {ex.Message}");
        }
        return new BadRequestResult();
    }

    public async Task<UpdateProfilePicModel> UnpackUpdateProfilePicModel(HttpRequest req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            if(!string.IsNullOrEmpty(body))
            {
                var model = JsonConvert.DeserializeObject<UpdateProfilePicModel>(body);
                if(model != null)
                    return model;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : UpdateProfilePic.Run() :: {ex.Message}");
        }
        return null!;
    }
}
