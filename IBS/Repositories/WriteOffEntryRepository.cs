using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class WriteOffEntryRepository : IWriteOffEntryRepository
    {
        private readonly ModelContext context;

        public WriteOffEntryRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<WriteOffEntryModel> GetWriteOfEntryList(DTParameters dtParameters, string Region)
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

                OracleParameter[] par = new OracleParameter[8];
                par[0] = new OracleParameter("p_frm_dt", OracleDbType.Varchar2, FrmDt, ParameterDirection.Input);
                par[1] = new OracleParameter("p_to_dt", OracleDbType.Varchar2, ToDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_bpo_CD", OracleDbType.Varchar2, BPOName, ParameterDirection.Input);
                par[3] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[4] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
                par[5] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
                par[6] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                par[7] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetWriteOffEntryDetails", par, 2);

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
                        Outstanding_AMT= Convert.ToDecimal(row["Outstanding_AMT"])
                    }).ToList();

                    query = list.AsQueryable();

                    int recordsTotal = 0;
                    if (ds != null && ds.Tables[1].Rows.Count > 0)
                    {
                        recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["total_records"]);
                    }
                    dTResult.recordsTotal = recordsTotal;
                    dTResult.recordsFiltered = recordsTotal;
                    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
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

            if (WritemAsterdetails == null)
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
                    obj.Id = Convert.ToInt32(WriteID);
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
