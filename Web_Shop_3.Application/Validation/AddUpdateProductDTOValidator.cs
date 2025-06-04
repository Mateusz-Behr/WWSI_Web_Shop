using FluentValidation;
using Web_Shop_3.Application.DTOs.ProductDTOs;

namespace Web_Shop_3.Application.Validation
{
    public class AddUpdateProductDTOValidator : AbstractValidator<AddUpdateProductDTO>
    {
        public AddUpdateProductDTOValidator()
        {
            RuleFor(request => request.Name)
                .Length(3, 70).WithMessage("Pole 'Nazwa' musi mieć od 3 do 70 znaków.");
            RuleFor(request => request.Description)
                .Length(3, 500).WithMessage("Pole 'Opis' musi mieć od 3 do 500 znaków.");
            RuleFor(request => request.Price)
                .NotNull().WithMessage("Cena jest wymagana")
                .GreaterThanOrEqualTo(0).WithMessage("Cena nie może być ujemna");
            RuleFor(request => request.Sku)
                .NotEmpty().WithMessage("SKU jest wymagane")
                .MaximumLength(30).WithMessage("SKU nie może mieć więcej niż 30 znaków.");
        }
    }
}
