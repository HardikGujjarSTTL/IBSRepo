using Humanizer;
using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class DEOCRISPurchesOrderRepository : IDEOCRISPurchesOrderRepository
    {
        private readonly ModelContext context;

        public DEOCRISPurchesOrderRepository(ModelContext context)
        {
            this.context = context;
        }

        public DEOCRISPurchesOrderMAModel FindByID(string Rly, int Makey, byte Slno)
        {
            DEOCRISPurchesOrderMAModel model = new();
            //MmpPomaDtl user = context.MmpPomaDtls.Find(Rly, Makey, Slno);

            var GetValuePO = (from h in context.ImmsRitesPoHdrs
                                  join r in context.T91Railways on h.RlyCd equals r.RlyCd
                                  join m in context.MmpPomaHdrs on h.ImmsPokey equals m.Pokey
                                  join d in context.MmpPomaDtls on m.Makey equals d.Makey
                                  where d.Rly == Rly && d.Makey == Makey && d.Slno == Slno
                                  select new {
                                      h,r,m,d
                                  }
                  ).ToList();

            if (GetValuePO == null)
                throw new Exception("Record Not found");
            else
            {
                model.Rly = GetValuePO[0].d.Rly;
                model.Makey = GetValuePO[0].d.Makey;
                model.Slno = GetValuePO[0].d.Slno;

                model.RitesCaseNo = GetValuePO[0].h.RitesCaseNo;
                model.ImmsPokey = GetValuePO[0].h.ImmsPokey;
                model.PoNo = GetValuePO[0].h.PoNo;
                model.PoDt = GetValuePO[0].h.PoDt;
                model.RecvDate = GetValuePO[0].h.RecvDate;
                model.ImmsRlyCd = GetValuePO[0].h.ImmsRlyCd;
                model.ImmsRlyShortname = GetValuePO[0].h.ImmsRlyShortname;
                model.VendorName = GetValuePO[0].h.ImmsVendorName + "," + GetValuePO[0].h.ImmsVendorDetail;
                model.Remarks = GetValuePO[0].h.Remarks;

                model.PoDoc = "Vendor/PO/" + GetValuePO[0].h.PoNo + ".pdf";
                model.MaNo = GetValuePO[0].m.MaNo;
                model.MaDate = GetValuePO[0].m.MaDate;
                model.Subject = GetValuePO[0].m.Subject;
                model.MaFldDescr = GetValuePO[0].d.MaFldDescr;
                model.NewValue = GetValuePO[0].d.NewValue;
                model.OldValue = GetValuePO[0].d.OldValue;


                return model;
            }
        }

        public DTResult<DEOCRISPurchesOrderMAModel> GetDataList(DTParameters dtParameters, string GetRegionCode)
        {

            DTResult<DEOCRISPurchesOrderMAModel> dTResult = new() { draw = 0 };
            IQueryable<DEOCRISPurchesOrderMAModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RitesCaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "RitesCaseNo";
                orderAscendingDirection = true;
            }

            string MaDt1 = Convert.ToDateTime("31-03-2020").ToString("dd-MM-yyyy");
            string MaDt2 = Convert.ToDateTime("01-01-2020").ToString("dd-MM-yyyy");

            query = from h in context.ImmsRitesPoHdrs
                    join r in context.T91Railways on h.ImmsRlyCd equals r.ImmsRlyCd
                    join m in context.MmpPomaHdrs on h.ImmsPokey equals m.Pokey
                    join d in context.MmpPomaDtls on m.Makey equals d.Makey
                    where h.ImmsPokey == m.Pokey && h.ImmsRlyCd == m.Rly && m.Makey == d.Makey && m.Rly == d.Rly && h.ImmsRlyCd == r.ImmsRlyCd
                    && h.RitesCaseNo != null 
                    && d.MaStatus == null
                    && h.RegionCode == GetRegionCode && m.MaDate > Convert.ToDateTime(MaDt1) && m.MaDate > Convert.ToDateTime(MaDt2)

                    select new DEOCRISPurchesOrderMAModel
                    {
                        RitesCaseNo = h.RitesCaseNo,
                        ImmsPokey = h.ImmsPokey,
                        PoNo = h.PoNo,
                        PoDt = h.PoDt,
                        RecvDate = h.RecvDate,
                        ImmsRlyCd = h.ImmsRlyCd,
                        ImmsRlyShortname = h.ImmsRlyShortname,
                        VendorName = h.ImmsVendorName + "," + h.ImmsVendorDetail,
                        Remarks = h.Remarks,
                        PoDoc = "Vendor/PO/" + h.PoNo + ".pdf",
                        MaNo = m.MaNo,
                        MaDate = m.MaDate,
                        Subject = m.Subject,
                        MaFldDescr = d.MaFldDescr,
                        NewValue = d.NewValue,
                        Rly = d.Rly,
                        Makey = d.Makey,
                        Slno = d.Slno,
                        OldValue = d.OldValue,
                        MaStatus = d.MaStatus

                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RitesCaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.VendorName).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsUpdate(DEOCRISPurchesOrderMAModel model)
        {
            int Id = 0;
            var GetValuePO = context.MmpPomaDtls.Find(model.Rly, model.Makey, model.Slno);

            #region save
            if (GetValuePO == null)
            {
                MmpPomaDtl obj = new MmpPomaDtl();
                obj.MaStatus = model.MaStatus;
                obj.ApprovedBy = model.ApprovedBy;
                obj.ApprovedDatetime = DateTime.Now;
                context.MmpPomaDtls.Add(obj);
                context.SaveChanges();
                Id = Convert.ToInt32(obj.Makey);
            }
            else
            {
                GetValuePO.MaStatus = model.MaStatus;
                GetValuePO.ApprovedBy = model.ApprovedBy;
                GetValuePO.ApprovedDatetime = DateTime.Now;
                context.SaveChanges();
                Id = Convert.ToInt32(GetValuePO.Makey);
            }
            #endregion
            return Id;
        }
    }
}
