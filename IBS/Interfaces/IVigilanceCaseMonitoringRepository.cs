using IBS.Models;

namespace IBS.Interfaces
{
    public interface IVigilanceCaseMonitoringRepository
    {
        public VigilanceCasesMasterModel FindByID(int Id);

        DTResult<VigilanceCasesMasterModel> GetVigilanceCaseList(DTParameters dtParameters);

        public DTResult<VigilanceCasesListModel> GetVigilanceList1(DTParameters dtParameters);

        public DTResult<VigilanceCasesListModel> GetVigilanceList2(DTParameters dtParameters);

        public int SaveDetails(VigilanceCasesMasterModel model);
    }
}