using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Enums
{
    /// <summary>
    /// Media Type
    /// </summary>
    public enum MediaType : int
    {
        image = 1,
        svg = 2,
        video = 3,
        audio = 4,
        document = 5,
        model = 6
    }
}
