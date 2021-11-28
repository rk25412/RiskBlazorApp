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
using System.Globalization;

namespace Clear.Risk.Pages.Company
{
    public partial class ProfilePicture : ComponentBase
    {
        [Inject]
        protected UserManager<ApplicationUser> userManager { get; set; }
        [Inject]
        protected RoleManager<IdentityRole> roleManager { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }
        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }

        [Parameter]
        public dynamic PersonId { get; set; }
        protected Clear.Risk.Models.ClearConnection.Person person { get; set; }
        protected async System.Threading.Tasks.Task Load()
        {
            person = await ClearConnection.GetPersonByPersonId(Convert.ToInt32(PersonId));
        }

        RadzenUpload upload;
        int progress;
        bool fileLength = true;
        protected void OnProgress(UploadProgressArgs args, string name)
        {
            this.progress = args.Progress;

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    person.UPLOAD_PROFILE = $@"\ProfilePictures\{ file.Name}";
                    if (file.Size > 20480000 || file.Size < 1000)
                    {
                        fileLength = false;
                        return;
                    }
                }
            }
        }

        public async Task RemoveDoc()
        {
            person.UPLOAD_PROFILE = null;
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                StateHasChanged();
                await Task.Delay(1);
                await Load();
            }
        }

        protected async Task Form0Submit(Clear.Risk.Models.ClearConnection.Person args)
        {
            try
            {
                if (!fileLength)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Upload request denied. File is too large. Maximum size if 2MB!");
                    StateHasChanged();
                    return;
                }

                var updatePerson = await ClearConnection.UpdatePerson(args.PERSON_ID, args);
                if (updatePerson != null)
                {
                    NotificationService.Notify(NotificationSeverity.Info, $"Success", $"Profile Picture Updated successfully.", 180000);
                    DialogService.Close(null);
                }
            }
            catch(Exception ex)
            {

            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

    }
}
