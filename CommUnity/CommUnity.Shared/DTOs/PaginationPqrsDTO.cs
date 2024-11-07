using CommUnity.Shared.Enums;

namespace CommUnity.Shared.DTOs
{
    public class PaginationPqrsDTO
    {


        public int ResidentialUnitId { get; set; }

        public PqrsType Type { get; set; }

        public PqrsState Status { get; set; }

        public int Page { get; set; } = 1;

        public int RecordsNumber { get; set; } = 10;

        public int ApartmentId { get; set; }

    }
}
