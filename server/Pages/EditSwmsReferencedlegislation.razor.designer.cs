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
    public partial class EditSwmsReferencedlegislationComponent : ComponentBase
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

        [Parameter]
        public dynamic REFLID { get; set; }

        Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation _swmsreferencedlegislation;
        protected Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation swmsreferencedlegislation
        {
            get
            {
                return _swmsreferencedlegislation;
            }
            set
            {
                if (!object.Equals(_swmsreferencedlegislation, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "swmsreferencedlegislation", NewValue = value, OldValue = _swmsreferencedlegislation };
                    _swmsreferencedlegislation = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.ReferencedLegislation> _getReferencedLegislationsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.ReferencedLegislation> getReferencedLegislationsResult
        {
            get
            {
                return _getReferencedLegislationsResult;
            }
            set
            {
                if (!object.Equals(_getReferencedLegislationsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getReferencedLegislationsResult", NewValue = value, OldValue = _getReferencedLegislationsResult };
                    _getReferencedLegislationsResult = value;
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
            var clearConnectionGetSwmsReferencedlegislationByReflidResult = await ClearConnection.GetSwmsReferencedlegislationByReflid(REFLID);
            swmsreferencedlegislation = clearConnectionGetSwmsReferencedlegislationByReflidResult;

            var clearConnectionGetReferencedLegislationsResult = await ClearConnection.GetReferencedLegislations();
            getReferencedLegislationsResult = clearConnectionGetReferencedLegislationsResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation args)
        {
            try
            {
                var clearConnectionUpdateSwmsReferencedlegislationResult = await ClearConnection.UpdateSwmsReferencedlegislation(REFLID, swmsreferencedlegislation);
                DialogService.Close(swmsreferencedlegislation);
            }
            catch (System.Exception clearConnectionUpdateSwmsReferencedlegislationException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update SwmsReferencedlegislation");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
