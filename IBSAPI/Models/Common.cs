using System.Text;

namespace IBSAPI.Models
{
    public static class Common
    {
        public static string EncryptQueryString(this String strToEncrypt)
        {
            if (!string.IsNullOrEmpty(strToEncrypt))
            {
                byte[] passBytes = Encoding.Unicode.GetBytes(strToEncrypt);
                string encryptPassword = Convert.ToBase64String(passBytes);
                return encryptPassword;
            }
            else
                return string.Empty;
        }

        public static string DecryptQueryString(this String strToDecrypt)
        {
            if (!string.IsNullOrEmpty(strToDecrypt))
            {
                byte[] passByteData = Convert.FromBase64String(strToDecrypt);
                string originalPassword = Encoding.Unicode.GetString(passByteData);
                return originalPassword;
            }
            else
                return string.Empty;
        }

    }
}
