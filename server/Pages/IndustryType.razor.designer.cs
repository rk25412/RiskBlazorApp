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
    public partial class IndustryTypeComponent : ComponentBase
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

        protected bool IsLoading = true;

        protected IList<Clear.Risk.Models.ClearConnection.IndustryType> getIndustryTypesResult = new List<Clear.Risk.Models.ClearConnection.IndustryType>();


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
            var clearConnectionGetIndustryTypesResult = await ClearConnection.GetIndustryTypes(new Query());
            getIndustryTypesResult = clearConnectionGetIndustryTypesResult.Select(x => new Clear.Risk.Models.ClearConnection.IndustryType
            {
                INDUSTRY_TYPE_ID = x.INDUSTRY_TYPE_ID,
                NAME = x.NAME
            }).ToList(); ;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddIndustryType>("Add Industry Type", null);
            // grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/29";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 29);
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
                    var clearConnectionDeleteIndustryTypeResult = await ClearConnection.DeleteIndustryType(int.Parse($"{data.INDUSTRY_TYPE_ID}"));
                    if (clearConnectionDeleteIndustryTypeResult != null)
                    {
                        getIndustryTypesResult.Remove(getIndustryTypesResult.FirstOrDefault(x => x.INDUSTRY_TYPE_ID == data.INDUSTRY_TYPE_ID));
                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearConnectionDeleteIndustryTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete IndustryType");
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditIndustryType>("Edit Industry Type", new Dictionary<string, object>() { { "INDUSTRY_TYPE_ID", data.INDUSTRY_TYPE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
    }
}
