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
using System.Security.Cryptography;
using System.Text;

namespace Clear.Risk.Pages.Company
{
    public partial class ChangePassword : ComponentBase
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
        protected bool isLoading { get; set; }
        protected Clear.Risk.Models.ClearConnection.Person person { get; set; }

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
        string oldPassword = "";
        string oldPasswordHash = "";
        protected async System.Threading.Tasks.Task Load()
        {
            var id = Security.getUserId();
            person = await ClearConnection.GetPersonByPersonId(id);
            oldPasswordHash = person.PASSWORDHASH;
            person.PASSWORDHASH = "";
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(args.PASSWORDHASH) || string.IsNullOrEmpty(args.ConfirmPassword))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Passwords fields cannot be empty", 180000);
                isLoading = false;
                return;
            }

            if (args.PASSWORDHASH != args.ConfirmPassword)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"New Passwords do not match", 180000);
                isLoading = false;
                return;
            }

            if(oldPasswordHash != HashPassword(oldPassword))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Old Passwords is wrong", 180000);
                isLoading = false;
                return;
            }

            args.PASSWORDHASH = HashPassword(args.PASSWORDHASH);

            try
            {
                var result = await ClearConnection.UpdatePerson(args.PERSON_ID, args);

                if (result != null)
                {
                    NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Employee Password changed successfully.", 180000);
                }
                DialogService.Close(null);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to change password!, " + ex.Message, 180000);
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }

        }

        protected string HashPassword(string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Encoding.ASCII.GetString(hash);
            }
        }

    }
}

