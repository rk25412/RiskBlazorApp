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
using Microsoft.AspNetCore.Hosting;

namespace Clear.Risk.Pages.Lookup
{
    public partial class AddDocument : ComponentBase
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
        protected bool IsLoading { get; set; }
        protected CompanyDocumentFile _companyDocumentFile;
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
                await Load();
            }

        }


        protected void GetVersionNoByName()
        {
            if (string.IsNullOrEmpty(companyDocumentFile.DOCUMENTNAME))
            {
                companyDocumentFile.VERSION_NUMBER = string.Empty;
                return;
            }

            foreach (var item in getCompanyDocumentFileResult.OrderByDescending(i => i.CREATED_DATE))
            {
                if (item.DOCUMENTNAME.ToLower().Contains(companyDocumentFile.DOCUMENTNAME.ToLower()))
                {
                    companyDocumentFile.VERSION_NUMBER = (Convert.ToDouble(item.VERSION_NUMBER) + 1).ToString();

                    if (!companyDocumentFile.VERSION_NUMBER.Contains('.'))
                        companyDocumentFile.VERSION_NUMBER += ".0";

                    return;
                }
                companyDocumentFile.VERSION_NUMBER = "1.0";
            }
        }


        protected IList<Clear.Risk.Models.ClearConnection.CompanyDocumentFile> getCompanyDocumentFileResult;
        protected async System.Threading.Tasks.Task Load()
        {
            int companyId = Security.getCompanyId();

            var clearRiskGetCompanyDocumentResult = await ClearRisk.GetCompanyDocumentFiles(new Query() { Filter = $@"i => i.COMPANY_ID == {Security.getCompanyId()}" });

            getCompanyDocumentFileResult = clearRiskGetCompanyDocumentResult.ToList();

            companyDocumentFile = new CompanyDocumentFile()
            {
                CREATED_DATE = DateTime.Now,
                CREATOR_ID = Security.getUserId(),
                UPDATED_DATE = DateTime.Now,
                UPDATER_ID = Security.getUserId(),
                COMPANY_ID = companyId,
            };
            companyDocumentFile.DOCUMENT_URL = @"UploadDocument\" + companyId.ToString();
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
                    companyDocumentFile.FILENAME = filename + fileExt;
                    var clearRiskCreateProcessTypeResult = await ClearRisk.UpladCompanyDocumentFile(companyDocumentFile);
                    IsLoading = false;
                    StateHasChanged();
                    DialogService.Close(companyDocumentFile);
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"File Extension Is InValid - Only Upload WORD/PDF/EXCEL/jpg File!", 180000);
                    IsLoading = false;
                    StateHasChanged();
                }
            }
            catch (System.Exception clearRiskCreateProcessTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to Upload new Document!", 180000);
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected bool fileLength = true;
        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        string filename;

        RadzenUpload upload;
        int progress;
        protected void Change(UploadProgressArgs args, string name)
        {

            foreach (var file in args.Files)
            {
                companyDocumentFile.FILENAME = $"{file.Name}";
                if (file.Size > 20480000 || file.Size < 1000)
                {
                    fileLength = false;
                    return;
                }
            }
        }
        public async Task RemoveDoc()
        {
            companyDocumentFile.FILENAME = null;
        }


        protected void OnProgress(UploadProgressArgs args)
        {
            this.progress = args.Progress;

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    companyDocumentFile.FILENAME = $"{file.Name}";

                    if (string.IsNullOrEmpty(companyDocumentFile.DOCUMENTNAME))
                    {
                        companyDocumentFile.DOCUMENTNAME = file.Name.Substring(0, file.Name.LastIndexOf('.'));
                        foreach (var item in getCompanyDocumentFileResult.OrderByDescending(i => i.CREATED_DATE))
                        {
                            if (item.DOCUMENTNAME.ToLower().Contains(companyDocumentFile.DOCUMENTNAME.ToLower()))
                            {
                                companyDocumentFile.VERSION_NUMBER = (Convert.ToDouble(item.VERSION_NUMBER) + 1).ToString();

                                if (!companyDocumentFile.VERSION_NUMBER.Contains('.'))
                                    companyDocumentFile.VERSION_NUMBER += ".0";

                                return;
                            }
                            companyDocumentFile.VERSION_NUMBER = "1.0";
                        }
                    }

                    if (file.Size > 2048000 || file.Size < 1000)
                    {
                        fileLength = false;
                        return;
                    }
                }
            }
        }

        async Task OnUploadChange(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                foreach (var item in getCompanyDocumentFileResult.OrderByDescending(i => i.CREATED_DATE))
                {
                    double version = 1.0;
                    string fileName = file.Name.Substring(0, file.Name.LastIndexOf('.'));

                    if (item.FILENAME.ToLower().Contains(fileName.ToLower()))
                    {
                        version = Convert.ToDouble(item.VERSION_NUMBER);
                        fileName = file.Name.Substring(0, file.Name.LastIndexOf('.'));

                        filename = fileName + "-v" + (version + 1);
                        return;
                    }

                    filename = fileName + "-v" + version;
                }
            }
        }

    }
}

