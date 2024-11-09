using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public  class SampleEntity
    {
        public Guid Id { get; set; }

        public DateTime EventTime { get; set; }

        public string? Description { get; set; }
        public int UserId {  get; set; }
    }
}
