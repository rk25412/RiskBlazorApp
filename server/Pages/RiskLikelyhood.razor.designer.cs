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
    public partial class RiskLikelyhoodComponent : ComponentBase
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

        protected IList<Clear.Risk.Models.ClearConnection.RiskLikelyhood> getRiskLikelyhoodsResult = new List<Clear.Risk.Models.ClearConnection.RiskLikelyhood>();

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
            var clearConnectionGetRiskLikelyhoodsResult = await ClearConnection.GetRiskLikelyhoods();

            getRiskLikelyhoodsResult = (from x in clearConnectionGetRiskLikelyhoodsResult
                                        select new Clear.Risk.Models.ClearConnection.RiskLikelyhood
                                        {
                                            RISK_VALUE_ID = x.RISK_VALUE_ID,
                                            NAME = x.NAME,
                                            RISK_VALUE = x.RISK_VALUE,
                                        }).ToList();

            //getRiskLikelyhoodsResult = clearConnectionGetRiskLikelyhoodsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddRiskLikelyhood>("Add Risk Likelyhood", null);          

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/20";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 20);
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
                    var clearConnectionDeleteRiskLikelyhoodResult = await ClearConnection.DeleteRiskLikelyhood(data.RISK_VALUE_ID);
                    if (clearConnectionDeleteRiskLikelyhoodResult != null) 
                    {
                        getRiskLikelyhoodsResult.Remove(getRiskLikelyhoodsResult.FirstOrDefault(x => x.RISK_VALUE_ID == data.RISK_VALUE_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"RiskLikelyHood Successfully deleted.",1800000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteRiskLikelyhoodException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete RiskLikelyhood",1800000);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditRiskLikelyhood>("Edit Risk Likelyhood", new Dictionary<string, object>() { { "RISK_VALUE_ID", data.RISK_VALUE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });


            await Load();
        }
    }
}
