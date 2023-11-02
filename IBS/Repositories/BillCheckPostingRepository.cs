using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class BillCheckPostingRepository : IBillCheckPostingRepository
    {
        private readonly ModelContext context;

        public BillCheckPostingRepository(ModelContext context)
        {
            this.context = context;
        }

        public BillCheckPostingModel FindByID(string BankName, string ChqNo, DateTime ChqDt, string Region)
        {
            BillCheckPostingModel model = new();
            //var CallData = context.ViewGetCallRegCancellations.Where(x => x.CaseNo == CaseNo && x.CallRecvDt == CallRecvDt && x.CallSno == Convert.ToInt32(CallSno)).FirstOrDefault();

            //if (CallData == null)
            //    throw new Exception("Record Not found");
            //else
            //{
            //    model.CaseNo = CallData.CaseNo;
            //    model.PoNo = CallData.PoNo;
            //    model.PoDt = CallData.PoDt;
            //    model.CallSno = Convert.ToInt16(CallData.CallSno);
            //    model.CallRecvDt = CallData.CallRecvDt;
            //    model.Vendor = CallData.Vendor;
            //    model.CallLetterNo = CallData.CallLetterNo;
            //}
            return model;
        }
    }
}
