using MediatR;
using UserService.BLL.DTO;
using UserService.BLL.Models;

namespace UserService.BLL.UseCases.UserUseCases.ListUsersWithPaginationUseCase;

public sealed record ListUsersWithPaginationRequest(int pageNo = 1, int pageSize = 10) 
    : IRequest<PaginatedListModel<UserInfoDTO>> { }