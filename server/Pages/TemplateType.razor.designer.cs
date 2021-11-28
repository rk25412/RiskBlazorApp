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
    public partial class TemplateTypeComponent : ComponentBase
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


        protected bool IsLoading { get; set; }

        protected IList<Clear.Risk.Models.ClearConnection.TemplateType> getTemplateTypesResult = new List<Clear.Risk.Models.ClearConnection.TemplateType>();
        

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
            var clearConnectionGetTemplateTypesResult = await ClearConnection.GetTemplateTypes();
            getTemplateTypesResult = clearConnectionGetTemplateTypesResult.Select(x => new Clear.Risk.Models.ClearConnection.TemplateType
            {
                TEMPLATE_TYPE_ID = x.TEMPLATE_TYPE_ID,
                NAME = x.NAME,
                ISACTIVE = x.ISACTIVE
            }).ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddTemplateType>("Add Template Type", null);
            //grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/28";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 28);
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
                    var clearConnectionDeleteTemplateTypeResult = await ClearConnection.DeleteTemplateType(data.TEMPLATE_TYPE_ID);
                    if (clearConnectionDeleteTemplateTypeResult != null)
                    {
                        getTemplateTypesResult.Remove(getTemplateTypesResult.FirstOrDefault(x => x.TEMPLATE_TYPE_ID == data.TEMPLATE_TYPE_ID));
                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                IsLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearConnectionDeleteTemplateTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete TemplateType");
                IsLoading = false;
                StateHasChanged();

            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditTemplateType>("Edit Template Type", new Dictionary<string, object>() { { "TEMPLATE_TYPE_ID", data.TEMPLATE_TYPE_ID } });
            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
    }
}
