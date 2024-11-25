using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductsService.Application.DTO;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.Validators;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDTO>
{
	private readonly IUnitOfWork _unitOfWork;

	public UpdateProductDtoValidator(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;

		RuleFor(p => p.Name)
			.NotEmpty()
			.WithMessage("Name can't be empty");

		RuleFor(p => p.Price)
			.GreaterThan(0)
			.WithMessage("Wrong price");

		RuleFor(p => p.Quantity)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Wrong quantity");

		RuleFor(p => p.CategoryId)
			.MustAsync(BeAValidCategoryIdAsync)
			.WithMessage("Wrong category");

		RuleFor(p => p.Image)
			.Must(BeAnImage)
			.WithMessage("You should upload an image")
			.When(p => p.Image != null);
	}

	private bool BeAnImage(IFormFile? file)
	{
		return file != null && file.ContentType.StartsWith("image/");
	}

	private async Task<bool> BeAValidCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
	{
		var category = await _unitOfWork.CategoryQueryRepository.GetByIdAsync(categoryId);

		return category is not null;
	}
}
