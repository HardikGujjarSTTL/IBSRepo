using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Dynamic;
using System.Reflection.Emit;
using System.Threading.Tasks;
using static IBS.Helper.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace IBS.Repositories
{
    public class BillingRRepository : IBillingRepository
    {
        private readonly ModelContext context;

        public BillingRRepository(ModelContext context)
        {
            this.context = context;
        }
       
    }
}
