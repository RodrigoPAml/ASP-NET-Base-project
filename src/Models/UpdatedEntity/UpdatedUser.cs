using API.Infra.Base;
using API.Infra.Decorators;
using API.Infra.Exceptions;

namespace API.Models.UpdatedEntity
{
    public class UpdatedUser : Validable
    {
        public ulong Id { get; set; }

        public string? Name { get; set; }

        public bool UpdateName { get; set; }

        #region Validations

        [Validator]
        protected void ValidateName()
        {
            if (!UpdateName)
                return;

            if (Name == null || Name.Count() == 0)
                throw new BusinessException("Nome é obrigatório");

            if (Name.Count() > 32)
                throw new BusinessException("Nome deve ter no máximo 32 caracteres");
        }

        #endregion
    }
}
