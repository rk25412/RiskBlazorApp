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
    public partial class TemplateComponent : ComponentBase
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

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.Template> grid0;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.Templateattachment> grid1;

        IEnumerable<Clear.Risk.Models.ClearConnection.Template> _getTemplatesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Template> getTemplatesResult
        {
            get
            {
                return _getTemplatesResult;
            }
            set
            {
                if (!object.Equals(_getTemplatesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTemplatesResult", NewValue = value, OldValue = _getTemplatesResult };
                    _getTemplatesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.Template _master;
        protected Clear.Risk.Models.ClearConnection.Template master
        {
            get
            {
                return _master;
            }
            set
            {
                if (!object.Equals(_master, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "master", NewValue = value, OldValue = _master };
                    _master = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Templateattachment> _Templateattachments;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Templateattachment> Templateattachments
        {
            get
            {
                return _Templateattachments;
            }
            set
            {
                if (!object.Equals(_Templateattachments, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "Templateattachments", NewValue = value, OldValue = _Templateattachments };
                    _Templateattachments = value;
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
            var clearConnectionGetTemplatesResult = await ClearConnection.GetTemplates();
            getTemplatesResult = clearConnectionGetTemplatesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddTemplate>("Add Template", null);
            grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            UriHelper.NavigateTo("Help" + "/" + 35);
        }
        protected async System.Threading.Tasks.Task Grid0RowDoubleClick(dynamic args)
        {
            DialogService.Open<EditTemplate>("Edit Template", new Dictionary<string, object>() { { "ID", args.ID } }, new DialogOptions() { Width = $"{1000}px" });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.Template args)
        {
            master = args;

            var clearConnectionGetTemplateattachmentsResult = await ClearConnection.GetTemplateattachments(new Query() { Filter = $@"i => i.TEMPLATEID == {args.ID}" });
            Templateattachments = clearConnectionGetTemplateattachmentsResult;
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteTemplateResult = await ClearConnection.DeleteTemplate(data.ID);
                    if (clearConnectionDeleteTemplateResult != null)
                    {
                        grid0.Reload();
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteTemplateException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Template");
            }
        }

        protected async System.Threading.Tasks.Task TemplateattachmentAddButtonClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddTemplateattachment>("Add Templateattachment", new Dictionary<string, object>() { { "TEMPLATEID", master.ID } });
            grid1.Reload();
        }

        protected async System.Threading.Tasks.Task Grid1RowSelect(Clear.Risk.Models.ClearConnection.Templateattachment args)
        {
            var dialogResult = await DialogService.OpenAsync<EditTemplateattachment>("Edit Templateattachment", new Dictionary<string, object>() { { "ID", args.ID } });
            grid1.Reload();
        }

        protected async System.Threading.Tasks.Task TemplateattachmentDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteTemplateattachmentResult = await ClearConnection.DeleteTemplateattachment(data.ID);
                if (clearConnectionDeleteTemplateattachmentResult != null)
                {
                    grid1.Reload();
                }
            }
            catch (System.Exception clearConnectionDeleteTemplateattachmentException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Template");
            }
        }
    }
}
