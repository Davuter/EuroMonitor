using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Models
{
    public class TokenModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}
