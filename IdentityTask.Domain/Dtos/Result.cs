using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityTask.Domain.Dtos
{
    public class Result
    {
        public int Code {  get; set; }
        public string? Error { get; set; }
        public object? Message { get; set; }
    }
}
