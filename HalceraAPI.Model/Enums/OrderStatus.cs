using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Enums
{
    public enum OrderStatus : int
    {
        Pending = 1,
        InProcess = 2,
        Completed = 3,
        Rejected = 4,
        Cancelled = 5
    }
}
