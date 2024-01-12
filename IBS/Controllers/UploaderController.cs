
using IBS.Controllers;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using IBS.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GSDMWeb.Controllers.Master
{
    public class UploaderController : BaseController
    {
        private readonly IDocument iDocument;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration _config;
        public UploaderController(IDocument _iDocumentRepository, IWebHostEnvironment _environment, IConfiguration configuration)
        {
            iDocument = _iDocumentRepository;
            env = _environment;
            _config = configuration;
        }


        #region FIle Upload Test
        [HttpPost]
        public async Task<JsonResult> UploadFiles(int DocumentID, IFormCollection frm)
        {
            StringBuilder strData = new StringBuilder();
            string ErrorMessage = string.Empty;

            try
            {
                //string TempFilePath = ConfigurationManager.AppSettings["TempFilePath"];
                //string TempFilePath = Path.Combine(env.WebRootPath, Enums.GetEnumDescription(Enums.FolderPath.TempFilePath));
                string TempFilePath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.TempFilePath);
                if (!Directory.Exists(TempFilePath))
                {
                    Directory.CreateDirectory(TempFilePath);
                }

                int i = 0;
                var fileName = string.Empty;
                string UniqueFileID = string.Empty;

                List<string> FileCollection = new List<string>();

                strData.Append("{UploadedList:[");

                foreach (var file in frm.Files)
                {
                    var Postedfile = file;
                    fileName = string.Empty;

                    string AllowedFileExtensions = string.Empty;
                    int MaxContentLengthInKB = 0;
                    IBS_DocumentDTO objDocumentDTO = iDocument.FindRecord(DocumentID);

                    if (DocumentID == 0)
                    {
                        AllowedFileExtensions = Convert.ToString(_config["MyAppSettings:MaxContentLengthInKB"]);
                        MaxContentLengthInKB = Convert.ToInt32(_config["MyAppSettings:MaxContentLengthInKB"]);
                    }
                    else if (objDocumentDTO != null && objDocumentDTO.AllowedFileExtensions != null)
                    {
                        AllowedFileExtensions = objDocumentDTO.AllowedFileExtensions;
                        MaxContentLengthInKB = (objDocumentDTO.MaxContentLengthInKB != null ? objDocumentDTO.MaxContentLengthInKB.Value : Convert.ToInt32(_config["MyAppSettings:MaxContentLengthInKB"]));
                    }
                    else
                    {
                        ErrorMessage = "DocumentID Not Exists.";
                        //throw new Exception(ErrorMessage);
                        return Json(new JsonResultDTO { AlterStyle = "red", IsSuccess = false, MessageText = ErrorMessage });
                    }

                    if (Postedfile != null && Postedfile.Length > 0 && ValidateFile(Postedfile, ref ErrorMessage, AllowedFileExtensions, MaxContentLengthInKB))
                    {
                        //var stream1 = new MemoryStream();

                        //bytes = ConvertToByteArray(fileUpload.InputStream);

                        fileName = Path.GetFileName(Postedfile.FileName);
                        UniqueFileID = System.IO.Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(Postedfile.FileName);

                        string TempFilePath1 = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.TempFilePath);

                        var path = Path.Combine(TempFilePath1, UniqueFileID);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            Postedfile.CopyTo(fileStream);
                            FileCollection.Add(path);
                        }

                        strData.Append("{");
                        strData.Append("\"FileName\":\"" + fileName + "\",");
                        strData.Append("\"UniqueFileID\":\"" + UniqueFileID + "\",");
                        strData.Append("}");

                        if (i < frm.Files.Count)
                        {
                            strData.Append(",");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ErrorMessage))
                        {
                            foreach (var files in FileCollection)
                                System.IO.File.Delete(files);

                            //throw new Exception(ErrorMessage);
                            return Json(new JsonResultDTO { AlterStyle = "red", IsSuccess = false, MessageText = ErrorMessage });
                        }

                        if (Postedfile.Length == 0)
                        {
                            ErrorMessage = "";
                            //throw new Exception(ErrorMessage);
                            return Json(new JsonResultDTO { AlterStyle = "red", IsSuccess = false, MessageText = ErrorMessage });
                        }
                    }
                }
                strData.Append("]}");
            }
            catch (Exception)
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Json((string.IsNullOrEmpty(ErrorMessage) ? "File Upload failed" : ErrorMessage));
                return Json(new JsonResultDTO { AlterStyle = "red", IsSuccess = false, MessageText = "File Upload failed" });
            }

            return Json(strData.ToString());
        }

        [HttpPost]
        public async Task<string> DeleteFile(string UniqueFileID)
        {
            string retStatus = "File Deleted Successfully.";
            try
            {
                string TempFilePath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.TempFilePath);      //string TempFilePath = Server.MapPath(EnumUtils.GetEnumDescription(GSDMUTIL.Constants.Enumes.FolderPath.TempFilePath));

                System.IO.File.Delete(Path.Combine(TempFilePath, UniqueFileID));
            }
            catch (Exception)
            {
                retStatus = "Looks Like Something Went Wrong. while deleting file...";
            }

            return retStatus;
        }
        #endregion

        #region Extension methods
        private bool ValidateFile(IFormFile file, ref string ErrorMessage, string AllowedFileExtensions, int MaxContentLengthInKB)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    if (!ValidateAttachment(file, ref ErrorMessage, AllowedFileExtensions, MaxContentLengthInKB))
                    {
                        return false;
                    }
                    return true;
                }
            }
            return true;
        }

        public static byte[] ConvertToByteArray(Stream fs)
        {
            BinaryReader br = new BinaryReader(fs);
            byte[] image = br.ReadBytes(Convert.ToInt32(fs.Length));
            fs.Seek(0, SeekOrigin.Begin);
            return image;
        }

        /// <summary>
        /// Checks the file extension.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="Extensions">The extensions.</param>
        /// <returns>Boolean true or false</returns>
        /// <remarks></remarks>
        public static bool CheckFileExtension(string FileName, string[] Extensions)
        {
            string _extension;
            try
            {
                _extension = Path.GetExtension(FileName).ToUpper();
                foreach (string ext in Extensions)
                {
                    if (_extension == ext.ToUpper()) return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static string GetFileHeaderHexCode(string FileExtension)
        {
            FileExtension = FileExtension.ToLower();
            Dictionary<string, string> d = new Dictionary<string, string>();
            //Images'
            d.Add(".bmp", "424D");
            d.Add(".gif", "47494638");
            d.Add(".jpeg", "FFD8FF");
            d.Add(".jpg", "FFD8FF");
            d.Add(".png", "89504E470D0A1A0A");
            d.Add(".tif", "492049");
            d.Add(".tiff", "492049");
            //Documents'
            d.Add(".doc", "D0CF11E0A1B11AE1");
            d.Add(".docx", "504B030414000600");
            d.Add(".pdf", "25504446");
            //Slideshows'
            d.Add(".ppt", "D0CF11E0A1B11AE1");
            d.Add(".pptx", "504B030414000600");
            //Data'
            d.Add(".xlsx", "504B030414000600");
            d.Add(".xls", "D0CF11E0A1B11AE1");
            //d.Add(".csv", "text/csv");
            d.Add(".xml", "3C");
            //d.Add(".txt", "text/plain");
            //Compressed Folders'
            d.Add(".zip", "504B");
            d.Add(".rar", "526172211A0700CF907300000D00000000000000");
            //Audio'
            d.Add(".ogg", "4F67675300020000000000000000");
            d.Add(".mp3", "494433");
            d.Add(".wma", "3026B2758E66CF11A6D900AA0062CE6C");
            d.Add(".wav", "52494646xxxxxxxx57415645666D7420");
            //Video'
            d.Add(".wmv", "3026B2758E66CF11A6D900AA0062CE6C");
            d.Add(".swf", "435753");
            d.Add(".avi", "52494646xxxxxxxx415649204C495354");
            //d.Add(".mp4", "000000186674797033677035");

            d.Add(".mp4", "0000001C667479706");
            //d.Add(".mp4", "000000146674797069736F6D");                  
            d.Add(".mpeg", "000001Bx");
            d.Add(".mpg", "000001Bx");
            //d.Add(".flv", "464C56010500000009000000001200010C");
            d.Add(".flv", "464C5601");
            d.Add(".mov", "0000001466747970717420200000020071742020");
            //d.Add(".qt", "video/quicktime");
            return d[FileExtension];
        }

        public static bool GetFileHeaderHexCodemp4(string FileHeader)
        {
            string[] strmprHeader = { "0000001C667479706", "0000001866747970" };

            for (int i = 0; i < strmprHeader.Length; i++)
            {
                if (FileHeader.Contains(strmprHeader[i]))
                {
                    return true;
                }
            }


            return false;
        }

        public static bool CheckFileHeader(string hexString, string _extension)
        {
            try
            {
                if (string.IsNullOrEmpty(hexString)) return false;
                else
                {
                    if (_extension.ToLower() == ".mp4")
                    {
                        if (hexString.Contains(GetFileHeaderHexCode(_extension)) == false)
                        {
                            if ((GetFileHeaderHexCodemp4(hexString)))
                            {
                                return true;
                            }
                        }
                        else
                            return true;
                    }
                    else if (hexString.Contains(GetFileHeaderHexCode(_extension)))
                        return true;
                }
            }

            catch { return false; }
            return false;
        }

        /// <summary>
        /// Files the type of the content.
        /// </summary>
        /// <param name="FileExtension">The file extension.</param>
        /// <returns>String of File Content Type</returns>
        /// <remarks></remarks>
        public static string FileContentType(string FileExtension)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            //Images'
            d.Add(".bmp", "image/bmp");
            d.Add(".gif", "image/gif");
            d.Add(".jpeg", "image/jpeg");
            d.Add(".jpg", "image/jpeg");
            d.Add(".png", "image/png");
            d.Add(".tif", "image/tiff");
            d.Add(".tiff", "image/tiff");
            d.Add(".ico", "image/ico");
            //Documents'
            d.Add(".doc", "application/msword");
            d.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            d.Add(".pdf", "application/pdf");
            //Slideshows'
            d.Add(".ppt", "application/vnd.ms-powerpoint");
            d.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            //Data'
            d.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            d.Add(".xls", "application/vnd.ms-excel");
            d.Add(".csv", "text/csv");
            d.Add(".xml", "text/xml");
            d.Add(".txt", "text/plain");
            //Compressed Folders'
            d.Add(".zip", "application/zip");
            d.Add(".rar", "application/octet-stream");
            //Audio'
            d.Add(".ogg", "application/ogg");
            d.Add(".mp3", "audio/mpeg");
            d.Add(".wma", "audio/x-ms-wma");
            d.Add(".wav", "audio/x-wav");
            //Video'
            d.Add(".wmv", "video/x-ms-wmv");
            d.Add(".swf", "application/x-shockwave-flash");
            d.Add(".avi", "video/avi");
            d.Add(".mp4", "video/mp4");
            d.Add(".mpeg", "video/mpeg");
            d.Add(".mpg", "video/mpeg");
            d.Add(".qt", "video/quicktime");
            return d[FileExtension];
        }

        /// <summary>
        /// Checks the type of the file content.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <param name="ContentType">Type of the content.</param>
        /// <param name="Extensions">The extensions.</param>
        /// <returns>Boolean true or false</returns>
        /// <remarks></remarks>
        public static bool CheckFileContentType(string FileName, string ContentType, string[] Extensions)
        {
            string _extension;
            try
            {
                _extension = Path.GetExtension(FileName).ToUpper();
                foreach (string ext in Extensions)
                {
                    if (_extension == ext.ToUpper() && ContentType.ToLower() == FileContentType(ext))
                        return true;
                }
            }
            catch { return false; }
            return false;
        }

        /// <summary>
        /// Upload File.  ValidateAttachment
        /// Comma seperated extensions. ext = ".jpeg,.jpg,.png"
        /// </summary>
        /// <returns>byte array</returns>
        /// <remarks></remarks>
        public static Boolean ValidateAttachment(IFormFile fileUpload, ref string isShowMessage, string ext, Int64 FileSize = 5000)
        {
            string filePhotoName = "";
            Byte[] bytes = null;
            string FileName = fileUpload.FileName;
            if (!string.IsNullOrEmpty(fileUpload.FileName))
            {
                if (FileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    isShowMessage = "Invalid file name";
                    return false;
                }
                else if (Path.GetFileNameWithoutExtension(FileName).Length >= 99)
                {
                    isShowMessage = "Invalid file name length";
                    return false;
                }

                using (var stream = new MemoryStream())
                {
                    fileUpload.CopyTo(stream);
                    bytes = stream.ToArray();

                }

                //bytes = ConvertToByteArray(fileUpload.InputStream);

                string hexString = string.Empty;
                int length = 20;
                if (bytes.Length < 20)
                {
                    length = bytes.Length;
                }

                for (int i = 0; i < length; i++)
                    hexString += bytes[i].ToString("X2");
                string extension = Path.GetExtension(fileUpload.FileName).ToUpper();

                string[] strExt = ext.Split(',');// ".png,.jpg"
                string[] getFileExtension = fileUpload.FileName.Split('.');
                filePhotoName = fileUpload.FileName.ToString();
                if (!CheckFileExtension(filePhotoName, strExt) || (getFileExtension.Length > 2))
                {
                    //Functions.AlertMessage(this.Page, Constants.GeneralMessages.SupportedImageType);                   
                    //GeneralFunction.ValidationError(this.Page, "You must select an image file .gif, .jpg, .jpeg, .png only.");
                    isShowMessage = "Only " + ext + " file formats are allowed.";
                    bytes = null;
                    return false;
                }
                if (!CheckFileHeader(hexString, extension))
                {
                    //GeneralFunction.ValidationError(this.Page, "Invalid file format");
                    isShowMessage = "Invalid file format";
                    bytes = null;
                    return false;
                }
                if (!CheckFileContentType(filePhotoName, fileUpload.ContentType, strExt))
                {
                    isShowMessage = "Invalid content type";
                    bytes = null;
                    return false;
                }
                decimal size = Math.Round((fileUpload.Length / (decimal)1024), 2);
                if (size < 1 || size > FileSize)
                {
                    //Functions.AlertMessage(this.Page, Constants.GeneralMessages.SupportedImageType);
                    //GeneralFunction.ValidationError(this.Page, "Image size should be between 15kb and 200kb.");
                    isShowMessage = "File size should be between 1kb and " + (FileSize / 1000) + "MB.";
                    bytes = null;
                    return false;
                }
                isShowMessage = "";
                return true;
            }
            else
            {
                return true;
                //isShowMessage = "Please select file.";
            }
        }
        #endregion

        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SaveFile(string FolderName, string DocumentcategoryID, string Documentid)
        {
            string result = "";
            string folderPath = env.WebRootPath + FolderName;
            string applicationid = "";
            string otherdocumentname = "";
            try
            {
                if (Directory.Exists(folderPath))
                {
                    StringBuilder queryBuilder = new StringBuilder();
                    string[] fileNames = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToArray();
                    if (FolderName == "/ReadWriteData/Files/TECH")
                    {
                        if (Convert.ToInt32(Documentid) == 11)
                            fileNames = fileNames.Where(fileName => !fileName.Contains("_")).ToArray();
                        else
                            fileNames = fileNames.Where(fileName => fileName.Contains("_")).ToArray();
                    }
                    else if (FolderName == "/ReadWriteData/Files/Vendor/DOC")
                    {
                        if (Convert.ToInt32(DocumentcategoryID) == 4 && Convert.ToInt32(Documentid) == 45)
                        {
                            fileNames = fileNames.Where(fileName => fileName.Contains("_I")).ToArray();
                            otherdocumentname = "Inernal Records";
                        }
                        else if (Convert.ToInt32(DocumentcategoryID) == 4 && Convert.ToInt32(Documentid) == 47)
                        {
                            fileNames = fileNames.Where(fileName => fileName.Contains("_F")).ToArray();
                            otherdocumentname = "Firm Certificate(Like RDSO, Approval, Type test etc.)";
                        }
                        else if (Convert.ToInt32(DocumentcategoryID) == 4 && Convert.ToInt32(Documentid) == 48)
                        {
                            fileNames = fileNames.Where(fileName => fileName.Contains("_R")).ToArray();
                            otherdocumentname = "Raw Material/Invoice";
                        }
                        else if (Convert.ToInt32(DocumentcategoryID) == 4 && Convert.ToInt32(Documentid) == 49)
                        {
                            fileNames = fileNames.Where(fileName => fileName.Contains("_C")).ToArray();
                            otherdocumentname = "Calibration Records";
                        }
                    }
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        string fileName = fileNames[i];
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                        string extension = Path.GetExtension(fileName);
                        int indexOfHyphen = fileName.IndexOf('-');
                        if (FolderName == "/ReadWriteData/TESTPLAN")
                        {
                            if (indexOfHyphen != -1)
                            {
                                applicationid = fileName.Substring(0, indexOfHyphen);
                            }
                            otherdocumentname = "Upload TestPlan";
                        }
                        else if (FolderName == "/ReadWriteData/Files/Vendor/PO")
                        {
                            applicationid = fileNameWithoutExtension;
                            otherdocumentname = "To get the RITES \'Case No.\' Kindly E-mail a copy of Purchase Order in \'PDF\' format on the Email-Id mentioned above or Upload a scanned copy of Purchase Order in 'PDF\' format from here. Scanned copy should be in Black & White and Low DPI.";
                        }
                        else if (FolderName == "/ReadWriteData/Files/CONTRACTS")
                        {
                            applicationid = fileNameWithoutExtension;
                            otherdocumentname = "Contract Documents (If Any)";
                        }
                        else if (FolderName == "/ReadWriteData/Files/CASE_NO")
                        {
                            applicationid = fileNameWithoutExtension;
                        }
                        else if (FolderName == "/ReadWriteData/CALLS_DOCUMENTS")
                        {
                            applicationid = fileNameWithoutExtension;
                            otherdocumentname = "CallRegistrationDoc";
                        }
                        else if (FolderName == "/ReadWriteData/CALL_CANCELLATION_DOCUMENTS")
                        {
                            if (indexOfHyphen != -1)
                            {
                                applicationid = fileName.Substring(0, indexOfHyphen);
                            }
                            otherdocumentname = "Cancellation Document";
                        }
                        else if (FolderName == "/ReadWriteData/Files/TECH")
                        {
                            int indexOfHyphen1 = fileName.IndexOf('_');
                            if (Convert.ToInt32(Documentid) == 12)
                            {
                                if (indexOfHyphen1 != -1)
                                {
                                    applicationid = fileName.Substring(0, indexOfHyphen1);
                                    otherdocumentname = "Upload Tech Ref Reply";
                                }
                            }
                            else
                            {
                                applicationid = fileNameWithoutExtension;
                                otherdocumentname = "Upload Tech Ref";
                            }
                            
                        }
                        else if (FolderName == "/ReadWriteData/Files/VENDOR_CREATION_BASIS")
                        {
                            applicationid = fileNameWithoutExtension;
                            otherdocumentname = "Document on the basis of which vendor/manufacturer is created( IN PDF ONLY)";
                        }
                        else if (FolderName == "/ReadWriteData/Files/Vendor/DOC")
                        {
                            int indexOfHyphen1 = fileName.IndexOf('_');
                            applicationid = fileName.Substring(0, indexOfHyphen1);
                            
                        }
                        else if (FolderName == "/ReadWriteData/VENDOR/MA")
                        {
                            int indexOfHyphen1 = fileName.IndexOf('_');
                            if (indexOfHyphen1 != -1)
                            {
                                applicationid = fileName.Substring(0, indexOfHyphen1);
                            }
                            otherdocumentname = "Vendor MA Doc";
                        }
                        else if (FolderName == "/ReadWriteData/Files/Online_Complaints")
                        {
                            applicationid = fileNameWithoutExtension;
                            otherdocumentname = "Upload Rejection Memo";
                        }
                        queryBuilder.AppendLine($"INSERT INTO ibs_appdocument (applicationid,documentcategory,documentid,relativepath,fileid,extension,filedisplayname,isotherdoc,otherdocumentname,isdeleted,latitude,longitude,camera,phototakendate,maker,accuracy,isvideo, thumnailpath,thumnailfileid,thumnailextension,couchdbdocid)" +
                            $"VALUES('{applicationid}','{DocumentcategoryID}','{Documentid}','{FolderName}','{fileName}','{extension}','{fileNameWithoutExtension}','',{otherdocumentname},null,null,null,null,null,null,null,0,null,null,null,null);");
                    }

                    result = queryBuilder.ToString();
                }
                else
                {
                    return Ok("Path not found");
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Handle exceptions if needed
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}