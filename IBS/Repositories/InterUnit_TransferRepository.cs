using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Buffers.Text;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using System.Numerics;
using IBS.Helper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class InterUnit_TransferRepository : IInterUnit_TransferRepository
    {
        private readonly ModelContext context;
        public InterUnit_TransferRepository(ModelContext context)
        {
            this.context = context;
        }

        #region Naimish Code
        public InterUnit_TransferModel Get_Inter_Unit_Transfer(string Bank, string ChqNo, string ChqDate, string Region)
        {
            InterUnit_TransferModel model = new();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_BANK_CD", OracleDbType.Varchar2, Bank, ParameterDirection.Input);
            par[1] = new OracleParameter("P_CHQNO", OracleDbType.Varchar2, ChqNo, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CHQDATE", OracleDbType.Varchar2, ChqDate, ParameterDirection.Input);
            par[3] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_INTER_UNIT_TRANSFER", par, 1);

            var BankName = "";
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                BankName = context.T94Banks.Where(x => x.BankCd == Convert.ToInt32(ds.Tables[0].Rows[0]["BANK_CD"])).Select(x => x.BankName).FirstOrDefault();
                model.BANK_NAME = BankName;
                
                model.VCHR_NO = Convert.ToString(ds.Tables[0].Rows[0]["VCHR_NO"]);
                model.VCHR_DT = Convert.ToString(ds.Tables[0].Rows[0]["VCHR_DT"]);
                model.SNO = Convert.ToInt32(ds.Tables[0].Rows[0]["SNO"]);
                model.CHQ_NO = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_NO"]);
                model.CHQ_DT = Convert.ToString(ds.Tables[0].Rows[0]["CHQ_DT"]);
                model.BANK_CD = Convert.ToInt32(ds.Tables[0].Rows[0]["BANK_CD"]);
                model.BPO = Convert.ToString(ds.Tables[0].Rows[0]["BPO"]);
                model.AMOUNT = Convert.ToDecimal(ds.Tables[0].Rows[0]["AMOUNT"]);
                model.AMT_TRANSFERRED = Convert.ToDecimal(ds.Tables[0].Rows[0]["AMT_TRANSFERRED"]);
                model.SUSPENSE_AMT = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUSPENSE_AMT"]);
            }

            var query = (from jv in context.T27Jvs
                        where jv.ChqNo == ChqNo
                           && jv.ChqDt == DateTime.ParseExact(ChqDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                           && jv.BankCd == Convert.ToByte(Bank)
                        select new
                        {
                            VCHR_NO = jv.VchrNo,
                            VCHR_DT = Convert.ToDateTime(jv.VchrDt).ToString("dd/MM/yyyy")
                        }).FirstOrDefault();
            if(query != null)
            {
                model.JV_NO = query.VCHR_NO;
                model.JV_DT = query.VCHR_DT;
            }            
            return model;
        }



        public DTResult<InterUnitTransferRegionModel> GetInterUnitTransferRegion(DTParameters dtParameters, List<InterUnitTransferRegionModel> UnitTransferModel)
        {
            DTResult<InterUnitTransferRegionModel> dTResult = new() { draw = 0 };
            IQueryable<InterUnitTransferRegionModel>? query = null;
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ID";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ID";
                orderAscendingDirection = true;
            }

            query = (from u in UnitTransferModel.OrderBy(x => x.ID)
                     select new InterUnitTransferRegionModel
                     {
                         ID = u.ID,
                         ACC_CD = u.ACC_CD,
                         R_AMOUNT = u.R_AMOUNT,
                         NARRATION = u.NARRATION,
                         RNO = u.RNO,
                         RDT = u.RDT
                     }).AsQueryable();

            dTResult.recordsTotal = query.Count();

            //if (!string.IsNullOrEmpty(searchBy))
            //    query = query.Where(w => w.IeDepartment.ToLower().Contains(searchBy.ToLower())
            //    );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }
        #endregion        
    }
}
