﻿using System;
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

namespace Clear.Risk.Pages.Lookup
{
    public partial class ManageProcessType: ComponentBase
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


        protected bool IsLoading { get; set; }

        protected IList<ProcessType> getProcessTypesResult = new List<ProcessType>();
        

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
            var clearRiskGetProcessTypesResult = await ClearRisk.GetProcessTypes();
            getProcessTypesResult = (from x in clearRiskGetProcessTypesResult
                                     select new ProcessType{
                                     PROCESS_TYPE_ID = x.PROCESS_TYPE_ID,
                                     NAME = x.NAME
                                     }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddProcessType>("Add Process Type", null);
              //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/31";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 31);
        }
        

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeleteProcessTypeResult = await ClearRisk.DeleteProcessType(int.Parse($"{data.PROCESS_TYPE_ID}"));
                    if (clearRiskDeleteProcessTypeResult != null)
                    {
                        getProcessTypesResult.Remove(getProcessTypesResult.FirstOrDefault(x => x.PROCESS_TYPE_ID == data.PROCESS_TYPE_ID));
                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteProcessTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete ProcessType");
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditProcessType>("Edit Process Type", new Dictionary<string, object>() { { "PROCESS_TYPE_ID", data.PROCESS_TYPE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
    }
}
