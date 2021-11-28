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

namespace Clear.Risk.Pages
{
    public partial class SystemroleComponent : ComponentBase
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

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.Systemrole> grid0;

        IEnumerable<Clear.Risk.Models.ClearConnection.Systemrole> _getSystemrolesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Systemrole> getSystemrolesResult
        {
            get
            {
                return _getSystemrolesResult;
            }
            set
            {
                if (!object.Equals(_getSystemrolesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSystemrolesResult", NewValue = value, OldValue = _getSystemrolesResult };
                    _getSystemrolesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetSystemrolesResult = await ClearConnection.GetSystemroles();
            getSystemrolesResult = clearConnectionGetSystemrolesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSystemrole>("Add Systemrole", null);
            grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.Systemrole args)
        {
            var dialogResult = await DialogService.OpenAsync<EditSystemrole>("Edit Systemrole", new Dictionary<string, object>() { {"ROLE_ID", args.ROLE_ID} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteSystemroleResult = await ClearConnection.DeleteSystemrole(data.ROLE_ID);
                    if (clearConnectionDeleteSystemroleResult != null) {
                        grid0.Reload();
}
                }
            }
            catch (System.Exception clearConnectionDeleteSystemroleException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Systemrole");
            }
        }
    }
}
