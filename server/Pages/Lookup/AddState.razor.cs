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
    public partial class AddState: ComponentBase
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

        [Parameter]
        public dynamic COUNTRYID { get; set; }

        protected RadzenContent content1;

        protected RadzenTemplateForm<State> form0;

        protected RadzenLabel idLabel;

        protected dynamic id;

        protected RadzenRequiredValidator idRequiredValidator;

        protected RadzenLabel statenameLabel;

        protected RadzenTextBox statename;

        protected RadzenRequiredValidator statenameRequiredValidator;

        protected RadzenButton button1;

        protected RadzenButton button2;

        State _state;
        protected State state
        {
            get
            {
                return _state;
            }
            set
            {
                if (!object.Equals(_state, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "state", NewValue = value, OldValue = _state };
                    _state = value;
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
            state = new State() { };
        }

        protected async System.Threading.Tasks.Task Form0Submit(State args)
        {
            state.COUNTRYID = int.Parse($"{COUNTRYID}");

            try
            {
                var clearRiskCreateStateResult = await ClearRisk.CreateState(state);
                DialogService.Close(state);
            }
            catch (System.Exception clearRiskCreateStateException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new State!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
