using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Clear.Risk.Models.ClearConnection;
namespace Clear.Risk.Pages.Lookup
{
    public partial class ManageBlog : ComponentBase
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

        protected IList<BlogTable> getBlogTable = new List<BlogTable>();
        
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
            var clearRiskgetBlogTableResult = await ClearRisk.GetBlogTable();

            getBlogTable = (from x in clearRiskgetBlogTableResult
                                       select new BlogTable
                                       {
                                           Blog_Id = x.Blog_Id,
                                           BgTittle = x.BgTittle,
                                           BgShortDetails = x.BgShortDetails,
                                           BgLongDetails = x.BgLongDetails,
                                           BgImgPath = x.BgImgPath,
                                           CreatedBy = x.CreatedBy,
                                           CreatedDate = x.CreatedDate
                                       })
                                  .ToList();
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddBlogTable>("Add Blogs", null);

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }

        protected async System.Threading.Tasks.Task DeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var clearRiskDeleteBlogTable = await ClearRisk.DeleteBlogTable(int.Parse($"{data.Blog_Id}"));
                    if (clearRiskDeleteBlogTable != null)
                    {
                        getBlogTable.Remove(getBlogTable.FirstOrDefault(x => x.Blog_Id == data.Blog_Id));
                        isLoading = false;
                        StateHasChanged();
                    }
                }
                isLoading = false;
                StateHasChanged();
            }
            catch (System.Exception clearRiskDeleteBlogTableException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete the Blog");
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async System.Threading.Tasks.Task EditButtonClick(MouseEventArgs args, dynamic data)
        {

            var dialogResult = await DialogService.OpenAsync<EditBlogTable>("Edit Blog", new Dictionary<string, object>() { { "Blog_Id", data.Blog_Id } });

            await InvokeAsync(() => { StateHasChanged(); });
            await Load();
        }
    }
}