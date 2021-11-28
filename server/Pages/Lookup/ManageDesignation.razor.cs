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

namespace Clear.Risk.Pages.Lookup
{
    public partial class ManageDesignation : ComponentBase
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
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }


        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddDesignation>("Add Designation", null);

            await InvokeAsync(() => { StateHasChanged(); });

            var clearRiskGetDesigationResult = await ClearRisk.GetDesigations();
            getDesigationResult = (from x in clearRiskGetDesigationResult
                                   select new Clear.Risk.Models.ClearConnection.Desigation
                                   {
                                       DESIGNATION_ID = x.DESIGNATION_ID,
                                       DESIGNATIONNAME = x.DESIGNATIONNAME
                                   })
                                  .ToList();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/37";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 37);
        }

        protected bool IsLoading { get; set; }
        //IEnumerable<Clear.Risk.Models.ClearConnection.Desigation> _getDesigation;
        protected IList<Clear.Risk.Models.ClearConnection.Desigation> getDesigationResult = new List<Clear.Risk.Models.ClearConnection.Desigation>();
        //{
        //    get
        //    {
        //        return _getDesigation;
        //    }
        //    set
        //    {
        //        if (!object.Equals(_getDesigation, value))
        //        {
        //            var args = new PropertyChangedEventArgs() { Name = "getDesigationResult", NewValue = value, OldValue = _getDesigation };
        //            _getDesigation = value;
        //            OnPropertyChanged(args);
        //            Reload();
        //        }
        //    }
        //}
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                IsLoading = false;
                StateHasChanged();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearRiskGetDesigationResult = await ClearRisk.GetDesigations();
            getDesigationResult = (from x in clearRiskGetDesigationResult
                                   select new Clear.Risk.Models.ClearConnection.Desigation
                                   {
                                       DESIGNATION_ID = x.DESIGNATION_ID,
                                       DESIGNATIONNAME = x.DESIGNATIONNAME
                                   })
                                  .ToList();
        }
        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeleteSurveyTypeResult = await ClearRisk.DeleteDesigation(int.Parse($"{data.DESIGNATION_ID}"));
                    if (clearRiskDeleteSurveyTypeResult != null)
                    {
                        getDesigationResult.Remove(getDesigationResult.FirstOrDefault(x => x.DESIGNATION_ID == data.DESIGNATION_ID));

                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteSurveyTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to Designation");
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditDesignation>("Edit Designation", new Dictionary<string, object>() { { "DESIGNATION_ID", data.DESIGNATION_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            var clearRiskGetDesigationResult = await ClearRisk.GetDesigations();
            getDesigationResult = (from x in clearRiskGetDesigationResult
                                   select new Clear.Risk.Models.ClearConnection.Desigation
                                   {
                                       DESIGNATION_ID = x.DESIGNATION_ID,
                                       DESIGNATIONNAME = x.DESIGNATIONNAME
                                   })
                                  .ToList();
        }
    }
}
