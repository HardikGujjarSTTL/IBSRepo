using IBS.DataAccess;
using IBS.Models;

namespace IBS.Repositories
{
    public class Print_Bank_Statement_VoucherRepository : IPrint_Bank_Statement_VoucherRepository
    {
        private readonly ModelContext context;
        public Print_Bank_Statement_VoucherRepository(ModelContext context)
        {
            this.context = context;
        }

        public Print_Bank_Statement_VoucherModel ReportData(string VCHR_NO, string Region)
        {
            Print_Bank_Statement_VoucherModel model = new();
            var result = (from r in context.T25RvDetails
                          join a in context.T95AccountCodes on r.AccCd equals a.AccCd into aGroup
                          from a in aGroup.DefaultIfEmpty()
                          join b in context.T12BillPayingOfficers on r.BpoCd equals b.BpoCd into bGroup
                          from b in bGroup.DefaultIfEmpty()
                          join d in context.T94Banks on r.BankCd equals d.BankCd
                          //where r.VchrNo.Substring(0, 1) == Region && r.VchrNo == VCHR_NO
                          orderby r.Sno
                          select new Print_Bank_Statement_VoucherModel
                          {
                              CHQ_NO = r.ChqNo ?? "",
                              //  CHQ_DT = Convert.ToString(r.ChqDt)?? "",
                              AMOUNT = Convert.ToString(r.Amount) ?? "",
                              BANK_NAME = d.BankName ?? ""
                          }).Take(1);

            model = result.FirstOrDefault();
            var result1 = result.ToList();
            model.Print_Bank_Statement = result1;
            return model;
        }
    }
}
