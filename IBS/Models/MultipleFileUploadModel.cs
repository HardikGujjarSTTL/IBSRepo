using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class MultipleFileUploadModel
    {
        //public List<IFormFile> Files { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        public List<IFormFile> Files { get; set; }
    }
}
