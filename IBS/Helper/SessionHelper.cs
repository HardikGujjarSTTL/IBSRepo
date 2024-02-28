using IBS.DataAccess;
using IBS.Models;
using System.Text.Json;

namespace IBS.Helper
{
    public class SessionHelper
    {
        private static IHttpContextAccessor httpContextAccessor;

        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }

        public static UserSessionModel UserModelDTO
        {
            get
            {
                string userInfoString = httpContextAccessor.HttpContext.Session.GetString("UserInfo");
                if (!string.IsNullOrWhiteSpace(userInfoString))
                {
                    UserSessionModel appUser = JsonSerializer.Deserialize<UserSessionModel>(userInfoString);

                    if (appUser != null)
                        return appUser;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("UserInfo", JsonSerializer.Serialize(value));
            }
        }

        public static MenuMasterModel CurrentAccess
        {
            get
            {
                string _currentaccess = httpContextAccessor.HttpContext.Session.GetString("MenuMasterModel");
                if (!string.IsNullOrWhiteSpace(_currentaccess))
                {
                    MenuMasterModel currentaccess = JsonSerializer.Deserialize<MenuMasterModel>(_currentaccess);

                    if (currentaccess != null)
                        return currentaccess;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("MenuMasterModel", JsonSerializer.Serialize(value));
            }
        }

        public static List<MenuMasterModel> MenuModelDTO
        {
            get
            {
                string menuInfoString = httpContextAccessor.HttpContext.Session.GetString("MenuModelDTO");
                if (!string.IsNullOrWhiteSpace(menuInfoString))
                {
                    List<MenuMasterModel> appMenu = JsonSerializer.Deserialize<List<MenuMasterModel>>(menuInfoString);

                    if (appMenu != null)
                        return appMenu;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("MenuModelDTO", JsonSerializer.Serialize(value));
            }
        }

        public List<InspectionEngineersListModel> lstInspectionEClusterModel
        {
            get
            {
                string IECluster = httpContextAccessor.HttpContext.Session.GetString("sessionInspectionEClusterModel");
                if (httpContextAccessor.HttpContext.Session != null && !string.IsNullOrWhiteSpace(IECluster))
                {
                    List<InspectionEngineersListModel> IEClusterModels = JsonSerializer.Deserialize<List<InspectionEngineersListModel>>(IECluster);

                    if (IEClusterModels != null)
                        return IEClusterModels;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("sessionInspectionEClusterModel", JsonSerializer.Serialize(value));
            }
        }

        public List<ProjectDetails> lstProjectDetails
        {
            get
            {
                string ProjectDetails = httpContextAccessor.HttpContext.Session.GetString("sessionProductDetailsModel");
                if (httpContextAccessor.HttpContext.Session != null && !string.IsNullOrWhiteSpace(ProjectDetails))
                {
                    List<ProjectDetails> ProjectDetailsModels = JsonSerializer.Deserialize<List<ProjectDetails>>(ProjectDetails);

                    if (ProjectDetailsModels != null)
                        return ProjectDetailsModels;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("sessionProductDetailsModel", JsonSerializer.Serialize(value));
            }
        }

        public List<ContractEntryList> lstContractEntryList
        {
            get
            {
                string ContractEntry = httpContextAccessor.HttpContext.Session.GetString("sessionContractEntryList");
                if (httpContextAccessor.HttpContext.Session != null && !string.IsNullOrWhiteSpace(ContractEntry))
                {
                    List<ContractEntryList> ContractEntryModels = JsonSerializer.Deserialize<List<ContractEntryList>>(ContractEntry);

                    if (ContractEntryModels != null)
                        return ContractEntryModels;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("sessionContractEntryList", JsonSerializer.Serialize(value));
            }
        }

        public List<InterUnitTransferRegionModel> lstInterUnitTransferRegionModel
        {
            get
            {
                string UnitTransfer = httpContextAccessor.HttpContext.Session.GetString("sessionInterUnitTransferRegionModel");
                if (httpContextAccessor.HttpContext.Session != null && !string.IsNullOrWhiteSpace(UnitTransfer))
                {
                    List<InterUnitTransferRegionModel> UnitTransferModels = JsonSerializer.Deserialize<List<InterUnitTransferRegionModel>>(UnitTransfer);

                    if (UnitTransferModels != null)
                        return UnitTransferModels;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("sessionInterUnitTransferRegionModel", JsonSerializer.Serialize(value));
            }
        }

        public List<PO_Amendments> lstPoAmendments
        {
            get
            {
                string UnitTransfer = httpContextAccessor.HttpContext.Session.GetString("sessionPO_AmendmentsModel");
                if (httpContextAccessor.HttpContext.Session != null && !string.IsNullOrWhiteSpace(UnitTransfer))
                {
                    List<PO_Amendments> PO_AmendmentModels = JsonSerializer.Deserialize<List<PO_Amendments>>(UnitTransfer);

                    if (PO_AmendmentModels != null)
                        return PO_AmendmentModels;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("sessionPO_AmendmentsModel", JsonSerializer.Serialize(value));
            }
        }
        public List<ManpowerDetailModel> lstManpowerDetailModel
        {
            get
            {
                string IECluster = httpContextAccessor.HttpContext.Session.GetString("sessionManpowerDetailModel");
                if (httpContextAccessor.HttpContext.Session != null && !string.IsNullOrWhiteSpace(IECluster))
                {
                    List<ManpowerDetailModel> ManpowerDetails = JsonSerializer.Deserialize<List<ManpowerDetailModel>>(IECluster);

                    if (ManpowerDetails != null)
                        return ManpowerDetails;
                }
                return null;
            }
            set
            {
                httpContextAccessor.HttpContext.Session.SetString("sessionManpowerDetailModel", JsonSerializer.Serialize(value));
            }
        }
    }
}
