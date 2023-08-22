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
                             VCHR_DT = Convert.ToDateTime(t24.VchrDt),
                             SNO = Convert.ToInt32(t25.Sno),
                             CHQ_NO = t25.ChqNo,
                             CHQ_DT = t25.ChqDt,
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
                                 VCHR_DT = Convert.ToDateTime(jv.VchrDt)
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
                              IU_ADV_DT = Convert.ToDateTime(t29.IuAdvDt)
                          };

                dTResult.recordsTotal = query.Count();
            dTResult.data = query;
            dTResult.recordsFiltered = query.Count();
            return dTResult;
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

       

    }
}
