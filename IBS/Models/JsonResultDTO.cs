using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Models
{
    public class JsonResultDTO
    {
        public bool IsSuccess { get; set; }
        public string MessageText { get; set; }
        public string AlterStyle { get; set; }
        public string StatusCode { get; set; }
        public bool IsSameEmail { get; set; }
    }
}
