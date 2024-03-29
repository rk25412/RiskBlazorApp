﻿using System;
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
    public partial class EditTradeCategoryComponent : ComponentBase
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

        [Parameter]
        public dynamic TRADE_CATEGORY_ID { get; set; }

        Clear.Risk.Models.ClearConnection.TradeCategory _tradecategory;
        protected Clear.Risk.Models.ClearConnection.TradeCategory tradecategory
        {
            get
            {
                return _tradecategory;
            }
            set
            {
                if (!object.Equals(_tradecategory, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "tradecategory", NewValue = value, OldValue = _tradecategory };
                    _tradecategory = value;
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
                    var args = new PropertyChangedEventArgs(){ Name = "getTradeCategoriesResult", NewValue = value, OldValue = _getTradeCategoriesResult };
                    _getTradeCategoriesResult = value;
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
                await Load();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var clearConnectionGetTradeCategoryByTradeCategoryIdResult = await ClearConnection.GetTradeCategoryByTradeCategoryId(TRADE_CATEGORY_ID);
            tradecategory = clearConnectionGetTradeCategoryByTradeCategoryIdResult;

            var clearConnectionGetTradeCategoriesResult = await ClearConnection.GetTradeCategories();
            getTradeCategoriesResult = clearConnectionGetTradeCategoriesResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.TradeCategory args)
        {
            try
            {
                var clearConnectionUpdateTradeCategoryResult = await ClearConnection.UpdateTradeCategory(TRADE_CATEGORY_ID, tradecategory);
                DialogService.Close(tradecategory);
            }
            catch (System.Exception clearConnectionUpdateTradeCategoryException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update TradeCategory");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
