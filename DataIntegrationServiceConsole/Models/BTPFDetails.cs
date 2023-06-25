using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    [JsonObject("pfDetails")]
    public class BTPFDetails
    {
        public string NoPFAccount { get; set; }
        public string PreviousPFAccountNumber { get; set; }
        public string dateOfExitFromPreviousEmployment { get; set; }
        public string schemeCertificateNoIfIssued { get; set; }
        public string ppoNumberIfIssued { get; set; }
        public string internationalWorkerYesno { get; set; }
        public string ifYesNameOfTheCountry { get; set; }
        public string pfNumber { get; set; }
        public string placeOfIssue { get; set; }
        public string dateOfIssue { get; set; }
        public string IssuingAuthority { get; set; }
        public string ValidFrom { get; set; }
        public string PreviousEmploymentUAN { get; set; }
        public string isFromNortheastnepalbhutan { get; set; }
        public string uanNumber { get; set; }
        public string PFDocUpload { get; set; }

    }
}