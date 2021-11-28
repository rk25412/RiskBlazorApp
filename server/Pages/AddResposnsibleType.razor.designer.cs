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
    public partial class AddResposnsibleTypeComponent : ComponentBase
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

        Clear.Risk.Models.ClearConnection.ResposnsibleType _resposnsibletype;
        protected Clear.Risk.Models.ClearConnection.ResposnsibleType resposnsibletype
        {
            get
            {
                return _resposnsibletype;
            }
            set
            {
                if (!object.Equals(_resposnsibletype, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "resposnsibletype", NewValue = value, OldValue = _resposnsibletype };
                    _resposnsibletype = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected bool IsLoading { get; set; }
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
            resposnsibletype = new Clear.Risk.Models.ClearConnection.ResposnsibleType(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.ResposnsibleType args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                var clearConnectionCreateResposnsibleTypeResult = await ClearConnection.CreateResposnsibleType(resposnsibletype);
                IsLoading = false;
                StateHasChanged();

                DialogService.Close(resposnsibletype);
            }
            catch (System.Exception clearConnectionCreateResposnsibleTypeException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new ResposnsibleType!");
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
