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
    public partial class ResposnsibleTypeComponent : ComponentBase
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

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.ResposnsibleType> grid0;
        protected bool IsLoading = true;

        protected IList<Clear.Risk.Models.ClearConnection.ResposnsibleType> getResposnsibleTypesResult = new List<Models.ClearConnection.ResposnsibleType>();
        

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
            var clearConnectionGetResposnsibleTypesResult = await ClearConnection.GetResposnsibleTypes();
            getResposnsibleTypesResult = (from x in clearConnectionGetResposnsibleTypesResult
                                            select new Models.ClearConnection.ResposnsibleType
                                            {
                                                RESPONSIBLE_ID=x.RESPONSIBLE_ID,
                                                NAME = x.NAME,
                                                RESPONSIBLE_VALUE = x.RESPONSIBLE_VALUE,
                                            }).ToList();



            //getResposnsibleTypesResult = clearConnectionGetResposnsibleTypesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddResposnsibleType>("Add Resposnsible Type", null);
            

            await InvokeAsync(() => { StateHasChanged(); });

            var clearConnectionGetResposnsibleTypesResult = await ClearConnection.GetResposnsibleTypes();
            getResposnsibleTypesResult = (from x in clearConnectionGetResposnsibleTypesResult
                                          select new Models.ClearConnection.ResposnsibleType
                                          {
                                              RESPONSIBLE_ID = x.RESPONSIBLE_ID,
                                              NAME = x.NAME,
                                              RESPONSIBLE_VALUE = x.RESPONSIBLE_VALUE,
                                          }).ToList();


        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/16";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            // UriHelper.NavigateTo("Help" + "/" + 16);
        }

        //protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.ResposnsibleType args)
        //{
        //    var dialogResult = await DialogService.OpenAsync<EditResposnsibleType>("Edit Resposnsible Type", new Dictionary<string, object>() { {"RESPONSIBLE_ID", args.RESPONSIBLE_ID} });
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
                    var clearConnectionDeleteResposnsibleTypeResult = await ClearConnection.DeleteResposnsibleType(data.RESPONSIBLE_ID);
                    if (clearConnectionDeleteResposnsibleTypeResult != null)
                    {
                        getResposnsibleTypesResult.Remove(getResposnsibleTypesResult.FirstOrDefault(x => x.RESPONSIBLE_ID == data.RESPONSIBLE_ID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Responsible Type successfully deleted.", 180000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteResposnsibleTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete ResposnsibleType",180000);
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditResposnsibleType>("Edit Resposnsible Type", new Dictionary<string, object>() { { "RESPONSIBLE_ID", data.RESPONSIBLE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });

            var clearConnectionGetResposnsibleTypesResult = await ClearConnection.GetResposnsibleTypes();
            getResposnsibleTypesResult = (from x in clearConnectionGetResposnsibleTypesResult
                                          select new Models.ClearConnection.ResposnsibleType
                                          {
                                              RESPONSIBLE_ID = x.RESPONSIBLE_ID,
                                              NAME = x.NAME,
                                              RESPONSIBLE_VALUE = x.RESPONSIBLE_VALUE,
                                          }).ToList();
        }
    }
}
