    using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Composition;
using System;
using System.Data;
using System.Diagnostics.Contracts;

namespace IBS.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly ModelContext context;

        public ContractRepository(ModelContext context)
        {
            this.context = context;
        }
        public ContractModel FindByID(int ContractId)
        {
            ContractModel model = new();
            T57OngoingContract tenant = context.T57OngoingContracts.Find(Convert.ToInt32(ContractId));

            if (tenant == null)
                throw new Exception("Contract Record Not found");
            else
            {
                model.ContractId = tenant.ContractId;
                model.ContractNo = tenant.ContractNo;
                model.ContractPanalty = tenant.ContractPanalty;
                model.ContractCm = tenant.ContractCm;
                model.ExpOr =  tenant.ExpOr;
                model.ClientName = tenant.ClientName;
                model.ContInspFee = tenant.ContInspFee;
                model.ContPerFrom = Convert.ToDateTime(tenant.ContPerFrom);
                model.Status =  tenant.Status;
                model.ContractSpecialCondn = tenant.ContractSpecialCondn;
                model.Datetime = tenant.Datetime;
                model.OfferDt = tenant.OfferDt;
                model.ContPerTo = tenant.ContPerTo;
                model.RegionCode = tenant.RegionCode;
                model.ContractFee = tenant.ContractFee;
                model.ContractFeeNum = tenant.ContractFeeNum;
                model.ScopeOfWork = tenant.ScopeOfWork;
                model.UserId= tenant.UserId;
                model.ContSignDt = tenant.ContSignDt;
                //model.Isdeleted = tenant.Isdeleted;
                //model.Createddate = tenant.Createddate;
                //model.Createdby = tenant.Createdby;
                //model.Updateddate = tenant.Updateddate;
                //model.Updatedby = tenant.Updatedby;
                return model;
            }
        }

        public DTResult<ContractModel> GetContractList(DTParameters dtParameters)
        { 
            DTResult<ContractModel> dTResult = new() { draw = 0 };
            IQueryable<ContractModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "ClientName";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "ClientName";
                orderAscendingDirection = true;
            }
            query = from l in context.T57OngoingContracts
                    //where l.Isdeleted == 0 
                    select new ContractModel
                    {
                        ContractId = l.ContractId,                         
                        ContractNo = l.ContractNo,
                        ContractPanalty = l.ContractPanalty,
                        ContractCm = l.ContractCm,
                        ExpOr = l.ExpOr,
                        ClientName = l.ClientName,
                        ContInspFee = l.ContInspFee,
                        ContPerFrom = Convert.ToDateTime(l.ContPerFrom),
                        Status = l.Status,
                        ContractSpecialCondn = l.ContractSpecialCondn,
                        Datetime = Convert.ToDateTime(l.Datetime),
                        OfferDt = Convert.ToDateTime(l.OfferDt),
                        ContPerTo = Convert.ToDateTime(l.ContPerTo),
                        RegionCode = l.RegionCode,
                        ContractFee = l.ContractFee,
                        ContractFeeNum = l.ContractFeeNum,
                        ScopeOfWork = l.ScopeOfWork,
                        UserId = l.UserId,
                        ContSignDt = Convert.ToDateTime(l.ContSignDt),                        
                        Isdeleted = l.Isdeleted,
                        Createdby = l.Createdby,
                        Createddate = l.Createddate,
                        Updatedby= l.Updatedby,
                        Updateddate= l.Updateddate,
            };

            dTResult.recordsTotal = query.Count();

            //if (!string.IsNullOrEmpty(searchBy))
            //    query = query.Where(w => Convert.ToString(w.Contractname).ToLower().Contains(searchBy.ToLower())
            //    || Convert.ToString(w.Contractdescription).ToLower().Contains(searchBy.ToLower())
            //    );

            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public bool Remove(int ContractId, int UserID)
        {
            var _contracts = context.T57OngoingContracts.Find(Convert.ToInt32(ContractId));
            if (_contracts == null) { return false; }

            _contracts.Isdeleted = Convert.ToByte(true);
            _contracts.Updatedby = Convert.ToInt32(UserID);
            _contracts.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int ContractDetailsInsertUpdate(ContractModel model)
        {
            int ContractId = 0; 
            var _contract = context.T57OngoingContracts.Find(model.ContractId);
            #region Contract save
            if (_contract == null)
            {
                T57OngoingContract obj = new T57OngoingContract();
                obj.ContractNo = model.ContractNo;
                obj.ContractPanalty = model.ContractPanalty;
                obj.ContractCm = model.ContractCm;
                obj.ExpOr = model.ExpOr;
                obj.ClientName = model.ClientName;
                obj.ContInspFee = model.ContInspFee;
                obj.ContPerFrom = Convert.ToDateTime(model.ContPerFrom);
                obj.Status = model.Status;
                obj.ContractSpecialCondn = model.ContractSpecialCondn;
                obj.Datetime = model.Datetime;
                obj.OfferDt = model.OfferDt;
                obj.ContPerTo = model.ContPerTo;
                obj.RegionCode = model.RegionCode;
                obj.ContractFee = model.ContractFee;
                obj.ContractFeeNum = model.ContractFeeNum;
                obj.ScopeOfWork = model.ScopeOfWork;
                obj.UserId = model.UserId;
                obj.ContSignDt = model.ContSignDt;
                 
                //obj.Contractname = model.Contractname;
                //obj.Contractdescription = model.Contractdescription;
                //obj.Issysadmin = Convert.ToByte(model.Issysadmin);
                //obj.Isactive = Convert.ToByte(model.Isactive);
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = Convert.ToInt32(model.UserId);
                obj.Createddate = DateTime.Now;
                obj.Updatedby = Convert.ToInt32(model.UserId);
                obj.Updateddate = DateTime.Now;
                context.T57OngoingContracts.Add(obj);
                context.SaveChanges();
                ContractId = Convert.ToInt32(obj.ContractId);
            }
            else
            {   
               _contract.ContractNo = model.ContractNo;
               _contract.ContractPanalty = model.ContractPanalty;
               _contract.ContractCm = model.ContractCm;
               _contract.ExpOr = model.ExpOr;
               _contract.ClientName = model.ClientName;
               _contract.ContInspFee = model.ContInspFee;
               _contract.ContPerFrom = Convert.ToDateTime(model.ContPerFrom);
               _contract.Status = model.Status;
               _contract.ContractSpecialCondn = model.ContractSpecialCondn;
               _contract.Datetime = model.Datetime;
               _contract.OfferDt = model.OfferDt;
               _contract.ContPerTo = model.ContPerTo;
               _contract.RegionCode = model.RegionCode;
               _contract.ContractFee = model.ContractFee;
               _contract.ContractFeeNum = model.ContractFeeNum;
               _contract.ScopeOfWork = model.ScopeOfWork;
               _contract.UserId = model.UserId;
               _contract.ContSignDt = model.ContSignDt;
                ContractId = Convert.ToInt32(_contract.ContractId);
                context.SaveChanges();
            }
            #endregion
            return ContractId;
        }
    }

}
