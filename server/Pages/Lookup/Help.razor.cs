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
    public partial class Help : ComponentBase
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
        protected ClearConnectionService ClearConnection { get; set; }

        //protected RadzenDataList<HelpReference> datalist0;

        IEnumerable<HowToUse> _getHowToUse;
        protected IEnumerable<HowToUse> getHowToUse
        {
            get
            {
                return _getHowToUse;
            }
            set
            {
                if (!object.Equals(_getHowToUse, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getHowToUse", NewValue = value, OldValue = _getHowToUse };
                    _getHowToUse = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddHowTo>("Add How To Use", null);
           
            await InvokeAsync(() => { StateHasChanged(); });
        }
        
        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, int id)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeleteHowToUse = await ClearConnection.DeleteHowToUse(id);
                    if (clearRiskDeleteHowToUse != null)
                    {
                        isLoading = false;
                        StateHasChanged();
                    }
                }
                isLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteHelpReferenceException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete");
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditHowTo>("Edit How To Use", new Dictionary<string, object>() { { "HowToId", data.HowToUseId } });

            await InvokeAsync(() => { StateHasChanged(); });
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
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetGetHowToUsesResult = await ClearConnection.GetHowToUses();
            getHowToUse = clearConnectionGetGetHowToUsesResult;
        }
    }
}
