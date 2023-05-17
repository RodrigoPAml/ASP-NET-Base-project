using API.Infra.Base;
using API.Infra.Decorators;
using API.Infra.Exceptions;

namespace API.Models.NewEntity
{
    public class NewSession : Validable
    {
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
