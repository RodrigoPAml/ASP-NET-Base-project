using API.Infra.Base;
using API.Infra.Decorators;
using API.Infra.Exceptions;

namespace API.Models.UpdatedEntity
{
    public class UpdatedSession : Validable
    {
        public ulong Id { get; set; }

        public ulong? MovieId { get; set; }

        public DateTime? Date { get; set; }

        #region Validations

        [Validator]
        protected void ValidateMovieId()
        {
            if (MovieId == null || MovieId == 0)
                throw new BusinessException("Movie is required");
        }

        [Validator]
        protected void ValidateDate()
        {
            if (Date == null)
                throw new BusinessException("Date is required");
        }

        #endregion
    }
}
