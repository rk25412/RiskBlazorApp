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
    public partial class EditPpevalueComponent : ComponentBase
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
        public dynamic PPE_ID { get; set; }
        protected bool IsLoading { get; set; }
        Clear.Risk.Models.ClearConnection.Ppevalue _ppevalue;
        protected Clear.Risk.Models.ClearConnection.Ppevalue ppevalue
        {
            get
            {
                return _ppevalue;
            }
            set
            {
                if (!object.Equals(_ppevalue, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "ppevalue", NewValue = value, OldValue = _ppevalue };
                    _ppevalue = value;
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
        protected bool fileLength = true;
        protected void Change(UploadProgressArgs args, string name)
        {
            fileLength = true;
            foreach (var file in args.Files)
            {
                ppevalue.ICONPATH = $"{file.Name}";
                if (file.Size > 10615)
                {
                    fileLength = false;
                    return;
                }
                
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetPpevalueByPpeIdResult = await ClearConnection.GetPpevalueByPpeId(PPE_ID);
            ppevalue = clearConnectionGetPpevalueByPpeIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Ppevalue args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (!fileLength)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Icon request denied. Icon is too large. Maximum size if 10.3 Kb!", 50000);
                    IsLoading = false;
                    StateHasChanged();
                    return;
                }
                var clearConnectionUpdatePpevalueResult = await ClearConnection.UpdatePpevalue(PPE_ID, ppevalue);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(ppevalue);
            }
            catch (System.Exception clearConnectionUpdatePpevalueException)
            {
                
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Ppevalue");
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
