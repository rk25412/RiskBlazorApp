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
    public partial class AddSmtpComponent : ComponentBase
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

        Clear.Risk.Models.ClearConnection.Smtpsetup _smtpsetup;
        protected Clear.Risk.Models.ClearConnection.Smtpsetup smtpsetup
        {
            get
            {
                return _smtpsetup;
            }
            set
            {
                if (!object.Equals(_smtpsetup, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "smtpsetup", NewValue = value, OldValue = _smtpsetup };
                    _smtpsetup = value;
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
            smtpsetup = new Clear.Risk.Models.ClearConnection.Smtpsetup(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Clear.Risk.Models.ClearConnection.Smtpsetup args)
        {
            try
            {
                var clearConnectionCreateSmtpsetupResult = await ClearConnection.CreateSmtpsetup(smtpsetup);
                DialogService.Close(smtpsetup);
            }
            catch (System.Exception clearConnectionCreateSmtpsetupException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Smtpsetup!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
