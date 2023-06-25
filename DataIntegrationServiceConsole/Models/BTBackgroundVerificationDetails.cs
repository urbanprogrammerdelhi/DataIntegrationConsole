using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataIntegrationServiceConsole.Model
{
    public class BTBackgroundVerificationDetails
    {
        public string employeeNumber { get; set; }
        public string BGVRequired { get; set; }
        public string BackgroundVerificationDocUpload { get; set; }
        public string AppointmentLetterUpload { get; set; }
    }
}