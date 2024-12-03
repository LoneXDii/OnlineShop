using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.BLL.DTO;
using UserService.BLL.Models;
using UserService.DAL.Entities;

namespace UserService.BLL.UseCases.UserUseCases.ListUsersWithPaginationUseCase;

internal class ListUsersWithPaginationRequestHandler(UserManager<AppUser> userManager, IMapper mapper)
    : IRequestHandler<ListUsersWithPaginationRequest, PaginatedListModel<UserInfoDTO>>
{
    public async Task<PaginatedListModel<UserInfoDTO>> Handle(ListUsersWithPaginationRequest request, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.OrderBy(user => user.Email)
            .Skip((request.pagination.PageNo - 1) * request.pagination.PageSize)
            .Take(request.pagination.PageSize)
            .ToListAsync();

        var response = new PaginatedListModel<UserInfoDTO>
        { 
            Items = mapper.Map<List<UserInfoDTO>>(users),
            CurrentPage = request.pagination.PageNo,
            TotalPages = request.pagination.TotalPages
        };

        return response;
    }
}
