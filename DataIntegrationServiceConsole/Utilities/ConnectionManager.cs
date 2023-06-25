using DataIntegrationServiceConsole.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace DataIntegrationServiceConsole
{
    public class ConnectionManager
    {

        static Dictionary<string, string> prefixValuePair = new Dictionary<string, string>();
        static DataTable employeeEntitlementTable = null;

        private static SqlConnection GetConnection()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(Configuration.ConnectionString);
                connection.Open();

            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
                throw;
            }
            return connection;
        }
        public static void UpsertBlueTreeERPData(List<BTEmployeeInformation> employeeInformationList, SqlConnection connection)
        {
            try
            {


                if (employeeInformationList != null && employeeInformationList.Count > 0)
                {

                    foreach (BTEmployeeInformation employeeInformation in employeeInformationList)
                    {
                        Utility.Logger.Info($"Syncing the Employee Details {JsonConvert.SerializeObject(employeeInformation)}");
                        Utility.Logger.Info($"Syncing {employeeInformation?.employeeBasicDetails?[0].EmployeeNumber} begins");


                        #region Filling Data for ERP
                        InsertDataintoERP(employeeInformation);
                        #endregion
                        Utility.Logger.Info($"Syncing {employeeInformation?.employeeBasicDetails?[0].EmployeeNumber} ends");

                    }

                    prefixValuePair.Clear();
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }

        private static List<string> InsertDataintoERP(BTEmployeeInformation employeeInformation)
        {
            var lstMessage = new List<string>();
            var connection = GetConnection();
            if (connection != null && connection.State == ConnectionState.Closed)
                connection.Open();
            try
            {
                ValidateData(employeeInformation);

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                lstMessage.Add($"Checking Data for {employeeInformation?.employeeBasicDetails[0]?.EmployeeNumber}");
                string selectQuery = string.Empty;
                string lastValue = string.Empty;
                int newValue = 0;
                string seqPrefix = string.Empty;
                string insertQuery = string.Empty;

                selectQuery = "Select CandNo from CandidatePersonalDetails where RegNo in ('" + employeeInformation?.employeeBasicDetails[0]?.EmployeeNumber + "')";
                command.CommandText = selectQuery;
                object candnumber = command.ExecuteScalar();
                string CandNo = candnumber == null ? string.Empty : command.ExecuteScalar().ToString();

                prefixValuePair.Clear();
                selectQuery = ("SELECT Prefix,LastValue FROM SequenceMaster WHERE Prefix IN ('APP','EMP','SWR')");
                command.CommandText = selectQuery;
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    {
                        while (dataReader.Read())
                            if (!prefixValuePair.ContainsKey(dataReader[0].ToString()))
                                prefixValuePair.Add(dataReader[0].ToString(), dataReader[1].ToString());
                    }
                }
                dataReader.Close();

                selectQuery = string.Format(@"SELECT SequenceMaster.Prefix FROM SequenceMaster WHERE Head = '{0}'", employeeInformation?.employeeBasicDetails[0]?.WorkLocation);
                command.CommandText = selectQuery;
                seqPrefix = command.ExecuteScalar().ToString();

                if (string.IsNullOrEmpty(CandNo))
                {
                    lastValue = ("000000" + prefixValuePair["APP"]);
                    lastValue = lastValue.Substring(lastValue.Length - 6);

                    newValue = (int.Parse(lastValue) + 1);
                    CandNo = seqPrefix + "-APP" + newValue;
                    prefixValuePair["APP"] = newValue.ToString();

                    command.CommandText = string.Format("UPDATE SequenceMaster SET LastValue = {0}  WHERE Prefix = 'APP'", newValue);
                    command.ExecuteNonQuery();

                    #region Insert data into CandidatePersonalDetails
                    insertQuery = @"INSERT INTO CandidatePersonalDetails(CandNo, ApplicationDate, CandName, ApplicationType, PostAppliedFor, CandSurName, CandFirstName, CandMiddleName, 
                    CandPresAddress, CandPermAddress, CandNationaity, CandState, CandDob, MobileNo, Email, CandTelNo, CandPlaceofBirth, Sex,CandMaritalStatus, CandDependents, Father,
                    CandFathHusbName, Profession, BandType, Reference, CandMotherName, isSelected, isCancel, CityID, CandPresentCity, CandCast, Remarks, HQual, CreatedBy
                    , CreatedOn, ModifiedBy, ModifiedOn, OldRegNo, RegNo, BranchCode, ESICNumber, RefereeRegNo, UANNo, IsActive, UnitCode, AadharNo, IsChargableUniform,CandReligion)
                     VALUES
                    (@CandNo, @ApplicationDate, @CandName, @ApplicationType, @PostAppliedFor, @CandSurName, @CandFirstName, @CandMiddleName, @CandPresAddress, @CandPermAddress
                    , @CandNationaity, @CandState, @CandDob, @MobileNo, @Email, @CandTelNo, @CandPlaceofBirth,@Sex, @CandMaritalStatus,@CandDependents, @Father, @CandFathHusbName, @Profession,
                    @BandType, @Reference,@CandMotherName, @isSelected,@isCancel, @CityID, @CandPresentCity, @CandCast, @Remarks, @HQual, @CreatedBy, @CreatedOn,
                    @ModifiedBy, @ModifiedOn, @OldRegNo, @RegNo, @BranchCode, @ESICNumber, @RefereeRegNo, @UANNo, @IsActive, @UnitCode, @AadharNo, @IsChargableUniform,@CandReligion)";

                    command.Parameters.AddWithValue("@CandNo", CandNo);
                    command.Parameters.AddWithValue("@ApplicationDate", DateTime.Now);
                    command.Parameters.AddWithValue("@CandName", (employeeInformation?.employeeBasicDetails?[0].Name + " " + employeeInformation?.employeeBasicDetails?[0].MiddleName + " " + employeeInformation?.employeeBasicDetails?[0].LastName).ToUpper());
                    command.Parameters.AddWithValue("@ApplicationType", "SGTOSSI");
                    command.Parameters.AddWithValue("@PostAppliedFor", employeeInformation?.additionalDetails?.Designation);
                    command.Parameters.AddWithValue("@CandSurName", string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0].LastName) ? string.Empty : employeeInformation?.employeeBasicDetails?[0].LastName.ToUpper());
                    command.Parameters.AddWithValue("@CandFirstName", string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0].Name) ? string.Empty : employeeInformation?.employeeBasicDetails?[0].Name.ToUpper());
                    command.Parameters.AddWithValue("@CandMiddleName", string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0].MiddleName) ? string.Empty : employeeInformation?.employeeBasicDetails?[0].MiddleName.ToUpper());
                    command.Parameters.AddWithValue("@CandPresAddress", employeeInformation?.currentAddress?.Address1);// + " " + employeeInformation?.currentAddress?.Address2 + " " + employeeInformation?.currentAddress?.District + " "  + employeeInformation?.currentAddress?.PostalCode);
                    command.Parameters.AddWithValue("@CandPermAddress", employeeInformation?.permanentAddress?.Address1);// + " " + employeeInformation?.permanentAddress?.Address2 + " " + employeeInformation?.permanentAddress?.City + " " + employeeInformation?.permanentAddress?.District + " " + employeeInformation?.permanentAddress?.State + " " + employeeInformation?.permanentAddress?.PostalCode);
                    command.Parameters.AddWithValue("@CandNationaity", employeeInformation?.currentAddress?.Nationality);
                    command.Parameters.AddWithValue("@CandState", employeeInformation?.currentAddress.State);
                    command.Parameters.AddWithValue("@CandDob", DateTime.ParseExact(employeeInformation?.employeeBasicDetails?[0].DateofBirth, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@MobileNo", employeeInformation?.employeeBasicDetails?[0].MobileNo);
                    command.Parameters.AddWithValue("@Email", employeeInformation?.employeeBasicDetails?[0].Email);
                    command.Parameters.AddWithValue("@CandTelNo", employeeInformation?.employeeBasicDetails?[0].HomePhone);
                    command.Parameters.AddWithValue("@CandPlaceofBirth", employeeInformation?.additionalDetails?.candidatePlaceOfBirth);
                    command.Parameters.AddWithValue("@Sex", employeeInformation?.employeeBasicDetails?[0].Gender);
                    command.Parameters.AddWithValue("@CandMaritalStatus", employeeInformation?.employeeBasicDetails?[0].MaritalStatus);
                    command.Parameters.AddWithValue("@CandDependents", DBNull.Value);
                    command.Parameters.AddWithValue("@Father", employeeInformation?.employeeBasicDetails?[0]?.MaritalStatus.ToUpper() == "MARRIED" ? true : false);

                    if (!string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0].SpouseName))
                        command.Parameters.AddWithValue("@CandFathHusbName", employeeInformation?.employeeBasicDetails?[0].SpouseName);
                    else
                        command.Parameters.AddWithValue("@CandFathHusbName", employeeInformation?.employeeBasicDetails?[0].FatherName);

                    command.Parameters.AddWithValue("@Profession", DBNull.Value);
                    command.Parameters.AddWithValue("@BandType", employeeInformation?.additionalDetails?.Position);
                    command.Parameters.AddWithValue("@Reference", DBNull.Value);
                    command.Parameters.AddWithValue("@CandMotherName", employeeInformation?.employeeBasicDetails?[0].MotherName);
                    command.Parameters.AddWithValue("@isSelected", 0);
                    command.Parameters.AddWithValue("@isCancel", 0);
                    command.Parameters.AddWithValue("@CityID", employeeInformation?.currentAddress.City);
                    command.Parameters.AddWithValue("@CandPresentCity", employeeInformation?.currentAddress.City);
                    command.Parameters.AddWithValue("@CandCast", employeeInformation?.additionalDetails?.Caste);
                    command.Parameters.AddWithValue("@Remarks", DBNull.Value);
                    command.Parameters.AddWithValue("@HQual", employeeInformation?.additionalDetails?.HQualification);
                    command.Parameters.AddWithValue("@CreatedBy", employeeInformation?.employeeBasicDetails?[0]?.CreateUser);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.ParseExact(employeeInformation?.employeeBasicDetails?[0]?.CreatedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@ModifiedBy", DBNull.Value);
                    command.Parameters.AddWithValue("@ModifiedOn", DBNull.Value);
                    command.Parameters.AddWithValue("@OldRegNo", DBNull.Value);
                    command.Parameters.AddWithValue("@RegNo", employeeInformation?.employeeBasicDetails?[0]?.EmployeeNumber);
                    command.Parameters.AddWithValue("@BranchCode", employeeInformation?.employeeBasicDetails?[0].WorkLocation);
                    command.Parameters.AddWithValue("@ESICNumber", employeeInformation?.employeeBasicDetails?[0].EsiNo);
                    command.Parameters.AddWithValue("@RefereeRegNo", DBNull.Value);
                    command.Parameters.AddWithValue("@UANNo", employeeInformation?.PFDetails?.uanNumber);
                    command.Parameters.AddWithValue("@IsActive", DBNull.Value);
                    command.Parameters.AddWithValue("@UnitCode", employeeInformation?.employeeBasicDetails?[0].Customer);
                    command.Parameters.AddWithValue("@AadharNo", employeeInformation?.employeeBasicDetails?[0].AadharNo);
                    command.Parameters.AddWithValue("@IsChargableUniform", DBNull.Value);
                    command.Parameters.AddWithValue("@CandReligion", employeeInformation?.additionalDetails?.Religion);

                    command.CommandText = insertQuery;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    #endregion

                    lstMessage.Add("Data Inserted into Candidate Details");

                }

                #region Check in EMPLOYEE MASTER
                selectQuery = "Select EMPID from EmployeeMaster where CandNo = '" + CandNo + "'";
                command.CommandText = selectQuery;
                object ID = command.ExecuteScalar();
                string EMPID = ID == null ? string.Empty : command.ExecuteScalar().ToString();


                selectQuery = string.Format("SELECT BasicPrice FROM ServiceMaster WHERE ServiceCode = '{0}'", employeeInformation?.additionalDetails?.Designation);
                command.CommandText = selectQuery;
                object value = command.ExecuteScalar();

                decimal basePrice = value == null ? 0 : decimal.Parse(value.ToString());

                if (string.IsNullOrEmpty(EMPID))
                {
                    lastValue = ("000000" + prefixValuePair["EMP"]);
                    lastValue = lastValue.Substring(lastValue.Length - 6);

                    newValue = (int.Parse(lastValue) + 1);
                    EMPID = seqPrefix + "-EMP" + newValue;
                    prefixValuePair["EMP"] = newValue.ToString();

                    command.CommandText = string.Format("UPDATE SequenceMaster SET LastValue = {0}  WHERE Prefix = 'EMP'", newValue);
                    command.ExecuteNonQuery();

                    #region Insert data into EmployeeMaster
                    insertQuery = @"INSERT INTO EmployeeMaster(EmpID,CandNo,BranchEmpCode,CardNo,Name,Department,IsExecutive,Designation,Grade,BranchCode,Position,HierarchyCode,PresAddress,
                    PresCity,PermAddress,PermCity,PhoneNo,PinCode,Email,Mobile,PFNo,PFDate,ESICNo,PanNo,Married,DoB,JoiningDt,Sex,FatherName,MotherName,BankAcNo,BankName,Dispensary,HQualification,BloodGroup,ESICSubCodeLocation,GrossSalary,FixSalary)
                    VALUES
                    (@EmpID,@CandNo,@BranchEmpCode,@CardNo,@Name,@Department,@IsExecutive,@Designation,@Grade,@BranchCode,@Position,@HierarchyCode,@PresAddress,@PresCity,@PermAddress,@PermCity,
                    @PhoneNo,@PinCode,@Email,@Mobile,@PFNo,@PFDate,@ESICNo,@PanNo,@Married,@DoB,@JoiningDt,@Sex,@FatherName,@MotherName,@BankAcNo,@BankName,@Dispensary,@HQualification,@BloodGroup,@ESICSubCodeLocation,@GrossSalary,@FixSalary)";

                    command.Parameters.AddWithValue("@EmpID", EMPID);
                    command.Parameters.AddWithValue("@CandNo", CandNo);
                    command.Parameters.AddWithValue("@BranchEmpCode", employeeInformation?.employeeBasicDetails[0]?.EmployeeNumber);
                    command.Parameters.AddWithValue("@CardNo", 0);
                    command.Parameters.AddWithValue("@Name", (employeeInformation?.employeeBasicDetails[0]?.Name + " " + employeeInformation?.employeeBasicDetails[0]?.MiddleName + " " + employeeInformation?.employeeBasicDetails?[0].LastName).ToUpper());
                    command.Parameters.AddWithValue("@Department", employeeInformation?.additionalDetails?.Department);
                    command.Parameters.AddWithValue("@IsExecutive", "SGTOSSI");
                    command.Parameters.AddWithValue("@Designation", employeeInformation?.additionalDetails?.Designation);
                    command.Parameters.AddWithValue("@Grade", "SG000082");
                    command.Parameters.AddWithValue("@BranchCode", employeeInformation?.employeeBasicDetails[0]?.WorkLocation);
                    command.Parameters.AddWithValue("@Position", employeeInformation?.additionalDetails?.Position);
                    command.Parameters.AddWithValue("@HierarchyCode", employeeInformation?.additionalDetails?.Position);
                    command.Parameters.AddWithValue("@PresAddress", employeeInformation?.currentAddress?.Address1 + " " + employeeInformation?.currentAddress?.Address2 + " " + employeeInformation?.currentAddress?.District + " " + employeeInformation?.currentAddress?.PostalCode);
                    command.Parameters.AddWithValue("@PresCity", employeeInformation?.currentAddress.City);
                    command.Parameters.AddWithValue("@PermAddress", employeeInformation?.permanentAddress?.Address1);// + " " + employeeInformation?.permanentAddress?.Address2 + " " + employeeInformation?.permanentAddress?.City + " " + employeeInformation?.permanentAddress?.District + " " + employeeInformation?.permanentAddress?.State + " " + employeeInformation?.permanentAddress?.PostalCode);
                    command.Parameters.AddWithValue("@PermCity", employeeInformation?.permanentAddress?.City);
                    command.Parameters.AddWithValue("@PhoneNo", employeeInformation?.employeeBasicDetails?[0]?.HomePhone);
                    command.Parameters.AddWithValue("@PinCode", employeeInformation?.currentAddress?.PostalCode);
                    command.Parameters.AddWithValue("@Email", employeeInformation?.employeeBasicDetails?[0]?.Email);
                    command.Parameters.AddWithValue("@Mobile", employeeInformation?.employeeBasicDetails?[0]?.MobileNo);
                    string pfNumber = employeeInformation?.PFDetails?.pfNumber;
                    command.Parameters.AddWithValue("@PFNo", pfNumber == "" ? "0" : pfNumber.Substring(pfNumber.Length - 5));
                    command.Parameters.AddWithValue("@PFDate", DBNull.Value);
                    command.Parameters.AddWithValue("@ESICNo", employeeInformation?.employeeBasicDetails?[0]?.EsiNo);
                    command.Parameters.AddWithValue("@PanNo", employeeInformation?.bankAccountDetails?.PANNumber);
                    command.Parameters.AddWithValue("@Married", employeeInformation?.employeeBasicDetails?[0]?.MaritalStatus);
                    command.Parameters.AddWithValue("@DoB", DateTime.ParseExact(employeeInformation?.employeeBasicDetails?[0]?.DateofBirth, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@JoiningDt", DateTime.ParseExact(employeeInformation?.additionalDetails?.JoiningDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@Sex", employeeInformation?.employeeBasicDetails?[0]?.Gender);
                    command.Parameters.AddWithValue("@FatherName", employeeInformation?.employeeBasicDetails?[0]?.FatherName);
                    command.Parameters.AddWithValue("@MotherName", employeeInformation?.employeeBasicDetails?[0]?.MotherName);
                    command.Parameters.AddWithValue("@BankAcNo", employeeInformation?.bankAccountDetails?.BankAccountNumber);
                    command.Parameters.AddWithValue("@BankName", employeeInformation?.bankAccountDetails?.BankName);
                    command.Parameters.AddWithValue("@Dispensary", employeeInformation?.esiInformation?.Dispensary);
                    command.Parameters.AddWithValue("@HQualification", employeeInformation?.additionalDetails?.HQualification);
                    command.Parameters.AddWithValue("@BloodGroup", employeeInformation?.employeeBasicDetails?[0]?.BloodGroup);
                    command.Parameters.AddWithValue("@ESICSubCodeLocation", employeeInformation?.esiInformation?.ESICSubCodeLocation);
                    command.Parameters.AddWithValue("@GrossSalary", basePrice);
                    command.Parameters.AddWithValue("@FixSalary", basePrice);

                    command.CommandText = insertQuery;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    #endregion

                    lstMessage.Add("Data Inserted into Employee Master");
                }

                #endregion

                #region Check in Candidate Photo
                selectQuery = "Select Count(*) from CandidatePhoto where CandNo = '" + CandNo + "'";
                command.CommandText = selectQuery;
                value = command.ExecuteScalar();

                if (int.Parse(value.ToString()) == 0)
                {
                    #region Insert data into Candidate Photo
                    insertQuery = @"INSERT INTO CandidatePhoto(CandNo,Image,OldCode,ImagePath,UploadDocumentPath,UploadBankDocumentPath,UploladPFDocumentPath)
                                VALUES (@CandNo,@Image,@OldCode,@ImagePath,@UploadDocumentPath,@UploadBankDocumentPath,@UploladPFDocumentPath)";
                    command.Parameters.AddWithValue("@CandNo", CandNo);
                    command.Parameters.AddWithValue("@Image", Convert.FromBase64String(employeeInformation?.employeeBasicDetails?[0]?.EmployeePhoto));
                    command.Parameters.AddWithValue("@OldCode", DBNull.Value);
                    command.Parameters.AddWithValue("@ImagePath", ConvertintoImage(employeeInformation?.employeeBasicDetails?[0]?.EmployeePhoto, CandNo, employeeInformation?.employeeBasicDetails?[0]?.EmployeeNumber));
                    command.Parameters.AddWithValue("@UploadDocumentPath", ConvertintoDocument(employeeInformation?.familyDetails?.FamilyDetailsDocument, CandNo + "_FamilyDoc", employeeInformation?.employeeBasicDetails?[0]?.EmployeeNumber));
                    command.Parameters.AddWithValue("@UploadBankDocumentPath", ConvertintoDocument(employeeInformation?.bankAccountDetails?.BankDocument, CandNo + "_BankDoc", employeeInformation?.employeeBasicDetails?[0]?.EmployeeNumber));
                    command.Parameters.AddWithValue("@UploladPFDocumentPath", ConvertintoDocument(employeeInformation?.PFDetails?.PFDocUpload, CandNo + "_PFDoc", employeeInformation?.employeeBasicDetails?[0]?.EmployeeNumber));
                    command.CommandText = insertQuery;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    #endregion

                    lstMessage.Add("Data Inserted into Candidate Photo");
                }

                #endregion

                #region Check in Employee Nominee Details
                selectQuery = "Select count(*) from EmployeeNomineeDetails where RegNo = '" + employeeInformation?.employeeBasicDetails[0]?.EmployeeNumber + "'";
                command.CommandText = selectQuery;
                value = command.ExecuteScalar();
                if (int.Parse(value.ToString()) == 0)
                {
                    #region Insert data into CandidateNomineeDetails
                    insertQuery = @"INSERT INTO EmployeeNomineeDetails(RegNo,NomineeName,RelationshipWithEmployee,Gender,DateofBirth,MobileNo,AddressAsPerAadhar,Address)
                     VALUES (@RegNo,@NomineeName,@RelationshipWithEmployee,@Gender,@DateofBirth,@MobileNo,@AddressAsPerAadhar,@Address)";
                    command.Parameters.AddWithValue("@RegNo", employeeInformation?.employeeBasicDetails?[0]?.EmployeeNumber);
                    command.Parameters.AddWithValue("@NomineeName", employeeInformation?.nomineeInformation?.Name);
                    command.Parameters.AddWithValue("@RelationshipWithEmployee", string.IsNullOrEmpty(employeeInformation?.nomineeInformation?.RelationshipwithEmployees) ? string.Empty : employeeInformation?.nomineeInformation?.RelationshipwithEmployees);
                    command.Parameters.AddWithValue("@Gender", employeeInformation?.nomineeInformation?.Gender);
                    command.Parameters.AddWithValue("@DateofBirth", DateTime.ParseExact(employeeInformation?.nomineeInformation?.DOB, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@MobileNo", employeeInformation?.nomineeInformation?.MobileNo);
                    command.Parameters.AddWithValue("@AddressAsPerAadhar", employeeInformation?.nomineeInformation?.AddressasPerAadhar == null ? "" : employeeInformation?.nomineeInformation?.AddressasPerAadhar);
                    if (employeeInformation?.nomineeInformation?.AddressasPerAadhar == "Yes")
                    {
                        command.Parameters.AddWithValue("@Address", employeeInformation?.permanentAddress?.Address1 + " " + employeeInformation?.permanentAddress?.Address2 + " " + employeeInformation?.permanentAddress?.City + " " + employeeInformation?.permanentAddress?.District + " " + employeeInformation?.permanentAddress?.State + " " + employeeInformation?.permanentAddress?.PostalCode);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address", employeeInformation?.nomineeInformation?.Address);
                    }
                    command.CommandText = insertQuery;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    #endregion

                    lstMessage.Add("Data Inserted into Candidate Nominee Detail");
                }
                #endregion

                #region Check in Employee Bank Detail
                selectQuery = "Select COUNT(*) from EmployeeBankDetail where EMPRegNo = '" + employeeInformation?.employeeBasicDetails[0]?.EmployeeNumber + "'";
                command.CommandText = selectQuery;
                value = command.ExecuteScalar();
                if (int.Parse(value.ToString()) == 0)
                {
                    #region Insert data into BankDetails
                    insertQuery = @"INSERT INTO EmployeeBankDetail(SisBranchCode,Date,BankCode,EmpRegNo,AcNo,ModifiedBy,ModifiedOn,CardNo,EmpCode,ActivationCharge,Active,IFSCCode,BankName)
                    VALUES (@SisBranchCode,@Date,@BankCode,@EmpRegNo,@AcNo,@ModifiedBy,@ModifiedOn,@CardNo,@EmpCode,@ActivationCharge,@Active,@IFSCCode,@BankName)";
                    command.Parameters.AddWithValue("@SisBranchCode", "BR000088");
                    command.Parameters.AddWithValue("@Date", DateTime.Now);
                    command.Parameters.AddWithValue("@BankCode", "CFM-BNK000090");
                    command.Parameters.AddWithValue("@EmpRegNo", employeeInformation?.employeeBasicDetails?[0]?.EmployeeNumber);
                    command.Parameters.AddWithValue("@AcNo", employeeInformation?.bankAccountDetails?.BankAccountNumber);
                    command.Parameters.AddWithValue("@ModifiedBy", employeeInformation?.employeeBasicDetails?[0]?.CreateUser);
                    command.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
                    command.Parameters.AddWithValue("@CardNo", string.Empty);
                    command.Parameters.AddWithValue("@EmpCode", EMPID);
                    command.Parameters.AddWithValue("@ActivationCharge", 1);
                    command.Parameters.AddWithValue("@Active", 1);
                    command.Parameters.AddWithValue("@IFSCCode", employeeInformation?.bankAccountDetails?.IFSCCode);
                    command.Parameters.AddWithValue("@BankName", employeeInformation?.bankAccountDetails?.BankName);
                    command.CommandText = insertQuery;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    #endregion

                    lstMessage.Add("Data Inserted into Bank Detail");

                }

                #endregion

                #region Check in SEWA Registration
                selectQuery = "SELECT COUNT(*) from SEWARegistration where RegNo  = '" + EMPID + "'";
                command.CommandText = selectQuery;
                value = command.ExecuteScalar();
                if (int.Parse(value.ToString()) == 0)
                {
                    #region Create SWRID
                    lastValue = ("000000" + prefixValuePair["SWR"]);
                    lastValue = lastValue.Substring(lastValue.Length - 6);

                    newValue = (int.Parse(lastValue) + 1);
                    string SWRID = seqPrefix + "-SWR" + newValue;
                    prefixValuePair["SWR"] = newValue.ToString();

                    command.CommandText = string.Format("UPDATE SequenceMaster SET LastValue = {0}  WHERE Prefix = 'SWR'", newValue);
                    command.ExecuteNonQuery();
                    #endregion

                    #region Insert data into SEWARegistration
                    insertQuery = @"INSERT INTO SEWARegistration (SEWACode,MemDate,CandidateNumber,RegNo,OldRegNo,RankOrBandPromotionDate,CategoryCode,Remarks,WtRegNo,RegFee,Relationship,
                    MonthlySubFee,NomineeName,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Band,IsActive)
                    VALUES(@SEWACode,@MemDate,@CandidateNumber,@RegNo,@OldRegNo,@RankOrBandPromotionDate,@CategoryCode,@Remarks,@WtRegNo,@RegFee,@Relationship,
                    @MonthlySubFee,@NomineeName,@CreatedBy,@CreatedOn,@ModifiedBy,@ModifiedOn,@Band,@IsActive)";
                    command.Parameters.AddWithValue("@SEWACode", SWRID);
                    command.Parameters.AddWithValue("@MemDate", DateTime.Now);
                    command.Parameters.AddWithValue("@CandidateNumber", DBNull.Value);
                    command.Parameters.AddWithValue("@RegNo", EMPID);
                    command.Parameters.AddWithValue("@OldRegNo", DBNull.Value);
                    command.Parameters.AddWithValue("@RankOrBandPromotionDate", DateTime.Now);
                    command.Parameters.AddWithValue("@CategoryCode", "118112");
                    command.Parameters.AddWithValue("@Remarks", "N.A");
                    command.Parameters.AddWithValue("@WtRegNo", "N.A");
                    command.Parameters.AddWithValue("@RegFee", 20);
                    command.Parameters.AddWithValue("@Relationship", DBNull.Value);
                    command.Parameters.AddWithValue("@MonthlySubFee", 20);
                    command.Parameters.AddWithValue("@NomineeName", DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedBy", employeeInformation?.employeeBasicDetails?[0]?.CreateUser);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                    command.Parameters.AddWithValue("@ModifiedBy", employeeInformation?.employeeBasicDetails?[0]?.CreateUser);
                    command.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
                    command.Parameters.AddWithValue("@Band", DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", 1);
                    command.CommandText = insertQuery;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    #endregion

                    lstMessage.Add("Data Inserted into SEWA Registartion");
                }
                #endregion

                #region Check in Employee Entitlement
                selectQuery = "Select COUNT(*) from EmployeeEntitlement where EmpCode = '" + EMPID + "'";
                command.CommandText = selectQuery;
                value = command.ExecuteScalar();
                if (int.Parse(value.ToString()) == 0)
                {
                    #region Insert data into Employee Entitlement


                    GetSchema();

                    DataRow row;

                    for (int counter = 1; counter < 9; counter++)
                    {
                        row = employeeEntitlementTable.NewRow();
                        row["EmpCode"] = EMPID;
                        row["Sno"] = counter;
                        row["SalHead"] = Configuration.entitlementSalHead[counter - 1];
                        row["IsEditable"] = (counter == 5 || counter == 7) ? true : false;
                        row["Entitle"] = null;
                        row["Changedddate"] = null;
                        row["Remarks"] = "N.A";
                        row["EntCatg"] = "SCD000001";
                        row["Type"] = null;
                        row["Deduction"] = null;
                        row["LedgerCode"] = null;
                        row["EmployeeSuperSeded"] = counter == 4 ? true : false;
                        row["UnitSuperSeded"] = counter == 4 ? false : true;
                        row["OfficeOrderNo"] = string.Empty;
                        row["Edate"] = DateTime.Now;
                        row["HeadDescription"] = Configuration.entitlementList[counter - 1];
                        row["EarnDeduction"] = (counter == 4 || counter == 5 || counter == 7) ? "Deduction" : "Earning";
                        row["PercentageEarnDeduction"] = 0.0;
                        row["OfSalaryHead"] = 0;
                        row["isBasic"] = counter == 1 ? true : false;
                        row["InsalarySlip"] = counter == 4 ? "1" : "0";
                        row["WEFDate"] = DateTime.Now;
                        row["AsperRule"] = (counter == 5 || counter == 7) ? true : false;
                        row["FormulaId"] = 0;
                        row["EntitlementHead"] = null;
                        row["FixedAmount"] = counter == 1 ? Convert.ToDecimal(string.Format("{0:0.00}", basePrice)) : Convert.ToDecimal(string.Format("{0:0.00}", 0));
                        row["FinalAmount"] = counter == 1 ? Convert.ToDecimal(string.Format("{0:0.00}", basePrice)) : Convert.ToDecimal(string.Format("{0:0.00}", 0));
                        row["AmountEarningDeduction"] = counter == 1 ? Convert.ToDecimal(string.Format("{0:0.000}", basePrice)) : Convert.ToDecimal(string.Format("{0:0.000}", 0));
                        row["Stop_Entitlement"] = false;
                        employeeEntitlementTable.Rows.Add(row);
                    }
                    #endregion
                    InsertData(employeeEntitlementTable);
                    employeeEntitlementTable.Rows.Clear();

                    lstMessage.Add("Data Inserted into Employee Entitlement");
                }
                #endregion


                lstMessage.Add("Employee integrated into ERP: " + employeeInformation?.employeeBasicDetails?[0].EmployeeNumber);



            }
            catch (Exception ex)
            {
                Utility.Logger.Error($"{ employeeInformation?.employeeBasicDetails?[0].EmployeeNumber}\n {ex}");

            }
            return lstMessage;

        }

        public static void ValidateData(BTEmployeeInformation employeeInformation)
        {
            try
            {
                int result;

                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.EmployeeNumber))
                    throw new Exception("Employee Number is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.Name))
                    throw new Exception("Name is missing");
                if (string.IsNullOrEmpty(employeeInformation?.additionalDetails?.Department))
                    throw new Exception("Department is missing");
                if (string.IsNullOrEmpty(employeeInformation?.additionalDetails?.Designation))
                    throw new Exception("Designation is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.WorkLocation))
                    throw new Exception("Branch Code is missing");
                if (string.IsNullOrEmpty(employeeInformation?.additionalDetails?.Position))
                    throw new Exception("Position is missing");
                if (string.IsNullOrEmpty(employeeInformation?.currentAddress?.City))
                    throw new Exception("Present City is missing");
                if (!int.TryParse(employeeInformation?.currentAddress.City, out result))
                    throw new Exception("Present City is not number");
                if (string.IsNullOrEmpty(employeeInformation?.permanentAddress?.City))
                    throw new Exception("Permanent City is missing");
                if (!int.TryParse(employeeInformation?.permanentAddress.City, out result))
                    throw new Exception("Permanent City is not number");
                if (string.IsNullOrEmpty(employeeInformation?.currentAddress?.PostalCode))
                    throw new Exception("Pin Code is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.MobileNo))
                    throw new Exception("Mobile number is missing");
                //if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.EsiNo))
                //    throw new Exception("ESIC Number is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.MaritalStatus))
                    throw new Exception("Marital status is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.DateofBirth))
                    throw new Exception("Date of birth is missing");
                if (string.IsNullOrEmpty(employeeInformation?.additionalDetails?.JoiningDate))
                    throw new Exception("Joining Date is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.Gender))
                    throw new Exception("Gender is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.FatherName))
                    throw new Exception("Father name is missing");
                //if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.MotherName))
                //    throw new Exception("Mother name is missing");
                if (string.IsNullOrEmpty(employeeInformation?.bankAccountDetails?.BankAccountNumber))
                    throw new Exception("Bank account number is missing");
                if (string.IsNullOrEmpty(employeeInformation?.bankAccountDetails?.IFSCCode))
                    throw new Exception("Bank IFSC code is missing");
                if (string.IsNullOrEmpty(employeeInformation?.bankAccountDetails?.BankName))
                    throw new Exception("Bank name is missing");
                if (string.IsNullOrEmpty(employeeInformation?.esiInformation?.ESICSubCodeLocation))
                    throw new Exception("ESIC subcode location is missing");
                if (string.IsNullOrEmpty(employeeInformation?.additionalDetails?.HQualification))
                    throw new Exception("HQualification is missing");
                if (string.IsNullOrEmpty(employeeInformation?.nomineeInformation?.Name))
                    throw new Exception("Nominee name is missing");
                if (string.IsNullOrEmpty(employeeInformation?.nomineeInformation?.RelationshipwithEmployees))
                    throw new Exception("Relationship with employee is missing");
                if (string.IsNullOrEmpty(employeeInformation?.nomineeInformation?.Gender))
                    throw new Exception("Nominee Gender is missing");
                if (string.IsNullOrEmpty(employeeInformation?.nomineeInformation?.DOB))
                    throw new Exception("Nominee date of birth is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.BloodGroup))
                    throw new Exception("Blood group is missing");
                if (string.IsNullOrEmpty(employeeInformation?.currentAddress?.Nationality))
                    throw new Exception("Nationality is missing");
                //if (string.IsNullOrEmpty(employeeInformation?.PFDetails?.uanNumber))
                //    throw new Exception("UAN number is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.Customer))
                    throw new Exception("Unit code is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.AadharNo))
                    throw new Exception("Aadhar number is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.CreateUser))
                    throw new Exception("Created By is missing");
                if (string.IsNullOrEmpty(employeeInformation?.employeeBasicDetails?[0]?.CreatedDate))
                    throw new Exception("Created Date is missing");
                if (string.IsNullOrEmpty(employeeInformation?.additionalDetails?.candidatePlaceOfBirth))
                    throw new Exception("Candidate place of birth is missing");
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);

                throw;
            }
        }

        public static void GetSchema()
        {
            try
            {
                if (employeeEntitlementTable == null)
                {
                    employeeEntitlementTable = new DataTable("EmployeeEntitlement");
                    employeeEntitlementTable.Columns.Add("EntitlementID");
                    employeeEntitlementTable.Columns.Add("EmpCode");
                    employeeEntitlementTable.Columns.Add("Sno");
                    employeeEntitlementTable.Columns.Add("SalHead");
                    employeeEntitlementTable.Columns.Add("IsEditable");
                    employeeEntitlementTable.Columns.Add("Entitle");
                    employeeEntitlementTable.Columns.Add("Changedddate");
                    employeeEntitlementTable.Columns.Add("Remarks");
                    employeeEntitlementTable.Columns.Add("EntCatg");
                    employeeEntitlementTable.Columns.Add("Type");
                    employeeEntitlementTable.Columns.Add("Deduction");
                    employeeEntitlementTable.Columns.Add("LedgerCode");
                    employeeEntitlementTable.Columns.Add("EmployeeSuperSeded");
                    employeeEntitlementTable.Columns.Add("UnitSuperSeded");
                    employeeEntitlementTable.Columns.Add("OfficeOrderNo");
                    employeeEntitlementTable.Columns.Add("Edate");
                    employeeEntitlementTable.Columns.Add("HeadDescription");
                    employeeEntitlementTable.Columns.Add("EarnDeduction");
                    employeeEntitlementTable.Columns.Add("PercentageEarnDeduction");
                    employeeEntitlementTable.Columns.Add("OfSalaryHead");
                    employeeEntitlementTable.Columns.Add("isBasic");
                    employeeEntitlementTable.Columns.Add("InsalarySlip");
                    employeeEntitlementTable.Columns.Add("WEFDate");
                    employeeEntitlementTable.Columns.Add("AsperRule");
                    employeeEntitlementTable.Columns.Add("FormulaId");
                    employeeEntitlementTable.Columns.Add("EntitlementHead");
                    employeeEntitlementTable.Columns.Add("FixedAmount");
                    employeeEntitlementTable.Columns.Add("FinalAmount");
                    employeeEntitlementTable.Columns.Add("AmountEarningDeduction");
                    employeeEntitlementTable.Columns.Add("Stop_Entitlement");
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);

            }
        }

        public static void InsertData(DataTable dataTable)
        {
            try
            {
                SqlConnection connection = GetConnection();
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    try
                    {
                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                        {
                            bulkcopy.BulkCopyTimeout = 660;
                            bulkcopy.DestinationTableName = dataTable.TableName;
                            bulkcopy.WriteToServer(dataTable);
                            bulkcopy.Close();
                        }
                    }
                    catch (Exception exp)
                    {
                        Utility.Logger.Error(exp);
                    }

                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }

        }

        public static string ConvertintoDocument(string base64String, string fileName, string employeeNumber)
        {
            string filePath = string.Empty;

            try
            {
                if (base64String.IsBase64String())
                {
                    var encodedByte = System.Convert.FromBase64String(base64String);
                    filePath = Path.Combine(Configuration.FilePath, employeeNumber);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = Path.Combine(filePath, fileName + ".pdf");

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    File.WriteAllBytes(filePath, encodedByte);
                }
                else
                {
                    Utility.Logger.Info(base64String);
                    Utility.Logger.Info("Invalid  Document as it was not a valid base64 encoded string");
                }

            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
                throw;
            }
            return filePath;

        }

        public static string ConvertintoImage(string base64String, string fileName, string employeeNumber)
        {
            string filePath = string.Empty;
            try
            {
                if (base64String.IsBase64String())
                {

                    filePath = Path.Combine(Configuration.FilePath, employeeNumber);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    filePath = Path.Combine(filePath, fileName + ".jpeg");
                    var bytes = Convert.FromBase64String(base64String);
                    using (var imageFile = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }

                }
                else
                {
                    Utility.Logger.Info(base64String);
                    Utility.Logger.Info("Invalid  Image as it was not a valid base64 encoded string");
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
                throw;
            }
            return filePath;

        }

        private static bool IsEmployeeNumberExists(string employeeNumber)
        {
            try
            {
                var connection = GetConnection();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format("SELECT COUNT(*) FROM EmployeeMaster WHERE BranchEmpCode = '{0}'", employeeNumber);
                object result = command.ExecuteScalar();

                return (int.Parse(result.ToString()) == 0) ? false : true;
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
            return false;
        }

        public static void UpdateDataintoERP(List<BTEmployeeESIInformation> employeeESIInformationList, SqlConnection connection, ref ListBox lstMessage, ref ProgressBar pb)
        {
            try
            {

                if (employeeESIInformationList != null && employeeESIInformationList.Count > 0)
                {
                    foreach (BTEmployeeESIInformation employeeESIInformation in employeeESIInformationList)
                    {
                        if (IsEmployeeNumberExists(employeeESIInformation?.employeeEsiPfDetails?.employeeNumber))
                        {

                            if (connection != null && connection.State == ConnectionState.Closed)
                                connection.Open();
                            SqlCommand command = new SqlCommand();
                            command.Connection = connection;

                            string updateQuery = @"UPDATE CandidatePersonalDetails SET ESICNumber = @ESICNumber,ModifiedBy = @ModifiedBy,ModifiedOn = @ModifiedOn,UANNO = @UANNO WHERE RegNo = 
                                                '" + employeeESIInformation?.employeeEsiPfDetails?.employeeNumber + "'";
                            command.Parameters.AddWithValue("@ESICNumber", employeeESIInformation?.employeeEsiPfDetails?.esiNo);
                            command.Parameters.AddWithValue("@ModifiedBy", employeeESIInformation?.employeeEsiPfDetails?.lastModifiedBy);
                            command.Parameters.AddWithValue("@ModifiedOn", DateTime.ParseExact(employeeESIInformation?.employeeEsiPfDetails?.lastModifiedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                            command.Parameters.AddWithValue("@UANNO", employeeESIInformation?.employeeEsiPfDetails?.uanNo);
                            command.CommandText = updateQuery;
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();

                            updateQuery = @"UPDATE EmployeeMaster SET ESICNo = @ESICNo, ModifiedDate=getdate() WHERE BranchEmpCode = '" + employeeESIInformation?.employeeEsiPfDetails?.employeeNumber + "'";
                            command.Parameters.AddWithValue("@ESICNo", employeeESIInformation?.employeeEsiPfDetails?.esiNo);
                            command.CommandText = updateQuery;
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();

                            lstMessage.Items.Add($"Data updated for {employeeESIInformation?.employeeEsiPfDetails?.employeeNumber}");

                        }
                        else
                        {
                            Utility.Logger.Error(employeeESIInformation?.employeeEsiPfDetails?.employeeNumber + " Does not Exists\n");
                            lstMessage.Items.Add(employeeESIInformation?.employeeEsiPfDetails?.employeeNumber + " Does not Exists");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
        }

        public static List<string> UpdateDataintoERP(BTEmployeeESIInformation employeeESIInformation)
        {
            var lstMessage = new List<string>();
            try
            {

                //Utility.Logger.Info($"Syncing the Employee Details {JsonConvert.SerializeObject(employeeESIInformation)}");
                Utility.Logger.Info($"Syncing {employeeESIInformation?.employeeEsiPfDetails?.employeeNumber} begins");
                SqlConnection connection = GetConnection();
                if (connection != null)
                {
                    if (IsEmployeeNumberExists(employeeESIInformation?.employeeEsiPfDetails?.employeeNumber))
                    {

                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;

                        string updateQuery = @"UPDATE CandidatePersonalDetails SET ESICNumber = @ESICNumber,ModifiedBy = @ModifiedBy,ModifiedOn = @ModifiedOn,UANNO = @UANNO WHERE RegNo = 
                                                '" + employeeESIInformation?.employeeEsiPfDetails?.employeeNumber + "'";
                        command.Parameters.AddWithValue("@ESICNumber", employeeESIInformation?.employeeEsiPfDetails?.esiNo);
                        command.Parameters.AddWithValue("@ModifiedBy", employeeESIInformation?.employeeEsiPfDetails?.lastModifiedBy);
                        command.Parameters.AddWithValue("@ModifiedOn", DateTime.ParseExact(employeeESIInformation?.employeeEsiPfDetails?.lastModifiedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));
                        command.Parameters.AddWithValue("@UANNO", employeeESIInformation?.employeeEsiPfDetails?.uanNo);
                        command.CommandText = updateQuery;
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        updateQuery = @"UPDATE EmployeeMaster SET ESICNo = @ESICNo, ModifiedDate=getdate() WHERE BranchEmpCode = '" + employeeESIInformation?.employeeEsiPfDetails?.employeeNumber + "'";
                        command.Parameters.AddWithValue("@ESICNo", employeeESIInformation?.employeeEsiPfDetails?.esiNo);
                        command.CommandText = updateQuery;
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        lstMessage.Add($"Data updated for {employeeESIInformation?.employeeEsiPfDetails?.employeeNumber}");

                    }
                    else
                    {
                        Utility.Logger.Error(employeeESIInformation?.employeeEsiPfDetails?.employeeNumber + " Does not Exists\n");
                        lstMessage.Add(employeeESIInformation?.employeeEsiPfDetails?.employeeNumber + " Does not Exists");
                    }
                }
                else
                {
                    throw new Exception("Unable to connect to the database for the further Sync process.");
                }
                Utility.Logger.Info($"Syncing {employeeESIInformation?.employeeEsiPfDetails?.employeeNumber} ends");

            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
            return lstMessage;
        }
        public static List<string> UpsertBlueTreeERPData(BTEmployeeInformation employeeInformation)
        {
            //Utility.Logger.Info($"Syncing the Employee Details {JsonConvert.SerializeObject(employeeInformation)}");
            Utility.Logger.Info($"Syncing {employeeInformation?.employeeBasicDetails?[0].EmployeeNumber} begins");
            try
            {
                return InsertDataintoERP(employeeInformation);
            }
            catch (Exception ex)
            {
                Utility.Logger.Error(ex);
            }
            finally
            {
                Utility.Logger.Info($"Syncing {employeeInformation?.employeeBasicDetails?[0].EmployeeNumber} ends");
            }
            return null;

        }
    }
}