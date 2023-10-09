using IBS.DataAccess;
using IBS.Models;
using System.Data;
namespace IBS.Interfaces
{
    public interface IBarcodeGeneration
    {
        DTResult<BarcodeGenerate> GetBarcodeData(DTParameters dtParameters);
        DTResult<BarcodeGenerate> CaseNoSearch(DTParameters dtParameters);
        bool SaveBarCode(BarcodeGenerate BarcodeGenerate,string IPADDRESS);
        DTResult<BarcodeGenerate> LoadCalculation(DTParameters dtParameters);
    }
}
