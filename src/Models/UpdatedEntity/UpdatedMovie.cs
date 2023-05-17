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

        public string? Name { get; set; }

        public bool UpdateName { get; set; }

        public string? Synopsis { get; set; }

        public bool UpdateSynopsis{ get; set; }

        public DateTime? Duration { get; set; }

        public bool UpdateDuration { get; set; }

        public MovieGenre? Genre { get; set; }

        public bool UpdateGenre { get; set; }

        #region Validations

        [Validator]
        protected void ValidateName()
        {
            if (!UpdateName)
                return;

            if (Name == null || Name.Count() == 0)
                throw new BusinessException("Name is required");

            if (Name.Count() > 32)
                throw new BusinessException("Name must have a maximum of 32 characters");
        }

        [Validator]
        protected void ValidateSynopsis()
        {
            if(!UpdateSynopsis) 
                return;

            if (Synopsis != null && Synopsis.Count() > 512)
                throw new BusinessException("Name must have a maximum of 512 characters");
        }

        [Validator]
        protected void ValidateDuration()
        {
            if (!UpdateDuration)
                return;

            if (Duration == null)
                throw new BusinessException("Duration is required");
        }

        [Validator]
        protected void ValidateGenre()
        {
            if (!UpdateGenre)
                return;

            if (Genre == null)
                throw new BusinessException("Genre is required");

            if (!Genre.IsInRange())
                throw new BusinessException("Invalid genre");
        }

        #endregion
    }
}
