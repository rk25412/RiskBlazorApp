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
    public partial class SwmsTemplateComponent : ComponentBase
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

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SwmsTemplate> grid0;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial> grid1;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SwmsLicencespermit> grid2;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SwmsPlantequipment> grid3;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SwmsPperequired> grid4;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation> grid5;

        protected RadzenGrid<Clear.Risk.Models.ClearConnection.SwmsTemplatestep> grid6;

        IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplate> _getSwmsTemplatesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplate> getSwmsTemplatesResult
        {
            get
            {
                return _getSwmsTemplatesResult;
            }
            set
            {
                if (!object.Equals(_getSwmsTemplatesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSwmsTemplatesResult", NewValue = value, OldValue = _getSwmsTemplatesResult };
                    _getSwmsTemplatesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

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
                    var args = new PropertyChangedEventArgs(){ Name = "master", NewValue = value, OldValue = _master };
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
                 getSwmsTemplatesResult = clearConnectionGetSwmsTemplatesResult;
            }
            else
            {
                var clearConnectionGetSwmsTemplatesResult = await ClearConnection.GetSwmsTemplates(new Query() { Filter = $@"i => i.COMPANYID == {Security.getCompanyId()} || i.COUNTRY_ID == {Security.getCountryId()} " });
                getSwmsTemplatesResult = clearConnectionGetSwmsTemplatesResult;
            }

        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddSwmsTemplate>("Add Swms Template", null, new DialogOptions() { Width = $"{1000}px" });
            grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowExpand(Clear.Risk.Models.ClearConnection.SwmsTemplate args)
        {
            master = args;

            var clearConnectionGetSwmsHazardousmaterialsResult = await ClearConnection.GetSwmsHazardousmaterials(new Query() { Filter = $@"i => i.SWMSID == {args.SWMSID}" });
            if (clearConnectionGetSwmsHazardousmaterialsResult != null) {
                args.SwmsHazardousmaterials = clearConnectionGetSwmsHazardousmaterialsResult.ToList();
}

            var clearConnectionGetSwmsLicencespermitsResult = await ClearConnection.GetSwmsLicencespermits(new Query() { Filter = $@"i => i.SWMSID == {args.SWMSID}" });
            if (clearConnectionGetSwmsLicencespermitsResult != null) {
                args.SwmsLicencespermits = clearConnectionGetSwmsLicencespermitsResult.ToList();
}

            var clearConnectionGetSwmsPlantequipmentsResult = await ClearConnection.GetSwmsPlantequipments(new Query() { Filter = $@"i => i.SWMSID == {args.SWMSID}" });
            if (clearConnectionGetSwmsPlantequipmentsResult != null) {
                args.SwmsPlantequipments = clearConnectionGetSwmsPlantequipmentsResult.ToList();
}

            var clearConnectionGetSwmsPperequiredsResult = await ClearConnection.GetSwmsPperequireds(new Query() { Filter = $@"i => i.SWMSID == {args.SWMSID}" });
            if (clearConnectionGetSwmsPperequiredsResult != null) {
                args.SwmsPperequireds = clearConnectionGetSwmsPperequiredsResult.ToList();
}

            var clearConnectionGetSwmsReferencedlegislationsResult = await ClearConnection.GetSwmsReferencedlegislations(new Query() { Filter = $@"i => i.SWMSID == {args.SWMSID}" });
            if (clearConnectionGetSwmsReferencedlegislationsResult != null) {
                args.SwmsReferencedlegislations = clearConnectionGetSwmsReferencedlegislationsResult.ToList();
}

            var clearConnectionGetSwmsTemplatestepsResult = await ClearConnection.GetSwmsTemplatesteps(new Query() { Filter = $@"i => i.SWMSID == {args.SWMSID}" });
            if (clearConnectionGetSwmsTemplatestepsResult != null) {
                args.SwmsTemplatesteps = clearConnectionGetSwmsTemplatestepsResult.ToList();
}
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Clear.Risk.Models.ClearConnection.SwmsTemplate args)
        {
            var dialogResult = await DialogService.OpenAsync<EditSwmsTemplate>("Edit Swms Template", new Dictionary<string, object>() { {"SWMSID", args.SWMSID} }, new DialogOptions() { Width = $"{1000}px" });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearConnectionDeleteSwmsTemplateResult = await ClearConnection.DeleteSwmsTemplate(data.SWMSID);
                    if (clearConnectionDeleteSwmsTemplateResult != null) {
                        grid0.Reload();
}
                }
            }
            catch (System.Exception clearConnectionDeleteSwmsTemplateException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SwmsTemplate");
            }
        }

        protected async System.Threading.Tasks.Task SwmsHazardousmaterialAddButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSwmsHazardousmaterial>("Add Hazardous Materials", new Dictionary<string, object>() { {"SWMSID", data.SWMSID} }, new DialogOptions() { Width = $"{800}px" });
            grid1.Reload();
        }

        protected async System.Threading.Tasks.Task Grid1RowSelect(Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSwmsHazardousmaterial>("Edit Hazardous Materials", new Dictionary<string, object>() { {"HAZARDOUSMATERIALID", args.HAZARDOUSMATERIALID} }, new DialogOptions() { Width = $"{800}px" });
            grid1.Reload();
        }

        protected async System.Threading.Tasks.Task SwmsHazardousmaterialDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteSwmsHazardousmaterialResult = await ClearConnection.DeleteSwmsHazardousmaterial(data.HAZARDOUSMATERIALID);
                if (clearConnectionDeleteSwmsHazardousmaterialResult != null) {
                    grid1.Reload();
}
            }
            catch (System.Exception clearConnectionDeleteSwmsHazardousmaterialException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SwmsTemplate");
            }
        }

        protected async System.Threading.Tasks.Task SwmsLicencespermitAddButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSwmsLicencespermit>("Add  Licences & Permits", new Dictionary<string, object>() { {"SWMSID", data.SWMSID} }, new DialogOptions() { Width = $"{800}px" });
            grid2.Reload();
        }

        protected async System.Threading.Tasks.Task Grid2RowSelect(Clear.Risk.Models.ClearConnection.SwmsLicencespermit args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSwmsLicencespermit>("Edit Licences & Permits", new Dictionary<string, object>() { {"LPID", args.LPID} }, new DialogOptions() { Width = $"{800}px" });
            grid2.Reload();
        }

        protected async System.Threading.Tasks.Task SwmsLicencespermitDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteSwmsLicencespermitResult = await ClearConnection.DeleteSwmsLicencespermit(data.LPID);
                if (clearConnectionDeleteSwmsLicencespermitResult != null) {
                    grid2.Reload();
}
            }
            catch (System.Exception clearConnectionDeleteSwmsLicencespermitException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SwmsTemplate");
            }
        }

        protected async System.Threading.Tasks.Task SwmsPlantequipmentAddButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSwmsPlantequipment>("Add Plant and Equipment", new Dictionary<string, object>() { {"SWMSID", data.SWMSID} }, new DialogOptions() { Width = $"{800}px" });
            grid3.Reload();
        }

        protected async System.Threading.Tasks.Task Grid3RowSelect(Clear.Risk.Models.ClearConnection.SwmsPlantequipment args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSwmsPlantequipment>("Edit Plant and Equipment", new Dictionary<string, object>() { {"PEID", args.PEID} }, new DialogOptions() { Width = $"{800}px" });
            grid3.Reload();
        }

        protected async System.Threading.Tasks.Task SwmsPlantequipmentDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteSwmsPlantequipmentResult = await ClearConnection.DeleteSwmsPlantequipment(data.PEID);
                if (clearConnectionDeleteSwmsPlantequipmentResult != null) {
                    grid3.Reload();
}
            }
            catch (System.Exception clearConnectionDeleteSwmsPlantequipmentException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SwmsTemplate");
            }
        }

        protected async System.Threading.Tasks.Task SwmsPperequiredAddButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSwmsPperequired>("Add PPE Required", new Dictionary<string, object>() { {"SWMSID", data.SWMSID} }, new DialogOptions() { Width = $"{800}px" });
            grid4.Reload();
        }

        protected async System.Threading.Tasks.Task Grid4RowSelect(Clear.Risk.Models.ClearConnection.SwmsPperequired args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSwmsPperequired>("Edit PPE Required", new Dictionary<string, object>() { {"PPEID", args.PPEID} }, new DialogOptions() { Width = $"{800}px" });
            grid4.Reload();
        }

        protected async System.Threading.Tasks.Task SwmsPperequiredDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteSwmsPperequiredResult = await ClearConnection.DeleteSwmsPperequired(data.PPEID);
                if (clearConnectionDeleteSwmsPperequiredResult != null) {
                    grid4.Reload();
}
            }
            catch (System.Exception clearConnectionDeleteSwmsPperequiredException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SwmsTemplate");
            }
        }

        protected async System.Threading.Tasks.Task SwmsReferencedlegislationAddButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSwmsReferencedlegislation>("Add Referenced Legislation", new Dictionary<string, object>() { {"SWMSID", data.SWMSID} }, new DialogOptions() { Width = $"{800}px" });
            grid5.Reload();
        }

        protected async System.Threading.Tasks.Task Grid5RowSelect(Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSwmsReferencedlegislation>("Edit Referenced Legislation", new Dictionary<string, object>() { {"REFLID", args.REFLID} }, new DialogOptions() { Width = $"{800}px" });
            grid5.Reload();
        }

        protected async System.Threading.Tasks.Task SwmsReferencedlegislationDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var clearConnectionDeleteSwmsReferencedlegislationResult = await ClearConnection.DeleteSwmsReferencedlegislation(data.REFLID);
                if (clearConnectionDeleteSwmsReferencedlegislationResult != null) {
                    grid5.Reload();
}
            }
            catch (System.Exception clearConnectionDeleteSwmsReferencedlegislationException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete SwmsTemplate");
            }
        }

        protected async System.Threading.Tasks.Task SwmsTemplatestepAddButtonClick(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<AddSwmsTemplatestepComponent>("Add Template Steps", new Dictionary<string, object>() { {"SWMSID", data.SWMSID} }, new DialogOptions() { Width = $"{1000}px" });
            grid6.Reload();
        }

        protected async System.Threading.Tasks.Task Grid6RowSelect(Clear.Risk.Models.ClearConnection.SwmsTemplatestep args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditSwmsTemplatestep>("Edit Template Steps", new Dictionary<string, object>() { {"STEPID", args.STEPID} }, new DialogOptions() { Width = $"{1000}px" });
            grid6.Reload();
        }
    }
}
