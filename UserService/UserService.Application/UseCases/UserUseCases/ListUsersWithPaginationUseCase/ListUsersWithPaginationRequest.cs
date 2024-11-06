using MediatR;
using UserService.Application.DTO;
using UserService.Application.Models;

namespace UserService.Application.UseCases.UserUseCases.ListUsersWithPaginationUseCase;

public sealed record ListUsersWithPaginationRequest(int pageNo = 1, int pageSize = 10) 
	: IRequest<PaginatedListModel<UserInfoDTO>> { }