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
    public partial class EditConsequenceComponent : ComponentBase
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
        public dynamic CONSEQUENCE_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.Consequence _consequence;
        protected Clear.Risk.Models.ClearConnection.Consequence consequence
        {
            get
            {
                return _consequence;
            }
            set
            {
                if (!object.Equals(_consequence, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "consequence", NewValue = value, OldValue = _consequence };
                    _consequence = value;
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
            var clearConnectionGetConsequenceByConsequenceIdResult = await ClearConnection.GetConsequenceByConsequenceId(CONSEQUENCE_ID);
            consequence = clearConnectionGetConsequenceByConsequenceIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Consequence args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);

            try
            {
                var clearConnectionUpdateConsequenceResult = await ClearConnection.UpdateConsequence(CONSEQUENCE_ID, consequence);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(consequence);
            }
            catch (System.Exception clearConnectionUpdateConsequenceException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Consequence");
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
