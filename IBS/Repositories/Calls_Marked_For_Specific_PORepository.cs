using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
//using NPOI.SS.Formula.Functions;
using System.Drawing;
using System.Reflection.PortableExecutable;

namespace IBS.Repositories
{
    public class Calls_Marked_For_Specific_PORepository : ICalls_Marked_For_Specific_PORepository
    {
        private readonly ModelContext context;  
        public Calls_Marked_For_Specific_PORepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<Calls_Marked_For_Specific_POModel> gridData(DTParameters dtParameters)
        {
            DTResult<Calls_Marked_For_Specific_POModel> dTResult = new() { draw = 0 };
            IQueryable<Calls_Marked_For_Specific_POModel>? query = null;

            string railwaytypes = dtParameters.AdditionalValues?.GetValueOrDefault("railwaytypes");
            string railwaytypes1 = dtParameters.AdditionalValues?.GetValueOrDefault("railwaytypes1");
            DateTime podt = Convert.ToDateTime(dtParameters.AdditionalValues?.GetValueOrDefault("podt"));



             query = (from t13 in context.T13PoMasters
                         join t17 in context.T17CallRegisters on t13.CaseNo equals t17.CaseNo
                         where t13.RlyNonrly == railwaytypes &&
                               t13.RlyCd == railwaytypes1 &&
                               t13.PoDt == podt
                         group new { t13, t17 } by new
                         {
                             t13.L5noPo,
                             t13.PoNo,
                             t13.PoDt,
                             t13.RlyNonrly,
                             t13.RlyCd
                         } into grouped
                         orderby grouped.Key.PoDt
                         select new Calls_Marked_For_Specific_POModel
                         {
                             L5NO_PO = grouped.Key.L5noPo,
                             PO_NO = grouped.Key.PoNo,
                             PO_DT = Convert.ToString(grouped.Key.PoDt),
                             RLY_NONRLY = grouped.Key.RlyNonrly,
                             RLY_CD = grouped.Key.RlyCd
                         });

            query = query.Distinct();

           
            var result = query.ToList();
            dTResult.data = result;
          
            return dTResult;
        }

        public List<railway_dropdown> GetValue(string selectedValue)
        {
            Calls_Marked_For_Specific_POModel result = null;
            var query = from railway in context.T91Railways
                        where railway.RlyCd != "CORE"
                        orderby railway.RlyCd
                        select new railway_dropdown
                        {
                            RLY_CD = railway.RlyCd, 
                            RAILWAY_ORGN = railway.Railway
                        };

           
            return query.ToList();
        }

        public List<railway_dropdown> GetValue2(string selectedValue)
        {
            Calls_Marked_For_Specific_POModel result = null;
            var query = context.T12BillPayingOfficers
             .Where(bpo => bpo.BpoType == selectedValue)
             .Select(bpo => new railway_dropdown
             {
                 //BPO_RLY = bpo.BPO_RLY,
                 RLY_CD = bpo.BpoRly,
                 RAILWAY_ORGN = bpo.BpoOrgn
             })
             .OrderBy(item => item.RLY_CD)
             .ToList();



            return query.ToList();
        }

        public Calls_Marked_For_Specific_POModel edit(string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD)
        {
            //var query = from t13 in context.T13PoMasters
            //            join t06 in context.T06Consignees on t13.PurchaserCd equals t06.ConsigneeCd
            //            join t20 in context.T20Ics on t13.CaseNo equals t20.CaseNo
            //            join t18 in context.T18CallDetails on new { t20.CaseNo, t20.CallRecvDt, t20.CallSno } equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
            //            join t09 in context.T09Ies on t20.IeCd equals t09.IeCd
            //            join t05 in context.T05Vendors on t13.VendCd equals t05.VendCd
            //            join t17 in context.T17CallRegisters on new { t18.CaseNo, t18.CallRecvDt, t18.CallSno } equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
            //          //  join t49 in context.T49IcPhotoEncloseds on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } equals new { t49.CaseNo, t49.CallRecvDt, t49.CallSno } into t49Group
            //           // from t49 in t49Group.DefaultIfEmpty()
            //            where t13.L5noPo.Trim().ToUpper() == PO_NO.Trim().ToUpper() &&
            //                  t13.PoDt == PDT &&
            //                  t13.RlyNonrly == CLT &&
            //                  t13.RlyCd == RLYCD
            //            orderby t20.IcDt, t18.ItemSrnoPo
            //            select new Calls_Marked_For_Specific_POModel
            //            {
            //                PURCHASER = t06.ConsigneeFirm + "/" + t06.ConsigneeDesig + "/" + t06.ConsigneeDept,
            //               PO_NO =  t13.PO_NO,
            //                PO_DT = t13.PoDt.ToString("dd/MM/yyyy"),
            //              IC_NO =  t20.IC_NO,
            //                IC_DATE = t20.IcDt.ToString("dd/MM/yyyy"),
            //               BkNo =  t20.BkNo,
            //               SetNo =  t20.SetNo,
            //               BillNo =  t20.BillNo,
            //               IeName =  t09.IeName,
            //                VENDOR = t05.VendName,
            //               ItemDescPo =  t18.ItemDescPo,
            //               QtyToInsp =  t18.QtyToInsp,
            //                QtyPassed =  t18.QtyPassed,
            //              QtyRejected =  t18.QtyRejected,
            //                Hologram = t17.Hologram,
            //             // ICPhoto =  t49.ICPhoto,
            //             // ICPhotoA1 = t49.ICPhotoA1,
            //             //  ICPhotoA2 = t49.ICPhotoA2
            //            };

            //// Execute the query and retrieve the results
            //var result = query.ToList();


            return null;
        }
    }
}
