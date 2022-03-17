using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace olm_inights_functions
{
    public static class HttpClientExtensions
    {
        public static StringContent AsUtf8Json(this object request)
        {
            return new StringContent(JsonConvert.SerializeObject(request),
                                  Encoding.UTF8,
                                  "application/json");
        }
    }
}
