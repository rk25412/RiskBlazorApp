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
using ClearCovid.Pages;

namespace Clear.Risk.Pages.Employees
{
    public partial class ManageEmployees: ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        protected bool isLoading = true;



        protected IList<Clear.Risk.Models.ClearConnection.Person> getPeopleResult = new List<Clear.Risk.Models.ClearConnection.Person>();
        
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                isLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                isLoading = false;
                StateHasChanged();
                
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (Security.IsInRole("System Administrator"))
            {
                var clearConnectionGetPeopleResult = await ClearConnection.GetAllPerson(new Query() { Expand = "State,Country,Person1,State1,Country1,PersonType,Applicence" });
                //getPeopleResult = (from x in clearConnectionGetPeopleResult
                //                   select new Models.ClearConnection.Person
                //                   {
                //                       PERSON_ID = x.PERSON_ID,
                //                       FIRST_NAME = x.FIRST_NAME,
                //                       LAST_NAME = x.LAST_NAME,
                //                       BUSINESS_MOBILE = x.BUSINESS_MOBILE,
                //                       PERSONAL_EMAIL = x.PERSONAL_EMAIL,
                //                       Manager = x.Manager ?? null,
                //                       ISMANAGER = x.ISMANAGER,
                //                       COMPANY_NAME=x.COMPANY_NAME
                //                   })
                //                 .ToList();

                getPeopleResult = (from x in clearConnectionGetPeopleResult 
                                   join m in clearConnectionGetPeopleResult 
                                   on x.PERSON_ID equals m.PARENT_PERSON_ID where m.COMPANYTYPE == 3
                                   select new Models.ClearConnection.Person
                                   {
                                       PERSON_ID = m.PERSON_ID,
                                       FIRST_NAME = m.FIRST_NAME,
                                       LAST_NAME = m.LAST_NAME,
                                       BUSINESS_MOBILE = m.BUSINESS_MOBILE,
                                       PERSONAL_EMAIL = m.PERSONAL_EMAIL,
                                       Manager = m.Manager ?? null,
                                       ISMANAGER = m.ISMANAGER,
                                       COMPANY_NAME = x.COMPANY_NAME
                                   })
                                 .ToList();

            }
            else
            {
                var clearConnectionGetPeopleResult = await ClearConnection.GetEmployee(Security.getCompanyId(), new Query() { Expand = "State,Country,Person1,State1,Country1,PersonType,Applicence" });


                getPeopleResult = (from x in clearConnectionGetPeopleResult
                                  select new Models.ClearConnection.Person
                                  {
                                      PERSON_ID = x.PERSON_ID,
                                      FIRST_NAME = x.FIRST_NAME,
                                      LAST_NAME = x.LAST_NAME,
                                      BUSINESS_MOBILE = x.BUSINESS_MOBILE,
                                      PERSONAL_EMAIL= x.PERSONAL_EMAIL,
                                      Manager = x.Manager ?? null,
                                      ISMANAGER = x.ISMANAGER
                                  })
                                  .ToList();
            }

        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {

            UriHelper.NavigateTo("add-Employee");

        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/9";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            UriHelper.NavigateTo("edit-employee" + "/" + data.PERSON_ID.ToString());
           
        }
        
        //protected async Task ChangePassword(MouseEventArgs args, dynamic data)
        //{
        //    await DialogService.OpenAsync<ChangeEmployeePassword>("Change Password", new Dictionary<string, object>() { { "PersonId" , data.PERSON_ID } });
        //}

    }
}
