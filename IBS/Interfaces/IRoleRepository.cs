using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRoleRepository
    {
        #region Role
        public RoleModel FindByID(int RoleId);
        DTResult<RoleModel> GetRoleList(DTParameters dtParameters);
        bool Remove(int RoleId, int UserID);
        int RoleDetailsInsertUpdate(RoleModel model);
        #endregion

        #region UserRole
        public RoleModel FindUserRoleByID(int ID);
        DTResult<RoleModel> GetUserRoleList(DTParameters dtParameters);
        bool RemoveUserRole(int ID, int UserID);
        int UserRoleInsertUpdate(RoleModel model);
        #endregion

        #region MenuRoleMapping
        DTResult<MenuListModel> GetMenuList(DTParameters dtParameters);
        public MenuroleMappingModel FindMenuRoleMappingByID(int ID);
        DTResult<MenuroleMappingListModel> GetMenuRoleMappingList(DTParameters dtParameters);
        int MenuRoleMappingInsertUpdate(MenuroleMappingModel model);
        bool RemoveenuRoleMapping(int ID, int UserID);
        #endregion
    }
}
