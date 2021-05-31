﻿using Bimodal.Test.Api.Core.Extensions;
using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using FluentValidation;
using System.Linq;

namespace Bimodal.Test.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomer>
    {
        private readonly AgencyContext _dbContext;

        public CreateCustomerValidator(AgencyContext agencyContext) 
        {
            _dbContext = agencyContext;

            RuleFor(c => c.DocumentNumber)
                .Must((documentNumber) => { return Utils.IsExistsCustomer(_dbContext, documentNumber); })
                .WithMessage("dni-Document number is already registered.");
        }
    }
}