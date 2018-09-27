namespace ApplicationKernel.Domain.DataQueries
{
    public struct Paging
    {
        public int PageNumber { get; }
        public int PageSize { get; }

        public Paging(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
