using System;
using System.Security.Claims;
using BlogApp.Constants;

//Purpose: Contains methods for ClaimsPrincipal object for getting user attributes
//Explanation: This class contains extension methods for the ClaimsPrincipal object that allow you to get the user's first name, last name, and avatar.
//GetFirstName: returns the user's first name



namespace BlogApp.Extentions;

public static class ClaimsPrincipalExtensions
{
    public static string GetFirstName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(CustomClaimsTypes.FirstName) ?? string.Empty;
    }

    public static string GetLastName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(CustomClaimsTypes.LastName) ?? string.Empty;
    }

    public static string GetAvatar(this ClaimsPrincipal principal)
    {   
        var avatarClaim = principal.Claims.FirstOrDefault(c => c.Type == CustomClaimsTypes.Avatar)?.Value;
        Console.WriteLine($"Avatar claim found: {avatarClaim}");
        return avatarClaim ?? string.Empty;
    }
}
