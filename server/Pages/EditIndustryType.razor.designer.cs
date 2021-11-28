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
using Microsoft.AspNetCore.Identity;
using Clear.Risk.Models;

namespace Clear.Risk.Pages
{
    public partial class EditIndustryTypeComponent : ComponentBase
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
        public dynamic INDUSTRY_TYPE_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.IndustryType _industrytype;
        protected Clear.Risk.Models.ClearConnection.IndustryType industrytype
        {
            get
            {
                return _industrytype;
            }
            set
            {
                if (!object.Equals(_industrytype, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "industrytype", NewValue = value, OldValue = _industrytype };
                    _industrytype = value;
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
            var clearConnectionGetIndustryTypeByIndustryTypeIdResult = await ClearConnection.GetIndustryTypeByIndustryTypeId(int.Parse($"{INDUSTRY_TYPE_ID}"));
            industrytype = clearConnectionGetIndustryTypeByIndustryTypeIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.IndustryType args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);

            try
            {
                var clearConnectionUpdateIndustryTypeResult = await ClearConnection.UpdateIndustryType(int.Parse($"{INDUSTRY_TYPE_ID}"), industrytype);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(industrytype);
            }
            catch (System.Exception clearConnectionUpdateIndustryTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update IndustryType");
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
