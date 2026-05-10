using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Application._Commom
{
    public abstract class ResponseBase
    {
        public bool Success { get; set; } = true;
        public string Error { get; set; } = null;
    }
}
