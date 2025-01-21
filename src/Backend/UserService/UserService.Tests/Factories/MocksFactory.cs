using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using UserService.DAL.Entities;

namespace UserService.Tests.Factories;

public static class MocksFactory
{
    public static Mock<UserManager<AppUser>> CreateUserManager()
    {
        var userStore = new Mock<IUserStore<AppUser>>();
        
        return new Mock<UserManager<AppUser>>(userStore.Object, 
            null, null, null, null, null, null, null, null);
    }

    public static Mock<SignInManager<AppUser>> CreateSignInManager()
    {
        var userManager = CreateUserManager();
        
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        var optionsAccessor = new Mock<IOptions<IdentityOptions>>();
        
        return new Mock<SignInManager<AppUser>>(
            userManager.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            optionsAccessor.Object,
            null, 
            null,
            null);
    }
}