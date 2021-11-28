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
using Clear.Risk.Data;

namespace Clear.Risk.Pages
{
    public partial class Blog : ComponentBase
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
        protected NavigationManager UriHelper { get; set; }
        [Inject]
        protected ClearConnectionService ClearConnection { get; set; }
        protected RadzenDataList<BlogTable> datalist0;


        IEnumerable<BlogTable> _getBlog;
        protected IEnumerable<BlogTable> getBlog
        {
            get
            {
                return _getBlog;
            }
            set
            {
                if (!object.Equals(_getBlog, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getBlog", NewValue = value, OldValue = _getBlog };
                    _getBlog = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

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
            var clearConnectionGetBlogTable = await ClearConnection.GetBlogTable();
            getBlog = clearConnectionGetBlogTable;
        }

        protected async System.Threading.Tasks.Task JumpToSingle(long id)
        {
            UriHelper.NavigateTo("single-blog" + "/" + id);

        }
    }
}
