﻿using Ambev.DeveloperEvaluation.Common.Constants;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    /// <summary>
    /// Validator for SaleValidator that defines validation rules for sale .
    /// </summary>
    public class SaleValidator : AbstractValidator<Sale>
    {
        /// <summary>
        /// Initializes a new instance of the SaleValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include: 
        /// - Customer: Required, length between 3 and 50 characters
        /// - SaleNumber: Required
        /// - Branch: Required
        /// - Products.Name: Required
        /// - Products.Quantities: Required
        /// - Products.UnitPrices: Required
        /// - Products.UnitPrices: Required
        /// </remarks>
        public SaleValidator()
        {
            RuleFor(x => x.SaleNumber)
           .NotEmpty()
           .WithMessage(ValidationMessages.General.FieldIsRequired);

            RuleFor(sale => sale.Customer)
                  .NotEmpty().WithMessage(ValidationMessages.General.FieldIsRequired)
                .MinimumLength(3)
                .WithMessage(ValidationMessages.General.MaximumLength)
                .MaximumLength(50)
                .WithMessage(ValidationMessages.General.MaximumLength);

            RuleFor(x => x.Branch)
                .NotEmpty()
                .WithMessage(ValidationMessages.General.FieldIsRequired);


            RuleFor(i => i.TotalValue)
         .NotEmpty()
         .WithMessage(ValidationMessages.General.FieldIsRequired)
         .GreaterThan(0).WithMessage(ValidationMessages.Sale.UnitPricesRequired);

            RuleFor(x => x.CreatedAt)
       .NotEmpty()
       .WithMessage(ValidationMessages.General.FieldIsRequired);

            RuleFor(x => x.UpdatedAt)
       .NotEmpty()
       .WithMessage(ValidationMessages.General.FieldIsRequired);

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage(ValidationMessages.Sale.ProductItensRequired);

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.General.FieldIsRequired);

                item.RuleFor(i => i.Quantities)
                      .NotEmpty()
                      .WithMessage(ValidationMessages.General.FieldIsRequired)
                      .InclusiveBetween(1, 20)
                      .WithMessage(ValidationMessages.General.InclusiveBetween);

                item.RuleFor(i => i.UnitPrices)
                .NotEmpty()
                .WithMessage(ValidationMessages.General.FieldIsRequired)
                .GreaterThan(0).WithMessage(ValidationMessages.Sale.UnitPricesRequired);

                item.RuleFor(i => i.TotalValueItem)
             .NotEmpty()
             .WithMessage(ValidationMessages.General.FieldIsRequired)
             .GreaterThan(0).WithMessage(ValidationMessages.Sale.UnitPricesRequired);

            });
        }
    }
}
