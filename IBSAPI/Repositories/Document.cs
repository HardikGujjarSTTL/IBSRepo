using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using System;

namespace IBSAPI.Repositories
{
    public class Document : IDocument
    {
        private readonly ModelContext context;
        private readonly IConfiguration _config;
        public Document(ModelContext _context, IConfiguration configuration)
        {
            context = _context;
            _config = configuration;
        }

        public List<IBS_DocumentDTO> GetRecordsList(int DocumentCategoryID, string ApplicationID)
        {
            //List<IBS_DocumentDTO> MainList = new List<IBS_DocumentDTO>();
            int MaxContentLengthInKB = Convert.ToInt32(_config["MyAppSettings:MaxContentLengthInKB"]);
            var MainList = (from x in context.IbsDocuments.Where(e => e.Documentcategory == DocumentCategoryID)
                            join y in context.IbsAppdocuments.Where(e => e.Applicationid == ApplicationID && (e.Isdeleted != Convert.ToByte(true))) on x.Id equals y.Documentid into APPDocuments
                            from Y1 in APPDocuments.DefaultIfEmpty()
                            select new IBS_DocumentDTO
                            {
                                ID = x.Id,
                                DocumentName = x.Documentname,
                                DocumentCategory = x.Documentcategory,
                                AllowedFileExtensions = x.Allowedfileextensions,
                                //MaxContentLengthInKB = x.MaxContentLengthInKB,
                                MaxContentLengthInKB = x.Maxcontentlengthinkb.HasValue ? x.Maxcontentlengthinkb : MaxContentLengthInKB,
                                Ismandatory = x.Ismandatory,
                                IsVisible = x.Isvisible,
                                APPDocumentID = Y1.Id,
                                ApplicationID = Y1.Applicationid,
                                DocumentID = Y1.Documentid ?? 0,
                                RelativePath = Y1.Relativepath,
                                FileID = Y1.Fileid,
                                Extension = Y1.Extension,
                                FileDisplayName = Y1.Filedisplayname,
                                IsOtherDoc = Y1.Isotherdoc,
                                OtherDocumentName = Y1.Otherdocumentname,
                                VerifyDSC = x.Verifydsc,
                                IsDownloadTemplate = x.Isdownloadtemplate != null ? x.Isdownloadtemplate : Convert.ToByte(false),
                                CouchDBDocID = Y1.Couchdbdocid,
                            }).ToList();

            return MainList;

        }

        public int GetRecordsMaxID(int DocumentCategoryID)
        {
            string? id = (from x in context.IbsAppdocuments
                          where x.Documentcategory == Convert.ToInt32(DocumentCategoryID)
                          select x.Applicationid).Max();

            return Convert.ToInt32(id);
        }

        public int SaveDocument(List<APPDocumentDTO> objAPPDocumentDTO, int[] DocumentIds = null)
        {
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
                        Documentcategory = item.DocumentCategoryID
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
            List<IbsAppdocument> objGNR_APPDocument = (from c in context.IbsAppdocuments where c.Applicationid == Convert.ToString(ApplicationID) select c).ToList();

            foreach (var Docitem in objGNR_APPDocument)
            {
                Docitem.Isdeleted = Convert.ToByte(true);
            }
            context.SaveChanges();
        }

        public IBS_DocumentDTO FindRecord(int ID)
        {
            //ModelContext repository = new ModelContext();
            using ModelContext repository = new(DbContextHelper.GetDbContextOptions());
            IbsDocument objAdd = repository.IbsDocuments.SingleOrDefault(x => x.Id == ID);


            if (objAdd == null)
            {
                return new IBS_DocumentDTO();
            }
            return new IBS_DocumentDTO
            {
                ID = objAdd.Id,
                DocumentName = objAdd.Documentname,
                DocumentCategory = objAdd.Documentcategory,
                AllowedFileExtensions = objAdd.Allowedfileextensions,
                MaxContentLengthInKB = objAdd.Maxcontentlengthinkb,
            };
        }
    }
}
