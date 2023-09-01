using IBS.Models;

namespace IBS.Interfaces.WebsitePages
{
    public interface IFeedbackRepository
    {
        #region Vendor feedback
        bool CheckAlreadyExist(VendorFeedbackModel model);
        int VendorFeedbackSave(VendorFeedbackModel model);
        #endregion

    }
}
