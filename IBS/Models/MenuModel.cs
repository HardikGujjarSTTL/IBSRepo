using IBS.DataAccess;

namespace IBS.Models
{
    public class MenuModel
    {
        public int Id { get; set; }

        public int? Parentid { get; set; }

        public string? Controllername { get; set; }

        public string? Actionname { get; set; }

        public string? Title { get; set; }

        public string? Menudescription { get; set; }

        public int? RoleId { get; set; }

        public string? Iconcssclass { get; set; }

        public bool? Isactive { get; set; }

        public int? Sortorder { get; set; }

        public string? Iconpath { get; set; }

        public virtual Role? Role { get; set; }

    }
    public class MenuListModel
    {
        public int ID { get; set; }
        public string? RootParentID { get; set; }
        public string? ParentID { get; set; }
        public string? ChildID { get; set; }
        public string? ParentTitle { get; set; }
        public string? ChildTitle { get; set; }
        public string? Selected { get; set; }
        public bool? Isadd { get; set; }

        public bool? Isedit { get; set; }

        public bool? Pisdelete { get; set; }

        public bool? Isview { get; set; }

        public string? controllername { get; set; }
        public string? actionname { get; set; }
        public string? title { get; set; }
        public string? menudescription { get; set; }
        public string? role_id { get; set; }
        public string? iconcssclass { get; set; }
        public string? isactive { get; set; }
        public string? sortorder { get; set; }
        public string? iconpath { get; set; }
        public string? isdisplay { get; set; }
    }
}
