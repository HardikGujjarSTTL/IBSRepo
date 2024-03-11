using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVendorDocumentRepository
    {
        public VendEquipClbrCertModel FindByID(int VendCd, string DocType);
        int VendorDocumentInsertUpdate(VendEquipClbrCertModel model);
        int VendorCalibrationRecordsInsertUpdate(VendEquipClbrCertModel model);
        int GetmaxSrNo(int VendCd, string DocType);
        DTResult<VendEquipClbrCertListModel> GetVendorCalibrationRecordssList(DTParameters dtParameters, int VendCd);
        public VendEquipClbrCertModel FindVendorCalibrationByID(int VendCd, string DocType, string EquipMkSl, string CalibCertNo, int EquipClbrCertSno);
    }
}
