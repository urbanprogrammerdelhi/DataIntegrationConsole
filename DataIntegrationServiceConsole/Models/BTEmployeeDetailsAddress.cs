using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    [JsonObject("address")]
    public class BTEmployeeDetailsAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get;set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}