using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorDocumentRepository
    {
        public VendEquipClbrCertModel FindByID(int VendCd,string DocType);
        int VendorDocumentInsertUpdate(VendEquipClbrCertModel model);
    }
}
