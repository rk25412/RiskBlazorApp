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

namespace Clear.Risk.Pages.Lookup
{
    public partial class EditDocument : ComponentBase
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
        protected ClearConnectionService ClearRisk { get; set; }
        [Parameter]
        public dynamic DOCUMENTID { get; set; }
        
        protected bool IsLoading { get; set; }
        CompanyDocumentFile _companyDocumentFile;
        protected CompanyDocumentFile companyDocumentFile
        {
            get
            {
                return _companyDocumentFile;
            }
            set
            {
                if (!object.Equals(_companyDocumentFile, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "companyDocumentFile", NewValue = value, OldValue = _companyDocumentFile };
                    _companyDocumentFile = value;
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
                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                IsLoading = false;
                StateHasChanged();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearRiskGetCompanyDocumentIdResult = await ClearRisk.GetCompanyDocumentFileId(int.Parse($"{DOCUMENTID}"));
            companyDocumentFile = clearRiskGetCompanyDocumentIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(CompanyDocumentFile args)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (!fileLength)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Upload request denied. File is too large. Maximum size if 2MB!");
                    IsLoading = false;
                    StateHasChanged();
                    return;
                }
                var fileExt = companyDocumentFile.FILENAME.Substring(companyDocumentFile.FILENAME.LastIndexOf('.'));
                if (fileExt == ".jpg" || fileExt == ".doc" || fileExt == ".docx" || fileExt == ".pdf" || fileExt == ".jpeg" || fileExt == ".xls" || fileExt == ".xlsx")
                {
                    if (!string.IsNullOrEmpty(filename))
                    {
                        companyDocumentFile.FILENAME = filename;
                    }

                    var clearRiskCreateProcessTypeResult = await ClearRisk.UpdateCompanyDocumentFile(int.Parse($"{DOCUMENTID}"), companyDocumentFile);
                    IsLoading = false;
                    StateHasChanged();
                    DialogService.Close(companyDocumentFile);
                }
            }
            catch (System.Exception clearRiskCreateProcessTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new ProcessType!");
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
        protected void Change(UploadProgressArgs args, string name)
        {
            foreach (var file in args.Files)
            {
                companyDocumentFile.FILENAME = $"{file.Name}";
            }
        }

        RadzenUpload upload;
        int progress;

        string filename;

        public async Task RemoveDoc()
        {
            companyDocumentFile.FILENAME = null;
            filename = Guid.NewGuid().ToString();
        }

        bool fileLength = true;

        protected void OnProgress(UploadProgressArgs args, string name)
        {
            this.progress = args.Progress;

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    companyDocumentFile.FILENAME = $"{file.Name}";
                    if (file.Size > 2048000 || file.Size < 1000)
                    {
                        fileLength = false;
                        return;
                    }
                }
            }
        }
    }
}