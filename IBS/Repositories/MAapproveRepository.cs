using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class MAapproveRepository : IMAapproveRepository
    {
        private readonly ModelContext context;

        public MAapproveRepository(ModelContext context)
        {
            this.context = context;
        }

        public MAapproveModel FindByID(string CaseNo, string MaNo, string MaDtc, byte MaSno)
        {
            MAapproveModel model = new();
            string first = MaDtc.Substring(0, 2);
            string second = MaDtc.Substring(2, 2);
            string third = MaDtc.Substring(4, 4);
            string conc = first + "-" + second + "-" + third;
            string SetMaDt = conc.ToString();

            var GetValuePO = (from v in context.VendPoMaMasters
                              join d in context.VendPoMaDetails on v.CaseNo equals d.CaseNo
                              where v.CaseNo == d.CaseNo && v.MaNo == v.MaNo
                              && v.MaDt == Convert.ToDateTime(SetMaDt)
                              && v.CaseNo == CaseNo && v.MaNo == MaNo
                              && d.MaStatus == "P" && d.MaSno == MaSno
                              select new
                              {
                                  v,
                                  d
                              }
                  ).ToList();

            if (GetValuePO == null)
                throw new Exception("Record Not found");
            else
            {
                model.CaseNo = GetValuePO[0].v.CaseNo;
                model.MaNo = GetValuePO[0].v.MaNo;
                model.MaDt = GetValuePO[0].v.MaDt;
                model.MaSno = GetValuePO[0].d.MaSno;
                model.PoNo = GetValuePO[0].v.PoNo;
                model.PoDt = GetValuePO[0].v.PoDt;
                model.RlyCd = GetValuePO[0].v.RlyNonrly.Equals("R") ? "Railway(" + GetValuePO[0].v.RlyCd + ")" : GetValuePO[0].v.RlyNonrly.Equals("P") ? "Private(" + GetValuePO[0].v.RlyCd + ")" : GetValuePO[0].v.RlyNonrly.Equals("S") ? "State Government(" + GetValuePO[0].v.RlyCd + ")" : GetValuePO[0].v.RlyNonrly.Equals("F") ? "Foreign Railways(" + GetValuePO[0].v.RlyCd + ")" : GetValuePO[0].v.RlyNonrly.Equals("U") ? "PSU(" + GetValuePO[0].v.RlyCd + ")" : GetValuePO[0].v.RlyNonrly;
                model.RlyNonrly = GetValuePO[0].v.RlyNonrly;
                model.PoOrLetter = GetValuePO[0].v.PoOrLetter;
                model.MaField = GetValuePO[0].d.MaField;
                model.MaDesc = GetValuePO[0].d.MaDesc;
                model.OldPoValue = GetValuePO[0].d.OldPoValue;
                model.NewPoValue = GetValuePO[0].d.NewPoValue;
                model.MADoc = "";
                model.MaStatus = GetValuePO[0].d.MaStatus;

                return model;
            }
        }

        public DTResult<MAapproveModel> GetDataList(DTParameters dtParameters, string GetRegionCode)
        {

            DTResult<MAapproveModel> dTResult = new() { draw = 0 };
            IQueryable<MAapproveModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string MaDt1 = Convert.ToDateTime("31-03-2020").ToString("dd-MM-yyyy");
            string MaDt2 = Convert.ToDateTime("01-01-2020").ToString("dd-MM-yyyy");

            query = from v in context.VendPoMaMasters
                    join d in context.VendPoMaDetails on v.CaseNo equals d.CaseNo
                    where v.CaseNo == d.CaseNo && v.MaNo == d.MaNo && v.MaDt == d.MaDt && d.MaStatus == "P" && v.CaseNo.Substring(0, 1) == GetRegionCode
                    //&& v.Region == GetRegionCode 

                    select new MAapproveModel
                    {
                        CaseNo = v.CaseNo,
                        PoNo = v.PoNo,
                        PoDt = v.PoDt,
                        MaNo = v.MaNo,
                        MaDt = v.MaDt,
                        RlyNonrly = v.RlyNonrly,
                        RlyCd = v.RlyNonrly.Equals("R") ? "Railway(" + v.RlyCd + ")" : v.RlyNonrly.Equals("P") ? "Private(" + v.RlyCd + ")" : v.RlyNonrly.Equals("S") ? "State Government(" + v.RlyCd + ")" : v.RlyNonrly.Equals("F") ? "Foreign Railways(" + v.RlyCd + ")" : v.RlyNonrly.Equals("U") ? "PSU(" + v.RlyCd + ")" : v.RlyNonrly,
                        PoOrLetter = v.PoOrLetter.Equals("P") ? "PO" : v.PoOrLetter.Equals("L") ? "Letter" : v.PoOrLetter,
                        MaDesc = d.MaDesc,
                        OldPoValue = d.OldPoValue,
                        NewPoValue = d.NewPoValue,
                        MADoc = "VENDOR/MA/" + v.CaseNo + "_" + v.MaNo + "_" + Convert.ToDateTime(v.MaDt).ToString("yyyyMMdd") + ".PDF",
                        MaStatus = d.MaStatus.Equals("P") ? "Pending" : d.MaStatus.Equals("A") ? "Approved" : d.MaStatus.Equals("R") ? "Return" : d.MaStatus,
                        PoSrc = v.PoSrc.Equals("V") ? "Vendor" : v.PoSrc.Equals("C") ? "Client" : v.PoSrc,
                        MaDtc = Convert.ToDateTime(v.MaDt).ToString("ddMMyyyy"),
                        MaSno = d.MaSno,
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsUpdate(MAapproveModel model)
        {
            int Id = 0;
            var GetValue = context.VendPoMaDetails.Find(model.CaseNo, model.MaNo, model.MaDt, model.MaSno);

            #region save
            if (GetValue == null)
            {
                VendPoMaDetail obj = new VendPoMaDetail();
                obj.MaStatus = model.MaStatus;
                obj.ApprovedBy = model.ApprovedBy;
                obj.MaRemarks = model.MaRemarks;
                obj.ApprovedDatetime = DateTime.Now;
                context.VendPoMaDetails.Add(obj);
                context.SaveChanges();
                Id = Convert.ToInt32(obj.MaSno);
            }
            else
            {
                GetValue.MaStatus = model.MaStatus;
                GetValue.ApprovedBy = model.ApprovedBy;
                GetValue.MaRemarks = model.MaRemarks;
                GetValue.ApprovedDatetime = DateTime.Now;
                context.SaveChanges();
                Id = Convert.ToInt32(GetValue.MaSno);
            }
            #endregion
            return Id;
        }
    }
}
