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
    public class ClientContractRepository : IClientContractRepository
    {
        private readonly ModelContext context;

        public ClientContractRepository(ModelContext context)
        {
            this.context = context;
        }
        public ClientContractModel FindByID(int ContractId)
        {
            ClientContractModel model = new();
            T58ClientContact tenant = context.T58ClientContacts.Find(Convert.ToInt32(ContractId));

            if (tenant == null)
                throw new Exception("Client Contract Record Not found");
            else
            {
                model.VisitDt = tenant.VisitDt;
                model.ClientOfficerName = tenant.ClientOfficerName;
                model.Designation = tenant.Designation;
                model.ClientType = tenant.ClientType;
                model.Client =  tenant.Client;
                //model.RitesOfficerCd = tenant.RitesOfficerCd;
                model.Highlights = tenant.Highlights;
                model.OverallOutcome = tenant.OverallOutcome;
                model.RegionCd =  tenant.RegionCd;
                model.UserId = tenant.UserId;
                model.Datetime = tenant.Datetime;
                model.TypeCb = tenant.TypeCb;
                model.OutAmt = tenant.OutAmt;
                //model.Isdeleted = tenant.Isdeleted;
                //model.Createddate = tenant.Createddate;
                //model.Createdby = tenant.Createdby;
                //model.Updateddate = tenant.Updateddate;
                //model.Updatedby = tenant.Updatedby;
                return model;
            }
        }

        public DTResult<ClientContractModel> GetClientContractList(DTParameters dtParameters)
        { 
            DTResult<ClientContractModel> dTResult = new() { draw = 0 };
            IQueryable<ClientContractModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "Id";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                //if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }
            query = from l in context.T58ClientContacts
                    //where l.Isdeleted == 0 
                    select new ClientContractModel
                    {
                        Id = Convert.ToInt32(l.Id),
                        VisitDt = Convert.ToDateTime(l.VisitDt),
                        ClientOfficerName = l.ClientOfficerName,
                        Designation = l.Designation,
                        ClientType = l.ClientType,
                        Client = l.Client,
                        //RitesOfficerCd = l.RitesOfficerCd,
                        Highlights = l.Highlights,
                        OverallOutcome =  l.OverallOutcome,
                        RegionCd = l.RegionCd,
                        //UserId = l.UserId,
                        Datetime = Convert.ToDateTime(l.Datetime),
                        TypeCb = l.TypeCb,
                        OutAmt = l.OutAmt,                                                
                        //Isdeleted = l.Isdeleted,
                        //Createdby = l.Createdby,
                        //Createddate = l.Createddate,
                        //Updatedby= l.Updatedby,
                        //Updateddate= l.Updateddate,
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
            var _contracts = context.T58ClientContacts.Find(Convert.ToInt32(ContractId));
            if (_contracts == null) { return false; }

             _contracts.Isdeleted = Convert.ToByte(true);
            //_contracts.Updatedby = Convert.ToInt32(UserID);
            //_contracts.Updateddate = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public int ClientContractDetailsInsertUpdate(ClientContractModel model)
        {
            int ContractId = 0;
            var _contract = context.T58ClientContacts.Find(model.Id);
            #region Contract save
            if (_contract == null)
            {
                T58ClientContact obj = new T58ClientContact();
                obj.VisitDt = Convert.ToDateTime(model.VisitDt.ToString());
                obj.ClientOfficerName = model.ClientOfficerName;
                obj.Designation = model.Designation;
                obj.ClientType = model.ClientType;
                obj.Client = model.Client;
                obj.RitesOfficerCd = model.RitesOfficerCd;
                obj.Highlights = model.Highlights;
                obj.OverallOutcome = model.OverallOutcome;
                obj.RegionCd = model.RegionCd;
                obj.UserId = model.UserId;
                obj.Datetime = model.Datetime;
                obj.TypeCb = "C";                
                obj.OutAmt = model.OutAmt;                  
                //obj.Isdeleted = Convert.ToByte(false);
                //obj.Createdby = Convert.ToInt32(model.UserId);
                //obj.Createddate = DateTime.Now;
                //obj.Updatedby = Convert.ToInt32(model.UserId);
                //obj.Updateddate = DateTime.Now;
                context.T58ClientContacts.Add(obj);
                context.SaveChanges();
                ContractId = Convert.ToInt32(obj.Id);
            }
            else
            {  
               _contract.VisitDt = Convert.ToDateTime(model.VisitDt.ToString());
               _contract.ClientOfficerName = model.ClientOfficerName;
               _contract.Designation = model.Designation;
               _contract.ClientType = model.ClientType;
               _contract.Client = model.Client;
               _contract.RitesOfficerCd = model.RitesOfficerCd;
               _contract.Highlights = model.Highlights;
               _contract.OverallOutcome = model.OverallOutcome;
               _contract.RegionCd = model.RegionCd;
                _contract.UserId = model.UserId;
                _contract.Datetime = model.Datetime;
               _contract.TypeCb = "C";
                _contract.OutAmt = model.OutAmt;                 
                ContractId = Convert.ToInt32(_contract.Id);
                context.SaveChanges();
            }
            #endregion
            return ContractId;
        }
    }

}
