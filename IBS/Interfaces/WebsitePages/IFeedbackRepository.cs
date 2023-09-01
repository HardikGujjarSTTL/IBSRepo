using IBS.Models;

namespace IBS.Interfaces.WebsitePages
{
    public interface IFeedbackRepository
    {
        #region Vendor feedback
        bool CheckAlreadyExist(VendorFeedbackModel model);
        int VendorFeedbackSave(VendorFeedbackModel model);
        #endregion

        #region Client feedback
        bool CheckClientAlreadyExist(ClientFeedbackModel model);
        int ClientFeedbackSave(ClientFeedbackModel model);
        #endregion
    }
}
