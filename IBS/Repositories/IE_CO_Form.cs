using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;

namespace IBS.Repositories
{
    public class IE_CO_Form : I_IE_CO_Form
    {
       
        private readonly ModelContext context;

            public IE_CO_Form(ModelContext context)
            {
                this.context = context;
            }
        public IE_CO_FormModel FindByID(int CoCd)
        {
            IE_CO_FormModel model = new();
            T08IeControllOfficer role = context.T08IeControllOfficers.Find(Convert.ToByte(CoCd));

            if (role == null)
                throw new Exception("Role Record Not found");
            else
            {
                model.CoCd = role.CoCd;
                model.CoName = role.CoName;
                model.CoDesig = role.CoDesig;
                model.CoPhoneNo = role.CoPhoneNo;
                model.CoEmail = role.CoEmail;
                model.Updatedby = role.Updatedby;
                model.Createdby = role.Createdby;
                model.Createddate = model.Createddate;
                model.Isdeleted = role.Isdeleted;
                return model;
            }
        }

        public DTResult<IE_CO_FormModel> GetCOList(DTParameters dtParameters)
            {

                DTResult<IE_CO_FormModel> dTResult = new() { draw = 0 };
                IQueryable<IE_CO_FormModel>? query = null;

                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                    if (orderCriteria == "")
                    {
                        orderCriteria = "CoCd";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                }
                else
                {
                    // if we have an empty search then just order the results by Id ascending
                    orderCriteria = "CoCd";
                    orderAscendingDirection = true;
                }
                query = from l in context.T08IeControllOfficers
                        where l.Isdeleted == 0 || l.Isdeleted == null
                        select new IE_CO_FormModel
                        {
                            CoCd = l.CoCd,
                            CoName = l.CoName,
                            CoDesig = l.CoDesig,
                            CoPhoneNo = l.CoPhoneNo,
                            CoEmail = l.CoEmail,
                            CoType = l.CoType,
                            Isdeleted = l.Isdeleted,
                            Createddate = l.Createddate,
                            Createdby = l.Createdby,
                            Updateddate = l.Updateddate,
                            Updatedby = l.Updatedby
                        };

                dTResult.recordsTotal = query.Count();

                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.CoName).ToLower().Contains(searchBy.ToLower())
                    || Convert.ToString(w.CoEmail).ToLower().Contains(searchBy.ToLower())
                    );

                dTResult.recordsFiltered = query.Count();

                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                dTResult.draw = dtParameters.Draw;

                return dTResult;
            }
        public bool Remove(int CoCd, int UserID)
        {
            var roles = context.T08IeControllOfficers.Find(Convert.ToByte(CoCd));
            if (roles == null) { return false; }

            roles.Isdeleted = Convert.ToByte(true);
            roles.Updatedby = Convert.ToInt32(UserID);
            roles.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int CODetailsInsertUpdate(IE_CO_FormModel model)
        {
            int RoleId = 0;
            var CO = context.T08IeControllOfficers.Where(x => x.CoCd == model.CoCd).FirstOrDefault();
            #region Role save
            if (CO == null || CO.CoCd == 0)
            {
                T08IeControllOfficer obj = new T08IeControllOfficer();
                obj.CoName = model.CoName;
                obj.CoDesig = model.CoDesig;
                obj.CoEmail = model.CoEmail;
                obj.CoPhoneNo = model.CoPhoneNo;
                obj.CoType = model.CoType;
                obj.Createdby = model.Createdby;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createddate = DateTime.Now;
                context.T08IeControllOfficers.Add(obj);
                context.SaveChanges();
                RoleId = Convert.ToInt32(obj.CoCd);
            }
            else
            {
                CO.CoName = model.CoName;
                CO.CoDesig = model.CoDesig;
                CO.CoStatus = model.CoStatus;
                CO.CoEmail = model.CoEmail;
                CO.CoPhoneNo = model.CoPhoneNo;
                CO.Updatedby = model.Updatedby;
                CO.Updateddate = DateTime.Now;
                context.SaveChanges();
                RoleId = Convert.ToInt32(CO.CoCd);
            }
            #endregion
            return RoleId;
        }
    }

    }
