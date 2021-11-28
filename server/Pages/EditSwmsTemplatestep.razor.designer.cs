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
    public partial class EditSwmsTemplatestepComponent : ComponentBase
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
        public dynamic STEPID { get; set; }

        Clear.Risk.Models.ClearConnection.SwmsTemplatestep _swmstemplatestep;
        protected Clear.Risk.Models.ClearConnection.SwmsTemplatestep swmstemplatestep
        {
            get
            {
                return _swmstemplatestep;
            }
            set
            {
                if (!object.Equals(_swmstemplatestep, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "swmstemplatestep", NewValue = value, OldValue = _swmstemplatestep };
                    _swmstemplatestep = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.RiskLikelyhood> _getRiskLikelyhoodsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.RiskLikelyhood> getRiskLikelyhoodsResult
        {
            get
            {
                return _getRiskLikelyhoodsResult;
            }
            set
            {
                if (!object.Equals(_getRiskLikelyhoodsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getRiskLikelyhoodsResult", NewValue = value, OldValue = _getRiskLikelyhoodsResult };
                    _getRiskLikelyhoodsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Consequence> _getConsequencesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Consequence> getConsequencesResult
        {
            get
            {
                return _getConsequencesResult;
            }
            set
            {
                if (!object.Equals(_getConsequencesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getConsequencesResult", NewValue = value, OldValue = _getConsequencesResult };
                    _getConsequencesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.ResposnsibleType> _getResposnsibleTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.ResposnsibleType> getResposnsibleTypesResult
        {
            get
            {
                return _getResposnsibleTypesResult;
            }
            set
            {
                if (!object.Equals(_getResposnsibleTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getResposnsibleTypesResult", NewValue = value, OldValue = _getResposnsibleTypesResult };
                    _getResposnsibleTypesResult = value;
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
            var clearConnectionGetSwmsTemplatestepByStepidResult = await ClearConnection.GetSwmsTemplatestepByStepid(STEPID);
            swmstemplatestep = clearConnectionGetSwmsTemplatestepByStepidResult;

            var clearConnectionGetRiskLikelyhoodsResult = await ClearConnection.GetRiskLikelyhoods();
            getRiskLikelyhoodsResult = clearConnectionGetRiskLikelyhoodsResult;

            var clearConnectionGetConsequencesResult = await ClearConnection.GetConsequences();
            getConsequencesResult = clearConnectionGetConsequencesResult;

            var clearConnectionGetResposnsibleTypesResult = await ClearConnection.GetResposnsibleTypes();
            getResposnsibleTypesResult = clearConnectionGetResposnsibleTypesResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SwmsTemplatestep args)
        {
            try
            {
                var clearConnectionUpdateSwmsTemplatestepResult = await ClearConnection.UpdateSwmsTemplatestep(STEPID, swmstemplatestep);
                DialogService.Close(swmstemplatestep);
            }
            catch (System.Exception clearConnectionUpdateSwmsTemplatestepException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update SwmsTemplatestep");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
