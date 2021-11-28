using Microsoft.AspNetCore.Mvc;
using Clear.Risk.Data;

namespace Clear.Risk
{
    public partial class ExportClearConnectionController : ExportController
    {
        private readonly ClearConnectionContext context;

        public ExportClearConnectionController(ClearConnectionContext context)
        {
            this.context = context;
        }

        [HttpGet("/export/ClearConnection/applicences/csv")]
        public FileStreamResult ExportApplicencesToCSV()
        {
            return ToCSV(ApplyQuery(context.Applicences, Request.Query));
        }

        [HttpGet("/export/ClearConnection/applicences/excel")]
        public FileStreamResult ExportApplicencesToExcel()
        {
            return ToExcel(ApplyQuery(context.Applicences, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesments/csv")]
        public FileStreamResult ExportAssesmentsToCSV()
        {
            return ToCSV(ApplyQuery(context.Assesments, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesments/excel")]
        public FileStreamResult ExportAssesmentsToExcel()
        {
            return ToExcel(ApplyQuery(context.Assesments, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmentattachements/csv")]
        public FileStreamResult ExportAssesmentAttachementsToCSV()
        {
            return ToCSV(ApplyQuery(context.AssesmentAttachements, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmentattachements/excel")]
        public FileStreamResult ExportAssesmentAttachementsToExcel()
        {
            return ToExcel(ApplyQuery(context.AssesmentAttachements, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmentemployees/csv")]
        public FileStreamResult ExportAssesmentEmployeesToCSV()
        {
            return ToCSV(ApplyQuery(context.AssesmentEmployees, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmentemployees/excel")]
        public FileStreamResult ExportAssesmentEmployeesToExcel()
        {
            return ToExcel(ApplyQuery(context.AssesmentEmployees, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmentemployeeattachements/csv")]
        public FileStreamResult ExportAssesmentEmployeeAttachementsToCSV()
        {
            return ToCSV(ApplyQuery(context.AssesmentEmployeeAttachements, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmentemployeeattachements/excel")]
        public FileStreamResult ExportAssesmentEmployeeAttachementsToExcel()
        {
            return ToExcel(ApplyQuery(context.AssesmentEmployeeAttachements, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmentemployeestatuses/csv")]
        public FileStreamResult ExportAssesmentEmployeeStatusesToCSV()
        {
            return ToCSV(ApplyQuery(context.AssesmentEmployeeStatuses, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmentemployeestatuses/excel")]
        public FileStreamResult ExportAssesmentEmployeeStatusesToExcel()
        {
            return ToExcel(ApplyQuery(context.AssesmentEmployeeStatuses, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmenttasks/csv")]
        public FileStreamResult ExportAssesmenttasksToCSV()
        {
            return ToCSV(ApplyQuery(context.Assesmenttasks, Request.Query));
        }

        [HttpGet("/export/ClearConnection/assesmenttasks/excel")]
        public FileStreamResult ExportAssesmenttasksToExcel()
        {
            return ToExcel(ApplyQuery(context.Assesmenttasks, Request.Query));
        }

        [HttpGet("/export/ClearConnection/consequences/csv")]
        public FileStreamResult ExportConsequencesToCSV()
        {
            return ToCSV(ApplyQuery(context.Consequences, Request.Query));
        }

        [HttpGet("/export/ClearConnection/consequences/excel")]
        public FileStreamResult ExportConsequencesToExcel()
        {
            return ToExcel(ApplyQuery(context.Consequences, Request.Query));
        }

        [HttpGet("/export/ClearConnection/controlmeasurevalues/csv")]
        public FileStreamResult ExportControlMeasureValuesToCSV()
        {
            return ToCSV(ApplyQuery(context.ControlMeasureValues, Request.Query));
        }

        [HttpGet("/export/ClearConnection/controlmeasurevalues/excel")]
        public FileStreamResult ExportControlMeasureValuesToExcel()
        {
            return ToExcel(ApplyQuery(context.ControlMeasureValues, Request.Query));
        }

        [HttpGet("/export/ClearConnection/countries/csv")]
        public FileStreamResult ExportCountriesToCSV()
        {
            return ToCSV(ApplyQuery(context.Countries, Request.Query));
        }

        [HttpGet("/export/ClearConnection/countries/excel")]
        public FileStreamResult ExportCountriesToExcel()
        {
            return ToExcel(ApplyQuery(context.Countries, Request.Query));
        }

        [HttpGet("/export/ClearConnection/entitystatuses/csv")]
        public FileStreamResult ExportEntityStatusesToCSV()
        {
            return ToCSV(ApplyQuery(context.EntityStatuses, Request.Query));
        }

        [HttpGet("/export/ClearConnection/entitystatuses/excel")]
        public FileStreamResult ExportEntityStatusesToExcel()
        {
            return ToExcel(ApplyQuery(context.EntityStatuses, Request.Query));
        }

        [HttpGet("/export/ClearConnection/escalationlevels/csv")]
        public FileStreamResult ExportEscalationLevelsToCSV()
        {
            return ToCSV(ApplyQuery(context.EscalationLevels, Request.Query));
        }

        [HttpGet("/export/ClearConnection/escalationlevels/excel")]
        public FileStreamResult ExportEscalationLevelsToExcel()
        {
            return ToExcel(ApplyQuery(context.EscalationLevels, Request.Query));
        }

        [HttpGet("/export/ClearConnection/hazardmaterialvalues/csv")]
        public FileStreamResult ExportHazardMaterialValuesToCSV()
        {
            return ToCSV(ApplyQuery(context.HazardMaterialValues, Request.Query));
        }

        [HttpGet("/export/ClearConnection/hazardmaterialvalues/excel")]
        public FileStreamResult ExportHazardMaterialValuesToExcel()
        {
            return ToExcel(ApplyQuery(context.HazardMaterialValues, Request.Query));
        }

        [HttpGet("/export/ClearConnection/hazardvalues/csv")]
        public FileStreamResult ExportHazardValuesToCSV()
        {
            return ToCSV(ApplyQuery(context.HazardValues, Request.Query));
        }

        [HttpGet("/export/ClearConnection/hazardvalues/excel")]
        public FileStreamResult ExportHazardValuesToExcel()
        {
            return ToExcel(ApplyQuery(context.HazardValues, Request.Query));
        }

        [HttpGet("/export/ClearConnection/highriskcategories/csv")]
        public FileStreamResult ExportHighRiskCategoriesToCSV()
        {
            return ToCSV(ApplyQuery(context.HighRiskCategories, Request.Query));
        }

        [HttpGet("/export/ClearConnection/highriskcategories/excel")]
        public FileStreamResult ExportHighRiskCategoriesToExcel()
        {
            return ToExcel(ApplyQuery(context.HighRiskCategories, Request.Query));
        }

        [HttpGet("/export/ClearConnection/impacttypes/csv")]
        public FileStreamResult ExportImpactTypesToCSV()
        {
            return ToCSV(ApplyQuery(context.ImpactTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/impacttypes/excel")]
        public FileStreamResult ExportImpactTypesToExcel()
        {
            return ToExcel(ApplyQuery(context.ImpactTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/industrytypes/csv")]
        public FileStreamResult ExportIndustryTypesToCSV()
        {
            return ToCSV(ApplyQuery(context.IndustryTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/industrytypes/excel")]
        public FileStreamResult ExportIndustryTypesToExcel()
        {
            return ToExcel(ApplyQuery(context.IndustryTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/licencepermits/csv")]
        public FileStreamResult ExportLicencePermitsToCSV()
        {
            return ToCSV(ApplyQuery(context.LicencePermits, Request.Query));
        }

        [HttpGet("/export/ClearConnection/licencepermits/excel")]
        public FileStreamResult ExportLicencePermitsToExcel()
        {
            return ToExcel(ApplyQuery(context.LicencePermits, Request.Query));
        }

        [HttpGet("/export/ClearConnection/people/csv")]
        public FileStreamResult ExportPeopleToCSV()
        {
            return ToCSV(ApplyQuery(context.People, Request.Query));
        }

        [HttpGet("/export/ClearConnection/people/excel")]
        public FileStreamResult ExportPeopleToExcel()
        {
            return ToExcel(ApplyQuery(context.People, Request.Query));
        }

        [HttpGet("/export/ClearConnection/personroles/csv")]
        public FileStreamResult ExportPersonRolesToCSV()
        {
            return ToCSV(ApplyQuery(context.PersonRoles, Request.Query));
        }

        [HttpGet("/export/ClearConnection/personroles/excel")]
        public FileStreamResult ExportPersonRolesToExcel()
        {
            return ToExcel(ApplyQuery(context.PersonRoles, Request.Query));
        }

        [HttpGet("/export/ClearConnection/personsites/csv")]
        public FileStreamResult ExportPersonSitesToCSV()
        {
            return ToCSV(ApplyQuery(context.PersonSites, Request.Query));
        }

        [HttpGet("/export/ClearConnection/personsites/excel")]
        public FileStreamResult ExportPersonSitesToExcel()
        {
            return ToExcel(ApplyQuery(context.PersonSites, Request.Query));
        }

        [HttpGet("/export/ClearConnection/persontypes/csv")]
        public FileStreamResult ExportPersonTypesToCSV()
        {
            return ToCSV(ApplyQuery(context.PersonTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/persontypes/excel")]
        public FileStreamResult ExportPersonTypesToExcel()
        {
            return ToExcel(ApplyQuery(context.PersonTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/plantequipments/csv")]
        public FileStreamResult ExportPlantEquipmentsToCSV()
        {
            return ToCSV(ApplyQuery(context.PlantEquipments, Request.Query));
        }

        [HttpGet("/export/ClearConnection/plantequipments/excel")]
        public FileStreamResult ExportPlantEquipmentsToExcel()
        {
            return ToExcel(ApplyQuery(context.PlantEquipments, Request.Query));
        }

        [HttpGet("/export/ClearConnection/ppevalues/csv")]
        public FileStreamResult ExportPpevaluesToCSV()
        {
            return ToCSV(ApplyQuery(context.Ppevalues, Request.Query));
        }

        [HttpGet("/export/ClearConnection/ppevalues/excel")]
        public FileStreamResult ExportPpevaluesToExcel()
        {
            return ToExcel(ApplyQuery(context.Ppevalues, Request.Query));
        }

        [HttpGet("/export/ClearConnection/referencedlegislations/csv")]
        public FileStreamResult ExportReferencedLegislationsToCSV()
        {
            return ToCSV(ApplyQuery(context.ReferencedLegislations, Request.Query));
        }

        [HttpGet("/export/ClearConnection/referencedlegislations/excel")]
        public FileStreamResult ExportReferencedLegislationsToExcel()
        {
            return ToExcel(ApplyQuery(context.ReferencedLegislations, Request.Query));
        }

        [HttpGet("/export/ClearConnection/resposnsibletypes/csv")]
        public FileStreamResult ExportResposnsibleTypesToCSV()
        {
            return ToCSV(ApplyQuery(context.ResposnsibleTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/resposnsibletypes/excel")]
        public FileStreamResult ExportResposnsibleTypesToExcel()
        {
            return ToExcel(ApplyQuery(context.ResposnsibleTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/risklikelyhoods/csv")]
        public FileStreamResult ExportRiskLikelyhoodsToCSV()
        {
            return ToCSV(ApplyQuery(context.RiskLikelyhoods, Request.Query));
        }

        [HttpGet("/export/ClearConnection/risklikelyhoods/excel")]
        public FileStreamResult ExportRiskLikelyhoodsToExcel()
        {
            return ToExcel(ApplyQuery(context.RiskLikelyhoods, Request.Query));
        }

        [HttpGet("/export/ClearConnection/scheduletypes/csv")]
        public FileStreamResult ExportScheduleTypesToCSV()
        {
            return ToCSV(ApplyQuery(context.ScheduleTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/scheduletypes/excel")]
        public FileStreamResult ExportScheduleTypesToExcel()
        {
            return ToExcel(ApplyQuery(context.ScheduleTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/smtpsetups/csv")]
        public FileStreamResult ExportSmtpsetupsToCSV()
        {
            return ToCSV(ApplyQuery(context.Smtpsetups, Request.Query));
        }

        [HttpGet("/export/ClearConnection/smtpsetups/excel")]
        public FileStreamResult ExportSmtpsetupsToExcel()
        {
            return ToExcel(ApplyQuery(context.Smtpsetups, Request.Query));
        }

        [HttpGet("/export/ClearConnection/states/csv")]
        public FileStreamResult ExportStatesToCSV()
        {
            return ToCSV(ApplyQuery(context.States, Request.Query));
        }

        [HttpGet("/export/ClearConnection/states/excel")]
        public FileStreamResult ExportStatesToExcel()
        {
            return ToExcel(ApplyQuery(context.States, Request.Query));
        }

        [HttpGet("/export/ClearConnection/statuslevels/csv")]
        public FileStreamResult ExportStatusLevelsToCSV()
        {
            return ToCSV(ApplyQuery(context.StatusLevels, Request.Query));
        }

        [HttpGet("/export/ClearConnection/statuslevels/excel")]
        public FileStreamResult ExportStatusLevelsToExcel()
        {
            return ToExcel(ApplyQuery(context.StatusLevels, Request.Query));
        }

        [HttpGet("/export/ClearConnection/statusmasters/csv")]
        public FileStreamResult ExportStatusMastersToCSV()
        {
            return ToCSV(ApplyQuery(context.StatusMasters, Request.Query));
        }

        [HttpGet("/export/ClearConnection/statusmasters/excel")]
        public FileStreamResult ExportStatusMastersToExcel()
        {
            return ToExcel(ApplyQuery(context.StatusMasters, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmshazardousmaterials/csv")]
        public FileStreamResult ExportSwmsHazardousmaterialsToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsHazardousmaterials, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmshazardousmaterials/excel")]
        public FileStreamResult ExportSwmsHazardousmaterialsToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsHazardousmaterials, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmslicencespermits/csv")]
        public FileStreamResult ExportSwmsLicencespermitsToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsLicencespermits, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmslicencespermits/excel")]
        public FileStreamResult ExportSwmsLicencespermitsToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsLicencespermits, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmsplantequipments/csv")]
        public FileStreamResult ExportSwmsPlantequipmentsToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsPlantequipments, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmsplantequipments/excel")]
        public FileStreamResult ExportSwmsPlantequipmentsToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsPlantequipments, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmspperequireds/csv")]
        public FileStreamResult ExportSwmsPperequiredsToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsPperequireds, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmspperequireds/excel")]
        public FileStreamResult ExportSwmsPperequiredsToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsPperequireds, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmsreferencedlegislations/csv")]
        public FileStreamResult ExportSwmsReferencedlegislationsToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsReferencedlegislations, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmsreferencedlegislations/excel")]
        public FileStreamResult ExportSwmsReferencedlegislationsToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsReferencedlegislations, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmssections/csv")]
        public FileStreamResult ExportSwmsSectionsToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsSections, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmssections/excel")]
        public FileStreamResult ExportSwmsSectionsToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsSections, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmstemplates/csv")]
        public FileStreamResult ExportSwmsTemplatesToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsTemplates, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmstemplates/excel")]
        public FileStreamResult ExportSwmsTemplatesToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsTemplates, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmstemplatecategories/csv")]
        public FileStreamResult ExportSwmsTemplateCategoriesToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsTemplateCategories, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmstemplatecategories/excel")]
        public FileStreamResult ExportSwmsTemplateCategoriesToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsTemplateCategories, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmstemplatesteps/csv")]
        public FileStreamResult ExportSwmsTemplatestepsToCSV()
        {
            return ToCSV(ApplyQuery(context.SwmsTemplatesteps, Request.Query));
        }

        [HttpGet("/export/ClearConnection/swmstemplatesteps/excel")]
        public FileStreamResult ExportSwmsTemplatestepsToExcel()
        {
            return ToExcel(ApplyQuery(context.SwmsTemplatesteps, Request.Query));
        }

        [HttpGet("/export/ClearConnection/systemroles/csv")]
        public FileStreamResult ExportSystemrolesToCSV()
        {
            return ToCSV(ApplyQuery(context.Systemroles, Request.Query));
        }

        [HttpGet("/export/ClearConnection/systemroles/excel")]
        public FileStreamResult ExportSystemrolesToExcel()
        {
            return ToExcel(ApplyQuery(context.Systemroles, Request.Query));
        }

        [HttpGet("/export/ClearConnection/templates/csv")]
        public FileStreamResult ExportTemplatesToCSV()
        {
            return ToCSV(ApplyQuery(context.Templates, Request.Query));
        }

        [HttpGet("/export/ClearConnection/templates/excel")]
        public FileStreamResult ExportTemplatesToExcel()
        {
            return ToExcel(ApplyQuery(context.Templates, Request.Query));
        }

        [HttpGet("/export/ClearConnection/templatetypes/csv")]
        public FileStreamResult ExportTemplateTypesToCSV()
        {
            return ToCSV(ApplyQuery(context.TemplateTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/templatetypes/excel")]
        public FileStreamResult ExportTemplateTypesToExcel()
        {
            return ToExcel(ApplyQuery(context.TemplateTypes, Request.Query));
        }

        [HttpGet("/export/ClearConnection/templateattachments/csv")]
        public FileStreamResult ExportTemplateattachmentsToCSV()
        {
            return ToCSV(ApplyQuery(context.Templateattachments, Request.Query));
        }

        [HttpGet("/export/ClearConnection/templateattachments/excel")]
        public FileStreamResult ExportTemplateattachmentsToExcel()
        {
            return ToExcel(ApplyQuery(context.Templateattachments, Request.Query));
        }

        [HttpGet("/export/ClearConnection/tradecategories/csv")]
        public FileStreamResult ExportTradeCategoriesToCSV()
        {
            return ToCSV(ApplyQuery(context.TradeCategories, Request.Query));
        }

        [HttpGet("/export/ClearConnection/tradecategories/excel")]
        public FileStreamResult ExportTradeCategoriesToExcel()
        {
            return ToExcel(ApplyQuery(context.TradeCategories, Request.Query));
        }

        [HttpGet("/export/ClearConnection/warninglevels/csv")]
        public FileStreamResult ExportWarningLevelsToCSV()
        {
            return ToCSV(ApplyQuery(context.WarningLevels, Request.Query));
        }

        [HttpGet("/export/ClearConnection/warninglevels/excel")]
        public FileStreamResult ExportWarningLevelsToExcel()
        {
            return ToExcel(ApplyQuery(context.WarningLevels, Request.Query));
        }
    }
}
