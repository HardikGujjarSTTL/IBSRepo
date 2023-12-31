﻿using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Numerics;
using Microsoft.AspNetCore.Http;
using IBS.Models.Reports;
using IBS.Interfaces.Reports;

namespace IBS.Repositories
{
    public class WriteOffEntryRepository : IWriteOffEntryRepository
    {
        private readonly ModelContext context;

        public WriteOffEntryRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<WriteOffEntryModel> GetWriteOfEntryList(DTParameters dtParameters)
        {
            DTResult<WriteOffEntryModel> dTResult = new() { draw = 0 };
            IQueryable<WriteOffEntryModel>? query = null;
            try
            {
                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                    if (orderCriteria == "")
                    {
                        orderCriteria = "Bill_No";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "Bill_No";
                    orderAscendingDirection = true;
                }

                string FrmDt = null, ToDt = null, BPOName = null;

                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FrmDt"]))
                {
                    FrmDt = Convert.ToString(dtParameters.AdditionalValues["FrmDt"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDt"]))
                {
                    ToDt = Convert.ToString(dtParameters.AdditionalValues["ToDt"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BPOName"]))
                {
                    BPOName = Convert.ToString(dtParameters.AdditionalValues["BPOName"]);
                }

                DataTable dt = new DataTable();

                DataSet ds;
               
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_frm_dt", OracleDbType.Varchar2, FrmDt, ParameterDirection.Input);
                par[1] = new OracleParameter("p_to_dt", OracleDbType.Varchar2, ToDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_bpo_CD", OracleDbType.Varchar2, BPOName, ParameterDirection.Input);
                par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                ds = DataAccessDB.GetDataSet("GetWriteOffEntryDetails", par, 1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    List<WriteOffEntryModel> list = dt.AsEnumerable().Select(row => new WriteOffEntryModel
                    {
                        Bill_No = Convert.ToString(row["BILL_NO"]),
                        BillDt = Convert.ToDateTime(row["BILL_DT"]),
                        BillAmt = Convert.ToDecimal(row["BILL_AMOUNT"]),
                        BillAmtRec = Convert.ToDecimal(row["AMOUNT_RECEIVED"]),
                        BillAmtClr = Convert.ToDecimal(row["BILL_AMT_CLEARED"]),
                        WRITE_OFF_AMT = (row["WRITE_OFF_AMT"] != DBNull.Value) ? Convert.ToDecimal(row["WRITE_OFF_AMT"]) : 0,
                }).ToList();

                    query = list.AsQueryable();

                    dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                    if (!string.IsNullOrEmpty(searchBy))
                        query = query.Where(w => Convert.ToString(w.Bill_No).ToLower().Contains(searchBy.ToLower())
                        || Convert.ToString(w.BillDt).ToLower().Contains(searchBy.ToLower())
                        );

                    dTResult.recordsFiltered = query.Count();
                    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                    dTResult.draw = dtParameters.Draw;

                }
                else
                {
                    return dTResult;
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return dTResult;
        }

        public int UpdateWriteAmtDetails(List<UpdateDataModel> dataArr, WriteOfMaster model)
        {
            int BillNO = 0;
            var billNos = dataArr.Select(d => d.Bill_No).ToList();

            var existingData = context.T22Bills.Where(x => billNos.Contains(x.BillNo)).ToList();

            foreach (var updateData in dataArr)
            {
                var existingRecord = existingData.FirstOrDefault(x => x.BillNo == updateData.Bill_No);
                if (existingRecord != null)
                {
                    existingRecord.WriteOffAmt = updateData.Write_Off_Amt;
                }
            }
            context.SaveChanges();

            int maxID = 0;
            if (model != null)
            {
                var WritemAster = (from r in context.WriteOffMasters where r.Id == Convert.ToInt32(model.ID) select r).FirstOrDefault();
                if (WritemAster == null)
                {
                    if (context.WriteOffMasters.Any())
                    {
                        maxID = context.WriteOffMasters.Max(x => x.Id) + 1;
                    }
                    else
                    {
                        maxID = 1;
                    }
                    WriteOffMaster obj = new WriteOffMaster();
                    obj.Id = maxID;
                    obj.Createdby = model.CreatedBy;
                    obj.Createddate = model.CreatedDT;
                    context.WriteOffMasters.Add(obj);
                    context.SaveChanges();
                }
            }

            WriteOfMasterDetails modeldetails = new WriteOfMasterDetails();
            var WritemAsterdetails = (from r in context.WriteOffDetails where r.Id == Convert.ToInt32(modeldetails.ID) select r).FirstOrDefault();
            int? WriteID = 0;

            if(WritemAsterdetails == null)
            {
                foreach (var updateData in dataArr)
                {
                    if (context.WriteOffDetails.Any())
                    {
                        WriteID = context.WriteOffDetails.Max(x => x.Id) + 1;
                    }
                    else
                    {
                        WriteID = 1;
                    }
                    WriteOffDetail obj = new WriteOffDetail();
                    obj.Id = WriteID;
                    obj.WriteOffMasterId = maxID;
                    obj.BillNo = updateData.Bill_No;
                    obj.WriteOffAmount = updateData.Write_Off_Amt;
                    context.WriteOffDetails.Add(obj);
                    context.SaveChanges();
                }
            }

            return BillNO;
        }
    }
}
