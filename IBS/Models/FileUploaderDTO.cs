using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBS.Models
{
    public class FileUploaderDTO
    {
        public List<IBS_DocumentDTO> IBS_DocumentList { get; set; }
        public List<IBS_DocumentDTO> OtherDocumentList { get; set; }
        public System.Boolean OthersSection { get; set; }
        public System.Int32 MaxUploaderinOthers { get; set; }
        public System.Int32 Mode { get; set; }
        public string MainTitle { get; set; }

        public string HideDocumentIds { get; set; }
        public System.Boolean isDSC { get; set; }
        public int FilUploadMode { get; set; }
    }
}
