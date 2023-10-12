using IBS.DataAccess;
using IBS.Interfaces;

namespace IBS.Repositories
{
    public class IC_RPT_IntermediateRepository: IIC_RPT_IntermediateRepository
    {
        private readonly ModelContext context;
        public IC_RPT_IntermediateRepository(ModelContext context)
        {
            this.context = context;
        }


        public string BindDropDown_IEStamps()
        {
            //var query1 = new[] { new { IE_STAMP_CD = 0, IE_STAMPS_DETAIL = "SELECT STAMP" } }.AsQueryable();
            //var query2 = context.IeStamps.Select(s => new { IE_STAMP_CD = s.IeStampCd, IE_STAMPS_DETAIL = s.IeStampsDetail });
            
            //var result = query1.Union(query2).OrderBy(item => item.IE_STAMP_CD);

            return "";
        }
    }
}
