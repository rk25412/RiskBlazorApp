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
    public partial class ManageLicences : ComponentBase
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

        //[Inject]
        //protected TooltipService TooltipService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }


        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }

        protected RadzenContent content1;

        protected RadzenHeading pageTitle;

        protected RadzenButton button0;

        //protected RadzenGrid<Applicence> grid0;


        protected IList<Applicence> getApplicencesResult = new List<Applicence>();

        protected bool IsLoading { get; set; }
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
            var clearRiskGetApplicencesResult = await ClearRisk.GetApplicences(new Query());

            getApplicencesResult = (from x in clearRiskGetApplicencesResult
                                    select new Applicence
                                    {
                                        APPLICENCEID = x.APPLICENCEID,
                                        LICENCE_NAME = x.LICENCE_NAME,
                                        VERSION = x.VERSION,
                                        DESCRIPTION = x.DESCRIPTION,
                                        IS_DEFAULT = x.IS_DEFAULT,
                                        PRICE = x.PRICE,
                                        DISCOUNT = x.DISCOUNT,
                                        NETPRICE = x.NETPRICE,
                                        CURRENCY_ID = x.CURRENCY_ID,
                                        COUNTRY_ID = x.COUNTRY_ID,
                                    }).ToList();


            //getApplicencesResult = clearRiskGetApplicencesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddApplicence>("Add Applicence", null, new DialogOptions() { Width = $"{600}px" });
            // grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            var clearRiskGetApplicencesResult = await ClearRisk.GetApplicences(new Query());

            getApplicencesResult = (from x in clearRiskGetApplicencesResult
                                    select new Applicence
                                    {
                                        APPLICENCEID = x.APPLICENCEID,
                                        LICENCE_NAME = x.LICENCE_NAME,
                                        VERSION = x.VERSION,
                                        DESCRIPTION = x.DESCRIPTION,
                                        IS_DEFAULT = x.IS_DEFAULT,
                                        PRICE = x.PRICE,
                                        DISCOUNT = x.DISCOUNT,
                                        NETPRICE = x.NETPRICE,
                                        CURRENCY_ID = x.CURRENCY_ID,
                                        COUNTRY_ID = x.COUNTRY_ID,
                                    }).ToList();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/13";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 13);
        }
        //protected async System.Threading.Tasks.Task Grid0RowSelect(Applicence args)
        //{
        //    var dialogResult = await DialogService.OpenAsync<EditApplicence>("Edit Applicence", new Dictionary<string, object>() { { "APPLICENCEID", args.APPLICENCEID } }, new DialogOptions() { Width = $"{600}px" });
        //    await InvokeAsync(() => { StateHasChanged(); });
        //}

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeleteApplicenceResult = await ClearRisk.DeleteApplicence(int.Parse($"{data.APPLICENCEID}"));

                    if (clearRiskDeleteApplicenceResult != null)
                    {
                        getApplicencesResult.Remove(getApplicencesResult.FirstOrDefault(x => x.APPLICENCEID == data.APPLICENCEID));
                    }
                }
            }
            catch (System.Exception clearRiskDeleteApplicenceException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Applicence");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditApplicence>("Edit Applicence", new Dictionary<string, object>() { { "APPLICENCEID", data.APPLICENCEID } }, new DialogOptions() { Width = $"{600}px" });
            await InvokeAsync(() => { StateHasChanged(); });

            var clearRiskGetApplicencesResult = await ClearRisk.GetApplicences(new Query());

            getApplicencesResult = (from x in clearRiskGetApplicencesResult
                                    select new Applicence
                                    {
                                        APPLICENCEID = x.APPLICENCEID,
                                        LICENCE_NAME = x.LICENCE_NAME,
                                        VERSION = x.VERSION,
                                        DESCRIPTION = x.DESCRIPTION,
                                        IS_DEFAULT = x.IS_DEFAULT,
                                        PRICE = x.PRICE,
                                        DISCOUNT = x.DISCOUNT,
                                        NETPRICE = x.NETPRICE,
                                        CURRENCY_ID = x.CURRENCY_ID,
                                        COUNTRY_ID = x.COUNTRY_ID,
                                    }).ToList();
        }
    }
}
