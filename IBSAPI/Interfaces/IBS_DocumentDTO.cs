using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IBSAPI.Models
{
    public class IBS_DocumentDTO
    {
        public int ID { get; set; }
        public string DocumentName { get; set; }
        public int? DocumentCategory { get; set; }
        public string AllowedFileExtensions { get; set; }
        public int? MaxContentLengthInKB { get; set; }
        public byte Ismandatory { get; set; }
        public byte IsVisible { get; set; }
        public long? APPDocumentID { get; set; }
        public string? ApplicationID { get; set; }
        public int? DocumentID { get; set; }
        public string RelativePath { get; set; }
        public string FileID { get; set; }
        public string Extension { get; set; }
        public string FileDisplayName { get; set; }
        public byte? IsOtherDoc { get; set; }
        public string OtherDocumentName { get; set; }
        public byte VerifyDSC { get; set; }
        public int FYID { get; set; }
        public byte? IsDownloadTemplate { get; set; }
        public byte? IsVideo { get; set; }
        public string ThumnailPath { get; set; }
        public string ThumnailFileID { get; set; }
        public string ThumnailExtension { get; set; }
        public string CouchDBDocID { get; set; }
        public string CouchDBFileBinary { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PhoneTakenDate { get; set; }

        public System.String TrimmedDocName
        {
            get
            {
                string DocName = IsOtherDoc == Convert.ToByte(true) ? OtherDocumentName : ID.ToString();
                return Regex.Replace(DocName, @"(\s+|@|&|'|\(|\)|<|>|#| |,|\/|%|-|^|\.|:)", "");
            }
        }

        public System.String UploaderIDName
        {
            get
            {
                return TrimmedDocName.Length > 30 ? TrimmedDocName.Substring(0, 30) : TrimmedDocName;
            }
        }
    }
}