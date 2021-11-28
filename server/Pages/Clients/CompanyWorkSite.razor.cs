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

namespace Clear.Risk.Pages.Clients
{
    public partial class CompanyWorkSite : ComponentBase
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



        IEnumerable<PersonSite> _getPersonSitesResult;
        protected IEnumerable<PersonSite> getPersonSitesResult
        {
            get
            {
                return _getPersonSitesResult;
            }
            set
            {
                if (!object.Equals(_getPersonSitesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPersonSitesResult", NewValue = value, OldValue = _getPersonSitesResult };
                    _getPersonSitesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

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

        IList<PersonSite> clearRiskGetPersonSitesResult = new List<PersonSite>();


        protected async System.Threading.Tasks.Task Load()
        {
            clearRiskGetPersonSitesResult = (await ClearRisk.GetPersonSites(Security.IsInRole("System Administrator") ? new Query() : new Query() { Filter = $@"i => i.PERSON_ID == {Security.getCompanyId()}" })).ToList();

            getPersonSitesResult = (from x in clearRiskGetPersonSitesResult
                                    select new PersonSite
                                    {
                                        PERSON_SITE_ID = x.PERSON_SITE_ID,
                                        SITE_NAME = x.SITE_NAME,
                                        BUILDING_NAME = x.BUILDING_NAME,
                                        SITE_ADDRESS1 = x.SITE_ADDRESS1,
                                        SITE_ADDRESS2 = x.SITE_ADDRESS2,
                                        CITY = x.CITY,
                                        State = x.State,
                                        POST_CODE = x.POST_CODE,
                                        LATITUDE = x.LATITUDE,
                                        LONGITUDE = x.LONGITUDE,
                                        IS_DEFAULT = x.IS_DEFAULT,
                                        Country = x.Country,
                                    }).ToList();

        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddPersonSite>("Add Work Site", new Dictionary<string, object>() { { "ClientId", Security.getCompanyId() } }, new DialogOptions() { Width = $"{800}px" });
            //await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            clearRiskGetPersonSitesResult = (await ClearRisk.GetPersonSites(Security.IsInRole("System Administrator") ? new Query() : new Query() { Filter = $@"i => i.PERSON_ID == {Security.getCompanyId()}" })).ToList();

            getPersonSitesResult = (from x in clearRiskGetPersonSitesResult
                                    select new PersonSite
                                    {
                                        PERSON_SITE_ID = x.PERSON_SITE_ID,
                                        SITE_NAME = x.SITE_NAME,
                                        BUILDING_NAME = x.BUILDING_NAME,
                                        SITE_ADDRESS1 = x.SITE_ADDRESS1,
                                        SITE_ADDRESS2 = x.SITE_ADDRESS2,
                                        CITY = x.CITY,
                                        State = x.State,
                                        POST_CODE = x.POST_CODE,
                                        LATITUDE = x.LATITUDE,
                                        LONGITUDE = x.LONGITUDE,
                                        IS_DEFAULT = x.IS_DEFAULT,
                                        Country = x.Country,
                                    }).ToList();
        }

        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/36";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");
            // UriHelper.NavigateTo("Help" + "/" + 36);
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeletePersonSiteResult = await ClearRisk.DeletePersonSite(data.PERSON_SITE_ID);
                    if (clearRiskDeletePersonSiteResult != null)
                    {
                        clearRiskGetPersonSitesResult.Remove(clearRiskGetPersonSitesResult.FirstOrDefault(x => x.PERSON_SITE_ID == data.PERSON_SITE_ID));
                        getPersonSitesResult = (from x in clearRiskGetPersonSitesResult
                                                select new PersonSite
                                                {
                                                    PERSON_SITE_ID = x.PERSON_SITE_ID,
                                                    SITE_NAME = x.SITE_NAME,
                                                    BUILDING_NAME = x.BUILDING_NAME,
                                                    SITE_ADDRESS1 = x.SITE_ADDRESS1,
                                                    SITE_ADDRESS2 = x.SITE_ADDRESS2,
                                                    CITY = x.CITY,
                                                    State = x.State,
                                                    POST_CODE = x.POST_CODE,
                                                    LATITUDE = x.LATITUDE,
                                                    LONGITUDE = x.LONGITUDE,
                                                    IS_DEFAULT = x.IS_DEFAULT,
                                                    Country = x.Country,
                                                }).ToList();
                    }
                }
            }
            catch (System.Exception clearRiskDeletePersonSiteException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Company Work Site", 180000);
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {

            var dialogResult = await DialogService.OpenAsync<EditPersonSite>("Edit Work Site", new Dictionary<string, object>() { { "PERSON_SITE_ID", data.PERSON_SITE_ID } }, new DialogOptions() { Width = $"{800}px" });
            await InvokeAsync(() => { StateHasChanged(); });

            clearRiskGetPersonSitesResult = (await ClearRisk.GetPersonSites(Security.IsInRole("System Administrator") ? new Query() : new Query() { Filter = $@"i => i.PERSON_ID == {Security.getCompanyId()}" })).ToList();

            getPersonSitesResult = (from x in clearRiskGetPersonSitesResult
                                    select new PersonSite
                                    {
                                        PERSON_SITE_ID = x.PERSON_SITE_ID,
                                        SITE_NAME = x.SITE_NAME,
                                        BUILDING_NAME = x.BUILDING_NAME,
                                        SITE_ADDRESS1 = x.SITE_ADDRESS1,
                                        SITE_ADDRESS2 = x.SITE_ADDRESS2,
                                        CITY = x.CITY,
                                        State = x.State,
                                        POST_CODE = x.POST_CODE,
                                        LATITUDE = x.LATITUDE,
                                        LONGITUDE = x.LONGITUDE,
                                        IS_DEFAULT = x.IS_DEFAULT,
                                        Country = x.Country,
                                    }).ToList();

        }

        protected async Task OnDefaultChange(dynamic data)
        {
            var updateData = clearRiskGetPersonSitesResult.FirstOrDefault(x => x.PERSON_SITE_ID == int.Parse($"{data.PERSON_SITE_ID}"));

            updateData.IS_DEFAULT = true;
            var clearConnectionUpdatePersonSiteResult = await ClearRisk.UpdatePersonSite(int.Parse($"{data.PERSON_SITE_ID}"), updateData);

            var OtherData = clearRiskGetPersonSitesResult.Where(i => i.PERSON_SITE_ID != int.Parse($"{data.PERSON_SITE_ID}"));

            foreach (var item in OtherData)
            {
                item.IS_DEFAULT = false;
                await ClearRisk.UpdatePersonSite(item.PERSON_SITE_ID, item);
            }

            getPersonSitesResult = (from x in clearRiskGetPersonSitesResult
                                    select new PersonSite
                                    {
                                        PERSON_SITE_ID = x.PERSON_SITE_ID,
                                        SITE_NAME = x.SITE_NAME,
                                        BUILDING_NAME = x.BUILDING_NAME,
                                        SITE_ADDRESS1 = x.SITE_ADDRESS1,
                                        SITE_ADDRESS2 = x.SITE_ADDRESS2,
                                        CITY = x.CITY,
                                        State = x.State,
                                        POST_CODE = x.POST_CODE,
                                        LATITUDE = x.LATITUDE,
                                        LONGITUDE = x.LONGITUDE,
                                        IS_DEFAULT = x.IS_DEFAULT,
                                        Country = x.Country,
                                    }).ToList();
        }
    }
}
