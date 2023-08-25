using IBS.Models;

namespace IBS.Interfaces
{
	public interface IUnitOfMeasurementsRepository
	{
        DTResult<UOMModel> GetUOMList(DTParameters dtParameters);

		public UOMModel FindByID(int id);

		bool Remove(int UomCd, int UserID);

		public int SaveDetails(UOMModel model);
	}
}
