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
    public partial class LoginComponent : ComponentBase
    {
        //[Parameter(CaptureUnmatchedValues = true)]
        //public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        [Parameter]
        public dynamic notificationType { get; set; }

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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var error = System.Web.HttpUtility.ParseQueryString(new Uri(UriHelper.ToAbsoluteUri(UriHelper.Uri).ToString()).Query).Get("error");
            var success = System.Web.HttpUtility.ParseQueryString(new Uri(UriHelper.ToAbsoluteUri(UriHelper.Uri).ToString()).Query).Get("success");

            if (!string.IsNullOrEmpty(success))
            {
                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"{success}", 180000);
            }
            else if (!string.IsNullOrEmpty(error))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"{error}",180000);
            }
        }

        protected async System.Threading.Tasks.Task Login0Register()
        {
            //DialogService.Open<RegisterApplicationUser>("Register Application User", null);
            UriHelper.NavigateTo("https://clear-peoplesafe.com/");
            
        }
        protected async System.Threading.Tasks.Task Forgot0Password()
        {            
            var dialogResult = await DialogService.OpenAsync<ForgotPassword>("Forgot Password", null, new DialogOptions() { Width = $"{500}px" });
           
        }
    }
}
