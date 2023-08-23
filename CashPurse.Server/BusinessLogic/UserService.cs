using CashPurse.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace CashPurse.Server.BusinessLogic;

public interface IUserService
{
    /// <summary>
    /// Get the currently logged In user via HttpContext
    /// </summary>
    /// <returns>Derived type of IdentityUser called ApplicationUser</returns>
    Task<ApplicationUser> GetLoggedInUser();

    UserManager<ApplicationUser> UserManager { get; init; }
}


public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserManager<ApplicationUser> UserManager { get; init; }

    public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        UserManager = userManager;
    }
    public async Task<ApplicationUser> GetLoggedInUser()
    {
        var currentlyLoggedInUser = await _userManager.FindByEmailAsync(
            _contextAccessor.HttpContext?.User!.Identity!.Name!);
        return currentlyLoggedInUser!;
    }
}
