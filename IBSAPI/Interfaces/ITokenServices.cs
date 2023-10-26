using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface ITokenServices
    {
        #region Interface member methods.

        TokenEntity GenerateToken(string userId);

        bool ValidateToken(string tokenId);

        bool ValidateToken(string tokenId, string user_id);

        bool Kill(string tokenId);

        bool DeleteByUserId(string userId);

        bool InActiveOldActiveTokens(string UserId, string AuthToken);

        string GetUserOnValidateToken(string tokenId);

        string GetUserByToken(string tokenid);

        #endregion
    }
}
