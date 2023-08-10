using IBS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace IBS.Interfaces
{
    public interface IHologramAccountalRepository
    {
        DTResult<HologramAccountalModel> GetHologramAcountList(DTParameters dTParameters, string Region);

        string GetRegionCode(string BK_NO, string SET_NO, string REGION);
        HologramAccountalModel GetHologramAccountalDetail(HologramAccountalFilter model);
        DTResult<HologramAccountalModel> GetHologramAccountalDetails([FromBody] DTParameters dtParameters);
        HologramAccountalModel GetSelectedHologramAccountal(string CaseNo, string CallRecvDt, int CCD, int CallSNo, int RecNo);
        string CheckDuplicateHologram(HologramAccountalModel model);
        int GetRecNo(string Case_No, string CallRecvDt, int CCD, int CallSNo);

        bool HologramInsertUpdate(HologramAccountalModel model, int RNO);
        bool HologramDelete(HologramAccountalModel model);
    }
}
