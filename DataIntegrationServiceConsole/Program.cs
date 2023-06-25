using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using DataIntegrationServiceConsole.Model;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace DataIntegrationServiceConsole
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new DataIntegrationTool());
                Utility.Logger.Info("Application started");
            }
            catch(Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }
           
    }
}
