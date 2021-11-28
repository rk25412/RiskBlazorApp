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
using Clear.Risk.Data;

namespace Clear.Risk.Pages.Company
{
    public partial class ManageCompanies : ComponentBase
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

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.Person> grid0;
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
            var clearConnectionGetPeopleResult = await ClearConnection.GetPeople(new Query()
            {
                Filter = $@"i => i.COMPANYTYPE == 2 ",
                Expand = "State,Country,Person1,State1,Country1,PersonType,Applicence"
            });
            getPeopleResult = clearConnectionGetPeopleResult.Select(x => new Clear.Risk.Models.ClearConnection.Person
            {
                PERSON_ID = x.PERSON_ID,
                COMPANY_NAME = x.COMPANY_NAME,
                FIRST_NAME = x.FIRST_NAME,
                MIDDLE_NAME = x.MIDDLE_NAME,
                LAST_NAME = x.LAST_NAME,
                PARENT_PERSON_ID = x.PARENT_PERSON_ID,
                PersonType =x.PersonType,
                Applicence = x.Applicence,
                APPLICENCE_STARTDATE = x.APPLICENCE_STARTDATE,
                CURRENT_BALANCE = x.CURRENT_BALANCE
            }).ToList();
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            UriHelper.NavigateTo("view-Company" + "/" + data.PERSON_ID.ToString());
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/8";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            // UriHelper.NavigateTo("Help" + "/" + 8);
        }

        protected async Task ChangePassword(MouseEventArgs args, dynamic data)
        {
            await DialogService.OpenAsync<ChangeCompanyPassword>("Change Password", new Dictionary<string, object>() { { "PersonId", data.PERSON_ID } });
        }
    }
}
