using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using Clear.Risk.Models.ClearConnection;

namespace Clear.Risk.Data
{
  public partial class ClearConnectionContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public ClearConnectionContext(DbContextOptions<ClearConnectionContext> options):base(options)
    {
            //if (!Database.EnsureCreated())
            //{
            //    Database.Migrate();
            //}
        }

    public ClearConnectionContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

       
        builder.Entity<Clear.Risk.Models.ClearConnection.PersonRole>().HasKey(table => new {
          table.PERSON_ID, table.ROLE_ID
        });
       
       

            builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
             .HasOne(i => i.Company)
             .WithMany(i => i.Assesments)
             .HasForeignKey(i => i.COMPANYID)
             .HasPrincipalKey(i => i.PERSON_ID);

            builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
           .HasOne(i => i.Client)
           .WithMany(i => i.ClientAssesments)
           .HasForeignKey(i => i.CLIENTID)
           .HasPrincipalKey(i => i.PERSON_ID);

            builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .HasOne(i => i.TradeCategory)
              .WithMany(i => i.Assesments)
              .HasForeignKey(i => i.TRADECATEGORYID)
              .HasPrincipalKey(i => i.TRADE_CATEGORY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .HasOne(i => i.StatusMaster)
              .WithMany(i => i.Assesments)
              .HasForeignKey(i => i.STATUS)
              .HasPrincipalKey(i => i.STATUS_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .HasOne(i => i.PersonSite)
              .WithMany(i => i.Assesments)
              .HasForeignKey(i => i.WORK_SITE_ID)
              .HasPrincipalKey(i => i.PERSON_SITE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .HasOne(i => i.Survey)
              .WithMany(i => i.Assesments)
              .HasForeignKey(i => i.COVID_SURVEY_ID)
              .HasPrincipalKey(i => i.SURVEY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .HasOne(i => i.IndustryType)
              .WithMany(i => i.Assesments)
              .HasForeignKey(i => i.INDUSTRY_ID)
              .HasPrincipalKey(i => i.INDUSTRY_TYPE_ID);
        


            builder.Entity<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement>()
       .HasKey(a => new { a.ASSESMENT_EMPLOYEE_ID, a.ATTACHEMENTID });


            builder.Entity<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction>()
              .HasOne(i => i.Person)
              .WithMany(i => i.CompanyAccountTransactions)
              .HasForeignKey(i => i.COMPANY_ID)
              .HasPrincipalKey(i => i.PERSON_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction>()
              .HasOne(i => i.Currency)
              .WithMany(i => i.CompanyAccountTransactions)
              .HasForeignKey(i => i.CURRENCY_ID)
              .HasPrincipalKey(i => i.CURRENCY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransaction>()
              .HasOne(i => i.TransactionStatus)
              .WithMany(i => i.CompanyTransactions)
              .HasForeignKey(i => i.TRANSACTION_STATUS_ID)
              .HasPrincipalKey(i => i.TRANSACTION_STATUS_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransaction>()
              .HasOne(i => i.Currency)
              .WithMany(i => i.CompanyTransactions)
              .HasForeignKey(i => i.CURRENCY_ID)
              .HasPrincipalKey(i => i.CURRENCY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail>()
              .HasOne(i => i.CompanyTransaction)
              .WithMany(i => i.CompanyTransactionDetails)
              .HasForeignKey(i => i.TransactionID)
              .HasPrincipalKey(i => i.TRANSACTIONID);
        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail>()
              .HasOne(i => i.Applicence)
              .WithMany(i => i.CompanyTransactionDetails)
              .HasForeignKey(i => i.PRODUCT_ID)
              .HasPrincipalKey(i => i.APPLICENCEID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .HasOne(i => i.Person1)
              .WithMany(i => i.People1)
              .HasForeignKey(i => i.PERSON_ID)
              .HasPrincipalKey(i => i.PERSON_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .HasOne(i => i.State)
              .WithMany(i => i.People)
              .HasForeignKey(i => i.PERSONAL_STATE_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .HasOne(i => i.Country)
              .WithMany(i => i.People)
              .HasForeignKey(i => i.PERSONAL_COUNTRY_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .HasOne(i => i.State1)
              .WithMany(i => i.People1)
              .HasForeignKey(i => i.BUSINESS_STATE_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .HasOne(i => i.Country1)
              .WithMany(i => i.People1)
              .HasForeignKey(i => i.BUSINESS_COUNTRY_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .HasOne(i => i.PersonType)
              .WithMany(i => i.People)
              .HasForeignKey(i => i.COMPANYTYPE)
              .HasPrincipalKey(i => i.PERSON_TYPE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .HasOne(i => i.Applicence)
              .WithMany(i => i.People)
              .HasForeignKey(i => i.APPLICENCEID)
              .HasPrincipalKey(i => i.APPLICENCEID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .HasOne(i => i.Currency)
              .WithMany(i => i.People)
              .HasForeignKey(i => i.CURRENCY_ID)
              .HasPrincipalKey(i => i.CURRENCY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .HasOne(i => i.Person)
              .WithMany(i => i.PersonSites)
              .HasForeignKey(i => i.PERSON_ID)
              .HasPrincipalKey(i => i.PERSON_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .HasOne(i => i.State)
              .WithMany(i => i.PersonSites)
              .HasForeignKey(i => i.STATE_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .HasOne(i => i.Country)
              .WithMany(i => i.PersonSites)
              .HasForeignKey(i => i.COUNTRY_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.State>()
              .HasOne(i => i.Country)
              .WithMany(i => i.States)
              .HasForeignKey(i => i.COUNTRYID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .HasOne(i => i.SurveyType)
              .WithMany(i => i.Surveys)
              .HasForeignKey(i => i.SURVEY_TYPE_ID)
              .HasPrincipalKey(i => i.SURVEY_TYPE_ID);
            builder.Entity<Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer>()
                  .HasOne(i => i.Question)
                  .WithMany(i => i.SurveyAnswers)
                  .HasForeignKey(i => i.SURVEY_QUESTION_ID)
                  .HasPrincipalKey(i => i.SURVEYQ_QUESTION_ID);

            builder.Entity<Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer>()
                 .HasOne(i => i.GoToQuestion)
                 .WithMany(i => i.SurveyGoToAnswers)
                 .HasForeignKey(i => i.GOTO_QUESTION_ID)
                 .HasPrincipalKey(i => i.SURVEYQ_QUESTION_ID);

            //builder.Entity<Clear.Risk.Models.ClearConnection.SurveyAnswerValue>()
            //      .HasOne(i => i.SurveyAnswerChecklist)
            //      .WithMany(i => i.SurveyAnswerValues)
            //      .HasForeignKey(i => i.SURVEY_CHECKLIST_ID)
            //    .HasPrincipalKey(i => i.SURVEY_ANSWER_CHECKLIST_ID);

            builder.Entity<Clear.Risk.Models.ClearConnection.SurveyAnswerValue>()
              .HasOne(i => i.SurveyAnswer)
              .WithMany(i => i.SurveyAnswerValues)
              .HasForeignKey(i => i.SURVEY_ANSWER_ID)
              .HasPrincipalKey(i => i.SURVEY_ANSWER_ID);

            builder.Entity<Clear.Risk.Models.ClearConnection.SurveyQuestion>()
              .HasOne(i => i.Survey)
              .WithMany(i => i.SurveyQuestions)
              .HasForeignKey(i => i.SURVEY_ID)
              .HasPrincipalKey(i => i.SURVEY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SurveyQuestion>()
              .HasOne(i => i.QuestionType)
              .WithMany(i => i.SurveyQuestions)
              .HasForeignKey(i => i.QUESTION_TYPE_ID)
              .HasPrincipalKey(i => i.QUESTION_TYPE_ID);
            //builder.Entity<Clear.Risk.Models.ClearConnection.SurveyQuestion>()
            //      .HasOne(i => i.ParentQuestion)
            //      .WithMany(i => i.Survey)
            //      .HasForeignKey(i => i.PARENT_Q_ID)
            //      .HasPrincipalKey(i => i.SURVEYQ_QUESTION_ID);

            builder.Entity<Clear.Risk.Models.ClearConnection.SurveyAnswerChecklist>()
                  .HasOne(i => i.Question)
                  .WithMany(i => i.SurveyAnswerChecklists)
                  .HasForeignKey(i => i.SURVEY_QUESTION_ID)
                  .HasPrincipalKey(i => i.SURVEYQ_QUESTION_ID);


            builder.Entity<Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial>()
              .HasOne(i => i.SwmsTemplate)
              .WithMany(i => i.SwmsHazardousmaterials)
              .HasForeignKey(i => i.SWMSID)
              .HasPrincipalKey(i => i.SWMSID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial>()
              .HasOne(i => i.HazardMaterialValue)
              .WithMany(i => i.SwmsHazardousmaterials)
              .HasForeignKey(i => i.HAZARD_MATERIAL_ID)
              .HasPrincipalKey(i => i.HAZARD_MATERIAL_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsLicencespermit>()
              .HasOne(i => i.SwmsTemplate)
              .WithMany(i => i.SwmsLicencespermits)
              .HasForeignKey(i => i.SWMSID)
              .HasPrincipalKey(i => i.SWMSID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsLicencespermit>()
              .HasOne(i => i.LicencePermit)
              .WithMany(i => i.SwmsLicencespermits)
              .HasForeignKey(i => i.LICENCE_PERMIT_ID)
              .HasPrincipalKey(i => i.PERMIT_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsPlantequipment>()
              .HasOne(i => i.SwmsTemplate)
              .WithMany(i => i.SwmsPlantequipments)
              .HasForeignKey(i => i.SWMSID)
              .HasPrincipalKey(i => i.SWMSID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsPlantequipment>()
              .HasOne(i => i.PlantEquipment)
              .WithMany(i => i.SwmsPlantequipments)
              .HasForeignKey(i => i.PLANT_EQUIPMENT_ID)
              .HasPrincipalKey(i => i.PLANT_EQUIPMENT_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsPperequired>()
              .HasOne(i => i.SwmsTemplate)
              .WithMany(i => i.SwmsPperequireds)
              .HasForeignKey(i => i.SWMSID)
              .HasPrincipalKey(i => i.SWMSID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsPperequired>()
              .HasOne(i => i.Ppevalue)
              .WithMany(i => i.SwmsPperequireds)
              .HasForeignKey(i => i.PPE_VALUE_ID)
              .HasPrincipalKey(i => i.PPE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation>()
              .HasOne(i => i.SwmsTemplate)
              .WithMany(i => i.SwmsReferencedlegislations)
              .HasForeignKey(i => i.SWMSID)
              .HasPrincipalKey(i => i.SWMSID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation>()
              .HasOne(i => i.ReferencedLegislation)
              .WithMany(i => i.SwmsReferencedlegislations)
              .HasForeignKey(i => i.REFERENCE_LEGISLATION_ID)
              .HasPrincipalKey(i => i.LEGISLATION_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.Person)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.COMPANYID)
              .HasPrincipalKey(i => i.PERSON_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.TemplateType)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.SWMSTYPE)
              .HasPrincipalKey(i => i.TEMPLATE_TYPE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.Template)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.TEMPLATE_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.StatusMaster)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.STATUS)
              .HasPrincipalKey(i => i.STATUS_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.SwmsTemplateCategory)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.TEMPLATEQUESTION)
              .HasPrincipalKey(i => i.TEMPLATE_CATEGORY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.Country)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.COUNTRY_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.EscalationLevel)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.ESCALATION_LEVEL_ID)
              .HasPrincipalKey(i => i.ESCALATION_LEVEL_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.WarningLevel)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.WARNING_LEVEL_ID)
              .HasPrincipalKey(i => i.WARNING_LEVEL_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.StatusLevel)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.STATUS_LEVEL_ID)
              .HasPrincipalKey(i => i.STATUS_LEVEL_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.State)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.STATEID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.TradeCategory)
              .WithMany(i => i.SwmsTemplates)
              .HasForeignKey(i => i.TRADECATEGORYID)
              .HasPrincipalKey(i => i.TRADE_CATEGORY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .HasOne(i => i.Person1)
              .WithMany(i => i.SwmsTemplates1)
              .HasForeignKey(i => i.FM_MANAGER_ID)
              .HasPrincipalKey(i => i.PERSON_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplatestep>()
              .HasOne(i => i.SwmsTemplate)
              .WithMany(i => i.SwmsTemplatesteps)
              .HasForeignKey(i => i.SWMSID)
              .HasPrincipalKey(i => i.SWMSID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplatestep>()
              .HasOne(i => i.RiskLikelyhood)
              .WithMany(i => i.SwmsTemplatesteps)
              .HasForeignKey(i => i.RISK_LIKELYHOOD_ID)
              .HasPrincipalKey(i => i.RISK_VALUE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplatestep>()
              .HasOne(i => i.Consequence)
              .WithMany(i => i.SwmsTemplatesteps)
              .HasForeignKey(i => i.CONSEQUENCE_ID)
              .HasPrincipalKey(i => i.CONSEQUENCE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplatestep>()
              .HasOne(i => i.RiskLikelyhood1)
              .WithMany(i => i.SwmsTemplatesteps1)
              .HasForeignKey(i => i.RISK_AFTER_LIKELYHOOD_ID)
              .HasPrincipalKey(i => i.RISK_VALUE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplatestep>()
              .HasOne(i => i.Consequence1)
              .WithMany(i => i.SwmsTemplatesteps1)
              .HasForeignKey(i => i.AFTER_CONSEQUENCE_ID)
              .HasPrincipalKey(i => i.CONSEQUENCE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplatestep>()
              .HasOne(i => i.ResposnsibleType)
              .WithMany(i => i.SwmsTemplatesteps)
              .HasForeignKey(i => i.RESPOSNSIBLE_TYPE_ID)
              .HasPrincipalKey(i => i.RESPONSIBLE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.StatusMaster)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.STATUS)
              .HasPrincipalKey(i => i.STATUS_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.TradeCategory)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.TRADECATEGORYID)
              .HasPrincipalKey(i => i.TRADE_CATEGORY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.Person)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.COMPANYID)
              .HasPrincipalKey(i => i.PERSON_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.SwmsTemplateCategory)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.TYPEFORID)
              .HasPrincipalKey(i => i.TEMPLATE_CATEGORY_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.Country)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.COUNTRY_ID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.EscalationLevel)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.ESCALATION_LEVEL_ID)
              .HasPrincipalKey(i => i.ESCALATION_LEVEL_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.WarningLevel)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.WARNING_LEVEL_ID)
              .HasPrincipalKey(i => i.WARNING_LEVEL_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.StatusLevel)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.STATUS_LEVEL_ID)
              .HasPrincipalKey(i => i.STATUS_LEVEL_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .HasOne(i => i.State)
              .WithMany(i => i.Templates)
              .HasForeignKey(i => i.STATEID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Templateattachment>()
              .HasOne(i => i.Template)
              .WithMany(i => i.Templateattachments)
              .HasForeignKey(i => i.TEMPLATEID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.Templateattachment>()
              .HasOne(i => i.TemplateType)
              .WithMany(i => i.Templateattachments)
              .HasForeignKey(i => i.TEMPLATETYPEID)
              .HasPrincipalKey(i => i.TEMPLATE_TYPE_ID);
        builder.Entity<Clear.Risk.Models.ClearConnection.TradeCategory>()
              .HasOne(i => i.TradeCategory1)
              .WithMany(i => i.TradeCategories1)
              .HasForeignKey(i => i.PARENT_ID)
              .HasPrincipalKey(i => i.TRADE_CATEGORY_ID);

        builder.Entity<Clear.Risk.Models.ClearConnection.Applicence>()
              .Property(p => p.IS_DEFAULT)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Applicence>()
              .Property(p => p.PRICE)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Applicence>()
              .Property(p => p.DISCOUNT)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Applicence>()
              .Property(p => p.NETPRICE)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.STATUS)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.ISCOMPLETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.ISCOVIDSURVEY)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.ISSCHEDULE)
              .HasDefaultValueSql("((0))");

        
 

        

        
       

      

        

        builder.Entity<Clear.Risk.Models.ClearConnection.AssesmentAttachement>()
              .Property(p => p.ATTACHEMENTDATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement>()
              .Property(p => p.ASSIGNED_DATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement>()
              .Property(p => p.EMPLOYEE_STATUS)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction>()
              .Property(p => p.TRANSACTION_DATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction>()
              .Property(p => p.PAYMENT_AMOUNT)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction>()
              .Property(p => p.DEPOSITE_AMOUNT)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Consequence>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Consequence>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Consequence>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Consequence>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ControlMeasureValue>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ControlMeasureValue>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ControlMeasureValue>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ControlMeasureValue>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HazardMaterialValue>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HazardMaterialValue>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HazardMaterialValue>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HazardMaterialValue>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HazardValue>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HazardValue>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HazardValue>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HazardValue>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HighRiskCategory>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HighRiskCategory>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HighRiskCategory>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.HighRiskCategory>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ImpactType>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ImpactType>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ImpactType>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ImpactType>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.LicencePermit>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.LicencePermit>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.LicencePermit>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.LicencePermit>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .Property(p => p.CURRENT_BALANCE)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .Property(p => p.CREATED_DATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .Property(p => p.UPDATED_DATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.PersonType>()
              .Property(p => p.REGDEFAULT)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.PersonType>()
              .Property(p => p.EMPDEFAULT)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.PlantEquipment>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.PlantEquipment>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.PlantEquipment>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.PlantEquipment>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Ppevalue>()
              .Property(p => p.ESCALATION_LEVEL)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Ppevalue>()
              .Property(p => p.WARNING_LEVEL)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Ppevalue>()
              .Property(p => p.STATUS_LEVEL)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ReferencedLegislation>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ReferencedLegislation>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ReferencedLegislation>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ReferencedLegislation>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ResposnsibleType>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ResposnsibleType>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ResposnsibleType>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.ResposnsibleType>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.RiskLikelyhood>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.RiskLikelyhood>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.RiskLikelyhood>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.RiskLikelyhood>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Smtpsetup>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.CREATED_DATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.UPDATED_DATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.TOTAL_SCORE)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.YES_NO_SCORE)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.CHOICE_SCORE)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.TEXT_SCORE)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

       

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsLicencespermit>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsPlantequipment>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsPperequired>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation>()
              .Property(p => p.IS_DELETED)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsSection>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsSection>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsSection>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsSection>()
              .Property(p => p.ENTITY_STATUS_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .Property(p => p.SWMSTYPE)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .Property(p => p.STATUS)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplatestep>()
              .Property(p => p.ISDELETE)
              .HasDefaultValueSql("((0))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Systemrole>()
              .Property(p => p.CREATED_DATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.Systemrole>()
              .Property(p => p.UPDATED_DATE)
              .HasDefaultValueSql("(getdate())");

        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .Property(p => p.ESCALATION_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .Property(p => p.WARNING_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .Property(p => p.STATUS_LEVEL_ID)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.TemplateType>()
              .Property(p => p.ISACTIVE)
              .HasDefaultValueSql("((1))");

        builder.Entity<Clear.Risk.Models.ClearConnection.TradeCategory>()
              .Property(p => p.HOURLY_COST)
              .HasDefaultValueSql("((0))");


        builder.Entity<Clear.Risk.Models.ClearConnection.Applicence>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Applicence>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Applicence>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.DOCUMENTDOWNLOAD)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.DOCUMENT_UPLOAD_DATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.WORKENDDATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.WORKSTARTDATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.ASSESMENTDATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesment>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.AssesmentAttachement>()
              .Property(p => p.ATTACHEMENTDATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement>()
              .Property(p => p.ASSIGNED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement>()
              .Property(p => p.ACCEPTED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement>()
              .Property(p => p.SINGNATURE_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesmenttask>()
              .Property(p => p.ASSIGNEDDATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.Assesmenttask>()
              .Property(p => p.SIGNEDDATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction>()
              .Property(p => p.TRANSACTION_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransaction>()
              .Property(p => p.TRANSACTIONDATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransaction>()
              .Property(p => p.PAYMENTDATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransaction>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransaction>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransaction>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail>()
              .Property(p => p.PlanValidFrom)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail>()
              .Property(p => p.PlanValidUpTo)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime2");

        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .Property(p => p.DOB)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .Property(p => p.APPLICENCE_STARTDATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Person>()
              .Property(p => p.APPLICENCE_ENDDATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.PersonSite>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Ppevalue>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Ppevalue>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Ppevalue>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Survey>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        

        
        

        builder.Entity<Clear.Risk.Models.ClearConnection.SurveyAnswerDetail>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.SurveyQuestion>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.SurveyQuestion>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.SurveyQuestion>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.SwmsTemplate>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Systemrole>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Systemrole>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Systemrole>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.Template>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.TradeCategory>()
              .Property(p => p.CREATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.TradeCategory>()
              .Property(p => p.UPDATED_DATE)
              .HasColumnType("datetime");

        builder.Entity<Clear.Risk.Models.ClearConnection.TradeCategory>()
              .Property(p => p.DELETED_DATE)
              .HasColumnType("datetime");

        this.OnModelBuilding(builder);
    }


    public DbSet<Clear.Risk.Models.ClearConnection.Applicence> Applicences
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Assesment> Assesments
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.AssesmentAttachement> AssesmentAttachements
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.AssesmentEmployee> AssesmentEmployees
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.AssesmentEmployeeAttachement> AssesmentEmployeeAttachements
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.AssesmentEmployeeStatus> AssesmentEmployeeStatuses
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Assesmenttask> Assesmenttasks
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.CompanyAccountTransaction> CompanyAccountTransactions
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.CompanyTransaction> CompanyTransactions
    {
      get;
      set;
    }
    public DbSet<Clear.Risk.Models.ClearConnection.CompanyDocumentFile> CompanyDocumentFiles
    {
      get;
      set;
    }
    public DbSet<Clear.Risk.Models.ClearConnection.CompanyTransactionDetail> CompanyTransactionDetails
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Consequence> Consequences
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.ControlMeasureValue> ControlMeasureValues
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Country> Countries
    {
      get;
      set;
    }
        public DbSet<Clear.Risk.Models.ClearConnection.Desigation> Desigations
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.Currency> Currencies
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.EntityStatus> EntityStatuses
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.EscalationLevel> EscalationLevels
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.HazardMaterialValue> HazardMaterialValues
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.HazardValue> HazardValues
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.HighRiskCategory> HighRiskCategories
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.ImpactType> ImpactTypes
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.IndustryType> IndustryTypes
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.LicencePermit> LicencePermits
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Person> People
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.PersonRole> PersonRoles
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.PersonSite> PersonSites
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.PersonType> PersonTypes
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.PlantEquipment> PlantEquipments
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Ppevalue> Ppevalues
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.QuestionType> QuestionTypes
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.ReferencedLegislation> ReferencedLegislations
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.ResposnsibleType> ResposnsibleTypes
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.RiskLikelyhood> RiskLikelyhoods
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.ScheduleType> ScheduleTypes
    {
      get;
      set;
    }

        public DbSet<Clear.Risk.Models.ClearConnection.ScheduleTime> ScheduleTimes
        {
            get;
            set;
        }
        public DbSet<Clear.Risk.Models.ClearConnection.AssesmentSchedule> AssesmentSchedules
        {
            get;
            set;
        }
        

        public DbSet<Clear.Risk.Models.ClearConnection.Smtpsetup> Smtpsetups
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.State> States
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.StatusLevel> StatusLevels
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.StatusMaster> StatusMasters
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Survey> Surveys
    {
      get;
      set;
    }

    //public DbSet<Clear.Risk.Models.ClearConnection.SurveyAnswer> SurveyAnswers
    //{
    //  get;
    //  set;
    //}

    public DbSet<Clear.Risk.Models.ClearConnection.SurveyAnswerChecklist> SurveyAnswerChecklists
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SurveyAnswerDetail> SurveyAnswerDetails
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SurveyAnswerValue> SurveyAnswerValues
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SurveyQuestion> SurveyQuestions
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SurveyType> SurveyTypes
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsHazardousmaterial> SwmsHazardousmaterials
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsLicencespermit> SwmsLicencespermits
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsPlantequipment> SwmsPlantequipments
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsPperequired> SwmsPperequireds
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsReferencedlegislation> SwmsReferencedlegislations
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsSection> SwmsSections
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsTemplate> SwmsTemplates
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsTemplateCategory> SwmsTemplateCategories
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.SwmsTemplatestep> SwmsTemplatesteps
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Systemrole> Systemroles
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Template> Templates
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.TemplateType> TemplateTypes
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.Templateattachment> Templateattachments
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.TradeCategory> TradeCategories
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.TransactionStatus> TransactionStatuses
    {
      get;
      set;
    }

    public DbSet<Clear.Risk.Models.ClearConnection.WarningLevel> WarningLevels
    {
      get;
      set;
    }

        public DbSet<Clear.Risk.Models.ClearConnection.SurveyReport> SurveyReports
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.WorkOrder> WorkOrders
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.WorkOrderType> WorkOrderTypes
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.WorkerPerson> WorkerPeople
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.ProcessType> ProcessTypes
        {
            get;
            set;
        }
        public DbSet<Clear.Risk.Models.ClearConnection.CriticalityMaster> CriticalityMasters
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.OrderStatus> OrderStatuses
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.PriorityMaster> PriorityMasters
        {
            get;
            set;
        }
        public DbSet<Clear.Risk.Models.ClearConnection.PersonContact> PersonContacts
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.AssesmentSite> AssesmentSites
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.SiteActivity> SiteActivities
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.HelpReference> HelpReferences
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.BlogTable> BlogTables
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.SystemFeatures> SystemFeaturess
        {
            get;
            set;
        }
        public DbSet<Clear.Risk.Models.ClearConnection.HowToUse> HowToUses
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.SurveyQuestionAnswer> SurveyQuestionAnswers
        {
            get;
            set;
        }

        public DbSet<Clear.Risk.Models.ClearConnection.AssessmentDocument> AssessmentDocuments { get; set; }

    }
}
