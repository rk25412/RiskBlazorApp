using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Clear.Risk.Models.ClearConnection;

namespace Clear.Risk.Pages
{
    public partial class SingleBlazor : ComponentBase
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
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }


        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }

        [Parameter]
        public dynamic Blog_Id { get; set; }
        protected RadzenDataList<BlogTable> datalist0;


        BlogTable getBlog = new BlogTable();
        

        protected bool isLoading { get; set; }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            
            isLoading = true;
            StateHasChanged();
            await Task.Delay(1);
            await Load();

            isLoading = false;
            StateHasChanged();

        }

        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetBlogTable = await ClearConnection.GetBlogTableById(long.Parse(Blog_Id));
            getBlog = clearConnectionGetBlogTable;
        }
    }
}
