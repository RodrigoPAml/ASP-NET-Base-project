using Domain.Attributes;
using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Models.Entities;
using Domain.Models.Validators.Base;

namespace Domain.Models.Validators
{
    /// <summary>
    /// Class to validate model
    /// </summary>
    public class MovieValidator : EntityValidator<Movie>
    {
        [Validator]
        protected void ValidateName()
        {
            if (Action != ActionTypeEnum.Create && !Fields.ContainsField(x => x.Name))
                return;

            if (Entity.Name == null || Entity.Name.Count() == 0)
                throw new BusinessException("Name is required");

            if (Entity.Name.Count() > 64)
                throw new BusinessException("Name must have a maximum of 64 characters");
        }

        [Validator]
        protected void ValidateSynopsis()
        {
            if (Action != ActionTypeEnum.Create && !Fields.ContainsField(x => x.Synopsis))
                return;

            if (Entity.Synopsis == null || Entity.Synopsis.Count() == 0)
                return;

            if (Entity.Synopsis.Count() > 512)
                throw new BusinessException("Name must have a maximum of 512 characters");
        }

        [Validator]
        protected void ValidateDuration()
        {
            if (Action != ActionTypeEnum.Create && !Fields.ContainsField(x => x.Duration))
                return;

            if (Entity.Duration <= 0)
                throw new BusinessException("Duration needs to be greater or equal than zero");
        }

        [Validator]
        protected void ValidateGenre()
        {
            if (Action != ActionTypeEnum.Create && !Fields.ContainsField(x => x.Genre))
                return;

            if (!Entity.Genre.IsInRange())
                throw new BusinessException("Invalid genre");
        }
    }
}
