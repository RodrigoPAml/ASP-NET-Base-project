namespace Domain.Responses
{
    /// <summary>
    /// Response for paged data
    /// </summary>
    public class PagedData
    {
        public object Data { get; set; }

        public uint Page { get; set; }

        public uint PageSize { get; set; }

        public int TotalRegisters { get; set; }
    }
}
