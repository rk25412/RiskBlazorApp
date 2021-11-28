using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Clear.Risk.Models.ClearConnection;
using Blazored.TextEditor;
using Radzen.Blazor;

namespace Clear.Risk.Pages.Lookup
{
    public partial class AddBlogTable : ComponentBase
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
        BlogTable _addBlogTable;
        protected BlogTable addBlogTable
        {
            get
            {
                return _addBlogTable;
            }
            set
            {
                if (!object.Equals(_addBlogTable, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "addBlogTable", NewValue = value, OldValue = _addBlogTable };
                    _addBlogTable = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected BlazoredTextEditor QuillHtml;

        protected Clear.Risk.Models.ClearConnection.Person person;

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
            var personName = await ClearRisk.GetPersonByPersonId(Security.getUserId());
            addBlogTable = new BlogTable()
            {
                CreatedDate = DateTime.Now,
                CreatedBy = personName.FullName
            };
        }

        protected async System.Threading.Tasks.Task Form0Submit(BlogTable args)
        {

            try
            {
                args.BgLongDetails = await this.QuillHtml.GetHTML();
                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                var fileExt = addBlogTable.BgImgPath.Substring(addBlogTable.BgImgPath.LastIndexOf('.'));
                if (fileExt == ".tiff" || fileExt == ".pjp" || fileExt == ".jfif" || fileExt == ".gif" || fileExt == ".svg" || fileExt == ".bmp" || fileExt == ".png" || fileExt == ".jpeg" || fileExt == ".svgz" || fileExt == ".jpg" || fileExt == ".webp" || fileExt == ".ico" || fileExt == ".xbm" || fileExt == ".dib" || fileExt == ".tif" || fileExt == ".pjpeg" || fileExt == ".avif")
                {
                var clearRiskBlogTableResult = await ClearRisk.CreateBlogTable(addBlogTable);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(addBlogTable);
                }
                 else
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"File Extension Is InValid - Only Upload Image File!", 180000);
                    IsLoading = false;
                    StateHasChanged();
                }
            }
            catch (System.Exception clearRiskBlogTableException)
            {
                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new BlogTable!");
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected void InsertImageClick()
        {
            //FileManagerControl.SetShowFileManager(true);
        }

        protected async Task InsertImage(string paramImageURL)
        {
            await this.QuillHtml.InsertImage(paramImageURL);

            //FileManagerControl.SetShowFileManager(false);
        }

        string filename = Guid.NewGuid().ToString();

        RadzenUpload upload;
        int progress;
        protected void Change(UploadProgressArgs args, string name)
        {

            foreach (var file in args.Files)
            {
                addBlogTable.BgImgPath = $"{file.Name}";
            }
        }
        public async Task RemoveDoc()
        {
            addBlogTable.BgImgPath = null;
        }


        protected void OnProgress(UploadProgressArgs args)
        {
            this.progress = args.Progress;

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    addBlogTable.BgImgPath = $"{file.Name}";
                }
            }
        }

    }
}