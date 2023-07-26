using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IBS.Repositories
{
	public class UnitOfMeasurements : IUnitOfMeasurements
	{
		private readonly ModelContext context;

		public UnitOfMeasurements(ModelContext context)
		{
			this.context = context;
		}
		public UOMModel FindByID(int UomCd)
		{
			UOMModel model = new();
            T04Uom role = context.T04Uoms.Find(Convert.ToByte(UomCd));

			if (role == null)
				throw new Exception("Role Record Not found");
			else
			{
                model.UomSDesc = role.UomSDesc;
                model.UomLDesc = role.UomLDesc;
                model.UomFactor = role.UomFactor;
                model.UomCd = role.UomCd;
                model.UserId = role.UserId;
				model.Updatedby = role.Updatedby;
				model.Createdby = role.Createdby;
				model.Createddate = model.Createddate;
				model.Isdeleted = role.Isdeleted;
				return model;
			}
		}

		public DTResult<UOMModel> GetUOMList(DTParameters dtParameters)
		{

			DTResult<UOMModel> dTResult = new() { draw = 0 };
			IQueryable<UOMModel>? query = null;

			var searchBy = dtParameters.Search?.Value;
			var orderCriteria = string.Empty;
			var orderAscendingDirection = true;

			if (dtParameters.Order != null)
			{
				// in this example we just default sort on the 1st column
				orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

				if (orderCriteria == "")
				{
					orderCriteria = "UomCd";
				}
				orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
			}
			else
			{
				// if we have an empty search then just order the results by Id ascending
				orderCriteria = "UomCd";
				orderAscendingDirection = true;
			}
			query = from l in context.T04Uoms
					where l.Isdeleted == 0 || l.Isdeleted == null
					select new UOMModel
					{
						UomSDesc= l.UomSDesc,
						UomFactor = l.UomFactor,
						ImmsUomCd=l.ImmsUomCd,
						UomCd = l.UomCd,
						UomLDesc=l.UomLDesc,
						UserId = l.UserId,
						Isdeleted = l.Isdeleted,
						Createddate = l.Createddate,
						Createdby = l.Createdby,
						Updateddate = l.Updateddate,
						Updatedby = l.Updatedby
					};

			dTResult.recordsTotal = query.Count();

			 if (!string.IsNullOrEmpty(searchBy))
				query = query.Where(w => Convert.ToString(w.UomSDesc).ToLower().Contains(searchBy.ToLower())
				|| Convert.ToString(w.UomFactor).ToLower().Contains(searchBy.ToLower())
				);

			dTResult.recordsFiltered = query.Count();

			dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

			dTResult.draw = dtParameters.Draw;

			return dTResult;
		}
		public bool Remove(int UomCd, int UserID)
		{
			var roles = context.T04Uoms.Find(Convert.ToByte(UomCd));
			if (roles == null) { return false; }

			roles.Isdeleted = Convert.ToByte(true);
			roles.Updatedby = Convert.ToInt32(UserID);
			roles.Updateddate = DateTime.Now;
			context.SaveChanges();
			return true;
		}

		public int UOMDetailsInsertUpdate(UOMModel model)
		{
			int RoleId = 0;
			var UOM = context.T04Uoms.Where(x=>x.UomCd == model.UomCd).FirstOrDefault();
			#region Role save
			if (UOM == null || UOM.UomCd == 0)
			{
                T04Uom obj = new T04Uom();
                obj.UomSDesc = model.UomSDesc;
                obj.UomLDesc = model.UomLDesc;
                obj.UomFactor = model.UomFactor;
                obj.Createdby = model.Createdby;
				obj.Isdeleted = Convert.ToByte(false);
				obj.Createddate = DateTime.Now;
				context.T04Uoms.Add(obj);
				context.SaveChanges();
				RoleId = Convert.ToInt32(obj.UomCd);
			}
			else
			{
                UOM.UomSDesc = model.UomSDesc;
                UOM.UomLDesc = model.UomLDesc;
                UOM.UomFactor = model.UomFactor;
                UOM.Updatedby = model.Updatedby;
                UOM.Updateddate = DateTime.Now;
				context.SaveChanges();
				RoleId = Convert.ToInt32(UOM.UomCd);
			}
			#endregion
			return RoleId;
		}
	}

}
