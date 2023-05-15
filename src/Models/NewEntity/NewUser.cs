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
                throw new BusinessException("Senha é obrigatória");

            if (Password.Count() < 10)
                throw new BusinessException("Senha deve ter pelo menos 10 caracteres");

            if (Password.Count() > 32)
                throw new BusinessException("Senha deve ter no máximo 32 caracteres");
        }

        [Validator]
        protected void ValidateLogin()
        {
            if (Login == null || Login.Count() == 0)
                throw new BusinessException("Login é obrigatório");

            if (!Regex.IsMatch(Login, @"^([a-zA-Z0-9]|\@|\.)+$"))
                throw new BusinessException("Login deve ter apenas letras ou/e números");

            if (Encoding.UTF8.GetByteCount(Login) != Login.Length)
                throw new BusinessException("Url possui caracteres não permitidos, como acentuações ou caracteres especias");

            if (Login.Count() < 4)
                throw new BusinessException("Login deve ter pelo menos 4 caracteres");

            if (Login.Count() > 64)
                throw new BusinessException("Login tem muitos caracteres");
        }


        [Validator]
        protected void ValidateName()
        {
            if (Name == null || Name.Count() == 0)
                throw new BusinessException("Nome é obrigatório");

            if (Name.Count() > 32)
                throw new BusinessException("Nome deve ter no máximo 32 caracteres");
        }

        #endregion
    }
}
