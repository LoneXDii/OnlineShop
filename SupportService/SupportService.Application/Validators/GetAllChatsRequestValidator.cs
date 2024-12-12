using FluentValidation;
using SupportService.Application.UseCases.GetAllChats;

namespace SupportService.Application.Validators;

//It's empty, but it's necessary for ValidationBehavior of MediatR piplene
//I need an implementation of IValidator<GetAllChatsRequest>
public class GetAllChatsRequestValidator : AbstractValidator<GetAllChatsRequest>
{
}
