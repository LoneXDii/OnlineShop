using MediatR;
using UserService.BLL.DTO;
using UserService.BLL.Models;

namespace UserService.BLL.UseCases.UserUseCases.ListUsersWithPaginationUseCase;

public sealed record ListUsersWithPaginationRequest(PaginationDTO pagination) 
    : IRequest<PaginatedListModel<UserWithRolesDTO>> { }