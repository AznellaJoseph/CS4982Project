using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneBackend.Model
{
    public class Response<T>
    {
        public int StatusCode { get; set; } = 200;
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
