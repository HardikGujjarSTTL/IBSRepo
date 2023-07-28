using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;

namespace IBS.Repositories
{
    public class Rly_Designation_Form : IRly_Designation_Form
    {
        private readonly ModelContext context;

        public Rly_Designation_Form(ModelContext context)
        {
            this.context = context;
        }
        public Rly_Designation_FormModel FindByID(string RlyDesigCd)
        {
            Rly_Designation_FormModel model = new();
            T90RlyDesignation role = context.T90RlyDesignations.Find(RlyDesigCd);

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.RlyDesigCd = role.RlyDesigCd;
                model.RlyDesigDesc = role.RlyDesigDesc;
                model.UserId = role.UserId;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                model.Updateddate= role.Updateddate;
                model.Datetime = role.Datetime;
                return model;
            }
        }

        public DTResult<Rly_Designation_FormModel> GetRly_Designation_FormList(DTParameters dtParameters)
        {

            DTResult<Rly_Designation_FormModel> dTResult = new() { draw = 0 };
            IQueryable<Rly_Designation_FormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "RlyDesigCd";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "RlyDesigCd";
                orderAscendingDirection = true;
            }
            query = from l in context.T90RlyDesignations
                    where l.Isdeleted == 0 || l.Isdeleted == null
                    select new Rly_Designation_FormModel
                    {
                        RlyDesigDesc = l.RlyDesigDesc,
                        RlyDesigCd = l.RlyDesigCd,
                        UserId = l.UserId,
                        Isdeleted = l.Isdeleted,
                        Createddate = l.Createddate,
                        Createdby = l.Createdby,
                        Updateddate = l.Updateddate,
                        Updatedby = l.Updatedby
                    };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.RlyDesigDesc).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.RlyDesigCd).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
        public bool Remove(string UomCd, int UserID)
        {
            var roles = context.T90RlyDesignations.Where(x => x.RlyDesigCd == UomCd).FirstOrDefault();
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

       
        public string Rly_Designation_FormInsertUpdate(Rly_Designation_FormModel model)
        {
            string RoleId = "";
            var RDF = context.T90RlyDesignations.Where(x => x.RlyDesigCd == model.RlyDesigCd).FirstOrDefault();
            #region Role save
            if (RDF == null)
            {
                T90RlyDesignation obj = new T90RlyDesignation();

                obj.RlyDesigCd = model.RlyDesigCd;
                obj.RlyDesigDesc = model.RlyDesigDesc;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T90RlyDesignations.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToString(obj.RlyDesigCd);
            }
            else
            {
                RDF.RlyDesigDesc = model.RlyDesigDesc;
                RDF.RlyDesigCd = model.RlyDesigCd;
                RDF.Updatedby = model.Updatedby;
                RDF.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToString(RDF.RlyDesigCd);
            }
            #endregion
            return RoleId;
        }
    }

}
    

