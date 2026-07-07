using FluentValidation;

namespace Webshop.Api.DTOs
{
    public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemDtoValidator() 
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
