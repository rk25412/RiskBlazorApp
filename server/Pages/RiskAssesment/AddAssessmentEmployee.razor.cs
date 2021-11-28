using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Clear.Risk.Models.ClearConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Clear.Risk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Logging.Abstractions;
using Novacode;
using System.Drawing;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Net;
using System.Net.Http;
using Geocoding;
using Geocoding.Google;
using Syncfusion.Blazor.Gantt;
using DocumentFormat.OpenXml.VariantTypes;
using Syncfusion.Blazor.Schedule;
using Clear.Risk.Pages.Lookup;
using Hangfire;
using Clear.Risk.Services;
using Clear.Risk.ViewModels;

namespace Clear.Risk.Pages.RiskAssesment
{
    public partial class AddAssessmentEmployee : ComponentBase
    {
        [Inject]
        protected IWebHostEnvironment _hosting { get; set; }
        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        [Parameter]
        public dynamic AssessmentId { get; set; }

        bool isLoading = false;

        protected Clear.Risk.Models.ClearConnection.AssesmentEmployee assessmentEmployee { get; set; }

        protected IList<Clear.Risk.Models.ClearConnection.Person> getEmployeeResult = new List<Clear.Risk.Models.ClearConnection.Person>();

        int SelectedEmployee = 0;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            StateHasChanged();
            await Load();
            isLoading = false;
            StateHasChanged();
        }



        async Task Load()
        {
            //assessmentEmployee = new Models.ClearConnection.AssesmentEmployee();
            var clearConnectionGetAssesmentEmployeesResult = await ClearConnection.GetAssesmentEmployees(new Query() { Filter = $@"i => i.ASSESMENT_ID == {Convert.ToInt32(AssessmentId)}" });

            var empIds = clearConnectionGetAssesmentEmployeesResult.Select(i => i.EMPLOYEE_ID).ToList();

            var employeeResult = await ClearConnection.GetEmployee(Security.getCompanyId(), new Query());



            getEmployeeResult = employeeResult.Where(i => !empIds.Contains(i.PERSON_ID)).Select(x => new Clear.Risk.Models.ClearConnection.Person
            {
                PERSON_ID = x.PERSON_ID,
                FIRST_NAME = x.FIRST_NAME,
                LAST_NAME = x.LAST_NAME
            }).ToList();


        }


        async Task Save0Click()
        {



            if (SelectedEmployee == 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "Please select an employee", 1500000);
                return;
            }

            var item = new Models.ClearConnection.AssesmentEmployee()
            {
                EMPLOYEE_ID = SelectedEmployee,
                WARNING_LEVEL_ID = 1,
                ASSESMENT_ID = Convert.ToInt32(AssessmentId),
            };

            assessmentEmployee = item;

            var newEmployee = await ClearConnection.CreateAssesmentEmployee(assessmentEmployee);

            StateHasChanged();

            var assesment = new Assesment();

            assesment = await ClearConnection.GetAssesmentByAssesmentid(Convert.ToInt32(AssessmentId));

            var getswmsresults = new Clear.Risk.Models.ClearConnection.SwmsTemplate();


            var template = await ClearConnection.GetTemplateByTradeId(assesment.TRADECATEGORYID);

            var swmsid = Convert.ToInt32(assesment.TEMPLATE_ID);

            getswmsresults = await ClearConnection.GetSwmsTemplateBySwmsid(swmsid);

            var result = getswmsresults;

            // var result = getswmsresults.Where(a => a.SWMSID == swmsid).FirstOrDefault();
            if (result != null)
            {
                if (assesment.AssesmentAttachements == null)
                    assesment.AssesmentAttachements = new List<AssesmentAttachement>();
                foreach (var temp in template.Templateattachments)
                {
                    assesment.AssesmentAttachements.Add(new AssesmentAttachement()
                    {
                        SWMS_TEMPLATE_ID = swmsid,
                        ATTACHEMENTDATE = DateTime.Now,
                        DOCUMENTTEMPLATEURL = temp.DOCUMENTURL
                    });
                }
            }


            await ReplaceTextInDocFileForPdf(assesment);

            StateHasChanged();

