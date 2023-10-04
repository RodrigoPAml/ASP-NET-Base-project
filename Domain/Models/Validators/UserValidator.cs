using Domain.Attributes;
using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.Validators.Base;
using System.Text.RegularExpressions;
using System.Text;

namespace Domain.Models.Validators
{
    /// <summary>
    /// Class to validate model
    /// </summary>
    public class UserValidator : EntityValidator<User>
    {
        [Validator]
        protected void ValidatePassword()
        {
            if (Action != ActionTypeEnum.Create && !Fields.ContainsField(x => x.Password))
                return;

            if (Entity.Password == null || Entity.Password.Count() == 0)
                throw new BusinessException("Password is required");

            if (Entity.Password.Count() < 10)
                throw new BusinessException("Password must be at least 10 characters long");

            if (Entity.Password.Count() > 32)
                throw new BusinessException("Password must have a maximum of 32 characters");
        }

        [Validator]
        protected void ValidateLogin()
        {
            if (Action != ActionTypeEnum.Create && !Fields.ContainsField(x => x.Login))
                return;

            if (Entity.Login == null || Entity.Login.Count() == 0)
                throw new BusinessException("Login is required");

            if (!Regex.IsMatch(Entity.Login, @"^([a-zA-Z0-9]|\@|\.)+$"))
                throw new BusinessException("Login must have only letters and numbers");

            if (Encoding.UTF8.GetByteCount(Entity.Login) != Entity.Login.Length)
                throw new BusinessException("Login must have only letters and numbers");

            if (Entity.Login.Count() < 4)
                throw new BusinessException("Login must be at least 4 characters long");

            if (Entity.Login.Count() > 64)
                throw new BusinessException("Login has too many characters");
        }

        [Validator]
        protected void ValidateName()
        {
            if (Action != ActionTypeEnum.Create && !Fields.ContainsField(x => x.Login))
                return;

            if (Entity.Name == null || Entity.Name.Count() == 0)
                throw new BusinessException("Name is required");

            if (Entity.Name.Count() > 32)
                throw new BusinessException("Name must have a maximum of 32 characters");
        }
    }
}
