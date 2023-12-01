
using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Hosting;
using IBSAPI.DataAccess;

namespace IBSAPI.Helpers
{
    public class DocumentHelper
    {
        
        private readonly IDocument pIDocument;
        public DocumentHelper(IDocument _pIDocument)
        {
            pIDocument = _pIDocument;
        }

        public static int SaveFiles(string ApplicationID, List<APPDocumentDTO> DocumentsList, string FolderPath, IWebHostEnvironment env, IDocument pIDocument1, string FilePreFix = "", String SpecificFileName = "", int[] DocumentIds = null)
        {

            int id = 0;
            List<APPDocumentDTO> NewDocumentsList = new List<APPDocumentDTO>();

            //string path = Path.Combine(env.WebRootPath, FolderPath);
            string path = env.WebRootPath + FolderPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string TempFilePath = env.WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.TempFilePath);

            foreach (APPDocumentDTO item in DocumentsList)
            {
                item.UniqueFileName = item.UniqueFileName.Replace("'", "");
                string TempPath = Path.Combine(TempFilePath, item.UniqueFileName);

                if (!string.IsNullOrEmpty(FilePreFix) && !item.UniqueFileName.Contains(FilePreFix))
                {
                    item.UniqueFileName = FilePreFix + "_" + item.UniqueFileName;
                }

                if (SpecificFileName != "")
                {
                    string fileExtension = Path.GetExtension(item.UniqueFileName);
                    item.UniqueFileName = SpecificFileName + fileExtension;
                    //TempPath = Path.Combine(TempFilePath, item.UniqueFileName);
                }

                string DestinationPath = Path.Combine(path, item.UniqueFileName);
                if (File.Exists(TempPath) && !File.Exists(DestinationPath))
                {
                    File.Copy(TempPath, DestinationPath, true);
                }
                FileInfo newfile = new FileInfo(DestinationPath);

                item.Applicationid = ApplicationID;
                item.Relativepath = FolderPath.Replace("~", "");
                item.Extension = newfile.Extension;
                item.FileDisplayName = item.FileName;

                if (item.Documentid == null || item.Documentid == 0)
                {
                    item.Documentid = null;
                    item.Isotherdoc = Convert.ToByte(true);
                }

                NewDocumentsList.Add(item);
            }
            id = pIDocument1.SaveDocument(NewDocumentsList, DocumentIds);
            return id;
        }

