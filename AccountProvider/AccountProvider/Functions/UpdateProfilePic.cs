using AccountProvider.Data.Contexts;
using AccountProvider.Models;
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
    private readonly Context _context;

    public UpdateProfilePic(ILogger<UpdateProfilePic> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }

    [Function("UpdateProfilePic")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        try
        {
            var model = await UnpackUpdateProfilePicModel(req);
            if(model != null)
            {
                var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == model.Email);
                if(user != null)
                {
                    user.ImageUrl = model.ImgUrl;

                    _context.Update(user);
                    await _context.SaveChangesAsync();

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
