using MediatR;

namespace UserService.BLL.UseCases.UserUseCases.RemoveUserFromAdmins;

public sealed record RemoveUserFromAdminsRequest(string UserId) : IRequest { }
