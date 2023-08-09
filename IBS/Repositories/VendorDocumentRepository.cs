using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
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
    }

}