            if (newEmployee.ASSESMENT_EMPLOYEE_ID > 0)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Success", "Employee added successfully", 1500000);
                DialogService.Close(null);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "Cannot add new employee", 1500000);
                return;
            }
        }


        IEnumerable<Clear.Risk.Models.ClearConnection.Person> _getContractorResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> getContractorResult
        {
            get
            {
                return _getContractorResult;
            }
            set
            {
                if (!object.Equals(_getContractorResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getContractorResult", NewValue = value, OldValue = _getContractorResult };
                    _getContractorResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<PersonContact> _getPersonContactsResult;
        protected IEnumerable<PersonContact> getPersonContactsResult
        {
            get
            {
                return _getPersonContactsResult;
            }
            set
            {
                if (!object.Equals(_getPersonContactsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPersonContactsResult", NewValue = value, OldValue = _getPersonContactsResult };
                    _getPersonContactsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Person> _getPeopleResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> getPeopleResult
        {
            get
            {
                return _getPeopleResult;
            }
            set
            {
                if (!object.Equals(_getPeopleResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPeopleResult", NewValue = value, OldValue = _getPeopleResult };
                    _getPeopleResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected async Task<string> ReplaceTextInDocFileForPdf(Assesment refViewModel)
        {
            var assesment = new Assesment();

            assesment = await ClearConnection.GetAssesmentByAssesmentid(Convert.ToInt32(AssessmentId));

            var getswmsresults = new Clear.Risk.Models.ClearConnection.SwmsTemplate();


            var template = await ClearConnection.GetTemplateByTradeId(assesment.TRADECATEGORYID);

            var swmsid = Convert.ToInt32(assesment.TEMPLATE_ID);

            getswmsresults = await ClearConnection.GetSwmsTemplateBySwmsid(swmsid);

            var result = getswmsresults;
            if (refViewModel != null)
            {
                string filePath = _hosting.WebRootPath;

                foreach (var item in refViewModel.AssesmentAttachements)
                {
                    string fullPath = Path.Combine(filePath, (filePath + @"\Uploads\Templates\" + item.DOCUMENTTEMPLATEURL));
                    Models.ClearConnection.Person Compnay;

                    if (item.SwmsTemplate == null)
                        item.SwmsTemplate = getswmsresults;  //await ClearConnection.GetSwmsBySwmsid(item.SWMS_TEMPLATE_ID);
                    if (refViewModel.Company == null)
                    {
                        Compnay = await ClearConnection.GetPersonByPersonId(refViewModel.COMPANYID);
                    }
                    else
                        Compnay = refViewModel.Company;

                    if (refViewModel.Contractor == null)
                        refViewModel.Contractor = getContractorResult.Where(a => a.PERSON_ID == refViewModel.CONTRACTOR_ID).FirstOrDefault();

                    if (refViewModel.ContractorContact == null)
                        refViewModel.ContractorContact = getPersonContactsResult.Where(a => a.PERSON_CONTACT_ID == refViewModel.CONTRACTOR_CONTACT_ID).FirstOrDefault();

                    if (refViewModel.Client == null)
                        refViewModel.Client = getPeopleResult.Where(a => a.PERSON_ID == refViewModel.CLIENTID).FirstOrDefault();

                    if (refViewModel.ClientContact == null)
                        refViewModel.ClientContact = getPersonContactsResult.Where(a => a.PERSON_CONTACT_ID == refViewModel.CLIENT_CONTACT_ID).FirstOrDefault();


                    Novacode.DocX document = Novacode.DocX.Load(fullPath);

                    document.ReplaceText("&&Company_ComapnyName", Compnay != null ? Compnay.COMPANY_NAME != null ? Compnay.COMPANY_NAME : string.Empty : string.Empty);
                    document.ReplaceText("&&Contractor_FullName", Compnay != null ? Compnay.FullName : string.Empty);
                    document.ReplaceText("&&Company_ComapnyPhoneNo", Compnay != null ? Compnay.BUSINESS_PHONE != null ? Compnay.BUSINESS_PHONE : string.Empty : string.Empty);
                    document.ReplaceText("&&Assessment_DateCreated", DateTime.Now.ToString());
                    document.ReplaceText("&&Assessment_Version", item.SwmsTemplate != null ? item.SwmsTemplate.VERSION != null ? item.SwmsTemplate.VERSION : string.Empty : "");
                    //document.ReplaceText("&&Assessment_WorkOrder", model.WorkOrderNumber != null ? model.WorkOrderNumber : "");
                    document.ReplaceText("&&Date", DateTime.Now.ToString());
                    document.ReplaceText("&&Contractor_PhoneNo", string.Empty);
                    document.ReplaceText("&&Contractor_ComapnyName", string.Empty);
                    document.ReplaceText("&&Company_Name", string.Empty);
                    document.ReplaceText("&&Customer_Manager", Compnay != null ? Compnay.COMPANY_NAME != null ? Compnay.COMPANY_NAME : "" : string.Empty);
                    document.ReplaceText("&&Scope", refViewModel.SCOPEOFWORK != null ? refViewModel.SCOPEOFWORK : string.Empty);
                    document.ReplaceText("&&Assessment_ContractorSiteManager", refViewModel.CONTRACTORSITEMANAGER != null ? refViewModel.CONTRACTORSITEMANAGER : string.Empty);
                    document.ReplaceText("&&CustomerManager_PhoneNo", refViewModel.CONTRACTORSITEMNGRMNO != null ? refViewModel.CONTRACTORSITEMNGRMNO : string.Empty);
                    // document.ReplaceText("&&Logo", model.OrderCompany.CompanyLogoId != null ? model.OrderCompany.CompanyLogoId : "");
                    document.ReplaceText("&&Permit_Number", refViewModel.WORKORDERNUMBER != null ? refViewModel.WORKORDERNUMBER : string.Empty);


                    #region Assessment Text Replace
                    document.ReplaceText("&&AssessmentTask_AssignedDate", DateTime.Now.ToString());
                    document.ReplaceText("&&Assessment_AssessmentDate", DateTime.Now.ToString());
                    document.ReplaceText("&&Assessment_AssessmentID", (refViewModel.ASSESMENTID.ToString()));
                    document.ReplaceText("&&Assessment_ClientID", (refViewModel.CLIENTID != null ? refViewModel.CLIENTID.ToString() : string.Empty));
                    document.ReplaceText("&&Assessment_CompanyID", (refViewModel.COMPANYID.ToString()));
                    document.ReplaceText("&&Assessment_ContractorSiteMngrMNo", refViewModel.CONTRACTORSITEMNGRMNO != null ? refViewModel.CONTRACTORSITEMNGRMNO : string.Empty);
                    document.ReplaceText("&&Assessment_CreationTime", DateTime.Now.TimeOfDay.ToString());
                    //document.ReplaceText("&&Assessment_EmployeeID", (refViewModel.model.EmployeeID != null ? refViewModel.model.EmployeeID.ToString() : ""));
                    document.ReplaceText("&&Assessment_IsCompleted", refViewModel.ISCOMPLETED ? "Completed" : "Pending");
                    document.ReplaceText("&&Assessment_OrderLocation", refViewModel.ORDERLOCATION != null ? refViewModel.ORDERLOCATION : "");
                    document.ReplaceText("&&Assessment_PermitNumber", refViewModel.PURCHASEORDER != null ? refViewModel.PURCHASEORDER : "");
                    document.ReplaceText("&&Assessment_PlaceofWorkAddress", refViewModel.PersonSite != null ? refViewModel.PersonSite.SITE_NAME : "");
                    document.ReplaceText("&&Assessment_ProjectName", refViewModel.PROJECTNAME != null ? refViewModel.PROJECTNAME : "");
                    document.ReplaceText("&&Assessment_PurchaseOrder", refViewModel.PURCHASEORDER != null ? refViewModel.PURCHASEORDER : "");
                    document.ReplaceText("&&Assessment_ReferenceNumber", refViewModel.REFERENCENUMBER != null ? refViewModel.REFERENCENUMBER : "");
                    document.ReplaceText("&&Assessment_RiskAssessmentNo", refViewModel.RISKASSESSMENTNO != null ? refViewModel.RISKASSESSMENTNO : "");
                    document.ReplaceText("&&Assessment_ScopeOfWork", refViewModel.SCOPEOFWORK != null ? refViewModel.SCOPEOFWORK : "");
                    document.ReplaceText("&&Assessment_TemplateId", item.SWMS_TEMPLATE_ID.ToString());
                    document.ReplaceText("&&Company_Country", Compnay != null ? Compnay.Country1 != null ? Compnay.Country1.COUNTRYNAME : "" : "");
                    document.ReplaceText("&&Assessment_TypeofAssessment", refViewModel.TemplateType != null ? refViewModel.TemplateType.NAME : "");
                    document.ReplaceText("&&Assessment_WorkOrderNumber", refViewModel.WORKORDERNUMBER != null ? refViewModel.WORKORDERNUMBER : "");
                    document.ReplaceText("&&Assessment_PrincipalContractor", refViewModel.PRINCIPALCONTRACTOR != null ? refViewModel.PRINCIPALCONTRACTOR : "");
                    document.ReplaceText("&&Assessment_WorkingContractor", refViewModel.WORKINGCONTRACTOR != null ? refViewModel.WORKINGCONTRACTOR : "");
                    document.ReplaceText("&&Assessment_WorkStartDate", string.Format("{0:d}", refViewModel.WORKSTARTDATE));
                    document.ReplaceText("&&Assessment_WorkEndDate", string.Format("{0:d}", refViewModel.WORKENDDATE));
                    #endregion

                    document.ReplaceText("&&Company_Address1", Compnay != null ? Compnay.BUSINESS_ADDRESS1 != null ? Compnay.BUSINESS_ADDRESS1 : "" : "");
                    document.ReplaceText("&&Company_Address2", Compnay != null ? Compnay.BUSINESS_ADDRESS2 != null ? Compnay.BUSINESS_ADDRESS2 : "" : "");
                    document.ReplaceText("&&Company_City", Compnay != null ? Compnay.BUSINESS_CITY != null ? Compnay.BUSINESS_CITY : "" : "");
                    document.ReplaceText("&&Company_CompanyName", Compnay != null ? Compnay.COMPANY_NAME != null ? Compnay.COMPANY_NAME : "" : "");
                    document.ReplaceText("&&Company_CompanyNumber", Compnay != null ? Compnay.BUSINESS_PHONE != null ? Compnay.BUSINESS_PHONE : "" : "");
                    document.ReplaceText("&&Company_CompanyOfficeNumber", Compnay != null ? Compnay.BUSINESS_MOBILE != null ? Compnay.BUSINESS_MOBILE : "" : "");
                    document.ReplaceText("&&Company_Country", Compnay != null ? Compnay.Country1 != null ? Compnay.Country1.COUNTRYNAME : "" : "");


                    #region "Logo ReplaceMent"
                    try
                    {
                        var headerparagraphs = document.Headers.odd.Paragraphs.Where(x => x.Text.Contains("&&LOGO"));
                        if (headerparagraphs != null)
                        {
                            foreach (var paragraph in headerparagraphs)
                            {
                                paragraph.ReplaceText("&&LOGO", "");
                                #region Replace Logo
                                string logourl;
                                if (!string.IsNullOrEmpty(Compnay.UPLOAD_PROFILE))
                                {
                                    logourl = "Uploads/Company/Logo/" + Compnay.UPLOAD_PROFILE;
                                }
                                else
                                {
                                    logourl = "app_development/images/logo.png";
                                }
                                string newpath = filePath + "/" + logourl;

                                System.IO.FileInfo newlogofile = new System.IO.FileInfo(newpath);
                                if (newlogofile.Exists)
                                {
                                    Novacode.Image img = document.AddImage(newpath);
                                    // Insert a Paragraph into the default Header.
                                    Novacode.Header header_default = document.Headers.odd;
                                    Novacode.Picture pic1 = img.CreatePicture();
                                    pic1.Width = 100;
                                    pic1.Height = 50;
                                    header_default.Pictures.Add(pic1);

                                    Novacode.Paragraph p1 = header_default.InsertParagraph();
                                    //paragraph.Direction = Novacode.Direction.RightToLeft;
                                    paragraph.AppendPicture(pic1);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            return "headerparagraphs not found";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    #endregion

                    //Add Employee
                    int count = 0;
                    foreach (var assinedemployee in refViewModel.AssesmentEmployees)
                    {
                        if (assinedemployee.Employee == null)
                            assinedemployee.Employee = await ClearConnection.GetPersonByPersonId(assinedemployee.EMPLOYEE_ID);
                        count++;
                        var nameparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&Employee" + count.ToString() + "_Name"));
                        if (nameparagraphs != null)
                        {
                            document.ReplaceText("&&Employee" + count.ToString() + "_Name", assinedemployee.Employee.FullName);
                        }
                        var singparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&Emp" + count.ToString() + "_Signature"));
                        if (singparagraphs != null)
                        {
                            document.ReplaceText("&&Emp" + count.ToString() + "_Signature", "&&" + assinedemployee.Employee.FIRST_NAME.Trim() + "_" + assinedemployee.Employee.LAST_NAME.Trim() + "_" + assinedemployee.Employee.PERSON_ID.ToString().Trim());
                        }
                        var dateparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&Signature" + count.ToString() + "_Date"));
                        if (dateparagraphs != null)
                        {
                            document.ReplaceText("&&Signature" + count.ToString() + "_Date", "&&Sign" + assinedemployee.Employee.FIRST_NAME.Trim() + "_" + assinedemployee.Employee.LAST_NAME.Trim() + "_date" + "_" + assinedemployee.Employee.PERSON_ID.ToString().Trim());
                        }
                    }

                    //SWMS TEMPLATE STEP

                    //var SwmsTemplatesteps = await ClearConnection.GetSwmsTemplatesteps(new Query() { Filter = $@"i => i.SWMSID == {item.SWMS_TEMPLATE_ID}" });
                    if (item.SwmsTemplate.SwmsTemplatesteps != null)
                    {
                        IList<SwmsTemplatestep> Swmssteps = item.SwmsTemplate.SwmsTemplatesteps.ToList();
                        // item.SwmsTemplate.SwmsTemplatesteps = SwmsTemplatesteps.ToList();
                        //IList<SwmsTemplatestep> Swmssteps = item.SwmsTemplate.SwmsTemplatesteps.ToList();
                        var tableSetupparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&Steps-table"));
                        var t = document.AddTable(item.SwmsTemplate.SwmsTemplatesteps.Count + 2, 12);
                        //t.Design = TableDesign.LightGrid;

                        t.AutoFit = AutoFit.Contents;
                        t.Alignment = Alignment.center;
                        Novacode.Border b = new Novacode.Border(Novacode.BorderStyle.Tcbs_single, BorderSize.two, 0, Color.Black);
                        t.SetBorder(TableBorderType.Top, b);
                        t.SetBorder(TableBorderType.Bottom, b);
                        t.SetBorder(TableBorderType.Left, b);
                        t.SetBorder(TableBorderType.Right, b);
                        t.Rows[0].Cells[0].Paragraphs[0].Append("Item").FontSize(10);
                        t.Rows[0].Cells[0].Width = 10d;
                        t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[0].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[1].Paragraphs[0].Append("Task & or Category of Hazard(Delete & Add items that are / not relevant)").FontSize(10);
                        t.Rows[0].Cells[1].Width = 100d;
                        t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[1].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[1].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[2].Paragraphs[0].Append("What are the Specific Hazards?").FontSize(10);
                        t.Rows[0].Cells[2].Width = 100d;
                        t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[2].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[2].FillColor = Color.SlateGray;

                        //t.Rows[0].Cells[3].Paragraphs[0].Append("Impact:Health And Safety Environment Community Operations notification and approval must be obtained.Care taken on unsealed roads and property.").FontSize(10);
                        t.Rows[0].Cells[3].Paragraphs[0].Append("Area of Impact").FontSize(10);
                        t.Rows[0].Cells[3].Width = 100d;
                        t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[3].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[3].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[4].Paragraphs[0].Append("Risk").FontSize(10);
                        t.Rows[0].Cells[4].Width = 50d;
                        //t.Rows[0].Cells[4].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[4].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[4].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[4].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[4].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[5].Paragraphs[0].Append("Before").FontSize(10);
                        t.Rows[0].Cells[5].Width = 50d;
                        //t.Rows[0].Cells[5].SetBorder(TableCellBorderType.Right, b);
                        // t.Rows[0].Cells[5].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[5].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[5].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[5].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[6].Paragraphs[0].Append("Controls").FontSize(10);
                        t.Rows[0].Cells[6].Width = 50d;
                        t.Rows[0].Cells[6].SetBorder(TableCellBorderType.Right, b);
                        //t.Rows[0].Cells[6].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[6].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[6].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[6].FillColor = Color.SlateGray;

                        //t.Rows[0].Cells[7].Paragraphs[0].Append("Correct vehicles driven to site.Before any person enters site, whether it is to visit or to work,").FontSize(10);
                        t.Rows[0].Cells[7].Paragraphs[0].Append("Methods of Controlling Hazards").FontSize(10);

                        t.Rows[0].Cells[7].Width = 100d;
                        t.Rows[0].Cells[7].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[7].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[7].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[7].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[7].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[8].Paragraphs[0].Append("Risk").FontSize(10);
                        t.Rows[0].Cells[8].Width = 50d;
                        // t.Rows[0].Cells[8].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[8].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[8].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[8].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[8].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[9].Paragraphs[0].Append("After").FontSize(10);
                        t.Rows[0].Cells[9].Width = 50d;
                        // t.Rows[0].Cells[9].SetBorder(TableCellBorderType.Right, b);
                        // t.Rows[0].Cells[9].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[9].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[9].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[9].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[10].Paragraphs[0].Append("Controls").FontSize(10);
                        t.Rows[0].Cells[10].Width = 50d;
                        t.Rows[0].Cells[10].SetBorder(TableCellBorderType.Right, b);
                        // t.Rows[0].Cells[10].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[10].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[10].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[10].FillColor = Color.SlateGray;

                        t.Rows[0].Cells[11].Paragraphs[0].Append("Who is responsible").FontSize(10);
                        t.Rows[0].Cells[11].Width = 80d;
                        t.Rows[0].Cells[11].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[0].Cells[11].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[0].Cells[11].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[0].Cells[11].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[0].Cells[11].FillColor = Color.SlateGray;


                        t.Rows[1].Cells[0].Paragraphs[0].Append("").FontSize(10);
                        // t.Rows[1].Cells[0].Width = 20d;
                        t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[0].SetBorder(TableCellBorderType.Bottom, b);

                        t.Rows[1].Cells[1].Paragraphs[0].Append("").FontSize(10);
                        // t.Rows[1].Cells[1].Width = 100d;
                        t.Rows[1].Cells[1].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[1].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[1].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[1].SetBorder(TableCellBorderType.Bottom, b);

                        t.Rows[1].Cells[2].Paragraphs[0].Append("").FontSize(10);
                        //t.Rows[1].Cells[2].Width = 100d;
                        t.Rows[1].Cells[2].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[2].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[2].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[2].SetBorder(TableCellBorderType.Bottom, b);

                        t.Rows[1].Cells[3].Paragraphs[0].Append("").FontSize(10);
                        //t.Rows[1].Cells[3].Width = 100d;
                        t.Rows[1].Cells[3].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[3].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[3].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[3].SetBorder(TableCellBorderType.Bottom, b);

                        t.Rows[1].Cells[4].Paragraphs[0].Append("L").FontSize(10);
                        //  t.Rows[1].Cells[4].Width = 50d;
                        t.Rows[1].Cells[4].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[4].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[4].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[4].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[4].FillColor = Color.SlateGray;

                        t.Rows[1].Cells[5].Paragraphs[0].Append("C").FontSize(10);
                        // t.Rows[1].Cells[5].Width = 50d;
                        t.Rows[1].Cells[5].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[5].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[5].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[5].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[5].FillColor = Color.SlateGray;

                        t.Rows[1].Cells[6].Paragraphs[0].Append("S").FontSize(10);
                        // t.Rows[1].Cells[6].Width = 50d;
                        t.Rows[1].Cells[6].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[6].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[6].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[6].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[6].FillColor = Color.SlateGray;
                        t.Rows[1].Cells[7].Paragraphs[0].Append("").FontSize(10);
                        // t.Rows[1].Cells[7].Width = 100d;
                        t.Rows[1].Cells[7].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[7].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[7].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[7].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[8].Paragraphs[0].Append("L").FontSize(10);
                        // t.Rows[1].Cells[8].Width = 50d;
                        t.Rows[1].Cells[8].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[8].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[8].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[8].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[8].FillColor = Color.SlateGray;

                        t.Rows[1].Cells[9].Paragraphs[0].Append("C").FontSize(10);
                        // t.Rows[1].Cells[9].Width = 50d;
                        t.Rows[1].Cells[9].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[9].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[9].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[9].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[9].FillColor = Color.SlateGray;

                        t.Rows[1].Cells[10].Paragraphs[0].Append("S").FontSize(10);
                        // t.Rows[1].Cells[10].Width = 50d;
                        t.Rows[1].Cells[10].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[10].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[10].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[10].SetBorder(TableCellBorderType.Bottom, b);
                        t.Rows[1].Cells[10].FillColor = Color.SlateGray;
                        t.Rows[1].Cells[11].Paragraphs[0].Append("").FontSize(10);
                        // t.Rows[1].Cells[11].Width = 80d;
                        t.Rows[1].Cells[11].SetBorder(TableCellBorderType.Right, b);
                        t.Rows[1].Cells[11].SetBorder(TableCellBorderType.Left, b);
                        t.Rows[1].Cells[11].SetBorder(TableCellBorderType.Top, b);
                        t.Rows[1].Cells[11].SetBorder(TableCellBorderType.Bottom, b);

                        t.MergeCellsInColumn(0, 0, 1);
                        t.MergeCellsInColumn(1, 0, 1);
                        t.MergeCellsInColumn(2, 0, 1);
                        t.MergeCellsInColumn(3, 0, 1);
                        t.MergeCellsInColumn(7, 0, 1);
                        t.MergeCellsInColumn(11, 0, 1);

                        for (int i = 0; i < Swmssteps.Count; i++)
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                try
                                {
                                    int currentitem = i + 1;
                                    if (j == 0)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].STEPNO.ToString()).FontSize(11);
                                    if (j == 1)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].TASKCATEGEORY).FontSize(11);
                                    if (j == 2)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].HAZARD).FontSize(11);
                                    if (j == 3)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].HEALTHIMPACT != null ? Swmssteps[i].HEALTHIMPACT : string.Empty).FontSize(11);
                                    if (j == 4)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].RiskLikelyhood != null ? Swmssteps[i].RiskLikelyhood.NAME.ToString() : string.Empty).FontSize(11);
                                    if (j == 5)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].RISK_CONTRL_SCORE.ToString()).FontSize(11);
                                    if (j == 6)
                                    {
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].RiskLikelyhood != null ? Swmssteps[i].RiskLikelyhood.NAME.ToString() : string.Empty).FontSize(11);
                                        if (Swmssteps[i].RISK_LIKELYHOOD_ID == 1)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                        }
                                        else if (Swmssteps[i].RISK_LIKELYHOOD_ID == 2)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                        }
                                        else if (Swmssteps[i].RISK_LIKELYHOOD_ID == 3)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                        }
                                        else if (Swmssteps[i].RISK_LIKELYHOOD_ID == 4)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                        else if (Swmssteps[i].RISK_LIKELYHOOD_ID == 5)
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                        else
                                        {
                                            if (Swmssteps[i].RISK_CONTRL_SCORE == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_CONTRL_SCORE == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                    }

                                    if (j == 7)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].CONTROLLINGHAZARDS.ToString()).FontSize(11);
                                    if (j == 8)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].RiskLikelyhood1 != null ? Swmssteps[i].RiskLikelyhood1.NAME.ToString() : string.Empty).FontSize(11);
                                    if (j == 9)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].AFTER_RISK_CONTROL_SCORE.ToString()).FontSize(11);
                                    if (j == 10)
                                    {
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append(Swmssteps[i].AFTER_RISK_CONTROL_SCORE.ToString()).FontSize(11);
                                        if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 1)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                        }
                                        else if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 2)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                        }
                                        else if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 3)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                        }
                                        else if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 4)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                        else if (Swmssteps[i].AFTER_RISK_CONTROL_SCORE == 4)
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                        else
                                        {
                                            if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 1)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.YellowGreen;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 2)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Blue;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 3)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Yellow;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 4)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Orange;
                                            else if (Swmssteps[i].RISK_AFTER_LIKELYHOOD_ID == 5)
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                            else
                                                t.Rows[i + 2].Cells[j].FillColor = Color.Red;
                                        }
                                    }
                                    if (j == 11)
                                        t.Rows[i + 2].Cells[j].Paragraphs[0].Append((Swmssteps[i].ResposnsibleType != null ? Swmssteps[i].ResposnsibleType.NAME.ToString() : string.Empty)).FontSize(11);

                                    t.Rows[i + 2].Cells[j].SetBorder(TableCellBorderType.Right, b);
                                    t.Rows[i + 2].Cells[j].SetBorder(TableCellBorderType.Left, b);
                                    t.Rows[i + 2].Cells[j].SetBorder(TableCellBorderType.Top, b);
                                    t.Rows[i + 2].Cells[j].SetBorder(TableCellBorderType.Bottom, b);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }

                        t.SetWidths(new float[] { 40, 200, 150, 100, 40, 55, 65, 120, 40, 45, 65, 120 });
                        if (tableSetupparagraphs != null)
                        {
                            foreach (var pra in tableSetupparagraphs)
                            {
                                pra.ReplaceText("&&Steps-table", "Steps-table Section");
                                pra.SpacingAfter(20d);
                                pra.InsertTableAfterSelf(t);
                            }
                        }
                        else
                        {
                            return "tableSetupparagraphs Not Found";
                        }
                    }

                    //PPE Section

                    try
                    {
                        var ppeparagraphs = document.Paragraphs.Where(x => x.Text.Contains("&&PPESection"));
                        if (ppeparagraphs != null)
                        {
                            var presults = await ClearConnection.GetSwmsPperequireds(new Query() { Filter = $@"i => i.SWMSID == {item.SWMS_TEMPLATE_ID}" });
                            IList<SwmsPperequired> SWMSPPERequired = presults.ToList();

                            // item.SwmsTemplate.SwmsPperequireds.ToList();
                            if (SWMSPPERequired != null)
                            {
                                Novacode.Border b = new Novacode.Border(Novacode.BorderStyle.Tcbs_single, BorderSize.two, 0, Color.Black);
                                var ppe = document.AddTable(SWMSPPERequired.Count + 1, 4);
                                ppe.Alignment = Alignment.center;
                                ppe.SetBorder(TableBorderType.Top, b);
                                ppe.SetBorder(TableBorderType.Bottom, b);
                                ppe.SetBorder(TableBorderType.Left, b);
                                ppe.SetBorder(TableBorderType.Right, b);
                                ppe.Rows[0].Cells[0].Paragraphs[0].Append("PPE Required").FontSize(10);
                                //ppe.Rows[0].Cells[0].Width = 350d;
                                //ppe.Rows[0].Cells[0].SetBorder(TableCellBorderType.Right, b);
                                ppe.Rows[0].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                ppe.Rows[0].Cells[0].SetBorder(TableCellBorderType.Top, b);
                                ppe.Rows[0].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                                ppe.Rows[0].Cells[0].FillColor = Color.SlateGray;
                                ppe.Rows[0].Cells[1].Paragraphs[0].Append("").FontSize(11);
                                // ppe.Rows[0].Cells[1].Width = 50d;
                                ppe.Rows[0].Cells[1].SetBorder(TableCellBorderType.Right, b);
                                //ppe.Rows[0].Cells[1].SetBorder(TableCellBorderType.Left, b);
                                ppe.Rows[0].Cells[1].SetBorder(TableCellBorderType.Top, b);
                                ppe.Rows[0].Cells[1].SetBorder(TableCellBorderType.Bottom, b);
                                ppe.Rows[0].Cells[1].FillColor = Color.SlateGray;
                                ppe.Rows[0].Cells[2].Paragraphs[0].Append("PPE Required").FontSize(10);
                                ppe.Rows[0].Cells[2].Width = 350d;
                                //ppe.Rows[0].Cells[2].SetBorder(TableCellBorderType.Right, b);
                                ppe.Rows[0].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                ppe.Rows[0].Cells[2].SetBorder(TableCellBorderType.Top, b);
                                ppe.Rows[0].Cells[2].SetBorder(TableCellBorderType.Bottom, b);
                                ppe.Rows[0].Cells[2].FillColor = Color.SlateGray;
                                ppe.Rows[0].Cells[3].Paragraphs[0].Append("").FontSize(11);
                                // ppe.Rows[0].Cells[3].Width = 50d;
                                ppe.Rows[0].Cells[3].SetBorder(TableCellBorderType.Right, b);
                                //ppe.Rows[0].Cells[3].SetBorder(TableCellBorderType.Left, b);
                                ppe.Rows[0].Cells[3].SetBorder(TableCellBorderType.Top, b);
                                ppe.Rows[0].Cells[3].SetBorder(TableCellBorderType.Bottom, b);
                                ppe.Rows[0].Cells[3].FillColor = Color.SlateGray;

                                var totalcoun = SWMSPPERequired.Count;
                                int k = 1;
                                for (int j = 0; j < SWMSPPERequired.Count;)
                                {
                                    var modulus = SWMSPPERequired.Count % 2;
                                    if (modulus == 0)
                                    {
                                        ppe.Rows[k].Cells[0].Paragraphs[0].Append(SWMSPPERequired[j].Ppevalue.KEY_DISPLAY).FontSize(10);
                                        //ppe.Rows[k].Cells[0].Width = 350d;
                                        ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Right, b);
                                        ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                                        //string pic1url = string.Empty;
                                        // var SWMSPPEImageIcon = await this.Storage.GetRepository<IPPEValueRepository>().GetByName(SWMSPPERequired[j].PPERequiredName);

                                        string newpath = Path.Combine(filePath, filePath + "/PPEImageIcon/" + SWMSPPERequired[j].Ppevalue.ICONPATH);
                                        System.IO.FileInfo newlogofile = new System.IO.FileInfo(newpath);
                                        if (newlogofile.Exists)
                                        {
                                            Novacode.Image img = document.AddImage(newpath);
                                            Novacode.Picture pic1 = img.CreatePicture();
                                            ppe.Rows[k].Cells[1].Paragraphs[0].AppendPicture(pic1).FontSize(11);
                                            // ppe.Rows[k].Cells[1].Width = 50d;
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Top, b);
                                            ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Bottom, b);
                                        }
                                        ppe.Rows[k].Cells[2].Paragraphs[0].Append(SWMSPPERequired[j + 1].Ppevalue.KEY_DISPLAY).FontSize(10);
                                        // ppe.Rows[k].Cells[2].Width = 350d;
                                        ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Right, b);
                                        ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                        ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Bottom, b);

                                        string pic1ur2 = string.Empty;

                                        string newpath2 = Path.Combine(filePath, filePath + "/PPEImageIcon/" + SWMSPPERequired[j + 1].Ppevalue.ICONPATH);
                                        System.IO.FileInfo newlogofile2 = new System.IO.FileInfo(newpath2);

                                        if (newlogofile2.Exists)
                                        {
                                            Novacode.Image img2 = document.AddImage(newpath2);
                                            Novacode.Picture pic2 = img2.CreatePicture();

                                            ppe.Rows[k].Cells[3].Paragraphs[0].AppendPicture(pic2).FontSize(11);
                                            // ppe.Rows[k].Cells[3].Width = 50d;
                                            ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Top, b);
                                            ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Bottom, b);
                                        }
                                    }
                                    else
                                    {
                                        if (totalcoun == 1)
                                        {
                                            ppe.Rows[k].Cells[0].Paragraphs[0].Append(SWMSPPERequired[j].Ppevalue.KEY_DISPLAY).FontSize(11);
                                            // ppe.Rows[k].Cells[0].Width = 350d;
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                                            string pic1url = string.Empty;

                                            string newpath = Path.Combine(filePath, filePath + "/PPEImageIcon/" + SWMSPPERequired[j].Ppevalue.ICONPATH);
                                            System.IO.FileInfo newlogofile = new System.IO.FileInfo(newpath);
                                            if (newlogofile.Exists)
                                            {
                                                Novacode.Image img = document.AddImage(newpath);
                                                Novacode.Picture pic1 = img.CreatePicture();
                                                ppe.Rows[k].Cells[1].Paragraphs[0].AppendPicture(pic1).FontSize(10);
                                                // ppe.Rows[k].Cells[1].Width = 50d;
                                                ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Right, b);
                                                ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Left, b);
                                                ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Top, b);
                                                ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Bottom, b);
                                            }
                                        }
                                        else
                                        {
                                            ppe.Rows[k].Cells[0].Paragraphs[0].Append(SWMSPPERequired[j].Ppevalue.KEY_DISPLAY).FontSize(11);

                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[0].SetBorder(TableCellBorderType.Bottom, b);
                                            string pic1url = string.Empty;

                                            string newpath = Path.Combine(filePath, filePath + "/PPEImageIcon/" + SWMSPPERequired[j].Ppevalue.ICONPATH);
                                            System.IO.FileInfo newlogofile = new System.IO.FileInfo(newpath);
                                            if (newlogofile.Exists)
                                            {
                                                Novacode.Image img = document.AddImage(newpath);
                                                Novacode.Picture pic1 = img.CreatePicture();
                                                ppe.Rows[k].Cells[1].Paragraphs[0].AppendPicture(pic1).FontSize(10);
                                                // ppe.Rows[k].Cells[1].Width = 50d;
                                                ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Right, b);
                                                ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Left, b);
                                                ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Top, b);
                                                ppe.Rows[k].Cells[1].SetBorder(TableCellBorderType.Bottom, b);
                                            }

                                            ppe.Rows[k].Cells[2].Paragraphs[0].Append(SWMSPPERequired[k].Ppevalue.KEY_DISPLAY).FontSize(11);
                                            //ppe.Rows[k].Cells[2].Width = 350d;
                                            ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Right, b);
                                            ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Left, b);
                                            ppe.Rows[k].Cells[2].SetBorder(TableCellBorderType.Bottom, b);

                                            string pic1ur2 = string.Empty;

                                            string newpath2 = Path.Combine(filePath, filePath + "/PPEImageIcon/" + SWMSPPERequired[j + 1].Ppevalue.ICONPATH);
                                            System.IO.FileInfo newlogofile2 = new System.IO.FileInfo(newpath2);
                                            if (newlogofile2.Exists)
                                            {
                                                Novacode.Image img2 = document.AddImage(newpath2);
                                                Novacode.Picture pic2 = img2.CreatePicture();

                                                ppe.Rows[k].Cells[3].Paragraphs[0].AppendPicture(pic2).FontSize(10);
                                                // ppe.Rows[k].Cells[3].Width = 50d;
                                                ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Right, b);
                                                ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Left, b);
                                                ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Top, b);
                                                ppe.Rows[k].Cells[3].SetBorder(TableCellBorderType.Bottom, b);
                                            }
                                        }
                                    }
                                    j = j + 2;
                                    totalcoun = totalcoun - 2;
                                    k++;
                                }
                                ppe.SetWidths(new float[] { 427, 100, 428, 100 });
                                foreach (var prappe in ppeparagraphs)
                                {
                                    prappe.ReplaceText("&&PPESection", "PPE SECTION");
                                    prappe.SpacingAfter(20d);
                                    prappe.InsertTableAfterSelf(ppe);
                                }
                            }
                        }
                        else
                        {
                            return "ppeparagraphs not found";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;

                    }

                    //Plant Euipment

                    try
                    {
                        var plantequipment = document.Paragraphs.Where(x => x.Text.Contains("&&Plant&Equipment"));
                        if (plantequipment != null)
                        {
                            string plantandreq = string.Empty;

                            if (item.SwmsTemplate.SwmsPlantequipments == null)
                            {
                                var presults = await ClearConnection.GetSwmsPlantequipments(new Query() { Filter = $@"i => i.SWMSID == {item.SWMS_TEMPLATE_ID}" });
                                item.SwmsTemplate.SwmsPlantequipments = presults.ToList();
                            }

                            if (item.SwmsTemplate.SwmsPlantequipments != null)
                            {
                                foreach (var plnt in item.SwmsTemplate.SwmsPlantequipments)
                                {
                                    if (plnt.PlantEquipment != null)
                                        plantandreq += plnt.PlantEquipment.NAME + ",";
                                }
                                foreach (var pra in plantequipment)
                                {
                                    pra.ReplaceText("&&Plant&Equipment", plantandreq);
                                }
                            }
                        }
                        else
                        {
                            return "plantequipment is Null";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                    try
                    {
                        var hazardmaterialspara = document.Paragraphs.Where(x => x.Text.Contains("&&HazardMaterials"));
                        if (hazardmaterialspara != null)
                        {
                            string _hazardousmaterial = string.Empty;

                            if (item.SwmsTemplate.SwmsHazardousmaterials == null)
                            {
                                var presults = await ClearConnection.GetSwmsHazardousmaterials(new Query() { Filter = $@"i => i.SWMSID == {item.SWMS_TEMPLATE_ID}" });
                                item.SwmsTemplate.SwmsHazardousmaterials = presults.ToList();
                            }

                            if (item.SwmsTemplate.SwmsHazardousmaterials != null)
                            {
                                foreach (var plnt in item.SwmsTemplate.SwmsHazardousmaterials)
                                {
                                    if (plnt.HazardMaterialValue != null)
                                        _hazardousmaterial += plnt.HazardMaterialValue.NAME + ",";
                                }
                            }
                            else
                            {
                                return "HazardousMaterials is Null";
                            }

                            foreach (var mpra in hazardmaterialspara)
                            {
                                mpra.ReplaceText("&&HazardMaterials", _hazardousmaterial);
                            }
                        }
                        else
                        {
                            return "hazardmaterialspara is Null";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                    try
                    {
                        var referencedlegislationpara = document.Paragraphs.Where(x => x.Text.Contains("&&ReferencedLegislation"));
                        if (referencedlegislationpara != null)
                        {
                            string _referencedlegislation = string.Empty;
                            if (item.SwmsTemplate.SwmsReferencedlegislations == null)
                            {
                                var presults = await ClearConnection.GetSwmsReferencedlegislations(new Query() { Filter = $@"i => i.SWMSID == {item.SWMS_TEMPLATE_ID}" });
                                item.SwmsTemplate.SwmsReferencedlegislations = presults.ToList();
                            }

                            if (item.SwmsTemplate.SwmsReferencedlegislations != null)
                            {
                                foreach (var plnt in item.SwmsTemplate.SwmsReferencedlegislations)
                                {
                                    if (plnt.ReferencedLegislation != null)
                                        _referencedlegislation += plnt.ReferencedLegislation.NAME + ",";
                                }
                            }
                            else
                            {
                                return "SwmsReferencedLegislations not found";
                            }

                            foreach (var rpra in referencedlegislationpara)
                            {
                                rpra.ReplaceText("&&ReferencedLegislation", _referencedlegislation);
                            }
                        }
                        else
                        {
                            return "licensesandpermitspara not found";

                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;

                    }

                    try
                    {
                        var licensesandpermitspara = document.Paragraphs.Where(x => x.Text.Contains("&&LicencedPermitsSection"));
                        if (licensesandpermitspara != null)
                        {
                            string _licensesandpermits = string.Empty;

                            if (item.SwmsTemplate.SwmsLicencespermits == null)
                            {
                                var presults = await ClearConnection.GetSwmsLicencespermits(new Query() { Filter = $@"i => i.SWMSID == {item.SWMS_TEMPLATE_ID}" });
                                item.SwmsTemplate.SwmsLicencespermits = presults.ToList();
                            }

                            if (item.SwmsTemplate.SwmsLicencespermits != null)
                            {
                                foreach (var plnt in item.SwmsTemplate.SwmsLicencespermits)
                                {
                                    if (plnt.LicencePermit != null)
                                        _licensesandpermits += plnt.LicencePermit.NAME + ",";
                                }
                            }
                            else
                            {
                                return "SwmsLicencesPermit not found";
                            }

                            foreach (var pra in licensesandpermitspara)
                            {
                                pra.ReplaceText("&&LicencedPermitsSection", _licensesandpermits);
                            }
                        }
                        else
                        {
                            return "licensesandpermitspara not found";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }


                    try
                    {

                        string docPath = Path.Combine(filePath, filePath + "/Uploads/Templates/Doc") + $@"\{item.SwmsTemplate.TEMPLATENAME + "-" + item.DOCUMENTTEMPLATEURL}";
                        document.SaveAs(docPath);
                        item.DOCUMENTTEMPLATEURL = item.SwmsTemplate.TEMPLATENAME + "-" + item.DOCUMENTTEMPLATEURL;
                        System.Random rand = new System.Random((int)System.DateTime.Now.Ticks);
                        int randompdf = rand.Next(1, 100000000);
                        string PdfName = refViewModel.RISKASSESSMENTNO + "-" + randompdf + ".pdf";
                        string pdfPath = Path.Combine(filePath, filePath + "/Uploads/Templates/Pdf") + $@"\{refViewModel.RISKASSESSMENTNO + "-" + randompdf + ".pdf"}";
                        bool isConverted = ConverttoPdf(docPath, pdfPath);

                        if (isConverted)
                        {
                            item.DOCUMENTPDFURL = PdfName;
                            int rowNo = 0;
                            foreach (var employee in refViewModel.AssesmentEmployees)
                            {
                                if (rowNo == 0)
                                    rowNo = 1;
                                else
                                    rowNo++;

                                AssesmentEmployeeAttachement model = new AssesmentEmployeeAttachement();
                                model.ASSESMENT_EMPLOYEE_ID = employee.ASSESMENT_EMPLOYEE_ID;
                                model.ATTACHEMENTID = item.ATTACHEMENTID;
                                model.DOCUMENTNAME = PdfName;
                                model.DOCUMENT_URL = "Uploads/Templates/Pdf";
                                model.ASSIGNED_DATE = DateTime.Now;
                                model.EMPLOYEE_STATUS = 1;
                                model.SRNo = rowNo;
                                model.WARNING_LEVEL_ID = 1;

                                if (employee.Employee != null)
                                    employee.Employee = null;

                                //await ClearConnection.CreateAssesmentEmployeeAttachement(model);

                                if (employee.AssignedEmployees == null)
                                    employee.AssignedEmployees = new List<AssesmentEmployeeAttachement>();

                                if (item.Attachments == null)
                                    item.Attachments = new List<AssesmentEmployeeAttachement>();

                                employee.AssignedEmployees.Add(model);
                                item.Attachments.Add(model);
                            }
                        }
                        else
                        {
                            return "Document conversion failed";
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            return string.Empty;
        }

        protected Boolean ConverttoPdf(string input, string output)
        {
            try
            {
                using (FileStream docStream = new FileStream(input, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    WordDocument document = new WordDocument(docStream, Syncfusion.DocIO.FormatType.Automatic);

                    DocIORenderer render = new DocIORenderer();
                    PdfDocument pdfDocument = render.ConvertToPDF(document);

                    FileStream docStream1 = new FileStream(output, FileMode.OpenOrCreate);
                    MemoryStream outputStream = new MemoryStream();

                    pdfDocument.Save(docStream1);
                    pdfDocument.Close();
                    render.Dispose();
                    document.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // return false;


        }



        async Task CancelButtonClick()
        {
            StateHasChanged();
            DialogService.Close(null);
        }
    }
}
