
using System.Security.Claims;
using BlogApp.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApplication1.Models;

/*
Purpose: adds custom claims to the user principal
Explanation: This class implements the IUserClaimsPrincipalFactory interface and adds custom claims to the user principal.
The CreateAsync method is called when the user logs in, and it creates a ClaimsPrincipal object with the custom claims.
claims: a list of user attributes eg. first name, last name, and avatar
identity: a ClaimsIdentity object that represents the user's identity i.e object that contains the user's claims
principal: a ClaimsPrincipal object that represents the user's principal i.e object that contains the user's identity
where: used in Program.cs
*/

namespace BlogApp.Services.UserService;

public class CustomClaimsFactory : IUserClaimsPrincipalFactory<User>
{
    private readonly UserManager<User> _userManager;
    public CustomClaimsFactory(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var principal = new ClaimsPrincipal();
        var claims = new List<Claim>
        {
            new Claim(CustomClaimsTypes.FirstName, user.FirstName),
            new Claim(CustomClaimsTypes.LastName, user.LastName),
            new Claim(CustomClaimsTypes.Avatar, user.Avatar)
        };
        
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


        principal.AddIdentity(identity);
        return Task.FromResult(principal);
    }


}

