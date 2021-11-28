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
    public partial class AddTemplateattachmentComponent : ComponentBase
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
        public dynamic TEMPLATEID { get; set; }

        IEnumerable<Clear.Risk.Models.ClearConnection.TemplateType> _getTemplateTypesResult;

        protected IEnumerable<Clear.Risk.Models.ClearConnection.TemplateType> getTemplateTypesResult
        {
            get
            {
                return _getTemplateTypesResult;
            }
            set
            {
                if (!object.Equals(_getTemplateTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTemplateTypesResult", NewValue = value, OldValue = _getTemplateTypesResult };
                    _getTemplateTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.Templateattachment _templateattachment;
        protected Clear.Risk.Models.ClearConnection.Templateattachment templateattachment
        {
            get
            {
                return _templateattachment;
            }
            set
            {
                if (!object.Equals(_templateattachment, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "templateattachment", NewValue = value, OldValue = _templateattachment };
                    _templateattachment = value;
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
            var result = await ClearConnection.GetTemplateTypes();
            getTemplateTypesResult = result;

            templateattachment = new Clear.Risk.Models.ClearConnection.Templateattachment(){
                ISDELETED = false,
                STATUS = 1,
                TEMPLATETYPEID = 1
            };
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Templateattachment args)
        {
            templateattachment.TEMPLATEID = int.Parse($"{TEMPLATEID}");

            try
            {
                var clearConnectionCreateTemplateattachmentResult = await ClearConnection.CreateTemplateattachment(templateattachment);
                DialogService.Close(templateattachment);
            }
            catch (System.Exception clearConnectionCreateTemplateattachmentException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Templateattachment!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected int progress;
        protected string info;
        protected Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();
        protected RadzenUpload upload;

        
        protected void OnProgress(UploadProgressArgs args, string name)
        {
            this.info = $"% '{name}' / {args.Loaded} of {args.Total} bytes.";
            this.progress = args.Progress;

            if (args.Progress == 100)
            {
                events.Clear();
                foreach (var file in args.Files)
                {
                    templateattachment.DOCUMENTURL = file.Name;
                    
                    //upload.Upload();
                    events.Add(DateTime.Now, $"Uploaded: {file.Name} / {file.Size} bytes");
                }
            }
        }
    }
}