        public static int SaveICFiles(string ApplicationID, List<APPDocumentDTO> DocumentsList, string FolderPath, IWebHostEnvironment env, IDocument pIDocument1, string FilePreFix = "", String SpecificFileName = "", int DocumentIds = 0,string IsStaging = null)
        {
            int id = 0;
            int AppID = 0;
            int Documentid = 0;
            List<APPDocumentDTO> NewDocumentsList = new List<APPDocumentDTO>();

            //string path = Path.Combine(env.WebRootPath, FolderPath);
            string WebRootPath = "";
            if (Convert.ToBoolean(IsStaging) == true)
            {
                WebRootPath = env.WebRootPath.Replace("IBS2API", "IBS2");
                //WebRootPath = env.WebRootPath.Replace("IBSAPI", "IBS");
            }
            else
            {
                WebRootPath = env.WebRootPath;
            }
            string path = WebRootPath + FolderPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string TempFilePath = WebRootPath + Enums.GetEnumDescription(Enums.FolderPath.TempFilePath);

            foreach (APPDocumentDTO item in DocumentsList)
            {

                item.UniqueFileName = item.UniqueFileName.Replace("'", "");
                string Filepath = "";
                string TempPath = Path.Combine(TempFilePath, item.UniqueFileName);

                if (!string.IsNullOrEmpty(FilePreFix) && !item.UniqueFileName.Contains(FilePreFix))
                {
                    if (item.DocName == "IC Image 1")
                    {
                        AppID = 1;
                        Filepath = FilePreFix + "_01.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload1;
                    }
                    if (item.DocName == "IC Image 2")
                    {
                        AppID = 2;
                        Filepath = FilePreFix + "_02.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload2;
                    }
                    if (item.DocName == "IC Image 3")
                    {
                        AppID = 3;
                        Filepath = FilePreFix + "_03.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload3;
                    }
                    if (item.DocName == "IC Image 4")
                    {
                        AppID = 4;
                        Filepath = FilePreFix + "_04.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload4;
                    }
                    if (item.DocName == "IC Image 5")
                    {
                        AppID = 5;
                        Filepath = FilePreFix + "_05.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload5;
                    }
                    if (item.DocName == "IC Image 6")
                    {
                        AppID = 6;
                        Filepath = FilePreFix + "_06.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload6;
                    }
                    if (item.DocName == "IC Image 7")
                    {
                        AppID = 7;
                        Filepath = FilePreFix + "_07.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload7;
                    }
                    if (item.DocName == "IC Image 8")
                    {
                        AppID = 8;
                        Filepath = FilePreFix + "_08.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload8;
                    }
                    if (item.DocName == "IC Image 9")
                    {
                        AppID = 9;
                        Filepath = FilePreFix + "_09.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload9;
                    }
                    if (item.DocName == "IC Image 10")
                    {
                        AppID = 10;
                        Filepath = FilePreFix + "_10.JPG";
                        Documentid = (int)Enums.DocumentCategory_CANRegisrtation.IC_Photos_Upload10;
                    }
                }

                string DestinationPath = Path.Combine(path, Filepath);
                using (var fileStream = System.IO.File.Create(DestinationPath))
                {
                    item.formFile.CopyTo(fileStream);
                }
                //if (File.Exists(TempPath) && !File.Exists(DestinationPath))
                //{
                //    File.Copy(TempPath, DestinationPath, true);
                //}

                FileInfo newfile = new FileInfo(DestinationPath);
                item.DocumentCategoryID = DocumentIds;
                item.Documentid = Documentid;
                item.Applicationid = ApplicationID + "_" + AppID;
                item.Relativepath = FolderPath.Replace("~", "");
                item.Extension = newfile.Extension;
                item.FileDisplayName = item.FileName;
                item.UniqueFileName = Filepath;

                if (item.Documentid == null || item.Documentid == 0)
                {
                    item.Documentid = null;
                    item.Isotherdoc = Convert.ToByte(true);
                }

                item.Latitude = item.Latitude;
                item.Longitude = item.Longitude;
                NewDocumentsList.Add(item);
            }
            id = SaveDocument(NewDocumentsList);
            return id;
        }

        public static int SaveDocument(List<APPDocumentDTO> objAPPDocumentDTO)
        {
            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            List<IbsAppdocument> objSaveData = new List<IbsAppdocument>();
            if (objAPPDocumentDTO.Count > 0)
            {
                foreach (var item in objAPPDocumentDTO)
                {
                    List<IbsAppdocument> objGNR_APPDocument = (from c in context.IbsAppdocuments
                                                               where c.Applicationid == Convert.ToString(item.Applicationid)
                                                               && c.Documentcategory == item.DocumentCategoryID
                                                               select c).ToList();
                    foreach (var Docitem in objGNR_APPDocument)
                    {
                        Docitem.Isdeleted = Convert.ToByte(true);
                    }
                    context.SaveChanges();

                    objSaveData.Add(new IbsAppdocument
                    {
                        Applicationid = Convert.ToString(item.Applicationid),
                        Documentid = item.Documentid,
                        Relativepath = item.Relativepath,
                        Fileid = item.UniqueFileName,
                        Extension = item.Extension,
                        Filedisplayname = item.FileDisplayName,
                        Isotherdoc = item.Isotherdoc,
                        Otherdocumentname = item.DocName,
                        Documentcategory = item.DocumentCategoryID,
                        Latitude = item.Latitude,
                        Longitude = item.Longitude
                    });
                }
                context.IbsAppdocuments.AddRange(objSaveData);
                context.SaveChanges();
                return objSaveData.FirstOrDefault().Id;
            }
            else
            {
                return 0;
            }

        }

        public void DeleteAllFiles(string ApplicationID)
        {
            pIDocument.DeleteAllFiles(ApplicationID);
        }
    }
}