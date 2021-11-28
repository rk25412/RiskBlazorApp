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
    public partial class EditCountry: ComponentBase
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
        public dynamic ID { get; set; }

        protected RadzenContent content1;

        protected RadzenTemplateForm<Country> form0;

        protected RadzenLabel idLabel;

        protected dynamic id;

        protected RadzenRequiredValidator idRequiredValidator;

        protected RadzenLabel countrynameLabel;

        protected RadzenTextBox countryname;

        protected RadzenRequiredValidator countrynameRequiredValidator;

        protected RadzenLabel shortnameLabel;

        protected RadzenTextBox shortname;

        protected RadzenRequiredValidator shortnameRequiredValidator;

        protected RadzenButton button1;

        protected RadzenButton button2;

        Country _country;
        protected Country country
        {
            get
            {
                return _country;
            }
            set
            {
                if (!object.Equals(_country, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "country", NewValue = value, OldValue = _country };
                    _country = value;
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
            var clearRiskGetCountryByIdResult = await ClearRisk.GetCountryById(ID);
            country = clearRiskGetCountryByIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Country args)
        {
            try
            {
                var clearRiskUpdateCountryResult = await ClearRisk.UpdateCountry(ID, country);
                DialogService.Close(country);
            }
            catch (System.Exception clearRiskUpdateCountryException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Country");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
