using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Application.DTO;
using UserService.Application.Models;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.UserUseCases.ListUsersWithPaginationUseCase;

internal class ListUsersWithPaginationRequestHandler(UserManager<AppUser> userManager, IMapper mapper)
	: IRequestHandler<ListUsersWithPaginationRequest, PaginatedListModel<UserInfoDTO>>
{
	public async Task<PaginatedListModel<UserInfoDTO>> Handle(ListUsersWithPaginationRequest request, CancellationToken cancellationToken)
	{
		int count = await userManager.Users.CountAsync();
		var totalPages = (int)Math.Ceiling(count / (double)request.pageSize);

		if(request.pageNo > totalPages)
		{
			throw new NotFoundException("No such page");
		}

		var users = await userManager.Users.OrderBy(user => user.Email)
			.Skip((request.pageNo - 1) * request.pageSize)
			.Take(request.pageSize)
			.ToListAsync();

		var response = new PaginatedListModel<UserInfoDTO>
		{ 
			Items = mapper.Map<List<UserInfoDTO>>(users),
			CurrentPage = request.pageNo,
			TotalPages = totalPages
		};

		return response;
	}
}
