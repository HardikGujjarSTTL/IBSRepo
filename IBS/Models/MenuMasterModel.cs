namespace IBS.Models
{
    public class MenuMasterModel
    {
        public int MenuId { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int SortOrder { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string IconPath { get; set; }
        public int Role_Id { get; set; }
        public bool? AddAccess { get; set; }
        public bool? EditAccess { get; set; }
        public bool? DeleteAccess { get; set; }
        public bool? ViewAccess { get; set; }
    }
}
