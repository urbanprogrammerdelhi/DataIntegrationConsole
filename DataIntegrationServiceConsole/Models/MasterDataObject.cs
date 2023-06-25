using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    [JsonObject("ServiceMaster")]
    public class ServiceMaster
    {
        [JsonProperty("ServiceCode")]
        public string ServiceCode { get; set; }
        [JsonProperty("Rank")]
        public string Rank { get; set; }
    }

    public class BranchMaster
    {
        public string Code { get; set; }
        public string BranchName { get; set; }
        public string Prefix { get; set; }
    }
    public class UnitMaster
    {
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string UnitCode { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
    }

    public class CityMaster
    {
        public string CityID { get; set; }
        public string CityName { get; set; }
    }

    public class CasteMaster
    {
        public string CasteCode { get; set; }
        public string CasteName { get; set; }
    }

}