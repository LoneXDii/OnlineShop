using MediatR;

namespace UserService.BLL.UseCases.UserUseCases.AssignToAdminRole;

public sealed record AssignToAdminRoleRequest(string UserId) : IRequest { }
