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
    public partial class ManageSWMS : ComponentBase
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


        protected IList<Clear.Risk.Models.ClearConnection.SwmsTemplate> getSwmsTemplatesResult = new List<Clear.Risk.Models.ClearConnection.SwmsTemplate>();

        dynamic _master;
        protected dynamic master
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
        protected bool isLoading { get; set; }
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
        protected async System.Threading.Tasks.Task Load()
        {
            if (Security.IsInRole("System Administrator"))
            {
                var clearConnectionGetSwmsTemplatesResult = await ClearConnection.GetSwmsTemplates();

                getSwmsTemplatesResult = (from x in clearConnectionGetSwmsTemplatesResult
                                          select new Clear.Risk.Models.ClearConnection.SwmsTemplate
                                          {
                                              SWMSID = x.SWMSID,
                                              Person = x.Person,
                                              TemplateType = x.TemplateType,
                                              TEMPLATENAME = x.TEMPLATENAME,
                                              VERSION = x.VERSION,
                                              IS_DRAFT = x.IS_DRAFT,
                                          }).ToList();

                //getSwmsTemplatesResult = clearConnectionGetSwmsTemplatesResult;
            }
            else
            {
                var clearConnectionGetSwmsTemplatesResult = await ClearConnection.GetSwmsTemplates(new Query() { Filter = $@"i => i.COMPANYID == {Security.getCompanyId()} || i.COUNTRY_ID == {Security.getCountryId()} " });

                getSwmsTemplatesResult = (from x in clearConnectionGetSwmsTemplatesResult
                                          select new Clear.Risk.Models.ClearConnection.SwmsTemplate
                                          {
                                              SWMSID = x.SWMSID,
                                              Person = x.Person,
                                              TemplateType = x.TemplateType,
                                              TEMPLATENAME = x.TEMPLATENAME,
                                              VERSION = x.VERSION,
                                              IS_DRAFT = x.IS_DRAFT,
                                          }).ToList();


                //getSwmsTemplatesResult = clearConnectionGetSwmsTemplatesResult;
            }

        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            UriHelper.NavigateTo("AddSwms");            
        }
        protected async System.Threading.Tasks.Task HelpClick(MouseEventArgs args)
        {
            string url = "/Help/4";
            await JSRuntime.InvokeAsync<object>("open", url, "_blank");

            // UriHelper.NavigateTo("Help" + "/" + 4);
        }        
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {           
            UriHelper.NavigateTo("EditSwms" + "/" + data.SWMSID);
        }
        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    isLoading = true;
                    var clearConnectionDeleteSwmsTemplateResult = await ClearConnection.DeleteSwmsTemplate(data.SWMSID);
                    if (clearConnectionDeleteSwmsTemplateResult != null)
                    {
                        getSwmsTemplatesResult.Remove(getSwmsTemplatesResult.FirstOrDefault(x => x.SWMSID == data.SWMSID));
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", "Template successfully deleted", 180000);
                    }
                }
            }
            catch (System.Exception clearConnectionDeleteSwmsTemplateException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SwmsTemplate", 180000);
            }
            finally
            {
                isLoading = false;
            }
        }

    }
}
