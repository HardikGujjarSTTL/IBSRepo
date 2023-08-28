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

       public InterUnit_TransferModel GetTextboxValues(int BankNameDropdown,string CHQ_NO,string CHQ_DATE,string region)
       {
           

            InterUnit_TransferModel query1 = null;
            var result = from t24 in context.T24Rvs
                         join t25 in context.T25RvDetails on t24.VchrNo equals t25.VchrNo
                         join b in context.T12BillPayingOfficers on t25.BpoCd equals b.BpoCd into bGroup
                         from b in bGroup.DefaultIfEmpty()
                         join c in context.T03Cities on b.BpoCityCd equals c.CityCd into cGroup
                         from c in cGroup.DefaultIfEmpty()
                         where t25.ChqNo == CHQ_NO
                               && t25.BankCd == BankNameDropdown
                               && t25.ChqDt == DateTime.ParseExact(CHQ_DATE, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                               && t24.VchrNo.StartsWith(region)
                         select new InterUnit_TransferModel
                         {
                             VCHR_NO = t24.VchrNo,
                             VCHR_DT = Convert.ToString(t24.VchrDt),
                             SNO = Convert.ToInt32(t25.Sno),
                             CHQ_NO = t25.ChqNo,
                             CHQ_DT = CHQ_DATE,
                             BANK_CD = t25.BankCd,
                             BPO = (t25.BpoCd != null
                                    ? $"{b.BpoCd}-{b.BpoName}/{(b.BpoAdd != null ? b.BpoAdd + "/" : "")}{(c.Location != null ? c.City + "/" + c.Location : c.City)}/{b.BpoRly}"
                                    : t25.Narration),
                             AMOUNT = (t25.Amount ?? 0),
                             AMT_TRANSFERRED = (t25.AmtTransferred ?? 0),
                             SUSPENSE_AMT = (t25.SuspenseAmt ?? 0)
                         };


            var resultList = result.FirstOrDefault();


            
            return resultList;

       }

        public InterUnit_TransferModel GetJVvalues(int BankNameDropdown, string CHQ_NO, string CHQ_DATE)
        {
            var jvDetails = (from jv in context.T27Jvs
                             where jv.ChqNo == CHQ_NO &&
                                   jv.ChqDt == DateTime.ParseExact(CHQ_DATE, "dd/MM/yyyy", CultureInfo.InvariantCulture) &&
                                   jv.BankCd == Convert.ToByte(BankNameDropdown)
                             select new InterUnit_TransferModel
                             {
                                 VCHR_NO = jv.VchrNo,
                                 VCHR_DT = Convert.ToString(jv.VchrDt)
                             }).FirstOrDefault();
            return jvDetails;
        }

        public DTResult<InterUnit_TransferModel> BillList(DTParameters dtParameters)
        {
            InterUnit_TransferModel model = new();
            DTResult<InterUnit_TransferModel> dTResult = new() { draw = 0 };
            IQueryable<InterUnit_TransferModel>? query = null;

            string JVNO = dtParameters.AdditionalValues?.GetValueOrDefault("JV_NO");

            query = from t27 in context.T27Jvs
                          join t29 in context.T29JvDetails on t27.VchrNo equals t29.VchrNo
                          where t27.VchrNo == JVNO
                    select new InterUnit_TransferModel
                          {
                              //CHQ_NO = t27.ChqNo,
                              //CHQ_DT = Convert.ToDateTime(t27.ChqDt),
                              //BANK_CD = Convert.ToInt32(t27.BankCd),
                              //VCHR_NO = t29.VchrNo,
                             // ACC_CD = Convert.ToString(t29.AccCd),
                              ACC_DESC = (t29.AccCd == 3007 ? "Northern"
                                            : t29.AccCd == 3008 ? "Eastern"
                                            : t29.AccCd == 3009 ? "Southern"
                                            : t29.AccCd == 3006 ? "Western"
                                            : t29.AccCd == 3066 ? "Central"
                                            : t29.AccCd == 9999 ? "Bill Adjustment of Old System"
                                            : t29.AccCd == 9998 ? "Miscellaneous Adjustments"
                                            : ""),
                              AMOUNT = Convert.ToDecimal(t29.Amount),
                              NARRATION = t29.Narration,
                              IU_ADV_NO = t29.IuAdvNo,
                              IU_ADV_DT = Convert.ToString(t29.IuAdvDt)
                          };

                dTResult.recordsTotal = query.Count();
            dTResult.data = query;
            dTResult.recordsFiltered = query.Count();
            return dTResult;
        }

        public bool modify(InterUnit_TransferModel InterUnit_TransferModel, string Region){

            var CHQ_NO = Convert.ToString(InterUnit_TransferModel.CHQ_NO);
            var CHQ_DT = InterUnit_TransferModel.CHQ_DT;
            var BANK_CD = Convert.ToString(InterUnit_TransferModel.BANK_CD);
            var SNO = Convert.ToString(InterUnit_TransferModel.SNO);
            var VCHR_DT = InterUnit_TransferModel.VCHR_DT;


            if (InterUnit_TransferModel.Action == "M")
            {
                updt_RV(InterUnit_TransferModel);
                UpdateJVDetails(InterUnit_TransferModel);
            }
            else if (InterUnit_TransferModel.Action == "")
            {
                InterUnit_TransferModel model = SelectInterUnit(CHQ_NO, Convert.ToString(CHQ_DT), BANK_CD);
                UpdateInterUnit(InterUnit_TransferModel);
                InsertJV_Details(InterUnit_TransferModel);
            }


            return true;
        }

        public bool Save(InterUnit_TransferModel InterUnit_TransferModel, string Region)
        {
            InsertInterUnit(InterUnit_TransferModel, Region);

            return true;
        }
        public string GetNewJVNumber(string region , string VCHR_DT)
        {
            string ss = "";
            ss = region + VCHR_DT.Substring(8, 2) + VCHR_DT.Substring(3, 2);
            var maxNumber = context.T27Jvs
                .Where(jv => jv.VchrNo.Substring(0, 5) == ss)
                .Select(jv => jv.VchrNo.Substring(5, 8))
                .ToList() // Execute query and retrieve the results
                .Select(numStr => int.TryParse(numStr, out int num) ? num : 0)
                .DefaultIfEmpty(0)
                .Max();

            var newNumber = (maxNumber + 1).ToString().PadLeft(3, '0');
            return newNumber;
        }

        public InterUnit_TransferModel SelectInterUnit( string CHQ_NO , string CHQ_DT , string BANK_CD)
        {

            //string Generate = GetNewJVNumber( region,  VCHR_DT);


            InterUnit_TransferModel model = new();

            try
            {
               
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_CHQ_NO", OracleDbType.Varchar2, CHQ_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CHQ_DT", OracleDbType.Date, CHQ_DT, ParameterDirection.Input);
                par[2] = new OracleParameter("p_BANK_CD", OracleDbType.Int32, BANK_CD, ParameterDirection.Input);
                par[3] = new OracleParameter("p_RESULT", OracleDbType.RefCursor, ParameterDirection.Input);



                var ds = DataAccessDB.GetDataSet("Select_InsterUnit", par, 4);
               
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new InterUnit_TransferModel
                    {
                       
                        AMOUNT = Convert.ToDecimal(row["AMOUNT"]),
                        AMT_TRANSFERRED = Convert.ToDecimal(row["AMT_TRANSFERRED"]),
                        SUSPENSE_AMT = Convert.ToDecimal(row["SUSPENSE_AMT"]),
                    };
                }
              


                
            }
            catch (Exception ex)
            {
                return model;
            }
            return model;
        }

        public bool InsertInterUnit(InterUnit_TransferModel InterUnit_TransferModel , string Region)
        {
            var CHQ_NO = Convert.ToString(InterUnit_TransferModel.CHQ_NO);
            var CHQ_DT = InterUnit_TransferModel.CHQ_DT;
            var BANK_CD = Convert.ToString(InterUnit_TransferModel.BANK_CD);
            var SNO = Convert.ToString(InterUnit_TransferModel.SNO);
            var VCHR_DT = InterUnit_TransferModel.VCHR_DT;
            var Generate = GetNewJVNumber(Region, Convert.ToString(VCHR_DT));
            InterUnit_TransferModel model = SelectInterUnit(CHQ_NO, Convert.ToString(CHQ_DT), BANK_CD);


            try
            {

                OracleParameter[] par = new OracleParameter[7];
                par[0] = new OracleParameter("p_JVNO", OracleDbType.Varchar2, InterUnit_TransferModel.JV_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_JVDT", OracleDbType.Date, InterUnit_TransferModel.VCHR_DT, ParameterDirection.Input);
                par[2] = new OracleParameter("p_VNo", OracleDbType.Varchar2, InterUnit_TransferModel.VCHR_NO, ParameterDirection.Input);
                par[3] = new OracleParameter("p_SNO", OracleDbType.Int32, InterUnit_TransferModel.SNO, ParameterDirection.Input);
                par[4] = new OracleParameter("p_BANK_CD", OracleDbType.Int32, InterUnit_TransferModel.BANK_CD, ParameterDirection.Input);
                par[5] = new OracleParameter("p_CNO", OracleDbType.Varchar2, InterUnit_TransferModel.CHQ_NO, ParameterDirection.Input);
                par[6] = new OracleParameter("p_CustomDate", OracleDbType.Date, InterUnit_TransferModel.CHQ_DT, ParameterDirection.Input);




                var ds = DataAccessDB.ExecuteNonQuery("InsertJVNO", par, 1);
                //string Sno = GetSNo(REG_NO);
                //InsertLabRegDetails(REG_NO, Sno, LABREGISTERModel);
                //UpdateLabReg(REG_NO, LABREGISTERModel);
                UpdateInterUnit(InterUnit_TransferModel);
                InsertJV_Details(InterUnit_TransferModel);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public bool UpdateInterUnit(InterUnit_TransferModel InterUnit_TransferModel)
        {






            try
            {

                OracleParameter[] par = new OracleParameter[6];
                par[0] = new OracleParameter("p_CHQ_NO", OracleDbType.Varchar2, InterUnit_TransferModel.CHQ_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CHQ_DT", OracleDbType.Date, InterUnit_TransferModel.CHQ_DT, ParameterDirection.Input);
                par[2] = new OracleParameter("p_BANK_CD", OracleDbType.Int32, InterUnit_TransferModel.BANK_CD, ParameterDirection.Input);
                par[3] = new OracleParameter("p_AmtAdj", OracleDbType.Int32, InterUnit_TransferModel.AMT_TRANSFERRED, ParameterDirection.Input);
                par[4] = new OracleParameter("p_TxtAmt", OracleDbType.Int32, InterUnit_TransferModel.AMOUNT, ParameterDirection.Input);
                par[5] = new OracleParameter("p_SusAmt", OracleDbType.Int32, InterUnit_TransferModel.SUSPENSE_AMT, ParameterDirection.Input);




               
                var ds = DataAccessDB.ExecuteNonQuery("Update_Amounts", par, 1);
                //string Sno = GetSNo(REG_NO);
                //InsertLabRegDetails(REG_NO, Sno, LABREGISTERModel);
                //UpdateLabReg(REG_NO, LABREGISTERModel);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public bool InsertJV_Details(InterUnit_TransferModel InterUnit_TransferModel)
        {






            try
            {

                OracleParameter[] par = new OracleParameter[6];
                par[0] = new OracleParameter("p_VCHR_NO", OracleDbType.Varchar2, InterUnit_TransferModel.JV_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_ACC_CD", OracleDbType.Varchar2, InterUnit_TransferModel.ACC_CD, ParameterDirection.Input);
                par[2] = new OracleParameter("p_AMOUNT", OracleDbType.Int32, InterUnit_TransferModel.AMOUNT, ParameterDirection.Input);
                par[3] = new OracleParameter("p_NARRATION", OracleDbType.Varchar2, InterUnit_TransferModel.NARRATION, ParameterDirection.Input);
                par[4] = new OracleParameter("p_IU_ADV_NO", OracleDbType.Varchar2, InterUnit_TransferModel.IU_ADV_NO, ParameterDirection.Input);
                par[5] = new OracleParameter("p_IU_ADV_DT", OracleDbType.Date, InterUnit_TransferModel.IU_ADV_DT, ParameterDirection.Input);





                var ds = DataAccessDB.ExecuteNonQuery("Insert_JV_Details", par, 1);
                //string Sno = GetSNo(REG_NO);
                //InsertLabRegDetails(REG_NO, Sno, LABREGISTERModel);
                //UpdateLabReg(REG_NO, LABREGISTERModel);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public InterUnit_TransferModel Del_Select(InterUnit_TransferModel InterUnit_TransferModel)
        {


            InterUnit_TransferModel model = new();



            try
            {

                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_CHQ_NO", OracleDbType.Varchar2, InterUnit_TransferModel.CHQ_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_CHQ_DT", OracleDbType.Varchar2, InterUnit_TransferModel.CHQ_DT, ParameterDirection.Input);
                par[2] = new OracleParameter("p_BANK_CD", OracleDbType.Int32, InterUnit_TransferModel.BANK_CD, ParameterDirection.Input);
                par[3] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Input);





                var ds = DataAccessDB.GetDataSet("Delete_Select", par, 3);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new InterUnit_TransferModel
                    {

                        AMOUNT = Convert.ToDecimal(row["AMOUNT"]),
                        AMT_TRANSFERRED = Convert.ToDecimal(row["AMT_TRANSFERRED"]),
                        SUSPENSE_AMT = Convert.ToDecimal(row["SUSPENSE_AMT"]),
                    };
                }

                Update_RV(InterUnit_TransferModel);
                Delete_JVDetails(InterUnit_TransferModel);

            }
            catch (Exception ex)
            {
                return model;
            }
            return model;
        }


        public InterUnit_TransferModel  Update_RV(InterUnit_TransferModel InterUnit_TransferModel)
        {

            DateTime parsedDate;
          
            DateTime.TryParseExact(InterUnit_TransferModel.CHQ_DT, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
         
            var recordToUpdate = context.T25RvDetails
            .Where(record => record.ChqNo == InterUnit_TransferModel.CHQ_NO &&
                             record.ChqDt == parsedDate &&
                             record.BankCd == InterUnit_TransferModel.BANK_CD)
            .SingleOrDefault();

            if (recordToUpdate != null)
            {
                InterUnit_TransferModel.SNO = InterUnit_TransferModel.SNO + 1;
                recordToUpdate.AmtTransferred = InterUnit_TransferModel.AMT_TRANSFERRED - InterUnit_TransferModel.AMOUNT;
                recordToUpdate.SuspenseAmt = InterUnit_TransferModel.SUSPENSE_AMT + InterUnit_TransferModel.AMOUNT;

                context.SaveChanges();
            }

            return null;
        }

        public bool Delete_JVDetails(InterUnit_TransferModel InterUnit_TransferModel)
        {



            try
            {

                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_VCHR_NO", OracleDbType.Varchar2, InterUnit_TransferModel.JV_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_ACC_CD", OracleDbType.Int32, InterUnit_TransferModel.ACC_CD, ParameterDirection.Input);
              





                var ds = DataAccessDB.ExecuteNonQuery("DELETE_JV_DETAILS", par, 1);
                //string Sno = GetSNo(REG_NO);
                //InsertLabRegDetails(REG_NO, Sno, LABREGISTERModel);
                //UpdateLabReg(REG_NO, LABREGISTERModel);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool updt_RV(InterUnit_TransferModel InterUnit_TransferModel)
        {

            DateTime parsedDate;

            DateTime.TryParseExact(InterUnit_TransferModel.CHQ_DT, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
            var chqNo = InterUnit_TransferModel.CHQ_NO;
            var chqDate = parsedDate;
            var bankCd = Convert.ToInt32(InterUnit_TransferModel.BANK_CD);
            var amtadj = InterUnit_TransferModel.AMT_TRANSFERRED;
            var susamt = InterUnit_TransferModel.SUSPENSE_AMT;
            var amtToSubtract = Convert.ToDouble(InterUnit_TransferModel.AMOUNT);
            var amtToAdd = Convert.ToDouble(InterUnit_TransferModel.AMOUNT);

            var recordToUpdate = context.T25RvDetails
                .SingleOrDefault(record =>
                    record.ChqNo == chqNo &&
                    record.ChqDt == chqDate &&
                    record.BankCd == bankCd);

            if (recordToUpdate != null)
            {
                recordToUpdate.AmtTransferred = amtadj-Convert.ToDecimal(amtToSubtract)+Convert.ToDecimal(amtToAdd);
                recordToUpdate.SuspenseAmt = susamt + Convert.ToDecimal(amtToSubtract)  - Convert.ToDecimal(amtToAdd);
                context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool UpdateJVDetails(InterUnit_TransferModel InterUnit_TransferModel)
        {
            try
            {

                OracleParameter[] par = new OracleParameter[6];
                par[0] = new OracleParameter("p_lstACD", OracleDbType.Varchar2, InterUnit_TransferModel.ACC_CD, ParameterDirection.Input);
                par[1] = new OracleParameter("p_txtAmt", OracleDbType.Int32, InterUnit_TransferModel.AMOUNT, ParameterDirection.Input);
                par[2] = new OracleParameter("p_txtNarrat", OracleDbType.Varchar2, InterUnit_TransferModel.NARRATION, ParameterDirection.Input);
                par[3] = new OracleParameter("p_txtRNo", OracleDbType.Varchar2, InterUnit_TransferModel.IU_ADV_NO, ParameterDirection.Input);
                par[4] = new OracleParameter("p_txtRDT", OracleDbType.Varchar2, InterUnit_TransferModel.IU_ADV_DT, ParameterDirection.Input);
                par[5] = new OracleParameter("p_lblJVNO", OracleDbType.Varchar2, InterUnit_TransferModel.JV_NO, ParameterDirection.Input);





                var ds = DataAccessDB.ExecuteNonQuery("UpdateJVDetails", par, 1);
     

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
    }
}
