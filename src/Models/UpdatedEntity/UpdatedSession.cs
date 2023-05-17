using API.Enums;
using API.Infra.Base;
using API.Infra.Decorators;
using API.Infra.Exceptions;
using API.Infra.Extensions;

namespace API.Models.UpdatedEntity
{
    public class UpdatedSession : Validable
    {
        public ulong Id { get; set; }

        public ulong? MovieId { get; set; }

        public bool UpdateMovieId { get; set; }

        public DateTime? Date { get; set; }

        public bool UpdateDate { get; set; }

        #region Validations

        [Validator]
        protected void ValidateMovieId()
        {
            if (!UpdateMovieId)
                return;

            if (MovieId == null || MovieId == 0)
                throw new BusinessException("Movie is required");
        }

        [Validator]
        protected void ValidateDate()
        {
            if (!UpdateDate)
                return;

            if (Date == null)
                throw new BusinessException("Date is required");
        }

        #endregion
    }
}
