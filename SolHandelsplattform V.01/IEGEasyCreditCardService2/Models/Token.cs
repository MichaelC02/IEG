using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IEGEasyCreditCardService.Models
{
    public class Token
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string serviceId { get; set; }
        public string token { get; set; }
    }
}
