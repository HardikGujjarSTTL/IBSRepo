using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICalls_Marked_For_Specific_PORepository
    {
        public List<railway_dropdown> GetValue(string selectedValue);
        public List<railway_dropdown> GetValue2(string selectedValue);
        public DTResult<Calls_Marked_For_Specific_POModel> gridData(DTParameters dtParameters);
        public Calls_Marked_For_Specific_POModel edit(string PO_NO, string PO_DT, string RLY_NONRLY, string RLY_CD);
    }
}
