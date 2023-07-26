using IBS.Models;
namespace IBS.Interfaces
{
	public interface IUnitOfMeasurements
	{
		public UOMModel FindByID(int UomCd);
		DTResult<UOMModel> GetUOMList(DTParameters dtParameters);
		bool Remove(int UomCd, int UserID);
		int UOMDetailsInsertUpdate(UOMModel model);
	}
}
