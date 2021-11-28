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
using Microsoft.AspNetCore.Identity;
using Clear.Risk.Models;

namespace Clear.Risk.Pages
{
    public partial class MangeUsersComponent : ComponentBase
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

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.Person> grid0;

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
                    var args = new PropertyChangedEventArgs(){ Name = "getPeopleResult", NewValue = value, OldValue = _getPeopleResult };
                    _getPeopleResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            //if (!Security.IsAuthenticated())
            //{
            //    UriHelper.NavigateTo("Login", true);
            //}
            //else
            //{
            //    await Load();
            //}

            await Load();

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetPeopleResult = await ClearConnection.GetPeople(new Query() { Expand = "State,Country,Person1,State1,Country1,PersonType,Applicence" });
            getPeopleResult = clearConnectionGetPeopleResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("add-user");
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.Person args)
        {
            UriHelper.NavigateTo($"edit-mange-users/{args.PERSON_ID}");
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeletePersonResult = await ClearConnection.DeletePerson(int.Parse($"{data.PERSON_ID}"));
                    if (clearConnectionDeletePersonResult != null) {
                        grid0.Reload();
}
                }
            }
            catch (System.Exception clearConnectionDeletePersonException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Person");
            }
        }
    }
}
