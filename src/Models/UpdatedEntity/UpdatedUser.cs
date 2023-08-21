using API.Infra.Base;
using API.Infra.Decorators;
using API.Infra.Exceptions;

namespace API.Models.UpdatedEntity
{
    public class UpdatedUser : Validable
    {
        public ulong Id { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        #region Validations

        [Validator]
        protected void ValidateName()
        {
            if (Name == null || Name.Count() == 0)
                throw new BusinessException("Name is required");

            if (Name.Count() > 32)
                throw new BusinessException("Name must have a maximum of 32 characters");
        }

        [Validator]
        protected void ValidatePassword()
        {
            if (Password == null || Password.Count() == 0)
                return;

            if (Password.Count() < 10)
                throw new BusinessException("Password must be at least 10 characters long");

            if (Password.Count() > 32)
                throw new BusinessException("Password must have a maximum of 32 characters");
        }

        #endregion
    }
}
