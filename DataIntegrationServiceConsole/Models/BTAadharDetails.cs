using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    [JsonObject("aadharDetails")]
    public class BTAadharDetails
    {        
        [JsonProperty("co")]
        public string Co { get; set; }
        [JsonProperty("district")]
        public string District { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("employeeNo")]
        public string EmployeeNo { get; set; }
        [JsonProperty("failureReason")]
        public string FailureReason { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("house")]
        public string House { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("landmark")]
        public string Landmark { get; set; }
        [JsonProperty("lc")]
        public string Lc { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("new")]
        public string New { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("pinCode")]
        public string Pincode { get; set; }
        [JsonProperty("postOffice")]
        public string PostOffice { get; set; }
        [JsonProperty("saveResponse")]
        public string SaveResponse { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("street")]
        public string Street { get; set; }
        [JsonProperty("subDistrict")]
        public string Subdistrict { get; set; }
        [JsonProperty("uidtag")]
        public string UIDTag { get; set; }
        [JsonProperty("uuid")]
        public string UuId { get; set; }
        [JsonProperty("vtc")]
        public string Vtc { get; set; }

    }
}