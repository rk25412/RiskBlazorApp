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
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Web;
using System.Text.Encodings.Web;

namespace Clear.Risk.Pages
{
    public partial class Help : ComponentBase
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

        protected ClearConnectionService ClearRisk { get; set; }

        [Parameter]
        public dynamic ScreenId { get; set; }
        protected Clear.Risk.Models.ClearConnection.HelpReference _getHelpReferencesResult;
        protected Clear.Risk.Models.ClearConnection.HelpReference getHelpReferencesResult
        {
            get
            {
                return _getHelpReferencesResult;
            }
            set
            {
                if (!object.Equals(_getHelpReferencesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getHelpReferencesResult", NewValue = value, OldValue = _getHelpReferencesResult };
                    _getHelpReferencesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        // protected static string HtmlEncode (object value);

        protected HtmlEncoder htmlValue { get; set; }
        protected bool isLoading { get; set; }
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
            var clearRiskGetHelpReferencesResult = await ClearRisk.GetHelpReferenceByHelpScreenId(int.Parse(ScreenId));
            getHelpReferencesResult = clearRiskGetHelpReferencesResult;
        }
    }
}
