namespace Tickets.WebAPI.Models.Departments.Response
{
    public class GetDepartmentsResponseModel
    {
        public IEnumerable<DepartmentModel> Items { get; set; } = new List<DepartmentModel>();

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => PageSize == 0 ? 0 :
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
