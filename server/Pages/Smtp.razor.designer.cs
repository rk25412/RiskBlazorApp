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
    public partial class SmtpComponent : ComponentBase
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
        protected bool IsLoading { get; set; }
       // protected RadzenGrid<Clear.Risk.Models.ClearConnection.Smtpsetup> grid0;

        IEnumerable<Clear.Risk.Models.ClearConnection.Smtpsetup> _getSmtpsetupsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Smtpsetup> getSmtpsetupsResult
        {
            get
            {
                return _getSmtpsetupsResult;
            }
            set
            {
                if (!object.Equals(_getSmtpsetupsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSmtpsetupsResult", NewValue = value, OldValue = _getSmtpsetupsResult };
                    _getSmtpsetupsResult = value;
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
            var clearConnectionGetSmtpsetupsResult = await ClearConnection.GetSmtpsetups();
            getSmtpsetupsResult = clearConnectionGetSmtpsetupsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSmtp>("Add SMTP", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/40";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");            
        }
        protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.Smtpsetup args)
        {
            var dialogResult = await DialogService.OpenAsync<EditSmtp>("Edit SMTP", new Dictionary<string, object>() { {"SMTP_SETUP_ID", args.SMTP_SETUP_ID} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteSmtpsetupResult = await ClearConnection.DeleteSmtpsetup(data.SMTP_SETUP_ID);
                    if (clearConnectionDeleteSmtpsetupResult != null) {
                        //grid0.Reload();
}
                }
            }
            catch (System.Exception clearConnectionDeleteSmtpsetupException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Smtpsetup");
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSmtp>("Edit SMTP", new Dictionary<string, object>() { { "SMTP_SETUP_ID", data.SMTP_SETUP_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
        }
    }
}
