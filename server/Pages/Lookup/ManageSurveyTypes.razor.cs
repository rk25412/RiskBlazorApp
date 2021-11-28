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
    public partial class ManageSurveyTypes : ComponentBase
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
        protected bool IsLoading { get; set; }

        //IEnumerable<SurveyType> _getSurveyTypesResult;
        protected IList<SurveyType> getSurveyTypesResult = new List<SurveyType>();
        //{
        //    get
        //    {
        //        return _getSurveyTypesResult;
        //    }
        //    set
        //    {
        //        if (!object.Equals(_getSurveyTypesResult, value))
        //        {
        //            var args = new PropertyChangedEventArgs() { Name = "getSurveyTypesResult", NewValue = value, OldValue = _getSurveyTypesResult };
        //            _getSurveyTypesResult = value;
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
            var clearRiskGetSurveyTypesResult = await ClearRisk.GetSurveyTypes();
            getSurveyTypesResult = (from x in clearRiskGetSurveyTypesResult
                                    select new SurveyType
                                    {
                                        SURVEY_TYPE_ID = x.SURVEY_TYPE_ID,
                                        NAME = x.NAME
                                    }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSurveyType>("Add Survey Type", null);
            //await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            var clearRiskGetSurveyTypesResult = await ClearRisk.GetSurveyTypes();
            getSurveyTypesResult = (from x in clearRiskGetSurveyTypesResult
                                    select new SurveyType
                                    {
                                        SURVEY_TYPE_ID = x.SURVEY_TYPE_ID,
                                        NAME = x.NAME
                                    }).ToList();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/27";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");
            //UriHelper.NavigateTo("Help" + "/" + 27);
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
                    var clearRiskDeleteSurveyTypeResult = await ClearRisk.DeleteSurveyType(int.Parse($"{data.SURVEY_TYPE_ID}"));
                    if (clearRiskDeleteSurveyTypeResult != null)
                    {

                        getSurveyTypesResult.Remove(getSurveyTypesResult.FirstOrDefault(x => x.SURVEY_TYPE_ID == data.SURVEY_TYPE_ID));
                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteSurveyTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SurveyType");
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSurveyType>("Edit Survey Type", new Dictionary<string, object>() { { "SURVEY_TYPE_ID", data.SURVEY_TYPE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            var clearRiskGetSurveyTypesResult = await ClearRisk.GetSurveyTypes();
            getSurveyTypesResult = (from x in clearRiskGetSurveyTypesResult
                                    select new SurveyType
                                    {
                                        SURVEY_TYPE_ID = x.SURVEY_TYPE_ID,
                                        NAME = x.NAME
                                    }).ToList();
        }
    }
}
