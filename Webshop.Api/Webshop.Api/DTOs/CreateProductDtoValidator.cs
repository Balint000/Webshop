using FluentValidation;
using Webshop.Api.DTOs;

namespace Webshop.Api.DTOs

{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);

        }
    }
}
