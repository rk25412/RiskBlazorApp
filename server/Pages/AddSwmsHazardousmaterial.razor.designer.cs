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
    public partial class AddSwmsHazardousmaterialComponent : ComponentBase
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
        public dynamic SWMSID { get; set; }

        IEnumerable<Clear.Risk.Models.ClearConnection.HazardMaterialValue> _getHazardMaterialValuesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.HazardMaterialValue> getHazardMaterialValuesResult
        {
            get
            {
                return _getHazardMaterialValuesResult;
            }
            set
            {
                if (!object.Equals(_getHazardMaterialValuesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getHazardMaterialValuesResult", NewValue = value, OldValue = _getHazardMaterialValuesResult };
                    _getHazardMaterialValuesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial _swmshazardousmaterial;
        protected Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial swmshazardousmaterial
        {
            get
            {
                return _swmshazardousmaterial;
            }
            set
            {
                if (!object.Equals(_swmshazardousmaterial, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "swmshazardousmaterial", NewValue = value, OldValue = _swmshazardousmaterial };
                    _swmshazardousmaterial = value;
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
            var clearConnectionGetHazardMaterialValuesResult = await ClearConnection.GetHazardMaterialValues();
            getHazardMaterialValuesResult = clearConnectionGetHazardMaterialValuesResult;

            swmshazardousmaterial = new Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial args)
        {
            swmshazardousmaterial.SWMSID = int.Parse($"{SWMSID}");

            try
            {
                var clearConnectionCreateSwmsHazardousmaterialResult = await ClearConnection.CreateSwmsHazardousmaterial(swmshazardousmaterial);
                DialogService.Close(swmshazardousmaterial);
            }
            catch (System.Exception clearConnectionCreateSwmsHazardousmaterialException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SwmsHazardousmaterial!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
