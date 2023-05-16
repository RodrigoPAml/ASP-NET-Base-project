using API.Infra.Base;
using API.Infra.Decorators;
using API.Infra.Exceptions;
using System.Text;
using System.Text.RegularExpressions;

namespace API.Models.NewEntity
{
    public class NewUser : Validable
    {
        public string Password { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        #region Validations

        [Validator]
        protected void ValidatePassword()
        {
            if (Password == null || Password.Count() == 0)
                throw new BusinessException("Password is required");

            if (Password.Count() < 10)
                throw new BusinessException("Password must be at least 10 characters long");

            if (Password.Count() > 32)
                throw new BusinessException("Password must be a maximum of 32 characters");
        }

        [Validator]
        protected void ValidateLogin()
        {
            if (Login == null || Login.Count() == 0)
                throw new BusinessException("Login is required");

            if (!Regex.IsMatch(Login, @"^([a-zA-Z0-9]|\@|\.)+$"))
                throw new BusinessException("Login deve ter apenas letras ou/e números");

            if (Encoding.UTF8.GetByteCount(Login) != Login.Length)
                throw new BusinessException("Login must have only letters and numbers");

            if (Login.Count() < 4)
                throw new BusinessException("Login must be at least 4 characters long");

            if (Login.Count() > 64)
                throw new BusinessException("Login has too many characters");
        }


        [Validator]
        protected void ValidateName()
        {
            if (Name == null || Name.Count() == 0)
                throw new BusinessException("Name is required");

            if (Name.Count() > 32)
                throw new BusinessException("Name must be a maximum of 32 characters");
        }

        #endregion
    }
}
