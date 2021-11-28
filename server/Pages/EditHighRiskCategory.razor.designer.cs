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
    public partial class EditHighRiskCategoryComponent : ComponentBase
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
        public dynamic RISK_CATEGORY_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.HighRiskCategory _highriskcategory;
        protected Clear.Risk.Models.ClearConnection.HighRiskCategory highriskcategory
        {
            get
            {
                return _highriskcategory;
            }
            set
            {
                if (!object.Equals(_highriskcategory, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "highriskcategory", NewValue = value, OldValue = _highriskcategory };
                    _highriskcategory = value;
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
            var clearConnectionGetHighRiskCategoryByRiskCategoryIdResult = await ClearConnection.GetHighRiskCategoryByRiskCategoryId(RISK_CATEGORY_ID);
            highriskcategory = clearConnectionGetHighRiskCategoryByRiskCategoryIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.HighRiskCategory args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearConnectionUpdateHighRiskCategoryResult = await ClearConnection.UpdateHighRiskCategory(RISK_CATEGORY_ID, highriskcategory);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(highriskcategory);
            }
            catch (System.Exception clearConnectionUpdateHighRiskCategoryException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update HighRiskCategory");
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
