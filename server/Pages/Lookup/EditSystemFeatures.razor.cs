using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Clear.Risk.Models.ClearConnection;
using Blazored.TextEditor;

namespace Clear.Risk.Pages.Lookup
{
    public partial class EditSystemFeatures : ComponentBase
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

        [Parameter]
        public dynamic Feature_ID { get; set; }

        SystemFeatures _editSystemFeatures;

        protected SystemFeatures editSystemFeatures
        {
            get
            {
                return _editSystemFeatures;
            }
            set
            {
                if (!object.Equals(_editSystemFeatures, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "editSystemFeatures", NewValue = value, OldValue = _editSystemFeatures };
                    _editSystemFeatures = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected BlazoredTextEditor QuillHtml;

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
            var clearRiskGetSystemFeaturesByFeature_ID = await ClearRisk.GetSystemFeaturesByFeature_ID(int.Parse($"{Feature_ID}"));
            editSystemFeatures = clearRiskGetSystemFeaturesByFeature_ID;

        }

        protected async System.Threading.Tasks.Task Form0Submit(SystemFeatures args)
        {


            try
            {
                args.Html_Content = await this.QuillHtml.GetHTML();
                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                var clearRiskUpdateSystemFeatures = await ClearRisk.UpdateSystemFeatures(int.Parse($"{Feature_ID}"), editSystemFeatures);
                IsLoading = false;
                StateHasChanged();
                DialogService.Close(editSystemFeatures);
            }
            catch (System.Exception clearRiskupdateSystemFeaturesException)
            {
                IsLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update SystemFeatures");
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
    }
}
