using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class VendorDocumentRepository : IVendorDocumentRepository
    {
        private readonly ModelContext context;

        public VendorDocumentRepository(ModelContext context)
        {
            this.context = context;
        }

        public VendEquipClbrCertModel FindByID(int VendCd, string DocType)
        {
            VendEquipClbrCertModel model = (from m in context.T103VendDocs
                                            where m.VendCd == VendCd && m.DocType == DocType
                                            select new VendEquipClbrCertModel
                                            {
                                                VendCd = m.VendCd,
                                                DocType = m.DocType,
                                                ID = m.Id
                                            }).FirstOrDefault();

            return model;
        }

        public int VendorDocumentInsertUpdate(VendEquipClbrCertModel model)
        {
            int ID = 0;
            var document = context.T103VendDocs.Where(x => x.VendCd == model.VendCd && x.DocType == model.DocType).FirstOrDefault();
            #region document save
            if (document == null)
            {
                T103VendDoc obj = new T103VendDoc();
                obj.VendCd = model.VendCd;
                obj.DocType = model.DocType;
                obj.Datetime = DateTime.Now;
                context.T103VendDocs.Add(obj);
                context.SaveChanges();
                ID = Convert.ToInt32(obj.Id);
            }
            else
            {
                document.VendCd = model.VendCd;
                document.DocType = model.DocType;
                document.Datetime = DateTime.Now;
                context.SaveChanges();
                ID = Convert.ToInt32(document.Id);
            }
            #endregion
            return ID;
        }

        public int GetmaxSrNo(int VendCd, string DocType)
        {
            int ID = 0;
            var maxSr = (from m in context.T104VendEquipClbrCerts
                         where m.VendCd == VendCd && m.DocType == DocType
                         select m.EquipClbrCertSno).Max() + 1;
            ID = Convert.ToInt32(maxSr);
            return ID;
        }

        public int VendorCalibrationRecordsInsertUpdate(VendEquipClbrCertModel model)
        {
            int ID = 0;
            var maxSr = (from m in context.T104VendEquipClbrCerts
                         where m.VendCd == model.VendCd && m.DocType == model.DocType
                         select m.EquipClbrCertSno).Max() + 1;
            var objVendEquipClbrCerts = context.T104VendEquipClbrCerts.Where(x => x.VendCd == model.VendCd && x.DocType == model.DocType
                            && x.EquipMkSl == model.EquipMkSl && model.CalibCertNo == model.CalibCertNo
                            && model.EquipClbrCertSno == model.EquipClbrCertSno).FirstOrDefault();
            #region T104VendEquipClbrCerts save
            if (objVendEquipClbrCerts == null)
            {
                T104VendEquipClbrCert obj = new T104VendEquipClbrCert();
                obj.VendCd = model.VendCd;
                obj.DocType = model.DocType;
                obj.EquipMkSl = model.EquipMkSl;
                obj.CalibCertNo = model.CalibCertNo;
                obj.EquipClbrCertSno = Convert.ToByte(maxSr);
                obj.EquipName = model.EquipName;
                obj.EquipRange = model.EquipRange;
                obj.CalibratedBy = model.CalibratedBy;
                obj.DtOfCalib = model.DtOfCalib;
                obj.NextDtCalib = model.NextDtCalib;
                obj.EquipDesc = model.EquipDesc;
                obj.NablAccDet = model.NablAccDet;
                obj.Datetime = DateTime.Now;
                context.T104VendEquipClbrCerts.Add(obj);
                context.SaveChanges();
                ID = Convert.ToInt32(obj.EquipClbrCertSno);
            }
            else
            {
                objVendEquipClbrCerts.EquipName = model.EquipName;
                objVendEquipClbrCerts.EquipRange = model.EquipRange;
                objVendEquipClbrCerts.CalibratedBy = model.CalibratedBy;
                objVendEquipClbrCerts.DtOfCalib = model.DtOfCalib;
                objVendEquipClbrCerts.NextDtCalib = model.NextDtCalib;
                objVendEquipClbrCerts.EquipDesc = model.EquipDesc;
                objVendEquipClbrCerts.NablAccDet = model.NablAccDet;
                objVendEquipClbrCerts.Datetime = DateTime.Now;
                context.SaveChanges();
                ID = Convert.ToInt32(objVendEquipClbrCerts.EquipClbrCertSno);
            }
            #endregion
            return ID;
        }

        public DTResult<VendEquipClbrCertListModel> GetVendorCalibrationRecordssList(DTParameters dtParameters, int VendCd)
        {
            DTResult<VendEquipClbrCertListModel> dTResult = new() { draw = 0 };
            IQueryable<VendEquipClbrCertListModel>? query = null;
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "EQUIP_CLBR_CERT_SNO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "EQUIP_CLBR_CERT_SNO";
                orderAscendingDirection = true;
            }

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_VEND_CD", OracleDbType.Char, VendCd, ParameterDirection.Input);
            par[1] = new OracleParameter("p_DOC_TYPE", OracleDbType.Char, "C", ParameterDirection.Input);
            par[2] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GetVendorDocumentDetails", par, 1);
            List<VendEquipClbrCertListModel> model = new List<VendEquipClbrCertListModel>();
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                model = JsonConvert.DeserializeObject<List<VendEquipClbrCertListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            query = model.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.EQUIP_NAME).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.CALIB_CERT_NO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public VendEquipClbrCertModel FindVendorCalibrationByID(int VendCd, string DocType, string EquipMkSl, string CalibCertNo, int EquipClbrCertSno)
        {
            VendEquipClbrCertModel model = (from m in context.T104VendEquipClbrCerts
                                            where m.VendCd == VendCd && m.DocType == DocType && m.EquipMkSl == EquipMkSl
                                            && m.CalibCertNo == CalibCertNo && m.EquipClbrCertSno == EquipClbrCertSno
                                            select new VendEquipClbrCertModel
                                            {
                                                VendCd = m.VendCd,
                                                DocType = m.DocType,
                                                EquipMkSl = m.EquipMkSl,
                                                CalibCertNo = m.CalibCertNo,
                                                EquipClbrCertSno = m.EquipClbrCertSno,
                                                EquipName = m.EquipName,
                                                EquipRange = m.EquipRange,
                                                CalibratedBy = m.CalibratedBy,
                                                DtOfCalib = m.DtOfCalib,
                                                NextDtCalib = m.NextDtCalib,
                                                EquipDesc = m.EquipDesc,
                                                NablAccDet = m.NablAccDet,
                                            }).FirstOrDefault();
            return model;
        }
    }

}

