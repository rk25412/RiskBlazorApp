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

namespace Clear.Risk.Pages
{
    public partial class PageElements : ComponentBase
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
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }


        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }
        protected RadzenDataList<HelpReference> datalist0;

        IEnumerable<HelpReference> _getHelpReferencesResult;
        protected IEnumerable<HelpReference> getHelpReferencesResult
        {
            get
            {
                return _getHelpReferencesResult;
            }
            set
            {
                if (!object.Equals(_getHelpReferencesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getHelpReferencesResult", NewValue = value, OldValue = _getHelpReferencesResult };
                    _getHelpReferencesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected bool isLoading { get; set; }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            //if (!Security.IsAuthenticated())
            //{
            //    UriHelper.NavigateTo("Login", true);
            //}
            //else
            //{
                isLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();

                isLoading = false;
                StateHasChanged();

            //}

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetHelpReferencesResult = await ClearConnection.GetHelpReferences();
            getHelpReferencesResult = clearConnectionGetHelpReferencesResult;
        }
    }
}
