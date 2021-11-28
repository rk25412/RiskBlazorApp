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
namespace Clear.Risk.Pages.Lookup
{
    public partial class DocumentManagement : ComponentBase
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
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }


        [Inject]
        protected ClearConnectionService ClearRisk { get; set; }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddDocument>("Upload Document", null);
            //await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {

            string url = "/Help/38";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            //UriHelper.NavigateTo("Help" + "/" + 38);
        }

        protected bool IsLoading { get; set; }
        //IEnumerable<Clear.Risk.Models.ClearConnection.CompanyDocumentFile> _getCompanyDocumentFile;
        protected IList<Clear.Risk.Models.ClearConnection.CompanyDocumentFile> getCompanyDocumentFileResult = new List<CompanyDocumentFile>();

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
            var clearRiskGetCompanyDocumentResult = await ClearRisk.GetCompanyDocumentFiles(new Query() { Filter = $@"i => i.COMPANY_ID == {Security.getCompanyId()}" });

            getCompanyDocumentFileResult = (from x in clearRiskGetCompanyDocumentResult
                                            select new CompanyDocumentFile
                                            {
                                                DOCUMENTID = x.DOCUMENTID,
                                                FILENAME = x.FILENAME,
                                                VERSION_NUMBER = x.VERSION_NUMBER,
                                                CREATED_DATE = x.CREATED_DATE,
                                                DOCUMENTNAME = x.DOCUMENTNAME,
                                                CreatedBy = x.CreatedBy,
                                            }).ToList();

            //getCompanyDocumentFileResult = clearRiskGetCompanyDocumentResult;
        }
        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            IsLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeleteSurveyTypeResult = await ClearRisk.DeleteCompanyDocumentFile(int.Parse($"{data.DOCUMENTID}"));
                    if (clearRiskDeleteSurveyTypeResult != null)
                    {
                        getCompanyDocumentFileResult.Remove(getCompanyDocumentFileResult.FirstOrDefault(i => i.DOCUMENTID == data.DOCUMENTID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Document is successfully deleted.");
                    }
                }
            }
            catch (System.Exception clearRiskDeleteSurveyTypeException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"The document is already referenced to the system.");
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditDocument>("Edit Document", new Dictionary<string, object>() { { "DOCUMENTID", data.DOCUMENTID } });
            await InvokeAsync(() => { StateHasChanged(); });

            await Load();
        }
    }
}
