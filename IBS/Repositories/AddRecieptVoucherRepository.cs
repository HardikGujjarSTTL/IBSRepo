using Microsoft.AspNetCore; 
using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Contracts;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using IBS.Helper;
namespace IBS.Repositories
{
    public class AddRecieptVoucherRepository : IAddRecieptVoucher
    {
        private readonly ModelContext context;
         public AddRecieptVoucherRepository(ModelContext context)
        {
            this.context = context;
        }
         
        public AddRecieptVoucherModel FindByID(string Vchr_No, string Case_No, string Chq_no)
        {

            AddRecieptVoucherModel model = new();
           

            T25RvDetail tenant = context.T25RvDetails.Find(Vchr_No,Case_No, Chq_no);
            T24Rv tenant2 = context.T24Rvs.Find(Vchr_No, Case_No, Chq_no);
            if (tenant == null)
            {

                //    throw new Exception("Voucher Record Not found");
            }
            else
            {
                model.VCHR_DT =Convert.ToString(tenant2.VchrDt);
                model.BANK_CD = Convert.ToString(tenant.BankCd);
                model.CHQ_NO = tenant.ChqNo;
                model.CHQ_DT = tenant.ChqDt;
                model.BANK_NAME = Convert.ToString(tenant.BankCd);
                model.AMOUNT = Convert.ToDouble(tenant.Amount);
                model.SAMPLE_NO = tenant.SampleNo;
                model.ACC_CD = Convert.ToString(tenant.AccCd);
                model.CASE_NO = tenant.CaseNo;
                model.NARRATION = tenant.Narration;
                model.BPO_CD = tenant.BpoCd;
                
            }
            return model;
        }

        public DTResult<AddRecieptVoucherModel> GetVoucherList(DTParameters dtParameters)
        {
            DTResult<AddRecieptVoucherModel> dTResult = new() { draw = 0 };
            IQueryable<AddRecieptVoucherModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "VCHR_NO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "VCHR_NO";
                orderAscendingDirection = true;
            }
            query = from l in context.ViewVoucherLists
                        //join i in context.T25RvDetails on l.VchrNo equals i.VchrNo
                        //join j in context.T95AccountCodes on i.AccCd equals j.AccCd
                        //join k in context.T12BillPayingOfficers on i.BpoCd equals k.BpoCd
                        //join m in context.T94Banks on i.BankCd equals m.BankCd
                        //join c in context.T03Cities on k.BpoCityCd equals c.CityCd
                        //where l.Isdeleted == 0  (nvl(T24.VCHR_TYPE, 'X') <> 'I')
                        
                    select new AddRecieptVoucherModel
                    {
                        VCHR_NO=l.VchrNo,
                        //SNO = Convert.ToInt32(l.Sno),
                        CHQ_NO = l.ChqNo,
                        CHQ_DT = Convert.ToDateTime(l.ChqDt),
                        AMOUNT = Convert.ToDouble(l.Amount),
                        BANK_CD = l.BankName,
                        BPO_CD = l.BpoName,
                        ACC_CD = Convert.ToString(l.AccDesc),
                        CASE_NO = l.CaseNo,
                        NARRATION = l.Narration,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.VCHR_NO).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CHQ_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        
        public int VoucherDetailsSave(AddRecieptVoucherModel model,string Region)
        {
            DTResult<AddRecieptVoucherModel> dTResult = new() { draw = 0 };
            IQueryable<AddRecieptVoucherModel>? query = null;


            string vchr_dt = model.VCHR_DT.ToString() ?? string.Empty;
            string ss = Region + vchr_dt.Substring(8, 2) + vchr_dt.Substring(3, 2);
            //string ss1 = ss.Substring(Convert.ToInt32(model.VCHR_NO), 5);


          //var  voucher = from l in context.Generatevouchers
          //          where l.VchrNo.Substring(1,5) == ss
          //          select new 
          //          {
          //           voucher=l.VchrNo  
          //          };

            string VCHR_NO = "";

            string voucher1 = (from l in context.T24Rvs
                            where l.VchrNo.Substring(0, 5) == ss
                            select l.VchrNo.Substring(5, 8)).Max();
            if(voucher1 != null)
            {
                VCHR_NO = ss + (Convert.ToInt32(voucher1) + 1);
            }
            else
            {
                VCHR_NO = ss + (Convert.ToInt32(0) + 1);
            }

            var GetValue = context.T24Rvs.Find(model.CASE_NO);



            if (GetValue == null)
            {
                T24Rv data = new T24Rv();
                data.VchrNo = Convert.ToString(VCHR_NO);
                data.VchrDt = Convert.ToDateTime(model.VCHR_DT);
                data.BankCd = Convert.ToByte(model.BANK_CD);
                data.VchrType = model.VCHR_TYPE;
                context.T24Rvs.Add(data);
                context.SaveChanges();
                VCHR_NO = Convert.ToString(data.VchrNo);

                T25RvDetail obj = new T25RvDetail();
                obj.VchrNo = Convert.ToString(VCHR_NO);
                obj.BankCd = Convert.ToByte(model.BANK_NAME);
                obj.Amount = Convert.ToDecimal(model.AMOUNT);
                obj.SampleNo = model.SAMPLE_NO;
                obj.AccCd = Convert.ToByte(model.ACC_CD);
                obj.CaseNo = model.CASE_NO;
                obj.Narration = model.NARRATION;
                obj.BpoCd = model.BPO_CD;
                obj.CaseNo = model.CASE_NO;
                obj.BpoCd = model.BPO_CD;
                obj.BpoType = model.BPO_TYPE;

                context.T25RvDetails.Add(obj);
                context.SaveChanges();



            }
            else
            {
                
            }
            return Convert.ToInt32(VCHR_NO);
        }


    }



}