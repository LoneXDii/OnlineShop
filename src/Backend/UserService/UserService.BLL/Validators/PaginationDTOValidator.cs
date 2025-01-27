using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.BLL.DTO;
using UserService.DAL.Entities;

namespace UserService.BLL.Validators;

public class PaginationDTOValidator : AbstractValidator<PaginationDTO>
{
    private readonly UserManager<AppUser> _userManager;

    public PaginationDTOValidator(UserManager<AppUser> userManager)
    {
        _userManager = userManager;

        RuleFor(pagination => pagination.PageNo)
            .GreaterThan(0)
            .WithMessage("Wrong page number");

        RuleFor(pagination => pagination.PageSize)
            .GreaterThan(0)
            .WithMessage("Wrong page size");

        RuleFor(pagination => pagination)
            .MustAsync(BeAValidPageNo)
            .WithMessage("No such page");
    }

    private async Task<bool> BeAValidPageNo(PaginationDTO pagination, CancellationToken cancellationToken = default)
    {
        int count = await _userManager.Users.CountAsync(cancellationToken);

        pagination.TotalPages = (int)Math.Ceiling(count / (double)pagination.PageSize);

        return pagination.PageNo <= pagination.TotalPages;
    }
}
