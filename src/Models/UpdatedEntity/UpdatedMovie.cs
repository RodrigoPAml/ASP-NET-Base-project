using API.Enums;
using API.Infra.Base;
using API.Infra.Decorators;
using API.Infra.Exceptions;
using API.Infra.Extensions;

namespace API.Models.UpdatedEntity
{
    public class UpdatedMovie : Validable
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string? Synopsis { get; set; }

        public int? Duration { get; set; }

        public MovieGenre? Genre { get; set; }

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
        protected void ValidateSynopsis()
        {
            if (Synopsis != null && Synopsis.Count() > 512)
                throw new BusinessException("Name must have a maximum of 512 characters");
        }

        [Validator]
        protected void ValidateDuration()
        {
            if (Duration == null)
                throw new BusinessException("Duration is required");
        }

        [Validator]
        protected void ValidateGenre()
        {
            if (Genre == null)
                throw new BusinessException("Genre is required");

            if (!Genre.IsInRange())
                throw new BusinessException("Invalid genre");
        }

        #endregion
    }
}
