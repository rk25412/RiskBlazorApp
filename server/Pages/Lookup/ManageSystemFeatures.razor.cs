using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clear.Risk.Models.ClearConnection;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace Clear.Risk.Pages.Lookup
{
    public partial class ManageSystemFeatures : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

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

        protected IList<SystemFeatures> getSystemFeatures = new List<SystemFeatures>();

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
            var cleargetSystemFeatures = await ClearRisk.GetSystemFeatures();
            //getHelpReferencesResult = clearRiskGetHelpReferencesResult.ToList();
            getSystemFeatures = (from x in cleargetSystemFeatures
                                 select new SystemFeatures
                                 {
                                     Feature_ID = x.Feature_ID,
                                     Feature_ScreenID = x.Feature_ScreenID,
                                     Features_ScreenName = x.Features_ScreenName,
                                     Html_Content = x.Html_Content
                                 })
                                  .ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSystemFeatures>("Add System Features", null);

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }

        protected async System.Threading.Tasks.Task DeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeleteSystemFeaturesResult = await ClearRisk.DeleteSystemFeatures(int.Parse($"{data.Feature_ID}"));
                    if (clearRiskDeleteSystemFeaturesResult != null)
                    {
                        getSystemFeatures.Remove(getSystemFeatures.FirstOrDefault(x => x.Feature_ID == data.Feature_ID));
                        isLoading = false;
                        StateHasChanged();
                    }
                }
                isLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteSystemFeatureException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete System Features");
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task EditButtonClick(MouseEventArgs args, dynamic data)
        {

            var dialogResult = await DialogService.OpenAsync<EditSystemFeatures>("Edit System Features", new Dictionary<string, object>() { { "Feature_ID", data.Feature_ID } });

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
    }
}
