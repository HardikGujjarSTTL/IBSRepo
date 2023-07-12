using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRoleRepository
    {
        public RoleModel FindByID(int RoleId);
        DTResult<RoleModel> GetRoleList(DTParameters dtParameters);
        bool Remove(int RoleId, int UserID);
        int RoleDetailsInsertUpdate(RoleModel model);
    }
}
