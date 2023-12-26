using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class InterUnit_TransferRepository : IInterUnit_TransferRepository
    {
        private readonly ModelContext context;
        public InterUnit_TransferRepository(ModelContext context)
        {
            this.context = context;
        }

        #region 
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
                model.CHQ_AMOUNT = Convert.ToDecimal(ds.Tables[0].Rows[0]["AMOUNT"]);
                model.AMT_TRANSFERRED = Convert.ToDecimal(ds.Tables[0].Rows[0]["AMT_TRANSFERRED"]);
                model.SUSPENSE_AMT = Convert.ToDecimal(ds.Tables[0].Rows[0]["SUSPENSE_AMT"]);
            }
            else
            {
                model.ErrorMsg = "InValid Cheque No.,Cheque Date Or Bank";
                return model;
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
            if (query != null)
            {
                model.JV_NO = query.VCHR_NO;
                model.JV_DT = query.VCHR_DT;


                OracleParameter[] param = new OracleParameter[2];
                param[0] = new OracleParameter("P_VCHR_NO", OracleDbType.Varchar2, query.VCHR_NO, ParameterDirection.Input);
                param[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds1 = DataAccessDB.GetDataSet("SP_GET_JV_DETAILS", param, 1);
                DataTable dt = ds1.Tables[0];

                List<InterUnitTransferRegionModel> lst = dt.AsEnumerable().Select(row => new InterUnitTransferRegionModel
                {
                    ID = Convert.ToInt32(row["ID"]),
                    CHQ_NO = Convert.ToString(row["CHQ_NO"]),
                    CHQ_DT = Convert.ToString(row["CHQ_DT"]),
                    BANK_CD = Convert.ToString(row["BANK_CD"]),
                    ACC_CD = Convert.ToString(row["ACC_CD"]),
                    ACC_DESC = Convert.ToString(row["ACC_DESC"]),
                    AMOUNT = Convert.ToString(row["AMOUNT"]),
                    NARRATION = Convert.ToString(row["NARRATION"]),
                    IU_ADV_NO = Convert.ToString(row["IU_ADV_NO"]),
                    IU_ADV_DT = Convert.ToString(row["IU_ADV_DT"]),
                    lblIUAMT = Convert.ToString(row["AMOUNT"]),
                    ACTION = "M",
                }).ToList();

                //var Srno = 1;
                //foreach (var item in lst)
                //{
                //    item.ID = Srno;
                //    Srno = Srno + 1;
                //}
                model.lstUnitTransfer = lst;
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
                    orderCriteria = "ACC_CD";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ACC_CD";
                orderAscendingDirection = true;
            }

            query = UnitTransferModel.OrderBy(x => x.ACC_CD).AsQueryable();
            dTResult.recordsTotal = query.Count();

            //if (!string.IsNullOrEmpty(searchBy))
            //    query = query.Where(w => w.IeDepartment.ToLower().Contains(searchBy.ToLower())
            //    );

            dTResult.recordsFiltered = query.Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = query.ToList(); //DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public bool DetailsInsertUpdate(InterUnit_TransferModel model, UserSessionModel user)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var item = model.Transfer;
                    //foreach (var item in model.lstUnitTransfer)
                    //{
                    var query = (context.T25RvDetails
                                    .Where(t => t.ChqNo == model.CHQ_NO &&
                                                t.ChqDt == DateTime.ParseExact(model.CHQ_DT, "dd/MM/yyyy", null) &&
                                                t.BankCd == model.BANK_CD)
                                    .Select(t => new
                                    {
                                        camt = t.Amount,
                                        amtadj = t.AmtTransferred ?? 0, // Use null coalescing operator to handle null values
                                        susamt = t.SuspenseAmt
                                    })).FirstOrDefault();
                    if (model.JV_NO == "")
                    {
                        var ss = user.Region + model.TXTV_DT.Substring(8, 2) + model.TXTV_DT.Substring(3, 2);
                        var res = GenerateJVNO(ss);
                        model.JV_NO = ss + res;

                        T27Jv Clst = new T27Jv();
                        {
                            Clst.VchrNo = model.JV_NO;
                            Clst.VchrDt = DateTime.ParseExact(model.JV_DT, "dd/MM/yyyy", null);
                            Clst.RvVchrNo = model.VCHR_NO;
                            Clst.RvSno = Convert.ToByte(model.SNO);
                            Clst.BankCd = Convert.ToByte(model.BANK_CD);
                            Clst.ChqNo = model.CHQ_NO;
                            Clst.ChqDt = DateTime.ParseExact(model.CHQ_DT, "dd/MM/yyyy", null);
                            Clst.Createdby = user.UserID;
                            Clst.Createddate = DateTime.Now;
                        }
                        context.T27Jvs.Add(Clst);
                        context.SaveChanges();

                        DateTime chqDate = DateTime.ParseExact(model.CHQ_DT, "dd/MM/yyyy", null);
                        var _data = context.T25RvDetails.Where(r => r.ChqNo == model.CHQ_NO && r.ChqDt == chqDate && r.BankCd == model.BANK_CD).FirstOrDefault();
                        if (_data != null)
                        {
                            _data.AmtTransferred = query.amtadj + Convert.ToDecimal(item.AMOUNT);
                            _data.SuspenseAmt = query.susamt - Convert.ToDecimal(item.AMOUNT);
                            _data.Updatedby = Convert.ToString(user.UserID);
                            _data.Updateddate = DateTime.Now;
                            context.SaveChanges();
                        }

                        T29JvDetail t29Jv = new T29JvDetail();
                        t29Jv.VchrNo = model.JV_NO;
                        t29Jv.AccCd = Convert.ToInt32(item.ACC_CD);
                        t29Jv.Amount = Convert.ToDecimal(item.AMOUNT);
                        t29Jv.Narration = item.NARRATION;
                        t29Jv.IuAdvNo = item.IU_ADV_NO;
                        t29Jv.IuAdvDt = DateTime.ParseExact(item.IU_ADV_DT, "dd/MM/yyyy", null);
                        t29Jv.Createdby = user.UserID;
                        t29Jv.Createddate = DateTime.Now;
                        context.T29JvDetails.Add(t29Jv);
                        context.SaveChanges();
                    }
                    else
                    {
                        DateTime chqDate = DateTime.ParseExact(model.CHQ_DT, "dd/MM/yyyy", null);
                        if (string.IsNullOrEmpty(item.ACTION))
                        {
                            var _data = context.T25RvDetails.Where(r => r.ChqNo == model.CHQ_NO && r.ChqDt == chqDate && r.BankCd == model.BANK_CD).FirstOrDefault();
                            if (_data != null)
                            {
                                _data.AmtTransferred = query.amtadj + Convert.ToDecimal(item.AMOUNT);
                                _data.SuspenseAmt = query.susamt - Convert.ToDecimal(item.AMOUNT);
                                _data.Updatedby = Convert.ToString(user.UserID);
                                _data.Updateddate = DateTime.Now;
                                context.SaveChanges();
                            }

                            T29JvDetail t29Jv = new T29JvDetail();
                            t29Jv.VchrNo = model.JV_NO;
                            t29Jv.AccCd = Convert.ToInt32(item.ACC_CD);
                            t29Jv.Amount = Convert.ToDecimal(item.AMOUNT);
                            t29Jv.Narration = item.NARRATION;
                            t29Jv.IuAdvNo = item.IU_ADV_NO;
                            t29Jv.IuAdvDt = string.IsNullOrEmpty(item.IU_ADV_DT) ? null : DateTime.ParseExact(item.IU_ADV_DT, "dd/MM/yyyy", null);
                            t29Jv.Createdby = user.UserID;
                            t29Jv.Createddate = DateTime.Now;
                            context.T29JvDetails.Add(t29Jv);
                            context.SaveChanges();
                        }
                        else
                        {
                            var _data = context.T25RvDetails.Where(r => r.ChqNo == model.CHQ_NO && r.ChqDt == chqDate && r.BankCd == model.BANK_CD).FirstOrDefault();
                            if (_data != null)
                            {
                                _data.AmtTransferred = query.amtadj - Convert.ToDecimal(item.lblIUAMT) + Convert.ToDecimal(item.AMOUNT);
                                _data.SuspenseAmt = query.susamt + Convert.ToDecimal(item.lblIUAMT) - Convert.ToDecimal(item.AMOUNT);
                                _data.Updatedby = Convert.ToString(user.UserID);
                                _data.Updateddate = DateTime.Now;
                                context.SaveChanges();
                            }

                            //var jvDetail = (from m in context.T29JvDetails
                            //                where m.VchrNo == model.JV_NO && m.AccCd == Convert.ToInt32(item.ACC_CD)
                            //                select m).FirstOrDefault();
                            var jvDetail = context.T29JvDetails.Where(m => m.VchrNo == model.JV_NO && m.AccCd == Convert.ToInt32(item.ACC_CD) && m.Id == item.ID).FirstOrDefault();
                            if (jvDetail != null)
                            {
                                jvDetail.AccCd = Convert.ToInt32(item.ACC_CD);
                                jvDetail.Amount = Convert.ToDecimal(item.AMOUNT);
                                jvDetail.Narration = item.NARRATION;
                                jvDetail.IuAdvNo = item.IU_ADV_NO;
                                jvDetail.IuAdvDt = string.IsNullOrEmpty(item.IU_ADV_DT) ? null : DateTime.ParseExact(item.IU_ADV_DT, "dd/MM/yyyy", null);
                                jvDetail.Updatedby = user.UserID;
                                jvDetail.Updateddate = DateTime.Now;
                                context.SaveChanges();
                            }
                        }
                    }
                    //}                   
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return false;
                }
            }
            return true;
        }

        public bool DetailDelete(string BANK_CD, string CHQ_NO, string CHQ_DT, string JV_NO, string DelID, InterUnitTransferRegionModel model, UserSessionModel user)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var query = (context.T25RvDetails
                                 .Where(t => t.ChqNo == CHQ_NO &&
                                             t.ChqDt == DateTime.ParseExact(CHQ_DT, "dd/MM/yyyy", null) &&
                                             t.BankCd == Convert.ToInt32(BANK_CD))
                                 .Select(t => new
                                 {
                                     camt = t.Amount,
                                     amtadj = t.AmtTransferred ?? 0, // Use null coalescing operator to handle null values
                                     susamt = t.SuspenseAmt
                                 })).FirstOrDefault();

                    // Update the T25RvDetails record
                    DateTime chqDate = DateTime.ParseExact(model.CHQ_DT, "dd/MM/yyyy", null);
                    var _data = context.T25RvDetails.Where(r => r.ChqNo == model.CHQ_NO && r.ChqDt == chqDate && r.BankCd == Convert.ToInt32(BANK_CD)).FirstOrDefault();
                    if (_data != null)
                    {
                        _data.AmtTransferred = query.amtadj - Convert.ToDecimal(model.AMOUNT);
                        _data.SuspenseAmt = query.susamt + Convert.ToDecimal(model.AMOUNT);
                        _data.Updatedby = Convert.ToString(user.UserID);
                        _data.Updateddate = DateTime.Now;
                        context.SaveChanges();
                    }

                    // Delete Record from T29JvDetails
                    var JvDetail = (from m in context.T29JvDetails
                                    where m.VchrNo == JV_NO && m.AccCd == Convert.ToInt32(model.ACC_CD) && m.Id == model.ID
                                    select m).FirstOrDefault();
                    //context.T29JvDetails.Remove(JvDetail);
                    //context.SaveChanges();
                    if (JvDetail != null)
                    {
                        JvDetail.Isdeleted = 1;
                        JvDetail.Updatedby = user.UserID;
                        JvDetail.Updateddate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        public string GenerateJVNO(string ss)
        {
            var result = "";
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "Select lpad(nvl(max(to_number(nvl(substr(VCHR_NO,6,8),0))),0)+1,3,'0') from T27_JV where substr(VCHR_NO,1,5)='" + ss + "'";
                    result = Convert.ToString(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return result;
        }
        #endregion        
    }
}
