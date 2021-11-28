using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Clear.Risk.Models;
using Clear.Risk.Data;
using Clear.Risk.Models.ClearConnection;

namespace Clear.Risk.Controllers
{
    public partial class RolesController : Controller
    {
        private readonly ClearConnectionContext context;

        public RolesController(ClearConnectionContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Systemrole> GetRoles()
        {
            var items = this.context.Systemroles.AsQueryable<Systemrole>();
            this.OnRolesRead(ref items);

            return items;
        }

        partial void OnRolesRead(ref IQueryable<Systemrole> items);
    }
}
