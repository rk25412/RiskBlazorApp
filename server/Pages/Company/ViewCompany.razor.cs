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
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace Clear.Risk.Pages.Company
{
    public partial class ViewCompany : ComponentBase
    {
        protected DateTime? value = DateTime.Now;

        IEnumerable<DateTime> dates = new DateTime[] { DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1) };

        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();
        void Change(DateTime? value, string name, string format)
        {
            events.Add(DateTime.Now, $"{name} value changed to {value?.ToString(format)}");
            StateHasChanged();
        }

        [Inject]
        protected UserManager<ApplicationUser> userManager { get; set; }
        [Inject]
        protected RoleManager<IdentityRole> roleManager { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected Clear.Risk.Models.ClearConnection.Person _person;

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }
        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }

        [Parameter]
        public dynamic PersonId { get; set; }
        protected Clear.Risk.Models.ClearConnection.Person person
        {
            get
            {
                return _person;
            }
            set
            {
                if (!object.Equals(_person, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "person", NewValue = value, OldValue = _person };
                    _person = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected CompanyStats _CompanyStats;
        protected CompanyStats GetCompanyStats
        {
            get
            {
                return _CompanyStats;
            }
            set
            {
                if (!object.Equals(_CompanyStats, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "GetCompanyStats", NewValue = value, OldValue = _CompanyStats };
                    _CompanyStats = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected IEnumerable<MonthlyWorkOrder> _monthlyWorkOrder;

        protected IEnumerable<MonthlyWorkOrder> GetMonthlyWorkOrders
        {
            get
            {
                return _monthlyWorkOrder;
            }
            set
            {
                if (!object.Equals(_monthlyWorkOrder, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "GetMonthlyWorkOrders", NewValue = value, OldValue = _monthlyWorkOrder };
                    _monthlyWorkOrder = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected IEnumerable<MonthlyAssesments> _monthlyAssessment;

        protected IEnumerable<MonthlyAssesments> GetMonthlyAssessments
        {
            get
            {
                return _monthlyAssessment;
            }
            set
            {
                if (!object.Equals(_monthlyAssessment, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "GetMonthlyAssessments", NewValue = value, OldValue = _monthlyAssessment };
                    _monthlyAssessment = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.Desigation> _getDesigation;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Desigation> getDesigationResult
        {
            get
            {
                return _getDesigation;
            }
            set
            {
                if (!object.Equals(_getDesigation, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getDesigationResult", NewValue = value, OldValue = _getDesigation };
                    _getDesigation = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected IList<CompanyAccountTransaction> getCompanyAccountTransactionsResult = new List<CompanyAccountTransaction>();
        

        protected void Change(object value, string name)
        {

            StateHasChanged();
        }

        protected void SameAsPersonal(bool value, string name)
        {
            if (value && person != null)
            {
                person.BUSINESS_ADDRESS1 = person.PERSONALADDRESS1;
                person.BUSINESS_ADDRESS2 = person.PERSONALADDRESS2;
                person.BUSINESS_CITY = person.PERSONAL_CITY;
                person.BUSINESS_COUNTRY_ID = person.PERSONAL_COUNTRY_ID;
                person.BUSINESS_STATE_ID = person.BUSINESS_STATE_ID;
                person.BUSINESS_EMAIL = person.PERSONAL_EMAIL;
                person.BUSINESS_MOBILE = person.PERSONAL_MOBILE;
                person.BUSINESS_PHONE = person.PERSONAL_PHONE;
                person.BUSINESS_POSTCODE = person.PERSONAL_POSTCODE;
                person.BUSINESS_FAX = person.PERSONAL_FAX;
                person.BUSINESS_WEB_ADD = person.PERSONAL_WEB_ADD;
            }
            StateHasChanged();
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                //string uri = UriHelper.Uri;
                //string baseuri = UriHelper.BaseUri;
                await Load();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetPeopleResult = await ClearConnection.GetPeople();
            getPeopleResult = clearConnectionGetPeopleResult;

            var clearConnectionGetStatesResult = await ClearConnection.GetStates();
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries();
            getCountriesResult = clearConnectionGetCountriesResult;

            var clearConnectionGetPersonTypesResult = await ClearConnection.GetPersonTypes();
            getPersonTypesResult = clearConnectionGetPersonTypesResult;

            var clearConnectionGetApplicencesResult = await ClearConnection.GetApplicences();
            getApplicencesResult = clearConnectionGetApplicencesResult;

            var clearConnectionGetDesigationsResult = await ClearConnection.GetDesigations();
            getDesigationResult = clearConnectionGetDesigationsResult;

            person = await ClearConnection.GetPersonByPersonId(int.Parse(PersonId));

            var clearRiskGetPersonSitesResult = await ClearConnection.GetPersonSites(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonSitesResult = clearRiskGetPersonSitesResult;

            var clearConnectionGetAssesmentsResult = await ClearConnection.GetAssesments(new Query() { Filter = $@"i => i.CLIENTID == {PersonId}", Expand = "StatusMaster,TradeCategory,PersonSite,IndustryType,Person,Person1" });
            getAssesmentsResult = clearConnectionGetAssesmentsResult;

            var clearRiskGetPersonContactsResult = await ClearConnection.GetPersonContacts(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            getPersonContactsResult = clearRiskGetPersonContactsResult;

            //var clearRiskGetCompanyTransactionsResult = await ClearConnection.GetCompanyTransactions(new Query() { Filter = $@"i => i.PERSON_ID == {PersonId}" });
            //getCompanyTransactionsResult = clearRiskGetCompanyTransactionsResult;

            if (Security.IsInRole("System Administrator"))
            {
                var clearRiskGetCompanyTransactionsResult = await ClearRisk.GetCompanyTransactions();
                getCompanyTransactionsResult = clearRiskGetCompanyTransactionsResult;
            }
            else
            {
                var clearRiskGetCompanyTransactionsResult = await ClearRisk.GetCompanyTransactions(new Query() { Filter = $@"i => i.PERSON_ID == {Security.getCompanyId()}  " });
                getCompanyTransactionsResult = clearRiskGetCompanyTransactionsResult;
            }



            //Stats = ClearConnection.GetAccountStats();
            //RevenueCompany = ClearConnection.GetRevenueByCompany();

            //MonthOrder = ClearConnection.MonthyWorkOrderStatus();
            ////SurveyDone = ClearConnection.MonthySurveyConductStats();
            ////SurveyMonthly = ClearConnection.MonthySurveyStats();

            //Statistics
            if (Security.IsInRole("System Administrator"))
            {
                Stats = ClearConnection.GetAccountStats();
                RevenueCompany = ClearConnection.GetRevenueByCompany();

                MonthOrder = ClearConnection.MonthyWorkOrderStatus();
                //SurveyDone = ClearConnection.MonthySurveyConductStats();
                //SurveyMonthly = ClearConnection.MonthySurveyStats();
            }
            else if (Security.IsInRole("Administrator"))
            {
                GetCompanyStats = ClearConnection.GetAccountStats(Security.getCompanyId());
                GetMonthlyWorkOrders = ClearConnection.GetCompanyOrderStatus(Security.getCompanyId());
                GetMonthlyAssessments = ClearConnection.GetCompanyAssessmentStatus(Security.getCompanyId());
            }

            //Account Information Tab
            if (Security.IsInRole("System Administrator"))
            {
                var clearRiskGetCompanyAccountTransactionsResult = await ClearRisk.GetCompanyAccountTransactions(new Query());
                decimal intialBalance = 0;
                foreach (var item in clearRiskGetCompanyAccountTransactionsResult)
                {
                    intialBalance = intialBalance + (item.DEPOSITE_AMOUNT - item.PAYMENT_AMOUNT);
                    item.Balance = intialBalance;
                }
                getCompanyAccountTransactionsResult = clearRiskGetCompanyAccountTransactionsResult.Select(x => new Clear.Risk.Models.ClearConnection.CompanyAccountTransaction
                {
                    Person = x.Person,
                    TRANSACTION_DATE = x.TRANSACTION_DATE,
                    PAYMENT_AMOUNT = x.PAYMENT_AMOUNT,
                    DEPOSITE_AMOUNT = x.DEPOSITE_AMOUNT,
                    Balance = x.Balance,
                    DESCRIPTION = x.DESCRIPTION,
                    TRXTYPE = x.TRXTYPE,
                    Currency = x.Currency,
                    COMPANY_TRANSACTION_ID = x.COMPANY_TRANSACTION_ID
                }).ToList();
            }
            else
            {
                var clearRiskGetCompanyAccountTransactionsResult = await ClearRisk.GetCompanyAccountTransactions(new Query() { Filter = $@"i => i.COMPANY_ID == {Security.getCompanyId()}  " });
                decimal intialBalance = 0;
                foreach (var item in clearRiskGetCompanyAccountTransactionsResult)
                {
                    intialBalance = intialBalance + (item.DEPOSITE_AMOUNT - item.PAYMENT_AMOUNT);
                    item.Balance = intialBalance;
                }
                getCompanyAccountTransactionsResult = clearRiskGetCompanyAccountTransactionsResult.Select(x => new Clear.Risk.Models.ClearConnection.CompanyAccountTransaction
                {
                    Person = x.Person,
                    TRANSACTION_DATE = x.TRANSACTION_DATE,
                    PAYMENT_AMOUNT = x.PAYMENT_AMOUNT,
                    DEPOSITE_AMOUNT = x.DEPOSITE_AMOUNT,
                    Balance = x.Balance,
                    DESCRIPTION = x.DESCRIPTION,
                    TRXTYPE = x.TRXTYPE,
                    Currency = x.Currency,
                    COMPANY_TRANSACTION_ID = x.COMPANY_TRANSACTION_ID
                }).ToList();
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


        IEnumerable<Clear.Risk.Models.ClearConnection.State> _getStatesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.State> getStatesResult
        {
            get
            {
                return _getStatesResult;
            }
            set
            {
                if (!object.Equals(_getStatesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getStatesResult", NewValue = value, OldValue = _getStatesResult };
                    _getStatesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Country> _getCountriesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Country> getCountriesResult
        {
            get
            {
                return _getCountriesResult;
            }
            set
            {
                if (!object.Equals(_getCountriesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCountriesResult", NewValue = value, OldValue = _getCountriesResult };
                    _getCountriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.PersonType> _getPersonTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.PersonType> getPersonTypesResult
        {
            get
            {
                return _getPersonTypesResult;
            }
            set
            {
                if (!object.Equals(_getPersonTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPersonTypesResult", NewValue = value, OldValue = _getPersonTypesResult };
                    _getPersonTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Applicence> _getApplicencesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Applicence> getApplicencesResult
        {
            get
            {
                return _getApplicencesResult;
            }
            set
            {
                if (!object.Equals(_getApplicencesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getApplicencesResult", NewValue = value, OldValue = _getApplicencesResult };
                    _getApplicencesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<ChangePassword>("Change Password", null, new DialogOptions() { Width = $"{700}px" });
            await InvokeAsync(() => { StateHasChanged(); });
        }



        protected async System.Threading.Tasks.Task BtnAddCredit(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCredit>("Add Credit", new Dictionary<string, object>() { { "PersonId", int.Parse(PersonId) } });
            await InvokeAsync(() => { StateHasChanged(); });

            person = await ClearConnection.GetPersonByPersonId(int.Parse(PersonId));

            Reload();

        }


        protected async System.Threading.Tasks.Task ButtonMakePayment(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<NewPayment>("New Payment", null);

            await InvokeAsync(() => { StateHasChanged(); });

            person = await ClearConnection.GetPersonByPersonId(int.Parse(PersonId));

            if (Security.IsInRole("Administrator"))
                GetCompanyStats = ClearConnection.GetAccountStats(Security.getCompanyId());
        }
        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            try
            {


                var securityCreateUserResult = await ClearConnection.UpdatePerson(args.PERSON_ID, args);
                NotificationService.Notify(NotificationSeverity.Info, $"Success", $"Client Information update successfully!, ");


            }
            catch (System.Exception clearConnectionCreatePersonException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Client!, " + clearConnectionCreatePersonException.Message);
            }


        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("Clients");
        }

        protected string HashPassword(string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Encoding.ASCII.GetString(hash);
            }
        }

        #region Client Work Site
        protected RadzenGrid<PersonSite> grid0;

        protected RadzenGrid<CompanyTransaction> grid3;

        IEnumerable<PersonSite> _getPersonSitesResult;
        protected IEnumerable<PersonSite> getPersonSitesResult
        {
            get
            {
                return _getPersonSitesResult;
            }
            set
            {
                if (!object.Equals(_getPersonSitesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPersonSitesResult", NewValue = value, OldValue = _getPersonSitesResult };
                    _getPersonSitesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeletePersonSiteResult = await ClearConnection.DeletePersonSite(data.PERSON_SITE_ID);
                    if (clearRiskDeletePersonSiteResult != null)
                    {
                        grid0.Reload();
                    }
                }
            }
            catch (System.Exception clearRiskDeletePersonSiteException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete PersonSite");
            }
        }
        #endregion

        #region Assesment History
        protected RadzenGrid<Clear.Risk.Models.ClearConnection.Assesment> grid1;

        IEnumerable<Clear.Risk.Models.ClearConnection.Assesment> _getAssesmentsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Assesment> getAssesmentsResult
        {
            get
            {
                return _getAssesmentsResult;
            }
            set
            {
                if (!object.Equals(_getAssesmentsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getAssesmentsResult", NewValue = value, OldValue = _getAssesmentsResult };
                    _getAssesmentsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region Client Contact
        protected RadzenGrid<PersonContact> gridContact;

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

        protected async System.Threading.Tasks.Task ButtonclientClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<Clear.Risk.Pages.Contacts.AddPersonContact>("Add Client Contact", new Dictionary<string, object>() { { "PersonId", person.PERSON_ID } }, new DialogOptions() { Width = $"{800}px" });
            gridContact.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridContactRowSelect(PersonContact args)
        {
            var dialogResult = await DialogService.OpenAsync<Clear.Risk.Pages.Contacts.EditPersonContact>("Edit Person Contact", new Dictionary<string, object>() { { "PERSON_CONTACT_ID", args.PERSON_CONTACT_ID } }, new DialogOptions() { Width = $"{800}px" });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridContactDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeletePersonContactResult = await ClearConnection.DeletePersonContact(int.Parse($"{data.PERSON_CONTACT_ID}"));
                    if (clearRiskDeletePersonContactResult != null)
                    {
                        gridContact.Reload();
                    }
                }
            }
            catch (System.Exception clearRiskDeletePersonContactException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete PersonContact");
            }
        }
        #endregion


        protected void Change(string value, string name)
        {
            events.Add(DateTime.Now, $"{name} value changed");
        }

        protected void Error(UploadErrorEventArgs args, string name)
        {
            events.Add(DateTime.Now, $"{args.Message}");
        }

        #region "Company Transaction"
        protected RadzenGrid<CompanyTransaction> gridTrans;
        IEnumerable<CompanyTransaction> _getCompanyTransactionsResult;
        protected IEnumerable<CompanyTransaction> getCompanyTransactionsResult
        {
            get
            {
                return _getCompanyTransactionsResult;
            }
            set
            {
                if (!object.Equals(_getCompanyTransactionsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCompanyTransactionsResult", NewValue = value, OldValue = _getCompanyTransactionsResult };
                    _getCompanyTransactionsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        dynamic _master;
        protected dynamic master
        {
            get
            {
                return _master;
            }
            set
            {
                if (!object.Equals(_master, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "master", NewValue = value, OldValue = _master };
                    _master = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected async System.Threading.Tasks.Task Grid0RowExpand(CompanyTransaction args)
        {
            master = args;

            var clearRiskGetCompanyTransactionDetailsResult = await ClearConnection.GetCompanyTransactionDetails(new Query() { Filter = $@"i => i.TransactionID == {args.TRANSACTIONID}" });
            if (clearRiskGetCompanyTransactionDetailsResult != null)
            {
                args.CompanyTransactionDetails = clearRiskGetCompanyTransactionDetailsResult.ToList();
            }
        }
        #endregion

        #region "Dashboard"
        protected IEnumerable<WorkOrderByMonth> _monthOrder;
        protected IEnumerable<WorkOrderByMonth> MonthOrder
        {
            get
            {
                return _monthOrder;
            }
            set
            {
                if (!object.Equals(_monthOrder, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "MonthOrder", NewValue = value, OldValue = _monthOrder };
                    _monthOrder = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected IEnumerable<SurveyByName> _surveyDone;
        protected IEnumerable<SurveyByName> SurveyDone
        {
            get
            {
                return _surveyDone;
            }
            set
            {
                if (!object.Equals(_surveyDone, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SurveyDone", NewValue = value, OldValue = _surveyDone };
                    _surveyDone = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        protected IEnumerable<SurveyByMonth> _surveyMonthly;
        protected IEnumerable<SurveyByMonth> SurveyMonthly
        {
            get
            {
                return _surveyMonthly;
            }
            set
            {
                if (!object.Equals(_surveyMonthly, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SurveyMonthly", NewValue = value, OldValue = _surveyMonthly };
                    _surveyMonthly = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected AccountStats _Stats;
        protected IEnumerable<RevenueByCompany> _revenueByCompany;
        protected IEnumerable<RevenueByCompany> RevenueCompany
        {
            get
            {
                return _revenueByCompany;
            }
            set
            {
                if (!object.Equals(_revenueByCompany, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "RevenueCompany", NewValue = value, OldValue = _revenueByCompany };
                    _revenueByCompany = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected AccountStats Stats
        {
            get
            {
                return _Stats;
            }
            set
            {
                if (!object.Equals(_Stats, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "Stats", NewValue = value, OldValue = _Stats };
                    _Stats = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected string FormatAsUSD(object value)
        {
            return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
        }
        #endregion

        #region Payment Process
        //protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        //{

        //    var dialogResult = await DialogService.OpenAsync<AddCredit>("Add Credit", null, new DialogOptions() { Width = $"{1000}px" });

        //    await InvokeAsync(() => { StateHasChanged(); });
        //}

        //protected async System.Threading.Tasks.Task Button1Click(MouseEventArgs args)
        //{

        //    var dialogResult = await DialogService.OpenAsync<AddPayment>("Add Payment", null, new DialogOptions() { Width = $"{1000}px" });

        //    await InvokeAsync(() => { StateHasChanged(); });
        //}
        #endregion

        #region "Profile Picture"
        protected async Task ChangeProfilePicture()
        {
            var dialogResult = await DialogService.OpenAsync<ProfilePicture>("Change Profile Picture", new Dictionary<string, object>() { { "PersonId", int.Parse(PersonId) } });
            await InvokeAsync(() => { StateHasChanged(); });

            person = await ClearConnection.GetPersonByPersonId(int.Parse(PersonId));
        }
        #endregion

    }
}
