using IBS.DataAccess;
using IBS.Interfaces.IE;
using IBS.Models;

namespace IBS.Repositories.IE
{
    public class ICPhotoEnclosedRepository : IICPhotoEnclosedRepository
    {
        private readonly ModelContext context;

        public ICPhotoEnclosedRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<ICPhotoEnclosedModel> GetDataList(DTParameters dtParameters, string GetRegionCode, int GetIeCd)
        {
            DTResult<ICPhotoEnclosedModel> dTResult = new() { draw = 0 };
            IQueryable<ICPhotoEnclosedModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", CallRecvDt = "", CallSno = "", BkNo = "", SetNo = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]))
            {
                CallRecvDt = Convert.ToString(dtParameters.AdditionalValues["CallRecvDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSno"]))
            {
                CallSno = Convert.ToString(dtParameters.AdditionalValues["CallSno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BkNo"]))
            {
                BkNo = Convert.ToString(dtParameters.AdditionalValues["BkNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SetNo"]))
            {
                SetNo = Convert.ToString(dtParameters.AdditionalValues["SetNo"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            //DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);
            CallSno = CallSno.ToString() == "" ? string.Empty : CallSno.ToString();
            BkNo = BkNo.ToString() == "" ? string.Empty : BkNo.ToString();
            SetNo = SetNo.ToString() == "" ? string.Empty : SetNo.ToString();

            query = from p in context.ViewGetIcphotoencloseds
                    where p.RegionCode == GetRegionCode 
                    && (string.IsNullOrEmpty(CaseNo) || p.CaseNo == CaseNo)
                    && (string.IsNullOrEmpty(CallRecvDt) || p.CallRecvDt == DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null))
                    && (string.IsNullOrEmpty(CallSno) || p.CallSno == int.Parse(CallSno))
                    && (string.IsNullOrEmpty(BkNo) || p.BkNo == BkNo)
                    && (string.IsNullOrEmpty(SetNo) || p.SetNo == SetNo)
                    && (p.IeCd == (int)GetIeCd)

                    select new ICPhotoEnclosedModel
                    {
                        CaseNo = p.CaseNo,
                        CallRecvDt = p.CallRecvDt,
                        CallSno = p.CallSno,
                        BkNo = p.BkNo,
                        SetNo = p.SetNo,
                        IeName = p.IeName,
                        IeCd = p.IeCd,
                        RegionCode = p.RegionCode,
                        File1 = p.File1,
                        File2 = p.File2,
                        File3 = p.File3,
                        File4 = p.File4,
                        File5 = p.File5,
                        File6 = p.File6,
                        File7 = p.File7,
                        File8 = p.File8,
                        File9 = p.File9,
                        File10 = p.File10,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
