using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime Date { get; set; } = DateTime.Now;

        public string? Picture { get; set; }

        public int ResidentialUnitId { get; set; }

        public ResidentialUnit? ResidentialUnit { get; set; }
    }
}