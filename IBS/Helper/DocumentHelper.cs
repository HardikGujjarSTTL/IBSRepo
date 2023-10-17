
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Hosting;

namespace IBS.Helpers
{
    public class DocumentHelper
    {
        private readonly IDocument pIDocument;
        public DocumentHelper( IDocument _pIDocument)
        {
            pIDocument = _pIDocument;
        }

        public static int SaveFiles(string ApplicationID, List<APPDocumentDTO> DocumentsList, string FolderPath, IWebHostEnvironment env, IDocument pIDocument1, string FilePreFix = "", String SpecificFileName = "", int[] DocumentIds = null)
        {

            int id =0;
            List <APPDocumentDTO> NewDocumentsList = new List<APPDocumentDTO>();

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
                    item.UniqueFileName = SpecificFileName;
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
            id= pIDocument1.SaveDocument(NewDocumentsList, DocumentIds);
            return id;
        }

        public static int SaveICFiles(string ApplicationID, List<APPDocumentDTO> DocumentsList, string FolderPath, IWebHostEnvironment env, IDocument pIDocument1, string FilePreFix = "", String SpecificFileName = "", int[] DocumentIds = null)
        {

            int id = 0;
            int AppID = 0;
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
                string Filepath = "";
                string TempPath = Path.Combine(TempFilePath, item.UniqueFileName);

                if (!string.IsNullOrEmpty(FilePreFix) && !item.UniqueFileName.Contains(FilePreFix))
                {
                    if(item.DocName == "IC Image 1")
                    {
                        AppID = 1;
                        Filepath = FilePreFix + "_01.JPG";
                    }
                    if (item.DocName == "IC Image 2")
                    {
                        AppID = 2;
                        Filepath = FilePreFix + "_02.JPG";
                    }
                    if (item.DocName == "IC Image 3")
                    {
                        AppID = 3;
                        Filepath = FilePreFix + "_03.JPG";
                    }
                    if (item.DocName == "IC Image 4")
                    {
                        AppID = 4;
                        Filepath = FilePreFix + "_04.JPG";
                    }
                    if (item.DocName == "IC Image 5")
                    {
                        AppID = 5;
                        Filepath = FilePreFix + "_05.JPG";
                    }
                    if (item.DocName == "IC Image 6")
                    {
                        AppID = 6;
                        Filepath = FilePreFix + "_06.JPG";
                    }
                    if (item.DocName == "IC Image 7")
                    {
                        AppID = 7;
                        Filepath = FilePreFix + "_07.JPG";
                    }
                    if (item.DocName == "IC Image 8")
                    {
                        AppID = 8;
                        Filepath = FilePreFix + "_08.JPG";
                    }
                    if (item.DocName == "IC Image 9")
                    {
                        AppID = 9;
                        Filepath = FilePreFix + "_09.JPG";
                    }
                    if (item.DocName == "IC Image 10")
                    {
                        AppID = 10;
                        Filepath = FilePreFix + "_10.JPG";
                    }
                }

                if (SpecificFileName != "")
                    item.UniqueFileName = SpecificFileName;
                string DestinationPath = Path.Combine(path, Filepath);
                if (File.Exists(TempPath) && !File.Exists(DestinationPath))
                {
                    File.Copy(TempPath, DestinationPath, true);
                } 
                FileInfo newfile = new FileInfo(DestinationPath);

                item.Applicationid = ApplicationID + "_" + AppID;
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

        public void DeleteAllFiles(string ApplicationID)
        {
            pIDocument.DeleteAllFiles(ApplicationID);
        }

        //public static bool IsValidPdfSignature(List<APPDocumentDTO> DocumentsList, string Base64publickey, string ApplicationID, string FormatName, out string msg, string FunctionHead = "", string DemandNo = "", string SanctionedAmount = "", string ApprovalDate = "", string LLMCDate = "", string MeetingVenue = "", string LLMCStage = "", string ReleaseAmount = "", string WorkCompleted = "", string EligibleSubsidyRecommded = "", string Remarks = "", string NoOfLoomJIT = "", string DispatchNo = "")
        //{
        //    msg = "";
        //    string TempFilePath = HttpContext.Current.Server.MapPath(EnumUtils.GetEnumDescription(GSDMUTIL.Constants.Enumes.FolderPath.TempFilePath));

        //    foreach (APPDocumentDTO item in DocumentsList)
        //    {
        //        item.UniqueFileName = item.UniqueFileName.Replace("'", "");
        //        string pdfFilePath = Path.Combine(TempFilePath, item.UniqueFileName);

        //        string FolderPath = Path.Combine(HttpContext.Current.Server.MapPath("~" + item.RelativePath), item.UniqueFileName);
        //        string pdfPath = string.Empty;
        //        if (File.Exists(pdfFilePath))
        //            pdfPath = pdfFilePath;
        //        else
        //            pdfPath = FolderPath;

        //        if (PDFSigner.verifyPdfSignature(pdfPath, Base64publickey, ApplicationID, FormatName, out msg, FunctionHead, DemandNo, SanctionedAmount, ApprovalDate, LLMCDate, MeetingVenue, LLMCStage, ReleaseAmount, WorkCompleted, EligibleSubsidyRecommded, Remarks, NoOfLoomJIT, DispatchNo) == false) return false;
        //    }

        //    return true;
        //}
    }
}