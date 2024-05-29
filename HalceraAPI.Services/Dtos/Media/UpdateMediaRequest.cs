using HalceraAPI.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Dtos.Media
{
    public class UpdateMediaRequest
    {
        public int? Id { get; set; }

        [StringLength(80)]
        public string? URL { get; set; }

        public MediaType? Type { get; set; }

        [StringLength(20)]
        public string? Name { get; set; }
    }
}
