using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICMIEWiseCancellationAcceptance_FormRepository
    {
        public CMIEWiseCancellationAcceptance_FormModel GetIEsByRegionAndCO(string selectedValue, string region);
        public DTResult<CMIEWiseCancellationAcceptance_FormModel> CMIEWTable(DTParameters dtParameters, string Region);
        public DTResult<CMIEWiseCancellationAcceptance_FormModel> CMIEWTable1(DTParameters dtParameters, string Region);

        public string update(CMIEWiseCancellationAcceptance_FormModel model, string Uname);
    }
}
