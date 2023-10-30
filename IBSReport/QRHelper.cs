using Net.Codecrete.QrCodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CrystalReportProject
{
    public static class QRHelper
    {
        private static readonly QrCode.Ecc[] errorCorrectionLevels = { QrCode.Ecc.Low, QrCode.Ecc.Medium, QrCode.Ecc.Quartile, QrCode.Ecc.High };
        public static byte[] GenerateSvg(string text)
        {
            int? ecc = 1, borderWidth = 3;
            //ecc = Math.Clamp(ecc ?? 1, 0, 3);
            //borderWidth = Math.Clamp(borderWidth ?? 3, 0, 999999);

            var qrCode = QrCode.EncodeText(text, errorCorrectionLevels[(int)ecc]);
            byte[] svg = Encoding.UTF8.GetBytes(qrCode.ToSvgString((int)borderWidth));
            // string str = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ0NDQwNUM3ODFFNDgyNTA3MkIzNENBNEY4QkRDNjA2Qzg2QjU3MjAiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJSRVFGeDRIa2dsQnlzMHlrLUwzR0JzaHJWeUEifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjA2QUFBQ1IwODMwUTFaQVwiLFwiQnV5ZXJHc3RpblwiOlwiMDZBQUJDVjExMjZFMVpWXCIsXCJEb2NOb1wiOlwiUjA2MDhMMjIvMDAxODJcIixcIkRvY1R5cFwiOlwiSU5WXCIsXCJEb2NEdFwiOlwiMjkvMDcvMjAyMlwiLFwiVG90SW52VmFsXCI6MTQ2OSxcIkl0ZW1DbnRcIjoxLFwiTWFpbkhzbkNvZGVcIjpcIjk5ODM0NlwiLFwiSXJuXCI6XCI0MzFmNjZlMjQzYTA3Nzc3NmZmZThhNTI5NTFhM2Q4YjdhMjgyZGEwOGQzN2JhOTkyOWNkMWViNmFlNTc1ZDY2XCIsXCJJcm5EdFwiOlwiMjAyMi0wOC0wNCAwOToxMTowMFwifSIsImlzcyI6Ik5JQyJ9.gq1Bh4NcB9RTECsq9B20IHYtEckHaQ8h7WVlH4pPjyTN7Ba48st4wRRh1N7BpaSQG_uiGH-t9KPm4Aw7WxMUrmOZr4MWhFKkfLqtSiOQejQcRcsOGG7wfgZRkJhwOSz9hejrP4VN2GJwpK9hubbJSkOAXWdM_zjB-dINRgoLVQ0z3ykdYvuPKX3ZtPcrPdQXrxKLQWf2hV52Pz6bpJut4VR5KRqW4B_mzTBL1E6hz_qQh6-4GIjXDMlzDldOm-FHF89VBs_v3CYjnCCgxOIbmCfeyl9Abf-1H6w1MhBYqCvoeGcb3jgTP3j0mHSMplG8w28HsJqbbIWRcLIOyKDhPA";

            //byte[] svg = Encoding.UTF8.GetBytes(str);
            return svg;
        }
        public static byte[] GeneratePng(string text)
        {
            //ecc = Math.Clamp(ecc ?? 1, 0, 3);
            //borderWidth = Math.Clamp(borderWidth ?? 3, 0, 999999);
            int? ecc = 3, borderWidth = 3;
            var qrCode = QrCode.EncodeText(text, errorCorrectionLevels[(int)ecc]);
            byte[] png = qrCode.ToPng(20, (int)borderWidth);
            return png;
        }
    }
}