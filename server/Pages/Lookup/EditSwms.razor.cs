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
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Clear.Risk.Pages.Lookup
{
    public partial class EditSwms : ComponentBase
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

        [Parameter]
        public dynamic SWMSID { get; set; }
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
        string filename = Guid.NewGuid().ToString();
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

        protected void Change(UploadProgressArgs args, string name)
        {
            foreach (var file in args.Files)
            {
                swmstemplate.RiskAssessmentDoc = $"{file.Name}";
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
                if (!object.Equals(_swmstemplate, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "swmstemplate", NewValue = value, OldValue = _swmstobasenew };
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

        protected int? tempSwmsId { get; set; }

        protected bool isLoading { get; set; }
        protected async System.Threading.Tasks.Task Load()
        {

            var clearConnectionGetImpactTypesResult = await ClearConnection.GetImpactTypes();
            getImpactTypesResult = clearConnectionGetImpactTypesResult;

            var clearConnectionGetHazardValuesResult = await ClearConnection.GetHazardValues();
            getHazardValuesResult = clearConnectionGetHazardValuesResult;

            var clearConnectionGetControlMeasureValuesResult = await ClearConnection.GetControlMeasureValues();
            getControlMeasureValuesResult = clearConnectionGetControlMeasureValuesResult;

            if (Security.IsInRole("System Administrator"))
            {
                getTemplateResult = await ClearConnection.GetTemplateList();
            }
            else
            {
                getTemplateResult = await ClearConnection.GetTemplateList(new Query() { Filter = $@"i => i.COMPANYID == {Security.getCompanyId()} || i.COUNTRY_ID == {Security.getCountryId()} " });

            }

            //var clearConnectionGetPeopleResult = await ClearConnection.GetPeople();
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

            var clearConnectionGetTemplateToBaseNewResult = await ClearConnection.GetSwmsTemplates();
            swmstobasenew = clearConnectionGetTemplateToBaseNewResult;

            var clearConnectionGetRiskLikelyhoodsResult = await ClearConnection.GetRiskLikelyhoods();
            getRiskLikelyhoodsResult = clearConnectionGetRiskLikelyhoodsResult;

            var clearConnectionGetConsequencesResult = await ClearConnection.GetConsequences();
            getConsequencesResult = clearConnectionGetConsequencesResult;

            var clearConnectionGetResposnsibleTypesResult = await ClearConnection.GetResposnsibleTypes();
            getResposnsibleTypesResult = clearConnectionGetResposnsibleTypesResult;

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


            var clearConnectionGetSwmsTemplateBySwmsidResult = await ClearConnection.GetSwmsBySwmsid(int.Parse(SWMSID));


            swmstemplate = clearConnectionGetSwmsTemplateBySwmsidResult;


            //swmstemplate.SwmsTemplatesteps = (ICollection<SwmsTemplatestep>)swmstemplate.SwmsTemplatesteps.GroupBy(i => i.STEPNO);
            SwmsTemplatesteps = swmstemplate.SwmsTemplatesteps;
            swmstemplate.SwmsTemplatesteps = swmstemplate.SwmsTemplatesteps.Where(i => i.ISDELETE == false).OrderBy(x => x.STEPNO).ToList();
            swmstemplate.SwmsPlantequipments = swmstemplate.SwmsPlantequipments.ToList();

            List<int> plants = new List<int>();
            foreach (var pitem in swmstemplate.SwmsPlantequipments)
            {
                plants.Add(pitem.PLANT_EQUIPMENT_ID);
            }
            if (otherdetail == null)
                otherdetail = new Swmsotherdetail();

            otherdetail.plants = plants;
            List<int> ppe = new List<int>();
            foreach (var pitem in swmstemplate.SwmsPperequireds)
            {
                ppe.Add(pitem.PPE_VALUE_ID);
            }
            if (otherdetail == null)
                otherdetail = new Swmsotherdetail();
            otherdetail.ppes = ppe;


            List<int> licenceList = new List<int>();
            foreach (var pitem in swmstemplate.SwmsLicencespermits)
            {
                licenceList.Add(pitem.LICENCE_PERMIT_ID);
            }
            if (otherdetail == null)
                otherdetail = new Swmsotherdetail();
            otherdetail.licenes = licenceList;


            List<int> legislationList = new List<int>();
            foreach (var pitem in swmstemplate.SwmsReferencedlegislations)
            {
                legislationList.Add(pitem.REFERENCE_LEGISLATION_ID);
            }
            if (otherdetail == null)
                otherdetail = new Swmsotherdetail();
            otherdetail.legislations = legislationList;

            List<int> MaterialList = new List<int>();
            foreach (var pitem in swmstemplate.SwmsHazardousmaterials)
            {
                MaterialList.Add((int)pitem.HAZARD_MATERIAL_ID);
            }
            if (otherdetail == null)
                otherdetail = new Swmsotherdetail();
            otherdetail.materialsHazardous = MaterialList;



        }




        private bool Exists(IEnumerable<int> arr, int num)
        {
            foreach (int a in arr)
            {
                if (a == num)
                    return true;
            }
            return false;
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
                    if (!string.IsNullOrEmpty(args.RiskAssessmentDoc))
                    {
                        args.RiskAssessmentDoc = filename + fileExt;
                    }

                    if (otherdetail != null)
                    {
                        IList<int> arr = new List<int>();
                        //Adding Plants
                        if (otherdetail.plants.Count() != 0)
                        {
                            if (args.SwmsPlantequipments.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in args.SwmsPlantequipments)
                                {
                                    arr.Add(item.PLANT_EQUIPMENT_ID);
                                }

                                //Adding a Plant Equipment
                                foreach (int i in otherdetail.plants)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        args.SwmsPlantequipments.Add(new SwmsPlantequipment
                                        {
                                            PLANT_EQUIPMENT_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = args.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.plants)
                                {
                                    args.SwmsPlantequipments.Add(new SwmsPlantequipment
                                    {
                                        PLANT_EQUIPMENT_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = args.SWMSID,
                                    });
                                }
                            }


                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in args.SwmsPlantequipments)
                            {
                                if (!Exists(otherdetail.plants, item.PLANT_EQUIPMENT_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }
                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in args.SwmsPlantequipments)
                            {
                                item.IS_DELETED = true;
                            }
                        }
                        /*--------------------------------------------------------------------------------------------------------------*/
                        //Add PPE Equipments
                        if (otherdetail.ppes.Count() != 0)
                        {
                            if (args.SwmsPperequireds.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in args.SwmsPperequireds)
                                {
                                    arr.Add(item.PPE_VALUE_ID);
                                }

                                //Adding a PPE Equipment
                                foreach (int i in otherdetail.ppes)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        args.SwmsPperequireds.Add(new SwmsPperequired
                                        {
                                            PPE_VALUE_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = args.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.ppes)
                                {
                                    args.SwmsPperequireds.Add(new SwmsPperequired
                                    {
                                        PPE_VALUE_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = args.SWMSID,
                                    });
                                }
                            }

                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in args.SwmsPperequireds)
                            {
                                if (!Exists(otherdetail.ppes, item.PPE_VALUE_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }

                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in args.SwmsPlantequipments)
                            {
                                item.IS_DELETED = true;
                            }
                        }


                        /*--------------------------------------------------------------------------------------------------------------*/

                        //Add Licence Permit 

                        if (otherdetail.licenes.Count() != 0)
                        {
                            if (args.SwmsLicencespermits.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in args.SwmsLicencespermits)
                                {
                                    arr.Add(item.LICENCE_PERMIT_ID);
                                }

                                //Adding License
                                foreach (int i in otherdetail.licenes)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        args.SwmsLicencespermits.Add(new SwmsLicencespermit
                                        {
                                            LICENCE_PERMIT_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = args.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.licenes)
                                {
                                    args.SwmsLicencespermits.Add(new SwmsLicencespermit
                                    {
                                        LICENCE_PERMIT_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = args.SWMSID,
                                    });
                                }
                            }

                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in args.SwmsLicencespermits)
                            {
                                if (!Exists(otherdetail.licenes, item.LICENCE_PERMIT_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }
                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in args.SwmsLicencespermits)
                            {
                                item.IS_DELETED = true;
                            }
                        }

                        /*--------------------------------------------------------------------------------------------------------------*/
                        //Add Referenced Legislation

                        if (otherdetail.legislations.Count() != 0)
                        {
                            if (args.SwmsReferencedlegislations.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in args.SwmsReferencedlegislations)
                                {
                                    arr.Add(item.REFERENCE_LEGISLATION_ID);
                                }

                                //Adding License
                                foreach (int i in otherdetail.legislations)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        args.SwmsReferencedlegislations.Add(new SwmsReferencedlegislation
                                        {
                                            REFERENCE_LEGISLATION_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = args.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.legislations)
                                {
                                    args.SwmsReferencedlegislations.Add(new SwmsReferencedlegislation
                                    {
                                        REFERENCE_LEGISLATION_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = args.SWMSID,
                                    });
                                }
                            }

                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in args.SwmsReferencedlegislations)
                            {
                                if (!Exists(otherdetail.legislations, item.REFERENCE_LEGISLATION_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }
                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in args.SwmsReferencedlegislations)
                            {
                                item.IS_DELETED = true;
                            }
                        }


                        /*--------------------------------------------------------------------------------------------------------------*/

                        if (otherdetail.materialsHazardous.Count() != 0)
                        {
                            if (args.SwmsHazardousmaterials.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in args.SwmsHazardousmaterials)
                                {
                                    arr.Add(item.HAZARD_MATERIAL_ID);
                                }

                                //Adding License
                                foreach (int i in otherdetail.materialsHazardous)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        args.SwmsHazardousmaterials.Add(new SwmsHazardousmaterial
                                        {
                                            HAZARD_MATERIAL_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = args.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.materialsHazardous)
                                {
                                    args.SwmsHazardousmaterials.Add(new SwmsHazardousmaterial
                                    {
                                        HAZARD_MATERIAL_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = args.SWMSID,
                                    });
                                }
                            }

                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in args.SwmsHazardousmaterials)
                            {
                                if (!Exists(otherdetail.materialsHazardous, item.HAZARD_MATERIAL_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }
                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in args.SwmsHazardousmaterials)
                            {
                                item.IS_DELETED = true;
                            }
                        }

                    }

                    /*--------------------------------------------------------------------------------------------------------------*/

                    /*Adding new items and updating directly to database*/

                    args.IS_DRAFT = false;

                    var clearConnectionUpdateSwmsTemplateResult = await ClearConnection.UpdateSwmsTemplate(int.Parse(SWMSID), swmstemplate);

                    foreach (var item in swmstemplate.SwmsTemplatesteps)
                    {
                        if (item.STEPID == 0)
                        {
                            item.SWMSID = args.SWMSID;
                            await ClearConnection.CreateSwmsTemplatestep(item);
                        }
                        else if (item.STEPID != 0 && item.ISDELETE == true)
                        {
                            await ClearConnection.DeleteSwmsTemplatestep(item.STEPID);
                        }
                        else
                        {
                            await ClearConnection.UpdateSwmsTemplateSteps(item.STEPID, item);
                        }
                    }



                    if (args.SwmsPlantequipments != null)
                    {
                        foreach (var item in args.SwmsPlantequipments)
                        {
                            if (item.IS_DELETED == true && item.PEID != 0)
                            {
                                await ClearConnection.DeleteSwmsPlantequipment(item.PEID);
                            }
                            else if (item.PEID == 0)
                            {
                                await ClearConnection.CreateSwmsPlantequipment(item);
                            }
                        }
                    }

                    foreach (var item in args.SwmsPperequireds)
                    {
                        if (item.IS_DELETED == true && item.PPEID != 0)
                        {
                            await ClearConnection.DeleteSwmsPperequired(item.PPEID);
                        }
                        else if (item.PPEID == 0)
                        {
                            await ClearConnection.CreateSwmsPperequired(item);
                        }
                    }

                    foreach (var item in args.SwmsLicencespermits)
                    {
                        if (item.IS_DELETED == true && item.LPID != 0)
                        {
                            await ClearConnection.DeleteSwmsLicencespermit(item.LPID);
                        }
                        else if (item.LPID == 0)
                        {
                            await ClearConnection.CreateSwmsLicencespermit(item);
                        }
                    }

                    foreach (var item in args.SwmsReferencedlegislations)
                    {
                        if (item.IS_DELETED == true && item.REFLID != 0)
                        {
                            await ClearConnection.DeleteSwmsReferencedlegislation(item.REFLID);
                        }
                        else if (item.REFLID == 0)
                        {
                            await ClearConnection.CreateSwmsReferencedlegislation(item);
                        }
                    }

                    foreach (var item in args.SwmsHazardousmaterials)
                    {
                        if (item.IS_DELETED == true && item.HAZARDOUSMATERIALID != 0)
                        {
                            await ClearConnection.DeleteSwmsHazardousmaterial(item.HAZARDOUSMATERIALID);
                        }
                        else if (item.HAZARDOUSMATERIALID == 0)
                        {
                            await ClearConnection.CreateSwmsHazardousmaterial(item);
                        }
                    }


                    NotificationService.Notify(NotificationSeverity.Success, $"Success", $"SWMS Template Updated successfully!");
                    UriHelper.NavigateTo("swms-template");
                }

            }
            catch (System.Exception clearConnectionCreateSwmsTemplateException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to Updated  SwmsTemplate!");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
                await Task.Delay(1);
            }
        }
        protected bool isAddEdit = false;
        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            stepTemplate = new SwmsTemplatestep();
            stepTemplate.IsAdd = true;
            stepTemplate.ISDELETE = false;
            stepTemplate.STEPNO = SwmsTemplatesteps.ToList().Count() + 1;
            isLoading = false;
            isAddEdit = true;
            StateHasChanged();
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            //UriHelper.NavigateTo("EditSwms"+"/"+SWMSID);
            stepTemplate = null;
            isAddEdit = false;
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
        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

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
                    var cresult = getConsequencesResult.Where(x => x.CONSEQUENCE_ID == (int)stepTemplate.CONSEQUENCE_ID).FirstOrDefault();
                    stepTemplate.RISK_CONTRL_SCORE = cresult.CONSEQUENCE_VALUE;
                }
                else
                {
                    stepTemplate.RISK_CONTRL_SCORE = null;
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
                        RiskLikelyhood = RiskLikelyBefor != null ? RiskLikelyBefor : null,
                        CONSEQUENCE_ID = stepTemplate.CONSEQUENCE_ID,
                        Consequence = conseqResult != null ? conseqResult : null,
                        RISK_CONTRL_SCORE = stepTemplate.RISK_CONTRL_SCORE,
                        CONTROLLINGHAZARDS = controlResult,
                        RISK_AFTER_LIKELYHOOD_ID = stepTemplate.RISK_AFTER_LIKELYHOOD_ID,
                        RiskLikelyhood1 = RiskLikelyAfter != null ? RiskLikelyAfter : null,
                        AFTER_CONSEQUENCE_ID = stepTemplate.AFTER_CONSEQUENCE_ID,
                        Consequence1 = conseqAfterResult != null ? conseqAfterResult : null,
                        RESPOSNSIBLE_TYPE_ID = stepTemplate.RESPOSNSIBLE_TYPE_ID,
                        ResposnsibleType = responsibleResult != null ? responsibleResult : null,
                        AFTER_RISK_CONTROL_SCORE = stepTemplate.AFTER_RISK_CONTROL_SCORE,
                        STATUS = stepTemplate.STATUS,
                        ISDELETE = stepTemplate.ISDELETE,
                        STEPNO = stepTemplate.STEPNO
                    });


                    //grid6.Reload();
                    stepTemplate = null;
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


                    foreach (var item in swmstemplate.SwmsTemplatesteps)
                    {
                        if (item.STEPNO >= stepTemplate.STEPNO)
                            item.STEPNO += 1;
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
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new SwmsTemplatestep!");
            }
            finally
            {
                isLoading = false;
                isAddEdit = false;
                StateHasChanged();

            }
        }


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
                            //result.ISDELETE = true;
                            await ClearConnection.DeleteSwmsTemplatestep(result.STEPID);


                            foreach (var item in swmstemplate.SwmsTemplatesteps)
                            {
                                if (item.STEPNO > data.STEPNO)
                                    item.STEPNO -= 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Step");
            }
        }
        protected async System.Threading.Tasks.Task GridEditButtonClick(MouseEventArgs args, dynamic data)
        {
            if (swmstemplate.SwmsTemplatesteps != null)
            {
                stepTemplate = swmstemplate.SwmsTemplatesteps.Where(s => s.STEPID == data.STEPID && s.STEPNO == data.STEPNO).FirstOrDefault();
                stepTemplate.IsAdd = false;
                isAddEdit = true;
            }
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
                IList<string> ImpactList = getImpactTypesResult.Select(x => x.NAME).ToList();
                foreach (string temp in tempArr)
                {
                    if (ImpactList.Contains(temp))
                    {
                        resultImpactValue += temp + ',';
                    }
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

            InvokeAsync(StateHasChanged);
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
                    if (hazardList.Contains(temp))
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


        //protected async System.Threading.Tasks.Task UpdateStep1()
        //{
        //    var clearConnectionUpdateSwmsTemplateResult = await ClearConnection.UpdateSwmsTemplate(int.Parse(SWMSID), swmstemplate);
        //}

        //protected async System.Threading.Tasks.Task UpdateStep2()
        //{
        //    try { 
        //    foreach(var item in swmstemplate.SwmsTemplatesteps) 
        //    { 
        //        var clearConnectionUpdateSwmsTemplateResult = await ClearConnection.UpdateSwmsTemplateSteps(item.STEPID, item);
        //    }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        //protected async System.Threading.Tasks.Task UpdateStep3()
        //{

        //}

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
                IList<string> resultControllingHazardList = getControlMeasureValuesResult.Select(x => x.NAME).ToList();
                foreach (string temp in tempArr)
                {
                    if (resultControllingHazardList.Contains(temp))
                    {
                        resultControllingHazardsValue += temp + ',';
                    }
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
                    if (otherdetail != null)
                    {
                        IList<int> arr = new List<int>();
                        //Adding Plants
                        if (otherdetail.plants.Count() != 0)
                        {
                            if (swmstemplate.SwmsPlantequipments.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in swmstemplate.SwmsPlantequipments)
                                {
                                    arr.Add(item.PLANT_EQUIPMENT_ID);
                                }

                                //Adding a Plant Equipment
                                foreach (int i in otherdetail.plants)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        swmstemplate.SwmsPlantequipments.Add(new SwmsPlantequipment
                                        {
                                            PLANT_EQUIPMENT_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = swmstemplate.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.plants)
                                {
                                    swmstemplate.SwmsPlantequipments.Add(new SwmsPlantequipment
                                    {
                                        PLANT_EQUIPMENT_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = swmstemplate.SWMSID,
                                    });
                                }
                            }


                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in swmstemplate.SwmsPlantequipments)
                            {
                                if (!Exists(otherdetail.plants, item.PLANT_EQUIPMENT_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }
                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in swmstemplate.SwmsPlantequipments)
                            {
                                item.IS_DELETED = true;
                            }
                        }
                        /*--------------------------------------------------------------------------------------------------------------*/
                        //Add PPE Equipments
                        if (otherdetail.ppes.Count() != 0)
                        {
                            if (swmstemplate.SwmsPperequireds.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in swmstemplate.SwmsPperequireds)
                                {
                                    arr.Add(item.PPE_VALUE_ID);
                                }

                                //Adding a PPE Equipment
                                foreach (int i in otherdetail.ppes)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        swmstemplate.SwmsPperequireds.Add(new SwmsPperequired
                                        {
                                            PPE_VALUE_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = swmstemplate.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.ppes)
                                {
                                    swmstemplate.SwmsPperequireds.Add(new SwmsPperequired
                                    {
                                        PPE_VALUE_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = swmstemplate.SWMSID,
                                    });
                                }
                            }

                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in swmstemplate.SwmsPperequireds)
                            {
                                if (!Exists(otherdetail.ppes, item.PPE_VALUE_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }

                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in swmstemplate.SwmsPlantequipments)
                            {
                                item.IS_DELETED = true;
                            }
                        }


                        /*--------------------------------------------------------------------------------------------------------------*/

                        //Add Licence Permit 

                        if (otherdetail.licenes.Count() != 0)
                        {
                            if (swmstemplate.SwmsLicencespermits.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in swmstemplate.SwmsLicencespermits)
                                {
                                    arr.Add(item.LICENCE_PERMIT_ID);
                                }

                                //Adding License
                                foreach (int i in otherdetail.licenes)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        swmstemplate.SwmsLicencespermits.Add(new SwmsLicencespermit
                                        {
                                            LICENCE_PERMIT_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = swmstemplate.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.licenes)
                                {
                                    swmstemplate.SwmsLicencespermits.Add(new SwmsLicencespermit
                                    {
                                        LICENCE_PERMIT_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = swmstemplate.SWMSID,
                                    });
                                }
                            }

                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in swmstemplate.SwmsLicencespermits)
                            {
                                if (!Exists(otherdetail.licenes, item.LICENCE_PERMIT_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }
                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in swmstemplate.SwmsLicencespermits)
                            {
                                item.IS_DELETED = true;
                            }
                        }

                        /*--------------------------------------------------------------------------------------------------------------*/
                        //Add Referenced Legislation

                        if (otherdetail.legislations.Count() != 0)
                        {
                            if (swmstemplate.SwmsReferencedlegislations.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in swmstemplate.SwmsReferencedlegislations)
                                {
                                    arr.Add(item.REFERENCE_LEGISLATION_ID);
                                }

                                //Adding License
                                foreach (int i in otherdetail.legislations)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        swmstemplate.SwmsReferencedlegislations.Add(new SwmsReferencedlegislation
                                        {
                                            REFERENCE_LEGISLATION_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = swmstemplate.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.legislations)
                                {
                                    swmstemplate.SwmsReferencedlegislations.Add(new SwmsReferencedlegislation
                                    {
                                        REFERENCE_LEGISLATION_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = swmstemplate.SWMSID,
                                    });
                                }
                            }

                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in swmstemplate.SwmsReferencedlegislations)
                            {
                                if (!Exists(otherdetail.legislations, item.REFERENCE_LEGISLATION_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }
                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in swmstemplate.SwmsReferencedlegislations)
                            {
                                item.IS_DELETED = true;
                            }
                        }


                        /*--------------------------------------------------------------------------------------------------------------*/

                        if (otherdetail.materialsHazardous.Count() != 0)
                        {
                            if (swmstemplate.SwmsHazardousmaterials.Count() != 0)
                            {
                                arr.Clear();
                                foreach (var item in swmstemplate.SwmsHazardousmaterials)
                                {
                                    arr.Add(item.HAZARD_MATERIAL_ID);
                                }

                                //Adding License
                                foreach (int i in otherdetail.materialsHazardous)
                                {
                                    if (!Exists((IEnumerable<int>)arr, i))
                                    {
                                        swmstemplate.SwmsHazardousmaterials.Add(new SwmsHazardousmaterial
                                        {
                                            HAZARD_MATERIAL_ID = i,
                                            IS_DELETED = false,
                                            SWMSID = swmstemplate.SWMSID,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (int i in otherdetail.materialsHazardous)
                                {
                                    swmstemplate.SwmsHazardousmaterials.Add(new SwmsHazardousmaterial
                                    {
                                        HAZARD_MATERIAL_ID = i,
                                        IS_DELETED = false,
                                        SWMSID = swmstemplate.SWMSID,
                                    });
                                }
                            }

                            //Setting IS_DELETED = true for unselected items
                            foreach (var item in swmstemplate.SwmsHazardousmaterials)
                            {
                                if (!Exists(otherdetail.materialsHazardous, item.HAZARD_MATERIAL_ID))
                                {
                                    item.IS_DELETED = true;
                                }
                            }
                        }
                        else
                        {
                            //Setting IS_DELETED = true for all items if all items are unselected
                            foreach (var item in swmstemplate.SwmsHazardousmaterials)
                            {
                                item.IS_DELETED = true;
                            }
                        }

                    }

                    swmstemplate.IS_DRAFT = true;

                    /*--------------------------------------------------------------------------------------------------------------*/

                    /*Adding new items and updating directly to database*/

                    var clearConnectionUpdateSwmsTemplateResult = await ClearConnection.UpdateSwmsTemplate(int.Parse(SWMSID), swmstemplate);

                    foreach (var item in swmstemplate.SwmsTemplatesteps)
                    {
                        if (item.STEPID == 0)
                        {
                            item.SWMSID = swmstemplate.SWMSID;
                            await ClearConnection.CreateSwmsTemplatestep(item);
                        }
                        else if (item.STEPID != 0 && item.ISDELETE == true)
                        {
                            await ClearConnection.DeleteSwmsTemplatestep(item.STEPID);
                        }
                        else
                        {
                            await ClearConnection.UpdateSwmsTemplateSteps(item.STEPID, item);
                        }
                    }

                    if (swmstemplate.SwmsPlantequipments != null)
                    {
                        foreach (var item in swmstemplate.SwmsPlantequipments)
                        {
                            if (item.IS_DELETED == true && item.PEID != 0)
                            {
                                await ClearConnection.DeleteSwmsPlantequipment(item.PEID);
                            }
                            else if (item.PEID == 0)
                            {
                                await ClearConnection.CreateSwmsPlantequipment(item);
                            }
                        }
                    }

                    foreach (var item in swmstemplate.SwmsPperequireds)
                    {
                        if (item.IS_DELETED == true && item.PPEID != 0)
                        {
                            await ClearConnection.DeleteSwmsPperequired(item.PPEID);
                        }
                        else if (item.PPEID == 0)
                        {
                            await ClearConnection.CreateSwmsPperequired(item);
                        }
                    }

                    foreach (var item in swmstemplate.SwmsLicencespermits)
                    {
                        if (item.IS_DELETED == true && item.LPID != 0)
                        {
                            await ClearConnection.DeleteSwmsLicencespermit(item.LPID);
                        }
                        else if (item.LPID == 0)
                        {
                            await ClearConnection.CreateSwmsLicencespermit(item);
                        }
                    }

                    foreach (var item in swmstemplate.SwmsReferencedlegislations)
                    {
                        if (item.IS_DELETED == true && item.REFLID != 0)
                        {
                            await ClearConnection.DeleteSwmsReferencedlegislation(item.REFLID);
                        }
                        else if (item.REFLID == 0)
                        {
                            await ClearConnection.CreateSwmsReferencedlegislation(item);
                        }
                    }

                    foreach (var item in swmstemplate.SwmsHazardousmaterials)
                    {
                        if (item.IS_DELETED == true && item.HAZARDOUSMATERIALID != 0)
                        {
                            await ClearConnection.DeleteSwmsHazardousmaterial(item.HAZARDOUSMATERIALID);
                        }
                        else if (item.HAZARDOUSMATERIALID == 0)
                        {
                            await ClearConnection.CreateSwmsHazardousmaterial(item);
                        }
                    }


                    NotificationService.Notify(NotificationSeverity.Success, $"Success", $"SWMS Template Saved as draft!");
                    UriHelper.NavigateTo("swms-template");


                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to save SwmsTemplate!");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
                await Task.Delay(1);
            }
        }



    }
}
