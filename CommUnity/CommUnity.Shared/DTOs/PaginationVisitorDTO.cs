using CommUnity.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.DTOs
{
    public class PaginationVisitorDTO
    {

        public int Id { get; set; }

        public VisitorStatus Status { get; set; }

        public int Page { get; set; } = 1;

        public int RecordsNumber { get; set; } = 10;

    }
}
