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
    public partial class EditSwmsSectionComponent : ComponentBase
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

        [Parameter]
        public dynamic SWMS_SECTION_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.SwmsSection _swmssection;
        protected Clear.Risk.Models.ClearConnection.SwmsSection swmssection
        {
            get
            {
                return _swmssection;
            }
            set
            {
                if (!object.Equals(_swmssection, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "swmssection", NewValue = value, OldValue = _swmssection };
                    _swmssection = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetSwmsSectionBySwmsSectionIdResult = await ClearConnection.GetSwmsSectionBySwmsSectionId(SWMS_SECTION_ID);
            swmssection = clearConnectionGetSwmsSectionBySwmsSectionIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SwmsSection args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearConnectionUpdateSwmsSectionResult = await ClearConnection.UpdateSwmsSection(SWMS_SECTION_ID, swmssection);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(swmssection);
            }
            catch (System.Exception clearConnectionUpdateSwmsSectionException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update SwmsSection");
                IsLoading = false;
                StateHasChanged();

            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
