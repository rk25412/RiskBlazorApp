using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;
using Clear.Risk.Models.ClearConnection;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.Web;

namespace Clear.Risk.Pages.Lookup
{
    public partial class EditHowTo : ComponentBase
    {



        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }
        [Inject]
        protected NotificationService NotificationService { get; set; }


        protected HowToUse howToUse { get; set; }

        [Parameter]
        public dynamic HowToId { get; set; }


        protected RadzenUpload upload1;
        protected RadzenUpload upload;

        protected bool IsLoading = false;

        protected override async Task OnInitializedAsync()
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


        protected async Task Load()
        {
            howToUse = await ClearRisk.GetHowToUseByHelpId(Convert.ToInt32(HowToId));
        }

        public async Task RemoveDoc()
        {
            howToUse.PdfPath = null;
        }

        public async Task RemoveDoc1()
        {
            howToUse.VideoPath = null;
        }

        int progress1;
        int progress;
        protected void OnProgress1(UploadProgressArgs args, string name)
        {
            this.progress1 = args.Progress;

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    howToUse.PdfPath = $@"\Upload\HowTo\pdf\{file.Name}";
                }
            }
        }

        protected void OnProgress(UploadProgressArgs args, string name)
        {
            this.progress = args.Progress;

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    howToUse.VideoPath = $@"\Upload\HowTo\Video\{file.Name}";
                }
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async Task Form0Submit(HowToUse args)
        {
            if(args != null)
            {

                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);

                try
                {

                    await ClearRisk.UpdateHowToUse(args.HowToUseId, args);
                    var fileExt = howToUse.PdfPath.Substring(howToUse.PdfPath.LastIndexOf('.'));

                    if (fileExt == ".pdf")
                    {
                        IsLoading = false;
                        StateHasChanged();
                        var fileExt1 = howToUse.VideoPath.Substring(howToUse.VideoPath.LastIndexOf('.'));
                        if (fileExt1 == ".mp4" || fileExt1 == ".ts" || fileExt1 == ".mov" || fileExt1 == ".flv" || fileExt1 == ".wmv" || fileExt1 == ".avi" || fileExt1 == ".avchd" || fileExt1 == ".omg" || fileExt1 == ".mpeg" || fileExt1 == ".mpg" || fileExt1 == ".ovg" || fileExt1 == ".asx" || fileExt1 == ".m4v" || fileExt1 == ".webm")
                        {
                            await ClearRisk.UpdateHowToUse(args.HowToUseId, args);
                            NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Updated Successfully!", 180000);
                            IsLoading = false;
                            StateHasChanged();
                            DialogService.Close(howToUse);
                        }
                        else
                        {
                            NotificationService.Notify(NotificationSeverity.Error, $"Error", $"File Extension Is InValid - Only Upload Video!", 180000);
                            IsLoading = false;
                            StateHasChanged();
                        }
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"File Extension Is InValid - Only Upload PDF!", 180000);
                        IsLoading = false;
                        StateHasChanged();
                    }
                }
                catch(Exception ex)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Something went Wrong. Unable to Update!", 180000);
                    IsLoading = false;
                    StateHasChanged();
                }
            }
        }
    }
}
