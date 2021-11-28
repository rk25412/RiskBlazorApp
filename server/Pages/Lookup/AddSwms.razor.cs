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
using Clear.Risk.Infrastructures.Helpers;
using System.Diagnostics.Eventing.Reader;
using Clear.Risk.ViewModels;


namespace Clear.Risk.Pages.Lookup
{
    public partial class AddSwms : ComponentBase
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
        protected Swmsotherdetail otherdetail { get; set; }

        string filename = Guid.NewGuid().ToString();

        public ICollection<SwmsTemplatestep> SwmsTemplatesteps { get; set; }

        protected SwmsTemplatestep stepTemplate { get; set; }

        IEnumerable<Clear.Risk.Models.ClearConnection.Person> _getPeopleResult;

        //protected RadzenGrid<Clear.Risk.Models.ClearConnection.SwmsTemplatestep> grid6;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Person> getPeopleResult
        {
            get
            {
                return _getPeopleResult;
            }
            set
            {
                if (!object.Equals(_getPeopleResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPeopleResult", NewValue = value, OldValue = _getPeopleResult };
                    _getPeopleResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

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

        IEnumerable<Clear.Risk.Models.ClearConnection.StatusMaster> _getStatusMastersResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.StatusMaster> getStatusMastersResult
        {
            get
            {
                return _getStatusMastersResult;
            }
            set
            {
                if (!object.Equals(_getStatusMastersResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getStatusMastersResult", NewValue = value, OldValue = _getStatusMastersResult };
                    _getStatusMastersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplateCategory> _getSwmsTemplateCategoriesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplateCategory> getSwmsTemplateCategoriesResult
        {
            get
            {
                return _getSwmsTemplateCategoriesResult;
            }
            set
            {
                if (!object.Equals(_getSwmsTemplateCategoriesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getSwmsTemplateCategoriesResult", NewValue = value, OldValue = _getSwmsTemplateCategoriesResult };
                    _getSwmsTemplateCategoriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.Country> _getCountriesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Country> getCountriesResult
        {
            get
            {
                return _getCountriesResult;
            }
            set
            {
                if (!object.Equals(_getCountriesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCountriesResult", NewValue = value, OldValue = _getCountriesResult };
                    _getCountriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.EscalationLevel> _getEscalationLevelsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.EscalationLevel> getEscalationLevelsResult
        {
            get
            {
                return _getEscalationLevelsResult;
            }
            set
            {
                if (!object.Equals(_getEscalationLevelsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getEscalationLevelsResult", NewValue = value, OldValue = _getEscalationLevelsResult };
                    _getEscalationLevelsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.WarningLevel> _getWarningLevelsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.WarningLevel> getWarningLevelsResult
        {
            get
            {
                return _getWarningLevelsResult;
            }
            set
            {
                if (!object.Equals(_getWarningLevelsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getWarningLevelsResult", NewValue = value, OldValue = _getWarningLevelsResult };
                    _getWarningLevelsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.StatusLevel> _getStatusLevelsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.StatusLevel> getStatusLevelsResult
        {
            get
            {
                return _getStatusLevelsResult;
            }
            set
            {
                if (!object.Equals(_getStatusLevelsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getStatusLevelsResult", NewValue = value, OldValue = _getStatusLevelsResult };
                    _getStatusLevelsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.State> _getStatesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.State> getStatesResult
        {
            get
            {
                return _getStatesResult;
            }
            set
            {
                if (!object.Equals(_getStatesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getStatesResult", NewValue = value, OldValue = _getStatesResult };
                    _getStatesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.TradeCategory> _getTradeCategoriesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.TradeCategory> getTradeCategoriesResult
        {
            get
            {
                return _getTradeCategoriesResult;
            }
            set
            {
                if (!object.Equals(_getTradeCategoriesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTradeCategoriesResult", NewValue = value, OldValue = _getTradeCategoriesResult };
                    _getTradeCategoriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        Clear.Risk.Models.ClearConnection.SwmsTemplate _swmstemplate;
        protected Clear.Risk.Models.ClearConnection.SwmsTemplate swmstemplate
        {
            get
            {
                return _swmstemplate;
            }
            set
            {
                if (!object.Equals(_swmstemplate, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "swmstemplate", NewValue = value, OldValue = _swmstemplate };
                    _swmstemplate = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplate> _swmstobasenew;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.SwmsTemplate> swmstobasenew
        {
            get
            {
                return _swmstobasenew;
            }
            set
            {
                if (!object.Equals(_swmstobasenew, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "swmstobasenew", NewValue = value, OldValue = _swmstobasenew };
                    _swmstobasenew = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Clear.Risk.Models.ClearConnection.RiskLikelyhood> _getRiskLikelyhoodsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.RiskLikelyhood> getRiskLikelyhoodsResult
        {
            get
            {
                return _getRiskLikelyhoodsResult;
            }
            set
            {
                if (!object.Equals(_getRiskLikelyhoodsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getRiskLikelyhoodsResult", NewValue = value, OldValue = _getRiskLikelyhoodsResult };
                    _getRiskLikelyhoodsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.Consequence> _getConsequencesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Consequence> getConsequencesResult
        {
            get
            {
                return _getConsequencesResult;
            }
            set
            {
                if (!object.Equals(_getConsequencesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getConsequencesResult", NewValue = value, OldValue = _getConsequencesResult };
                    _getConsequencesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.ResposnsibleType> _getResposnsibleTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.ResposnsibleType> getResposnsibleTypesResult
        {
            get
            {
                return _getResposnsibleTypesResult;
            }
            set
            {
                if (!object.Equals(_getResposnsibleTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getResposnsibleTypesResult", NewValue = value, OldValue = _getResposnsibleTypesResult };
                    _getResposnsibleTypesResult = value;
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
                isLoading = true;
                StateHasChanged();
                await Task.Delay(1);
                await Load();
                isLoading = false;
                StateHasChanged();

            }

        }

        IEnumerable<Clear.Risk.Models.ClearConnection.PlantEquipment> _getPlantEquipmentsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.PlantEquipment> getPlantEquipmentsResult
        {
            get
            {
                return _getPlantEquipmentsResult;
            }
            set
            {
                if (!object.Equals(_getPlantEquipmentsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPlantEquipmentsResult", NewValue = value, OldValue = _getPlantEquipmentsResult };
                    _getPlantEquipmentsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.LicencePermit> _getLicencePermitsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.LicencePermit> getLicencePermitsResult
        {
            get
            {
                return _getLicencePermitsResult;
            }
            set
            {
                if (!object.Equals(_getLicencePermitsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLicencePermitsResult", NewValue = value, OldValue = _getLicencePermitsResult };
                    _getLicencePermitsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.Ppevalue> _getPpevaluesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Ppevalue> getPpevaluesResult
        {
            get
            {
                return _getPpevaluesResult;
            }
            set
            {
                if (!object.Equals(_getPpevaluesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPpevaluesResult", NewValue = value, OldValue = _getPpevaluesResult };
                    _getPpevaluesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.ReferencedLegislation> _getReferencedLegislationsResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.ReferencedLegislation> getReferencedLegislationsResult
        {
            get
            {
                return _getReferencedLegislationsResult;
            }
            set
            {
                if (!object.Equals(_getReferencedLegislationsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getReferencedLegislationsResult", NewValue = value, OldValue = _getReferencedLegislationsResult };
                    _getReferencedLegislationsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.HazardMaterialValue> _getHazardMaterialValuesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.HazardMaterialValue> getHazardMaterialValuesResult
        {
            get
            {
                return _getHazardMaterialValuesResult;
            }
            set
            {
                if (!object.Equals(_getHazardMaterialValuesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getHazardMaterialValuesResult", NewValue = value, OldValue = _getHazardMaterialValuesResult };
                    _getHazardMaterialValuesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.Template> _getTemplateResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.Template> getTemplateResult
        {
            get
            {
                return _getTemplateResult;
            }
            set
            {
                if (!object.Equals(_getTemplateResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTemplateResult", NewValue = value, OldValue = _getTemplateResult };
                    _getTemplateResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.ImpactType> _getImpactTypesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.ImpactType> getImpactTypesResult
        {
            get
            {
                return _getImpactTypesResult;
            }
            set
            {
                if (!object.Equals(_getImpactTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getImpactTypesResult", NewValue = value, OldValue = _getImpactTypesResult };
                    _getImpactTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.HazardValue> _getHazardValuesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.HazardValue> getHazardValuesResult
        {
            get
            {
                return _getHazardValuesResult;
            }
            set
            {
                if (!object.Equals(_getHazardValuesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getHazardValuesResult", NewValue = value, OldValue = _getHazardValuesResult };
                    _getHazardValuesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.ControlMeasureValue> _getControlMeasureValuesResult;
        protected IEnumerable<Clear.Risk.Models.ClearConnection.ControlMeasureValue> getControlMeasureValuesResult
        {
            get
            {
                return _getControlMeasureValuesResult;
            }
            set
            {
                if (!object.Equals(_getControlMeasureValuesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getControlMeasureValuesResult", NewValue = value, OldValue = _getControlMeasureValuesResult };
                    _getControlMeasureValuesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected bool isLoading { get; set; }
        protected async System.Threading.Tasks.Task Load()
        {

            //var clearConnectionGetPeopleResult = await ClearConnection.GetPeople(new Query() { Filter = $@"i => i.PARENT_PERSON_ID == {Security.getCompanyId()} &&  i.COMPANYTYPE == 6 " });
            //getPeopleResult = clearConnectionGetPeopleResult;

            if (Security.IsInRole("System Administrator"))
            {
                getPeopleResult = await ClearConnection.GetEmployee(new Query() { Filter = "i => i.ISMANAGER == true" });
            }
            else
            {
                getPeopleResult = await ClearConnection.GetEmployee(Security.getCompanyId(), new Query() { Filter = "i => i.ISMANAGER == true" });
            }

            var clearConnectionGetTemplateTypesResult = await ClearConnection.GetTemplateTypes();
            getTemplateTypesResult = clearConnectionGetTemplateTypesResult;

            var clearConnectionGetStatusMastersResult = await ClearConnection.GetStatusMasters();
            getStatusMastersResult = clearConnectionGetStatusMastersResult;

            var clearConnectionGetSwmsTemplateCategoriesResult = await ClearConnection.GetSwmsTemplateCategories();
            getSwmsTemplateCategoriesResult = clearConnectionGetSwmsTemplateCategoriesResult;

            var clearConnectionGetCountriesResult = await ClearConnection.GetCountries();
            getCountriesResult = clearConnectionGetCountriesResult;

            var clearConnectionGetEscalationLevelsResult = await ClearConnection.GetEscalationLevels();
            getEscalationLevelsResult = clearConnectionGetEscalationLevelsResult;

            var clearConnectionGetWarningLevelsResult = await ClearConnection.GetWarningLevels();
            getWarningLevelsResult = clearConnectionGetWarningLevelsResult;

            var clearConnectionGetStatusLevelsResult = await ClearConnection.GetStatusLevels();
            getStatusLevelsResult = clearConnectionGetStatusLevelsResult;

            var clearConnectionGetStatesResult = await ClearConnection.GetStates();
            getStatesResult = clearConnectionGetStatesResult;

            var clearConnectionGetTradeCategoriesResult = await ClearConnection.GetTradeCategories();
            getTradeCategoriesResult = clearConnectionGetTradeCategoriesResult;



            if (Security.IsInRole("System Administrator"))
            {
                var clearConnectionGetTemplateToBaseNewResult = await ClearConnection.GetSwmsTemplates();
                swmstobasenew = clearConnectionGetTemplateToBaseNewResult;
            }
            else
            {
                var clearConnectionGetTemplateToBaseNewResult = await ClearConnection.GetSwmsTemplates(new Query() { Filter = $@"i => i.COMPANYID == {Security.getCompanyId()} || i.COUNTRY_ID == {Security.getCountryId()} " });
                swmstobasenew = clearConnectionGetTemplateToBaseNewResult;
            }


            var clearConnectionGetRiskLikelyhoodsResult = await ClearConnection.GetRiskLikelyhoods();
            getRiskLikelyhoodsResult = clearConnectionGetRiskLikelyhoodsResult;

            var clearConnectionGetConsequencesResult = await ClearConnection.GetConsequences();
            getConsequencesResult = clearConnectionGetConsequencesResult;

            var clearConnectionGetResposnsibleTypesResult = await ClearConnection.GetResposnsibleTypes();
            getResposnsibleTypesResult = clearConnectionGetResposnsibleTypesResult;

            if (Security.IsInRole("System Administrator"))
            {
                getTemplateResult = await ClearConnection.GetTemplateList();
            }
            else
            {
                getTemplateResult = await ClearConnection.GetTemplateList(new Query() { Filter = $@"i => i.COMPANYID == {Security.getCompanyId()} || i.COUNTRY_ID == {Security.getCountryId()} " });

            }

            int companyId = Security.getUserId();
            swmstemplate = new Clear.Risk.Models.ClearConnection.SwmsTemplate()
            {

                //COMPANYID = companyId,
                CREATED_DATE = DateTime.Now,
                TEMPLATEQUESTION = 2,
                STATUS = 1,
                CREATOR_ID = Security.getUserId(),
                UPDATED_DATE = DateTime.Now,
                UPDATER_ID = Security.getUserId(),
                ESCALATION_LEVEL_ID = 1,
                WARNING_LEVEL_ID = 1,
                STATUS_LEVEL_ID = 1,
                IS_DELETED = false,
                VERSION = "1",
                TRADECATEGORYID = 73,
                SWMSTYPE = 1,
            };

            if (Security.IsInRole("System Administrator"))
            {
                swmstemplate.TEMPLATEQUESTION = 2;
            }
            else
            {
                swmstemplate.TEMPLATEQUESTION = 1;
                swmstemplate.COMPANYID = companyId;
            }

            if (swmstemplate.SwmsTemplatesteps == null)
                swmstemplate.SwmsTemplatesteps = new List<SwmsTemplatestep>();

            SwmsTemplatesteps = swmstemplate.SwmsTemplatesteps;
            otherdetail = new Swmsotherdetail();

            var clearConnectionGetPlantEquipmentsResult = await ClearConnection.GetPlantEquipments();
            getPlantEquipmentsResult = clearConnectionGetPlantEquipmentsResult;

            var clearConnectionGetLicencePermitsResult = await ClearConnection.GetLicencePermits();
            getLicencePermitsResult = clearConnectionGetLicencePermitsResult;

            var clearConnectionGetPpevaluesResult = await ClearConnection.GetPpevalues();
            getPpevaluesResult = clearConnectionGetPpevaluesResult;

            var clearConnectionGetReferencedLegislationsResult = await ClearConnection.GetReferencedLegislations();
            getReferencedLegislationsResult = clearConnectionGetReferencedLegislationsResult;

            var clearConnectionGetHazardMaterialValuesResult = await ClearConnection.GetHazardMaterialValues();
            getHazardMaterialValuesResult = clearConnectionGetHazardMaterialValuesResult;

            var clearConnectionGetImpactTypesResult = await ClearConnection.GetImpactTypes();
            getImpactTypesResult = clearConnectionGetImpactTypesResult;

            var clearConnectionGetHazardValuesResult = await ClearConnection.GetHazardValues();
            getHazardValuesResult = clearConnectionGetHazardValuesResult;

            var clearConnectionGetControlMeasureValuesResult = await ClearConnection.GetControlMeasureValues();
            getControlMeasureValuesResult = clearConnectionGetControlMeasureValuesResult;

        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.SwmsTemplate args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                string fileExt = string.Empty;

                if (!string.IsNullOrEmpty(swmstemplate.RiskAssessmentDoc))
                {
                    if (!fileLength)
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Upload request denied. File is too large. Maximum size if 2MB!");
                        StateHasChanged();
                        return;
                    }

                    fileExt = args.RiskAssessmentDoc.Substring(args.RiskAssessmentDoc.LastIndexOf('.'));

                    string[] allowedExtension = { ".jpg", ".doc", ".docx", ".pdf", ".jpeg", ".xls", ".xlsx" };

                    if (!allowedExtension.Contains(fileExt.ToLower()))
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Invalid File Selected - Only Upload WORD/PDF/EXCEL/jpg File!", 180000);
                        StateHasChanged();
                        return;
                    }
                }

                if (args != null)
                {
                    if (!string.IsNullOrEmpty(swmstemplate.RiskAssessmentDoc))
                    {
                        swmstemplate.RiskAssessmentDoc = filename + fileExt;
                        args.RiskAssessmentDoc = filename + fileExt;
                    }

                    if (otherdetail != null)
                    {
                        //Add Plant & equipment
                        if (otherdetail.plants != null)
                        {
                            foreach (int i in otherdetail.plants)
                            {
                                if (args.SwmsPlantequipments == null)
                                    args.SwmsPlantequipments = new List<SwmsPlantequipment>();

                                args.SwmsPlantequipments.Add(new SwmsPlantequipment
                                {
                                    PLANT_EQUIPMENT_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                        //Add PPE 
                        if (otherdetail.ppes != null)
                        {
                            foreach (int i in otherdetail.ppes)
                            {
                                if (args.SwmsPperequireds == null)
                                    args.SwmsPperequireds = new List<SwmsPperequired>();

                                args.SwmsPperequireds.Add(new SwmsPperequired
                                {
                                    PPE_VALUE_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                        //Add Licence Permit 
                        if (otherdetail.licenes != null)
                        {
                            foreach (int i in otherdetail.licenes)
                            {
                                if (args.SwmsLicencespermits == null)
                                    args.SwmsLicencespermits = new List<SwmsLicencespermit>();

                                args.SwmsLicencespermits.Add(new SwmsLicencespermit
                                {
                                    LICENCE_PERMIT_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                        //Add Referenced Legislation
                        if (otherdetail.legislations != null)
                        {
                            foreach (int i in otherdetail.legislations)
                            {
                                if (args.SwmsReferencedlegislations == null)
                                    args.SwmsReferencedlegislations = new List<SwmsReferencedlegislation>();

                                args.SwmsReferencedlegislations.Add(new SwmsReferencedlegislation
                                {
                                    REFERENCE_LEGISLATION_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                        //Add Hazard Material
                        if (otherdetail.materialsHazardous != null)
                        {
                            foreach (int i in otherdetail.materialsHazardous)
                            {
                                if (args.SwmsHazardousmaterials == null)
                                    args.SwmsHazardousmaterials = new List<SwmsHazardousmaterial>();

                                args.SwmsHazardousmaterials.Add(new SwmsHazardousmaterial
                                {
                                    HAZARD_MATERIAL_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                    }

                    var templateName = swmstobasenew.Where(s => s.TEMPLATENAME == swmstemplate.TEMPLATENAME).FirstOrDefault();
                    if (templateName != null)
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Duplicate Template name are not allowed.");
                    }
                    else
                    {
                        var clearConnectionCreateSwmsTemplateResult = await ClearConnection.CreateSwmsTemplate(swmstemplate);
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"SWMS Template created successfully!");
                        //UriHelper.NavigateTo("EditSwms" + "/" +clearConnectionCreateSwmsTemplateResult.SWMSID);
                        UriHelper.NavigateTo("swms-template");
                    }
                }

            }
            catch (System.Exception clearConnectionCreateSwmsTemplateException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SwmsTemplate!");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
                await Task.Delay(1);
            }
        }

        protected int? newstepno;

        void ChangeStepNo(dynamic data)
        {
            if ((int)data > newstepno || (int)data < 0)
            {
                stepTemplate.STEPNO = newstepno;
            }
        }


        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            stepTemplate = new SwmsTemplatestep();
            stepTemplate.ISDELETE = false;
            stepTemplate.IsAdd = true;
            stepTemplate.STEPNO = swmstemplate.SwmsTemplatesteps.ToList().Count() + 1;
            newstepno = swmstemplate.SwmsTemplatesteps.ToList().Count() + 1;
            isLoading = false;
            isAddEdit = true;
            StateHasChanged();
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            ///UriHelper.NavigateTo("AddSwms");  
            stepTemplate = null;
            isAddEdit = false;
            newstepno = null;
            StateHasChanged();
        }
        protected async System.Threading.Tasks.Task CancelClick(MouseEventArgs args)
        {
            UriHelper.NavigateTo("swms-template");
        }
        protected async System.Threading.Tasks.Task ButtonBackClick(MouseEventArgs args)
        {
            if (await DialogService.Confirm("Are you sure, You want to go back, You'll lose your data") == true)
            {
                UriHelper.NavigateTo("swms-template");
            }
        }

        protected async Task SaveAsDraft()
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (string.IsNullOrEmpty(swmstemplate.TEMPLATENAME))
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Template Name is Required");
                    StateHasChanged();
                    return;
                }

                if (swmstemplate.TEMPLATE_ID <= 0)
                {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Please Select Template ID");
                    StateHasChanged();
                    return;
                }


                if (!string.IsNullOrEmpty(swmstemplate.RiskAssessmentDoc))
                {
                    if (!fileLength)
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Upload request denied. File is too large. Maximum size if 2MB!");
                        StateHasChanged();
                        return;
                    }

                    string fileExt = swmstemplate.RiskAssessmentDoc.Substring(swmstemplate.RiskAssessmentDoc.LastIndexOf('.'));

                    string[] allowedExtension = { ".jpg", ".doc", ".docx", ".pdf", ".jpeg", ".xls", ".xlsx" };

                    if (!allowedExtension.Contains(fileExt.ToLower()))
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Invalid File Selected - Only Upload WORD/PDF/EXCEL/jpg File!", 180000);
                        StateHasChanged();
                        return;
                    }
                }

                if (swmstemplate != null)
                {
                    if (swmstemplate.REFERENCETEMPLATEID >= 0 || swmstemplate.REFERENCETEMPLATEID != null)
                    {
                        if (swmstemplate.SwmsTemplatesteps == null || swmstemplate.SwmsTemplatesteps.Count() == 0)
                        {
                            await ChangeTemplate(1, "index");
                        }
                    }

                    if (otherdetail != null)
                    {
                        //Add Plant & equipment
                        if (otherdetail.plants != null)
                        {
                            foreach (int i in otherdetail.plants)
                            {
                                if (swmstemplate.SwmsPlantequipments == null)
                                    swmstemplate.SwmsPlantequipments = new List<SwmsPlantequipment>();

                                swmstemplate.SwmsPlantequipments.Add(new SwmsPlantequipment
                                {
                                    PLANT_EQUIPMENT_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                        //Add PPE 
                        if (otherdetail.ppes != null)
                        {
                            foreach (int i in otherdetail.ppes)
                            {
                                if (swmstemplate.SwmsPperequireds == null)
                                    swmstemplate.SwmsPperequireds = new List<SwmsPperequired>();

                                swmstemplate.SwmsPperequireds.Add(new SwmsPperequired
                                {
                                    PPE_VALUE_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                        //Add Licence Permit 
                        if (otherdetail.licenes != null)
                        {
                            foreach (int i in otherdetail.licenes)
                            {
                                if (swmstemplate.SwmsLicencespermits == null)
                                    swmstemplate.SwmsLicencespermits = new List<SwmsLicencespermit>();

                                swmstemplate.SwmsLicencespermits.Add(new SwmsLicencespermit
                                {
                                    LICENCE_PERMIT_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                        //Add Referenced Legislation
                        if (otherdetail.legislations != null)
                        {
                            foreach (int i in otherdetail.legislations)
                            {
                                if (swmstemplate.SwmsReferencedlegislations == null)
                                    swmstemplate.SwmsReferencedlegislations = new List<SwmsReferencedlegislation>();

                                swmstemplate.SwmsReferencedlegislations.Add(new SwmsReferencedlegislation
                                {
                                    REFERENCE_LEGISLATION_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                        //Add Hazard Material
                        if (otherdetail.materialsHazardous != null)
                        {
                            foreach (int i in otherdetail.materialsHazardous)
                            {
                                if (swmstemplate.SwmsHazardousmaterials == null)
                                    swmstemplate.SwmsHazardousmaterials = new List<SwmsHazardousmaterial>();

                                swmstemplate.SwmsHazardousmaterials.Add(new SwmsHazardousmaterial
                                {
                                    HAZARD_MATERIAL_ID = i,
                                    IS_DELETED = false
                                });
                            }
                        }
                    }

                    swmstemplate.IS_DRAFT = true;

                    var templateName = swmstobasenew.Where(s => s.TEMPLATENAME == swmstemplate.TEMPLATENAME).FirstOrDefault();
                    if (templateName != null)
                    {
                        NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Duplicate Template name are not allowed.");
                    }
                    else
                    {
                        var clearConnectionCreateSwmsTemplateResult = await ClearConnection.CreateSwmsTemplate(swmstemplate);
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"SWMS Template created successfully!");
                        //UriHelper.NavigateTo("EditSwms" + "/" +clearConnectionCreateSwmsTemplateResult.SWMSID);
                        UriHelper.NavigateTo("swms-template");
                    }
                }


            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SwmsTemplate!");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
                await Task.Delay(1);
            }
        }

        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        protected int? tempSwmsId { get; set; }

        void ChangeTemplateTobase(object value, string name)
        {
            if (value != null)
            {
                var result = swmstobasenew.Where(x => x.SWMSID == (int)value).FirstOrDefault();
                if (result != null)
                {
                    swmstemplate.TEMPLATENAME = result.TEMPLATENAME;

                    if (swmstemplate.REFERENCETEMPLATEID != null)
                        swmstemplate.VERSION = Convert.ToString((Convert.ToInt32(result.VERSION)) + 1);
                    else
                        swmstemplate.VERSION = "1";
                }
            }
            StateHasChanged();
        }
        void ChangeRISK_LIKELYHOOD(object value, string name)
        {
            if (value != null)
            {
                var result = getRiskLikelyhoodsResult.Where(x => x.RISK_VALUE_ID == (int)value).FirstOrDefault();
                if (result != null)
                {
                    if (stepTemplate.RISK_CONTRL_SCORE != null)
                    {
                        if (stepTemplate.CONSEQUENCE_ID != null)
                        {
                            var cresult = getConsequencesResult.Where(x => x.CONSEQUENCE_ID == (int)stepTemplate.CONSEQUENCE_ID).FirstOrDefault();
                            stepTemplate.RISK_CONTRL_SCORE = result.RISK_VALUE * cresult.CONSEQUENCE_VALUE;
                        }
                        else
                        {
                            stepTemplate.RISK_CONTRL_SCORE = result.RISK_VALUE;
                        }
                    }
                    else
                    {
                        stepTemplate.RISK_CONTRL_SCORE = result.RISK_VALUE;
                    }
                }
            }
            else
            {
                if (stepTemplate.RISK_CONTRL_SCORE != null)
                {
                    if (stepTemplate.CONSEQUENCE_ID != null)
                    {
                        var cresult = getConsequencesResult.Where(x => x.CONSEQUENCE_ID == (int)stepTemplate.CONSEQUENCE_ID).FirstOrDefault();
                        stepTemplate.RISK_CONTRL_SCORE = cresult.CONSEQUENCE_VALUE;
                    }
                    else
                    {
                        stepTemplate.RISK_CONTRL_SCORE = null;
                    }
                }
            }

            StateHasChanged();
        }
        void ChangeCONSEQUENCE_ID(object value, string name)
        {
            if (value != null)
            {
                var result = getConsequencesResult.Where(x => x.CONSEQUENCE_ID == (int)value).FirstOrDefault();
                if (result != null)
                {
                    if (stepTemplate.RISK_CONTRL_SCORE != null)
                    {
                        if (stepTemplate.RISK_LIKELYHOOD_ID != null)
                        {
                            var cresult = getRiskLikelyhoodsResult.Where(x => x.RISK_VALUE_ID == (int)stepTemplate.RISK_LIKELYHOOD_ID).FirstOrDefault();
                            stepTemplate.RISK_CONTRL_SCORE = result.CONSEQUENCE_VALUE * cresult.RISK_VALUE;
                        }
                        else
                        {
                            stepTemplate.RISK_CONTRL_SCORE = result.CONSEQUENCE_VALUE;
                        }

                    }
                    else
                    {
                        stepTemplate.RISK_CONTRL_SCORE = result.CONSEQUENCE_VALUE;
                    }
                }

            }
            else
            {
                if (stepTemplate.RISK_CONTRL_SCORE != null)
                {
                    if (stepTemplate.RISK_LIKELYHOOD_ID != null)
                    {
                        var cresult = getRiskLikelyhoodsResult.Where(x => x.RISK_VALUE_ID == (int)stepTemplate.RISK_LIKELYHOOD_ID).FirstOrDefault();
                        stepTemplate.RISK_CONTRL_SCORE = cresult.RISK_VALUE;
                    }
                    else
                    {
                        stepTemplate.RISK_CONTRL_SCORE = null;
                    }
                }
            }

            StateHasChanged();
        }
        void ChangeRISK_AFTER_LIKELYHOOD_ID(object value, string name)
        {
            if (value != null)
            {
                var result = getRiskLikelyhoodsResult.Where(x => x.RISK_VALUE_ID == (int)value).FirstOrDefault();
                if (result != null)
                {
                    if (stepTemplate.AFTER_RISK_CONTROL_SCORE != null)
                    {
                        if (stepTemplate.AFTER_CONSEQUENCE_ID != null)
                        {
                            var cresult = getConsequencesResult.Where(x => x.CONSEQUENCE_ID == (int)stepTemplate.AFTER_CONSEQUENCE_ID).FirstOrDefault();
                            stepTemplate.AFTER_RISK_CONTROL_SCORE = result.RISK_VALUE * cresult.CONSEQUENCE_VALUE;
                        }
                        else
                        {
                            stepTemplate.AFTER_RISK_CONTROL_SCORE = result.RISK_VALUE;
                        }

                    }
                    else
                    {
                        stepTemplate.AFTER_RISK_CONTROL_SCORE = result.RISK_VALUE;
                    }
                }
            }
            else
            {
                if (stepTemplate.AFTER_RISK_CONTROL_SCORE != null)
                {
                    if (stepTemplate.AFTER_CONSEQUENCE_ID != null)
                    {
                        var cresult = getConsequencesResult.Where(x => x.CONSEQUENCE_ID == (int)stepTemplate.AFTER_CONSEQUENCE_ID).FirstOrDefault();
                        stepTemplate.AFTER_RISK_CONTROL_SCORE = cresult.CONSEQUENCE_VALUE;
                    }
                    else
                    {
                        stepTemplate.AFTER_RISK_CONTROL_SCORE = null;
                    }
                }
            }
            StateHasChanged();
        }

        void ChangeAFTER_CONSEQUENCE_ID(object value, string name)
        {
            if (value != null)
            {
                var result = getConsequencesResult.Where(x => x.CONSEQUENCE_ID == (int)value).FirstOrDefault();
                if (result != null)
                {
                    if (stepTemplate.AFTER_RISK_CONTROL_SCORE != null)
                    {
                        if (stepTemplate.RISK_AFTER_LIKELYHOOD_ID != null)
                        {
                            var cresult = getRiskLikelyhoodsResult.Where(x => x.RISK_VALUE_ID == (int)stepTemplate.RISK_AFTER_LIKELYHOOD_ID).FirstOrDefault();
                            stepTemplate.AFTER_RISK_CONTROL_SCORE = result.CONSEQUENCE_VALUE * cresult.RISK_VALUE;
                        }
                        else
                        {
                            stepTemplate.AFTER_RISK_CONTROL_SCORE = result.CONSEQUENCE_VALUE;
                        }

                    }
                    else
                    {
                        stepTemplate.AFTER_RISK_CONTROL_SCORE = result.CONSEQUENCE_VALUE;
                    }
                }
            }
            else
            {
                if (stepTemplate.AFTER_RISK_CONTROL_SCORE != null)
                {
                    if (stepTemplate.RISK_AFTER_LIKELYHOOD_ID != null)
                    {
                        var cresult = getRiskLikelyhoodsResult.Where(x => x.RISK_VALUE_ID == (int)stepTemplate.RISK_AFTER_LIKELYHOOD_ID).FirstOrDefault();
                        stepTemplate.AFTER_RISK_CONTROL_SCORE = cresult.RISK_VALUE;
                    }
                    else
                    {
                        stepTemplate.AFTER_RISK_CONTROL_SCORE = null;
                    }
                }
            }

            StateHasChanged();
        }
        protected async System.Threading.Tasks.Task SaveClick(MouseEventArgs args)
        {
            //            if (
            //                stepTemplate.STEPNO == null || stepTemplate.TASKCATEGEORY == null || stepTemplate.HAZARD == null || stepTemplate.HEALTHIMPACT == null ||
            //stepTemplate.RISK_LIKELYHOOD_ID == null || stepTemplate.CONSEQUENCE_ID == null || stepTemplate.RISK_CONTRL_SCORE == null || stepTemplate.CONTROLLINGHAZARDS == null ||
            //stepTemplate.RISK_AFTER_LIKELYHOOD_ID == null || stepTemplate.AFTER_CONSEQUENCE_ID == null || stepTemplate.AFTER_RISK_CONTROL_SCORE == null || stepTemplate.RESPOSNSIBLE_TYPE_ID == null)
            //            {
            //                isAddEdit = true;
            //                return;
            //            }


            if (stepTemplate.STEPNO == null || stepTemplate.STEPNO <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Step No is required", 1800000);
                return;
            }
            else if (string.IsNullOrEmpty(stepTemplate.TASKCATEGEORY))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Task Category is required.", 1800000);
                return;
            }
            else if (string.IsNullOrEmpty(stepTemplate.HAZARD))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Hazard is required.", 1800000);
                return;
            }
            else if (string.IsNullOrEmpty(stepTemplate.HEALTHIMPACT))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Health Impact is required.", 1800000);
                return;
            }
            else if (stepTemplate.RISK_LIKELYHOOD_ID == null || stepTemplate.RISK_LIKELYHOOD_ID <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Risk Before Control Likelyhood.", 1800000);
                return;
            }
            else if (stepTemplate.CONSEQUENCE_ID == null || stepTemplate.CONSEQUENCE_ID <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Risk Before Control Consequence is required", 1800000);
                return;
            }
            else if (string.IsNullOrEmpty(stepTemplate.CONTROLLINGHAZARDS))
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Methods of Controlling Hazards is required", 1800000);
                return;
            }
            else if (stepTemplate.RISK_AFTER_LIKELYHOOD_ID == 0 || stepTemplate.RISK_AFTER_LIKELYHOOD_ID <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Risk After Control Likelyhood is required", 1800000);
                return;
            }
            else if (stepTemplate.AFTER_CONSEQUENCE_ID == null || stepTemplate.AFTER_CONSEQUENCE_ID <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Risk After Control Consequence is required", 1800000);
                return;
            }
            else if (stepTemplate.RESPOSNSIBLE_TYPE_ID == null || stepTemplate.RESPOSNSIBLE_TYPE_ID <= 0)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Who is Responsible is required", 1800000);
                return;
            }

            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);

            try
            {

                if (stepTemplate.IsAdd)
                {
                    if (swmstemplate.SwmsTemplatesteps == null)
                        swmstemplate.SwmsTemplatesteps = new List<SwmsTemplatestep>();

                    var RiskLikelyBefor = getRiskLikelyhoodsResult.Where(r => r.RISK_VALUE_ID == stepTemplate.RISK_LIKELYHOOD_ID).FirstOrDefault();
                    var RiskLikelyAfter = getRiskLikelyhoodsResult.Where(r => r.RISK_VALUE_ID == stepTemplate.RISK_AFTER_LIKELYHOOD_ID).FirstOrDefault();
                    var conseqResult = getConsequencesResult.Where(c => c.CONSEQUENCE_ID == stepTemplate.CONSEQUENCE_ID).FirstOrDefault();
                    var conseqAfterResult = getConsequencesResult.Where(c => c.CONSEQUENCE_ID == stepTemplate.AFTER_CONSEQUENCE_ID).FirstOrDefault();
                    var responsibleResult = getResposnsibleTypesResult.Where(c => c.RESPONSIBLE_ID == stepTemplate.RESPOSNSIBLE_TYPE_ID).FirstOrDefault();
                    string hazards = string.Empty;



                    /*-----------------------------------------------------*/
                    if (!string.IsNullOrEmpty(stepTemplate.HAZARD))
                    {
                        string[] hazardArr = stepTemplate.HAZARD.Split(',');
                        stepTemplate.SpecificHazards = hazardArr;
                    }
                    if (!string.IsNullOrEmpty(stepTemplate.CONTROLLINGHAZARDS))
                    {
                        string[] ctrlHazard = stepTemplate.CONTROLLINGHAZARDS.Split(',');
                        stepTemplate.Controllings = ctrlHazard;
                    }
                    if (!string.IsNullOrEmpty(stepTemplate.HEALTHIMPACT))
                    {
                        string[] impactArr = stepTemplate.HEALTHIMPACT.Split(',');
                        stepTemplate.HealthImpacts = impactArr;
                    }
                    /*-----------------------------------------------------*/




                    if (stepTemplate.SpecificHazards != null)
                    {
                        foreach (var s in stepTemplate.SpecificHazards)
                        {
                            if (string.IsNullOrEmpty(hazards))
                                hazards = s;
                            else
                                hazards = hazards + "," + s;
                        }
                    }

                    string impactResult = string.Empty;
                    if (stepTemplate.HealthImpacts != null)
                    {
                        foreach (var s in stepTemplate.HealthImpacts)
                        {
                            if (string.IsNullOrEmpty(impactResult))
                                impactResult = s;
                            else
                                impactResult = impactResult + "," + s;
                        }
                    }

                    string controlResult = string.Empty;
                    if (stepTemplate.Controllings != null)
                    {
                        foreach (var s in stepTemplate.Controllings)
                        {
                            if (string.IsNullOrEmpty(controlResult))
                                controlResult = s;
                            else
                                controlResult = controlResult + "," + s;
                        }
                    }

                    foreach (var item in swmstemplate.SwmsTemplatesteps)
                    {
                        if (item.STEPNO >= stepTemplate.STEPNO)
                            item.STEPNO += 1;
                    }


                    swmstemplate.SwmsTemplatesteps.Add(new SwmsTemplatestep
                    {
                        TASKCATEGEORY = stepTemplate.TASKCATEGEORY,
                        HAZARD = hazards,
                        HEALTHIMPACT = impactResult,
                        SWMSTYPE = stepTemplate.SWMSTYPE,
                        RISK_LIKELYHOOD_ID = stepTemplate.RISK_LIKELYHOOD_ID,
                        RiskLikelyHoodBeforeName = RiskLikelyBefor != null ? RiskLikelyBefor.NAME : string.Empty,
                        CONSEQUENCE_ID = stepTemplate.CONSEQUENCE_ID,
                        ConsequenceName = conseqResult != null ? conseqResult.NAME : string.Empty,
                        RISK_CONTRL_SCORE = stepTemplate.RISK_CONTRL_SCORE,
                        CONTROLLINGHAZARDS = controlResult,
                        RISK_AFTER_LIKELYHOOD_ID = stepTemplate.RISK_AFTER_LIKELYHOOD_ID,
                        RiskLikelyHoodAfterName = RiskLikelyAfter != null ? RiskLikelyAfter.NAME : string.Empty,
                        AFTER_CONSEQUENCE_ID = stepTemplate.AFTER_CONSEQUENCE_ID,
                        ConsequenceAfterName = conseqAfterResult != null ? conseqAfterResult.NAME : string.Empty,
                        RESPOSNSIBLE_TYPE_ID = stepTemplate.RESPOSNSIBLE_TYPE_ID,
                        ResposnsibleTypeName = responsibleResult != null ? responsibleResult.NAME : string.Empty,
                        AFTER_RISK_CONTROL_SCORE = stepTemplate.AFTER_RISK_CONTROL_SCORE,
                        STATUS = stepTemplate.STATUS,
                        ISDELETE = stepTemplate.ISDELETE,
                        STEPNO = stepTemplate.STEPNO
                    });



                    //swmstemplate.SwmsTemplatesteps = swmstemplate.SwmsTemplatesteps.GroupBy(i => i.STEPNO);

                    //grid6.Reload();
                    stepTemplate = null;
                    newstepno = null;
                    StateHasChanged();
                }
                else
                {
                    var RiskLikelyBefor = getRiskLikelyhoodsResult.Where(r => r.RISK_VALUE_ID == stepTemplate.RISK_LIKELYHOOD_ID).FirstOrDefault();
                    var RiskLikelyAfter = getRiskLikelyhoodsResult.Where(r => r.RISK_VALUE_ID == stepTemplate.RISK_AFTER_LIKELYHOOD_ID).FirstOrDefault();
                    var conseqResult = getConsequencesResult.Where(c => c.CONSEQUENCE_ID == stepTemplate.CONSEQUENCE_ID).FirstOrDefault();
                    var conseqAfterResult = getConsequencesResult.Where(c => c.CONSEQUENCE_ID == stepTemplate.AFTER_CONSEQUENCE_ID).FirstOrDefault();
                    var responsibleResult = getResposnsibleTypesResult.Where(c => c.RESPONSIBLE_ID == stepTemplate.RESPOSNSIBLE_TYPE_ID).FirstOrDefault();



                    if (!string.IsNullOrEmpty(stepTemplate.HAZARD))
                    {
                        string[] hazardArr = stepTemplate.HAZARD.Split(',');
                        stepTemplate.SpecificHazards = hazardArr;
                    }
                    if (!string.IsNullOrEmpty(stepTemplate.CONTROLLINGHAZARDS))
                    {
                        string[] ctrlHazard = stepTemplate.CONTROLLINGHAZARDS.Split(',');
                        stepTemplate.Controllings = ctrlHazard;
                    }
                    if (!string.IsNullOrEmpty(stepTemplate.HEALTHIMPACT))
                    {
                        string[] impactArr = stepTemplate.HEALTHIMPACT.Split(',');
                        stepTemplate.HealthImpacts = impactArr;
                    }

                    string hazards = string.Empty;
                    if (stepTemplate.SpecificHazards != null)
                    {
                        foreach (var s in stepTemplate.SpecificHazards)
                        {
                            if (string.IsNullOrEmpty(hazards))
                                hazards = s;
                            else
                                hazards = hazards + "," + s;
                        }
                    }

                    string impactResult = string.Empty;
                    if (stepTemplate.HealthImpacts != null)
                    {
                        foreach (var s in stepTemplate.HealthImpacts)
                        {
                            if (string.IsNullOrEmpty(impactResult))
                                impactResult = s;
                            else
                                impactResult = impactResult + "," + s;
                        }
                    }

                    string controlResult = string.Empty;
                    if (stepTemplate.Controllings != null)
                    {
                        foreach (var s in stepTemplate.Controllings)
                        {
                            if (string.IsNullOrEmpty(controlResult))
                                controlResult = s;
                            else
                                controlResult = controlResult + "," + s;
                        }
                    }

                    stepTemplate.HAZARD = hazards;
                    stepTemplate.HEALTHIMPACT = impactResult;
                    stepTemplate.CONTROLLINGHAZARDS = controlResult;
                    stepTemplate.RiskLikelyHoodBeforeName = RiskLikelyBefor != null ? RiskLikelyBefor.NAME : string.Empty;
                    stepTemplate.ConsequenceName = conseqResult != null ? conseqResult.NAME : string.Empty;
                    stepTemplate.RiskLikelyHoodAfterName = RiskLikelyAfter != null ? RiskLikelyAfter.NAME : string.Empty;
                    stepTemplate.ConsequenceAfterName = conseqAfterResult != null ? conseqAfterResult.NAME : string.Empty;
                    stepTemplate.ResposnsibleTypeName = responsibleResult != null ? responsibleResult.NAME : string.Empty;
                    //grid6.Reload();
                    stepTemplate = null;
                    StateHasChanged();
                }

            }
            catch (System.Exception clearConnectionCreateSwmsTemplatestepException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SwmsTemplatestep!", 1800000);
            }
            finally
            {
                isAddEdit = false;
                isLoading = false;
                StateHasChanged();
            }
        }

        protected void Change(UploadProgressArgs args, string name)
        {
            foreach (var file in args.Files)
            {
                swmstemplate.RiskAssessmentDoc = $"{file.Name}";
            }
        }

        RadzenUpload upload;
        int progress;
        protected bool fileLength = true;

        protected void OnProgress(UploadProgressArgs args, string name)
        {
            this.progress = args.Progress;

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    swmstemplate.RiskAssessmentDoc = $"{file.Name}";
                    if (file.Size > 2048000 || file.Size < 1000)
                    {
                        fileLength = false;
                        return;
                    }
                }
            }
        }

        public async Task RemoveDoc()
        {
            swmstemplate.RiskAssessmentDoc = null;
        }

        //string nextStyle = @"";
        //string prevStyle = @".ui-steps-prev { visibility:hidden }";

        protected async Task ChangeTemplate(object value, string name)
        {
            //if ((int)value == 0)
            //{
            //    string nextStyle = @"";
            //    string prevStyle = @".ui-steps-prev { visibility:hidden !important }";
            //}
            //else if ((int)value == 1)
            //{
            //    string nextStyle = @"";
            //    string prevStyle = @"";
            //}
            //else if ((int)value == 2)
            //{
            //    string nextStyle = @".ui-steps-next { visibility:hidden !important }";
            //    string prevStyle = @"";
            //}

            if (swmstemplate.REFERENCETEMPLATEID != null && (int)value == 1)
            {
                if (tempSwmsId != swmstemplate.REFERENCETEMPLATEID)
                {
                    swmstemplate.SwmsTemplatesteps = null;
                    var result = await ClearConnection.GetSwmsTemplatesteps(new Query() { Filter = $@"i => i.SWMSID == {swmstemplate.REFERENCETEMPLATEID}" });
                    tempSwmsId = swmstemplate.REFERENCETEMPLATEID;
                    if (result != null)
                    {
                        //Add Setup

                        foreach (var item in result)
                        {
                            if (swmstemplate.SwmsTemplatesteps == null)
                                swmstemplate.SwmsTemplatesteps = new List<SwmsTemplatestep>();

                            swmstemplate.SwmsTemplatesteps.Add(new SwmsTemplatestep
                            {
                                TASKCATEGEORY = item.TASKCATEGEORY,
                                HAZARD = item.HAZARD,
                                HEALTHIMPACT = item.HEALTHIMPACT,
                                SWMSTYPE = item.SWMSTYPE,
                                RISK_LIKELYHOOD_ID = item.RISK_LIKELYHOOD_ID,
                                RiskLikelyHoodBeforeName = item.RiskLikelyhood != null ? item.RiskLikelyhood.NAME : string.Empty,
                                CONSEQUENCE_ID = item.CONSEQUENCE_ID,
                                ConsequenceName = item.Consequence != null ? item.Consequence.NAME : string.Empty,
                                RISK_CONTRL_SCORE = item.RISK_CONTRL_SCORE,
                                CONTROLLINGHAZARDS = item.CONTROLLINGHAZARDS,
                                RISK_AFTER_LIKELYHOOD_ID = item.RISK_AFTER_LIKELYHOOD_ID,
                                RiskLikelyHoodAfterName = item.RiskLikelyhood1 != null ? item.RiskLikelyhood1.NAME : string.Empty,
                                AFTER_CONSEQUENCE_ID = item.AFTER_CONSEQUENCE_ID,
                                ConsequenceAfterName = item.Consequence1 != null ? item.Consequence1.NAME : string.Empty,
                                RESPOSNSIBLE_TYPE_ID = item.RESPOSNSIBLE_TYPE_ID,
                                ResposnsibleTypeName = item.ResposnsibleType != null ? item.ResposnsibleType.NAME : string.Empty,
                                STATUS = item.STATUS,
                                AFTER_RISK_CONTROL_SCORE = item.AFTER_RISK_CONTROL_SCORE,
                                ISDELETE = item.ISDELETE,
                                STEPNO = item.STEPNO
                            });
                        }
                    }
                }

                //PlantEquipments

                var plantEquipments = await ClearConnection.GetSwmsPlantequipments(new Query() { Filter = $@"i => i.SWMSID == {swmstemplate.REFERENCETEMPLATEID}" });
                List<int> plants = new List<int>();
                foreach (var pitem in plantEquipments)
                {
                    plants.Add(pitem.PLANT_EQUIPMENT_ID);
                }
                if (otherdetail == null)
                    otherdetail = new Swmsotherdetail();

                otherdetail.plants = plants;
                //pPE

                var pPE = await ClearConnection.GetSwmsPperequireds(new Query() { Filter = $@"i => i.SWMSID == {swmstemplate.REFERENCETEMPLATEID}" });
                List<int> ppe = new List<int>();
                foreach (var pitem in pPE)
                {
                    ppe.Add(pitem.PPE_VALUE_ID);
                }
                if (otherdetail == null)
                    otherdetail = new Swmsotherdetail();
                otherdetail.ppes = ppe;
                //Licence Permit
                var licence = await ClearConnection.GetSwmsLicencespermits(new Query() { Filter = $@"i => i.SWMSID == {swmstemplate.REFERENCETEMPLATEID}" });
                List<int> licenceList = new List<int>();
                foreach (var pitem in licence)
                {
                    licenceList.Add(pitem.LICENCE_PERMIT_ID);
                }
                if (otherdetail == null)
                    otherdetail = new Swmsotherdetail();
                otherdetail.licenes = licenceList;
                //Referenced Legislation
                var referencedLegislation = await ClearConnection.GetSwmsReferencedlegislations(new Query() { Filter = $@"i => i.SWMSID == {swmstemplate.REFERENCETEMPLATEID}" });
                List<int> legislationList = new List<int>();
                foreach (var pitem in referencedLegislation)
                {
                    legislationList.Add(pitem.REFERENCE_LEGISLATION_ID);
                }
                if (otherdetail == null)
                    otherdetail = new Swmsotherdetail();
                otherdetail.legislations = legislationList;
                //Hazard Material Value
                var hazardMaterial = await ClearConnection.GetSwmsHazardousmaterials(new Query() { Filter = $@"i => i.SWMSID == {swmstemplate.REFERENCETEMPLATEID}" });
                List<int> MaterialList = new List<int>();
                foreach (var pitem in hazardMaterial)
                {
                    MaterialList.Add((int)pitem.HAZARD_MATERIAL_ID);
                }
                if (otherdetail == null)
                    otherdetail = new Swmsotherdetail();
                otherdetail.materialsHazardous = MaterialList;
            }

        }

        protected bool isAddEdit = false;
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            isLoading = true;
            newstepno = swmstemplate.SwmsTemplatesteps.Count();
            StateHasChanged();
            await Task.Delay(1);
            if (swmstemplate.SwmsTemplatesteps != null)
            {
                stepTemplate = swmstemplate.SwmsTemplatesteps.Where(s => s.STEPID == data.STEPID && s.STEPNO == data.STEPNO).FirstOrDefault();
                string[] hazards = stepTemplate.HAZARD.Split(',');
                List<string> hazardlist = new List<string>();
                foreach (var s in hazards)
                {
                    hazardlist.Add(s);
                }
                stepTemplate.SpecificHazards = hazardlist;

                string[] impactes = stepTemplate.HEALTHIMPACT.Split(',');
                List<string> impactlist = new List<string>();
                foreach (var s in impactes)
                {
                    impactlist.Add(s);
                }
                stepTemplate.HealthImpacts = impactes;

                string[] controlling = stepTemplate.CONTROLLINGHAZARDS.Split(',');
                List<string> contrloList = new List<string>();
                foreach (var s in controlling)
                {
                    contrloList.Add(s);
                }
                stepTemplate.Controllings = controlling;

                stepTemplate.IsAdd = false;
                isAddEdit = true;
                StateHasChanged();
            }

            isLoading = false;
            StateHasChanged();

        }



        protected RadzenPanel stepListPanel { get; set; }
        protected async System.Threading.Tasks.Task SwmsStepDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (swmstemplate.SwmsTemplatesteps != null)
                {
                    if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                    {
                        var result = swmstemplate.SwmsTemplatesteps.Where(s => s.STEPID == data.STEPID && s.STEPNO == data.STEPNO).FirstOrDefault();
                        if (result != null)
                        {
                            swmstemplate.SwmsTemplatesteps.Remove(result);

                            foreach (var item in swmstemplate.SwmsTemplatesteps)
                            {
                                if (item.STEPNO > data.STEPNO)
                                    item.STEPNO -= 1;
                            }
                        }
                    }


                }
                // var clearConnectionDeleteSwmsHazardousmaterialResult = await ClearConnection.DeleteSwmsHazardousmaterial(data.HAZARDOUSMATERIALID);

            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Step");
            }
        }
        void ValidateTempleteName(string value)
        {
            var templateName = swmstobasenew.Where(s => s.TEMPLATENAME == value.Trim()).FirstOrDefault();
            if (templateName != null)
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Duplicate Template name are not allowed.");
        }

        //code for impact
        protected string resultImpactValue { get; set; }
        void ChangeImpact(object value, string name)
        {
            string matchvalue = string.Empty;

            if (value.ToString() != "")
            {
                matchvalue = getImpactTypesResult.Where(c => c.NAME == value.ToString()).Select(x => x.NAME).FirstOrDefault();
                if (matchvalue != null)
                {
                    if (resultImpactValue != null)
                        resultImpactValue += matchvalue + ",";
                    else
                        resultImpactValue = matchvalue + ",";
                }
                else
                {
                    string strchk = string.Empty;
                    if (value.ToString().Count(x => x == ',') > 1)
                        strchk = value.ToString().Remove(0, value.ToString().LastIndexOf(',') + 1);
                    else
                        strchk = value.ToString().Remove(0, value.ToString().IndexOf(',') + 1);
                    if (strchk.Length > 1)
                    {
                        if (resultImpactValue != "")
                        {
                            resultImpactValue += strchk + ",";
                        }
                        else
                        {
                            resultImpactValue = strchk + ",";
                        }
                    }
                }
            }
            else
            {
                resultImpactValue = string.Empty;
            }

            if (!string.IsNullOrEmpty(resultImpactValue))
            {
                string[] tempArr = resultImpactValue.Split(',').ToArray();

                resultImpactValue = "";
                IList<string> impactList = getImpactTypesResult.Select(x => x.NAME).ToList();
                foreach (string temp in tempArr)
                {
                    if (impactList.Contains(temp) && !resultImpactValue.Contains(temp))
                        resultImpactValue += temp + ',';
                }
            }


            stepTemplate.HEALTHIMPACT = resultImpactValue;

            StateHasChanged();
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.ImpactType> customImpactData;
        void LoadImpactData(LoadDataArgs args)
        {
            string str = string.Empty;
            if (args.Filter != null)
            {
                if (args.Filter.IndexOf(',') != -1)
                {
                    if (args.Filter.Count(x => x == ',') > 1)
                        str = args.Filter.Remove(0, args.Filter.LastIndexOf(',') + 1);
                    else
                        str = args.Filter.Remove(0, args.Filter.IndexOf(',') + 1);
                }
                else
                    str = args.Filter;
                if (str != null)
                    customImpactData = getImpactTypesResult.Where(c => c.NAME.ToLower().Contains(str.ToLower())).ToList();

            }

            //InvokeAsync(StateHasChanged);
        }
        //code for end impact

        // Code What are the Specific Hazards
        protected string resultSpecificHazardsValue { get; set; }
        void ChangeSpecificHazards(object value, string name)
        {
            string matchvalue = string.Empty;

            if (value.ToString() != "")
            {
                matchvalue = getHazardValuesResult.Where(c => c.NAME == value.ToString()).Select(x => x.NAME).FirstOrDefault();
                if (matchvalue != null)
                {
                    if (resultSpecificHazardsValue != null)
                        resultSpecificHazardsValue += matchvalue + ",";
                    else
                        resultSpecificHazardsValue = matchvalue + ",";
                }
                else
                {

                    string strchk = string.Empty;
                    if (value.ToString().Count(x => x == ',') > 1)
                        strchk = value.ToString().Remove(0, value.ToString().LastIndexOf(',') + 1);
                    else
                        strchk = value.ToString().Remove(0, value.ToString().IndexOf(',') + 1);
                    if (strchk.Length > 1)
                    {
                        if (resultSpecificHazardsValue != "")
                        {
                            resultSpecificHazardsValue += strchk + ",";
                        }
                        else
                        {
                            resultSpecificHazardsValue = strchk + ",";
                        }
                    }
                }
            }
            else
            {
                resultSpecificHazardsValue = string.Empty;
            }

            if (!string.IsNullOrEmpty(resultSpecificHazardsValue))
            {
                string[] tempArr = resultSpecificHazardsValue.Split(',').ToArray();

                resultSpecificHazardsValue = "";
                IList<string> hazardList = getHazardValuesResult.Select(x => x.NAME).ToList();
                foreach (string temp in tempArr)
                {
                    if (hazardList.Contains(temp) && !resultSpecificHazardsValue.Contains(temp))
                    {
                        resultSpecificHazardsValue += temp + ',';
                    }
                }
            }


            stepTemplate.HAZARD = resultSpecificHazardsValue;


            StateHasChanged();
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.HazardValue> customHazardData;
        void LoadSpecificHazardData(LoadDataArgs args)
        {
            string str = string.Empty;
            if (args.Filter != null)
            {
                if (args.Filter.IndexOf(',') != -1)
                {
                    if (args.Filter.Count(x => x == ',') > 1)
                        str = args.Filter.Remove(0, args.Filter.LastIndexOf(',') + 1);
                    else
                        str = args.Filter.Remove(0, args.Filter.IndexOf(',') + 1);
                }
                else
                    str = args.Filter;
                if (str != null)
                    customHazardData = getHazardValuesResult.Where(c => c.NAME.ToLower().Contains(str.ToLower())).ToList();
            }

            InvokeAsync(StateHasChanged);
        }
        // END Code What are the Specific Hazards?

        // Code for Methods of Controlling Hazards
        protected string resultControllingHazardsValue { get; set; }
        void ChangeControllingHazards(object value, string name)
        {

            string matchvalue = string.Empty;

            if (value.ToString() != "")
            {
                matchvalue = getControlMeasureValuesResult.Where(c => c.NAME == value.ToString()).Select(x => x.NAME).FirstOrDefault();
                if (matchvalue != null)
                {
                    if (resultControllingHazardsValue != null)
                        resultControllingHazardsValue += matchvalue + ",";
                    else
                        resultControllingHazardsValue = matchvalue + ",";
                }
                else
                {

                    string strchk = string.Empty;
                    if (value.ToString().Count(x => x == ',') > 1)
                        strchk = value.ToString().Remove(0, value.ToString().LastIndexOf(',') + 1);
                    else
                        strchk = value.ToString().Remove(0, value.ToString().IndexOf(',') + 1);
                    if (strchk.Length > 1)
                    {
                        if (resultControllingHazardsValue != "")
                        {
                            resultControllingHazardsValue += strchk + ",";
                        }
                        else
                        {
                            resultControllingHazardsValue = strchk + ",";
                        }
                    }
                }
            }
            else
            {
                resultControllingHazardsValue = string.Empty;
            }

            if (!string.IsNullOrEmpty(resultControllingHazardsValue))
            {
                string[] tempArr = resultControllingHazardsValue.Split(',').ToArray();
                resultControllingHazardsValue = "";

                IList<string> controllingHazardsList = getControlMeasureValuesResult.Select(x => x.NAME).ToList();
                foreach (string temp in tempArr)
                {
                    if (controllingHazardsList.Contains(temp) && !resultControllingHazardsValue.Contains(temp))
                        resultControllingHazardsValue += temp + ',';
                }
            }

            stepTemplate.CONTROLLINGHAZARDS = resultControllingHazardsValue;

            StateHasChanged();
        }
        IEnumerable<Clear.Risk.Models.ClearConnection.ControlMeasureValue> customControllingHazardData;
        void LoadControllingHazardData(LoadDataArgs args)
        {
            string str = string.Empty;
            if (args.Filter != null)
            {
                if (args.Filter.IndexOf(',') != -1)
                {
                    if (args.Filter.Count(x => x == ',') > 1)
                        str = args.Filter.Remove(0, args.Filter.LastIndexOf(',') + 1);
                    else
                        str = args.Filter.Remove(0, args.Filter.IndexOf(',') + 1);
                }
                else
                    str = args.Filter;
                if (str != null)
                    customControllingHazardData = getControlMeasureValuesResult.Where(c => c.NAME.ToLower().Contains(str.ToLower())).ToList();
            }

            InvokeAsync(StateHasChanged);
        }
        // End Code for Methods of Controlling Hazards


    }
}

