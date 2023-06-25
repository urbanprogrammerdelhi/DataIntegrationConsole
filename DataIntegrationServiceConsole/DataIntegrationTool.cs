using DataIntegrationServiceConsole.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;

namespace DataIntegrationServiceConsole
{
    public partial class DataIntegrationTool : Form
    {
        string message;
        #region Properties
        private string StartDate
        {
            get
            {
                return string.Format("{0} {1}:{1}:00", dtpStartDate.Text, cmbStartHour.SelectedValue, cmbStartMinutes.SelectedValue);

            }
        }
        private string EndDate
        {
            get
            {
                return string.Format("{0} {1}:{1}:00", dtpEndDate.Text, cmbEndHour.SelectedValue, cmbEndMinutes.SelectedValue);

            }
        }
        dynamic DataIntegrationParameter
        {
            get
            {
                return new
                {
                    dateOfVerification = StartDate
                    //"2021-09-22 11:20:00"

                    ,
                    toDate = EndDate
                    // "2021-09-22 11:35:00"
                };
            }
        }
        dynamic DataIntegrationEsiParameter
        {
            get
            {
                return new
                {
                    dateOfVerification = StartDate
                   //"2021-09-22 11:20:00"

                   ,
                    toDate = EndDate
                    // "2021-09-22 11:35:00"
                };

            }
        }
        static List<BTEmployeeInformation> EmployeeInformationList { get; set; }
        static List<BTEmployeeESIInformation> EmployeeEsiInformationList { get; set; }
        #endregion
        void ResetControl()
        {
            try
            {
                cmbStartHour.DataSource = null;
                cmbStartMinutes.DataSource = null;
                cmbEndHour.DataSource = null;
                cmbEndMinutes.DataSource = null;
                cmbStartHour.DataSource = Utility.Hours();
                cmbStartMinutes.DataSource = Utility.Minutes();
                cmbEndHour.DataSource = Utility.Hours();
                cmbEndMinutes.DataSource = Utility.Minutes();
                //CurrentStatus = DataIntegrationStatus.NotStarted;
                dtpStartDate.MinDate = dtpEndDate.MinDate = new DateTime(2018, 1, 1);
                dtpStartDate.MaxDate = dtpEndDate.MaxDate = DateTime.Now;
                InitializeControl();
                btnFetch.Visible = btnFetch.Enabled = true;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }
        public DataIntegrationTool()
        {
            try
            {
                InitializeComponent();
                ResetControl();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }
        
        private async Task FetchEmployeeInformation()
        {
            try
            {
                message = "Fetching Employee Information from Blue Tree API";
                Utility.Logger.Info(message);
                lstMessage.Items.Add(message);
                pbDataIntegrationService.Value = 5;
                btnFetch.Enabled = false;
                ApiRequest request = new ApiRequest
                {
                    Address = Configuration.BlueTreeURL,
                    Data = DataIntegrationParameter,
                    Password = Configuration.BasicAuthPassword,
                    UserName = Configuration.BasicAuthUsername
                };
                pbDataIntegrationService.Value += 5;
                message="Establishing connection with Blue Tree API to fetch Employee Information";
                Utility.Logger.Info(message);
                lstMessage.Items.Add(message);
                var response = await Task.Run(() => ApiService.InvokeApirequest<List<BTEmployeeInformation>>(request));
                if (response != null && !string.IsNullOrEmpty(response.Message) && response.Message.ToLower() == "success")
                {
                    message="Received response from API";
                    Utility.Logger.Info(message);
                    lstMessage.Items.Add(message);
                    
                    pbDataIntegrationService.Value += 5;
                    EmployeeInformationList = response.Output;
                    pbDataIntegrationService.Value += 5;
                    Thread.Sleep(200);
                    if (EmployeeInformationList != null && EmployeeInformationList.Count > 0)
                    {
                        message="Successfully fetched employee Information from the Blue Tree API.";
                        Utility.Logger.Info(message);
                        lstMessage.Items.Add(message);
                        

                        pbDataIntegrationService.Value += 5;
                    }
                    else
                    {
                        message="No Employee Information was fetched from the server.";
                        Utility.Logger.Info(message);
                        lstMessage.Items.Add(message);
                    }
                }
                else
                {
                    message="Failed to establish connection with the Blue Tree API.";
                    Utility.Logger.Info(message);
                    lstMessage.Items.Add(message);
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }

        private async Task FetchEsiEmployeeInformation()
        {
            try
            {
                message="Trying to fetch Employee Information from ESI API";
                Utility.Logger.Info(message);
                lstMessage.Items.Add(message);
                var request = new ApiRequest
                {
                    Address = Configuration.BlueTreeESIURL,
                    Data = DataIntegrationEsiParameter,
                    Password = Configuration.BasicAuthPassword,
                    UserName = Configuration.BasicAuthUsername
                };
                pbDataIntegrationService.Value += 5;
                message="Establishing connection with Blue Tree  ESI API to fetch Employee Information";
                Utility.Logger.Info(message);
                lstMessage.Items.Add(message);
                var esiResponse = await Task.Run(() => ApiService.InvokeApirequest<List<BTEmployeeESIInformation>>(request));
                if (esiResponse != null && !string.IsNullOrEmpty(esiResponse.Message) && esiResponse.Message.ToLower() == "success")
                {
                    message="Received response from ESI API";
                    Utility.Logger.Info(message);
                    lstMessage.Items.Add(message);
                    pbDataIntegrationService.Value += 5;
                    EmployeeEsiInformationList = esiResponse.Output;
                    pbDataIntegrationService.Value += 5;
                    Thread.Sleep(200);
                    if (EmployeeEsiInformationList != null && EmployeeEsiInformationList.Count > 0)
                    {
                        message="Successfully fetched ESI employee Information from the Blue Tree API.";
                        Utility.Logger.Info(message);
                        lstMessage.Items.Add(message);
                        pbDataIntegrationService.Value = pbDataIntegrationService.Maximum;
                    }
                    else
                    {
                        message="No Employee ESI Information was fetched from the server.";
                        Utility.Logger.Info(message);
                        lstMessage.Items.Add(message);
                    }
                }
                else
                {
                    message="Failed to establish connection with the Blue Tree ESI API.";
                    Utility.Logger.Info(message);
                    lstMessage.Items.Add(message);
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }

        async Task SyncEmployeeInformation()
        {
            lblMessage.Text = string.Empty;
            try
            {
                if (EmployeeInformationList != null)
                {
                    //Upsert BlueTree and ERP Data
                    message = "Process of Syncing of employee information begins";
                    lstMessage.Items.Add(message);
                    Utility.Logger.Info(message);
                    pbDataIntegrationService.Value = 0;
                    pbDataIntegrationService.Maximum = EmployeeInformationList.Count;
                    foreach (var employeeInfo in EmployeeInformationList)
                    {
                        lblMessage.Text = string.Format("Syncing {0} of {1} employees", pbDataIntegrationService.Value+1 , EmployeeInformationList.Count);
                        var empData = JsonConvert.SerializeObject(employeeInfo);
                        var messages= await Task.Run(()=> ConnectionManager.UpsertBlueTreeERPData(employeeInfo));
                        lstMessage.Items.AddRange(messages.ToArray());
                        pbDataIntegrationService.Value += 1;
                        
                    }
                    message = "Process of Syncing of employee information ends";
                    lstMessage.Items.Add(message);
                    Utility.Logger.Info(message);
                }

            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }

        }
        async Task SyncEmployeeEsiInformation()
        {
            lblMessage.Text = string.Empty;

            try
            {
                if (EmployeeEsiInformationList != null)
                {
                    message = "Process of Syncing of ESI employee information begins";
                    lstMessage.Items.Add(message);
                    Utility.Logger.Info(message);
                    pbDataIntegrationService.Value = 0;
                    pbDataIntegrationService.Maximum = EmployeeEsiInformationList.Count;
                    foreach (var employeeESIInformation in EmployeeEsiInformationList)
                    {
                        lblMessage.Text = string.Format("Syncing {0} of {1} ESI Employees", pbDataIntegrationService.Value + 1, EmployeeEsiInformationList.Count);

                        //Upsert BlueTree and ERP Data
                        var messages = await Task.Run(()=>ConnectionManager.UpdateDataintoERP(employeeESIInformation));
                        lstMessage.Items.AddRange(messages.ToArray());
                        pbDataIntegrationService.Value += 1;

                    }
                    lblMessage.Text = string.Format("Synced {0} of {1} ESI Employees", pbDataIntegrationService.Value , EmployeeEsiInformationList.Count);

                    message = "Process of Syncing of ESI employee information ends";
                    lstMessage.Items.Add(message);
                    Utility.Logger.Info(message);
                }

            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }
        private async void btnSync_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to Sync the Employee Information ?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    lstMessage.Items.Add("The Process of Syncing begins");
                    btnSync.Enabled = false;
                    await SyncEmployeeInformation();
                    await SyncEmployeeEsiInformation();
                    btnSync.Visible = false;
                    btnFetch.Visible = true;
                    btnFetch.Enabled = true;
                    lstMessage.Items.Add("The Process of Syncing ends");
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }
        void InitializeControl()
        {
            btnFetch.Visible = false;
            btnSync.Visible = false;
            lstMessage.Items.Clear();
            pbDataIntegrationService.Value = 0;
            EmployeeInformationList = new List<BTEmployeeInformation>();
            EmployeeEsiInformationList = new List<BTEmployeeESIInformation>();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetControl();
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }

        private async void btnFetch_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime startDate = Convert.ToDateTime(StartDate);
                DateTime endDate = Convert.ToDateTime(EndDate);
                if (startDate > endDate || startDate == endDate)
                {
                    MessageBox.Show("Start Date should always be less than End date.");
                    return;
                }
                lstMessage.Items.Clear();
                message = "The Process of fetching Employee Information begins";
                Utility.Logger.Info(message);
                lstMessage.Items.Add(message);                    
                pbDataIntegrationService.Maximum = 50;
                await FetchEmployeeInformation();
                await FetchEsiEmployeeInformation();
                if ((EmployeeEsiInformationList != null && EmployeeEsiInformationList.Count > 0) || (EmployeeInformationList != null && EmployeeInformationList.Count > 0))
                {
                    btnSync.Visible = true;
                }
                message = "The Process of fetching Employee Information ends";
                Utility.Logger.Info(message);
                lstMessage.Items.Add(message);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
                ResetControl();
            }
        }

        private void pbDataIntegrationService_Click(object sender, EventArgs e)
        {

        }

        private void lstMessage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DataIntegrationTool_Load(object sender, EventArgs e)
        {

        }
    }
}
