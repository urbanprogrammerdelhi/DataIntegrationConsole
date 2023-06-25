using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    [JsonObject("additionalDetails")]
    public class BTAdditionalDetails
    {
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("employeeType")]
        public string EmployeeType { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("designation")]
        public string Designation { get; set; }
        [JsonProperty("joiningDate")]
        public string JoiningDate { get; set; }
        [JsonProperty("hqualification")]
        public string HQualification { get; set; }        
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string candidatePlaceOfBirth { get; set; }
        public string erpLastName { get; set; }

    }
}