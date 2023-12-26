using IBS.Models;
namespace IBS.Interfaces
{
    public interface IBarcodeGeneration
    {
        DTResult<BarcodeGenerate> GetBarcodeData(DTParameters dtParameters);
        DTResult<BarcodeGenerate> CaseNoSearch(DTParameters dtParameters);
        bool SaveBarCode(BarcodeGenerate BarcodeGenerate, string IPADDRESS);
        DTResult<BarcodeGenerate> LoadCalculation(DTParameters dtParameters);
        bool InsertDataForLabTran(BarcodeGenerate BarcodeGenerate);
        bool SaveBarcodeGenerated(string Barcode, int quantity, string caseno, int callsno, string calldate, string IPADDRESS, string USERID, string CREATEDBY);
    }
}
