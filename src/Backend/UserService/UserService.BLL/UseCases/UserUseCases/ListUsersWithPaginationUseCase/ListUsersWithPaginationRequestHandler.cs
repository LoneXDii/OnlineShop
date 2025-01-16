using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.BLL.DTO;
using UserService.BLL.Models;
using UserService.DAL.Entities;

namespace UserService.BLL.UseCases.UserUseCases.ListUsersWithPaginationUseCase;

internal class ListUsersWithPaginationRequestHandler(UserManager<AppUser> userManager, IMapper mapper)
    : IRequestHandler<ListUsersWithPaginationRequest, PaginatedListModel<UserWithRolesDTO>>
{
    public async Task<PaginatedListModel<UserWithRolesDTO>> Handle(ListUsersWithPaginationRequest request, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.OrderBy(user => user.Email)
            .Skip((request.pagination.PageNo - 1) * request.pagination.PageSize)
            .Take(request.pagination.PageSize)
            .ToListAsync(cancellationToken);

        var usersWithRoles = new List<UserWithRolesDTO>();

        foreach (var user in users)
        {
            var userWithRoles = mapper.Map<UserWithRolesDTO>(user);

            userWithRoles.Roles = (await userManager.GetRolesAsync(user)).ToList();

            usersWithRoles.Add(userWithRoles);
        }

        var response = new PaginatedListModel<UserWithRolesDTO>
        { 
            Items = usersWithRoles,
            CurrentPage = request.pagination.PageNo,
            TotalPages = request.pagination.TotalPages
        };

        return response;
    }
}
