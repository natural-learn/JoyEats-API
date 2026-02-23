namespace JoyEats.Core.DTO.Order
{
    public class OrdersPageQueryDTO
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string? Number { get; set; }

        public string? Phone { get; set; }

        public int? Status { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public long? UserId { get; set; }
    }
}
