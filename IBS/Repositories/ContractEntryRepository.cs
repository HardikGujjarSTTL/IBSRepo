using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class ContractEntryRepository : IContractEntryRepository
    {
        private readonly ModelContext context;

        public ContractEntryRepository(ModelContext context)
        {
            this.context = context;
        }

        public int ContractDetailsInsertUpdate(ContractEntry model)
        {
            int ContractId = 0;
            int maxID = 0;

            if (!string.IsNullOrEmpty(model.CLIENTNAME))
            {
                var Client = model.CLIENTNAME.Split("=");
                model.CLIENTCODE = Client[0].Trim();
                model.CLIENTNAME = Client[1].Trim();
            }
            var Contract = (from r in context.T100Contracts where r.Id == Convert.ToInt32(model.ID) select r).FirstOrDefault();
            #region Contract save
            if (Contract == null)
            {
                if (context.T100Contracts.Any())
                {
                    maxID = context.T100Contracts.Max(x => x.Id) + 1;
                }
                else
                {
                    maxID = 1;
                }
                T100Contract obj = new T100Contract();
                obj.Id = maxID;
                obj.LetterNo = model.LETTER_NO;
                obj.LetterDate = model.LETTER_DATE;
                obj.Tpfrom = model.TPFROM;
                obj.Tpto = model.TPTO;
                obj.Clienttype = model.CLIENTTYPE;
                obj.Clientname = model.CLIENTNAME;
                obj.Inspfee = model.INSPFEE;
                obj.Mandaybasis = model.MANDAYBASIS;
                obj.Lotofinsp = model.LOTOFINSP;
                obj.Materialvalue = model.MATERIALVALUE;
                obj.Materialdescription = model.Materialdescription;
                obj.Minpoval = model.MINPOVAL;
                obj.Maxpoval = model.MAXPOVAL;
                obj.Callcancelation = model.CALLCANCELATION;
                obj.Isdeleted = Convert.ToByte(false);
                obj.Createdby = model.CreatedBy;
                obj.Createddate = DateTime.Now;
                obj.Clientcode = model.CLIENTCODE;
                context.T100Contracts.Add(obj);
                context.SaveChanges();
                ContractId = Convert.ToInt32(obj.Id);
            }
            else
            {
                Contract.LetterNo = model.LETTER_NO;
                Contract.LetterDate = model.LETTER_DATE;
                Contract.Tpfrom = model.TPFROM;
                Contract.Tpto = model.TPTO;
                Contract.Clienttype = model.CLIENTTYPE;
                Contract.Clientname = model.CLIENTNAME;
                Contract.Inspfee = model.INSPFEE;
                Contract.Mandaybasis = model.MANDAYBASIS;
                Contract.Lotofinsp = model.LOTOFINSP;
                Contract.Materialvalue = model.MATERIALVALUE;
                Contract.Materialdescription = model.Materialdescription;
                Contract.Minpoval = model.MINPOVAL;
                Contract.Maxpoval = model.MAXPOVAL;
                Contract.Callcancelation = model.CALLCANCELATION;
                Contract.Isdeleted = Convert.ToByte(false);
                Contract.Updatedby = model.UpdatedBy;
                Contract.Updatedate = DateTime.Now;
                Contract.Clientcode = model.CLIENTCODE;
                context.SaveChanges();
                ContractId = Convert.ToInt32(Contract.Id);
            }
            #endregion
            return ContractId;
        }

        public DTResult<ContractEntry> GetContractList(DTParameters dtParameters)
        {
            DTResult<ContractEntry> dTResult = new() { draw = 0 };
            IQueryable<ContractEntry>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CLIENTNAME";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "CLIENTNAME";
                orderAscendingDirection = true;
            }

            string clientname = "", Clienttype = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ClientName"]))
            {
                clientname = Convert.ToString(dtParameters.AdditionalValues["ClientName"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Clienttype"]))
            {
                Clienttype = Convert.ToString(dtParameters.AdditionalValues["Clienttype"]);
            }

            DataTable dt = new DataTable();
            DataSet ds;

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_clientname", OracleDbType.Varchar2, clientname, ParameterDirection.Input);
            par[1] = new OracleParameter("p_clienttype", OracleDbType.Varchar2, Clienttype, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GetContractInfo", par, 1);
            dt = ds.Tables[0];
            List<ContractEntry> list = null;

            if (ds != null && ds.Tables.Count > 0)
            {
                list = dt.AsEnumerable().Select(row => new ContractEntry
                {
                    LETTER_NO = row["LETTER_NO"] != DBNull.Value ? Convert.ToString(row["LETTER_NO"]) : "",
                    ID = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0,
                    LETTER_DATE = row["LETTER_DATE"] != DBNull.Value ? Convert.ToDateTime(row["LETTER_DATE"]) : DateTime.MinValue,
                    TPFROM = row["TPFROM"] != DBNull.Value ? Convert.ToDateTime(row["TPFROM"]) : DateTime.MinValue,
                    TPTO = row["TPTO"] != DBNull.Value ? Convert.ToDateTime(row["TPTO"]) : DateTime.MinValue,
                    CLIENTTYPE = row["CLIENTTYPE"] != DBNull.Value ? Convert.ToString(row["CLIENTTYPE"]) : string.Empty,
                    CLIENTNAME = row["CLIENTNAME"] != DBNull.Value ? Convert.ToString(row["CLIENTNAME"]) : string.Empty,
                    INSPFEE = row["INSPFEE"] != DBNull.Value ? Convert.ToInt32(row["INSPFEE"]) : 0,
                    MANDAYBASIS = row["MANDAYBASIS"] != DBNull.Value ? Convert.ToInt32(row["MANDAYBASIS"]) : 0,
                    LOTOFINSP = row["LOTOFINSP"] != DBNull.Value ? Convert.ToInt32(row["LOTOFINSP"]) : 0,
                    MATERIALVALUE = row["MATERIALVALUE"] != DBNull.Value ? Convert.ToInt32(row["MATERIALVALUE"]) : 0,
                    MINPOVAL = row["MINPOVAL"] != DBNull.Value ? Convert.ToInt32(row["MINPOVAL"]) : 0,
                    MAXPOVAL = row["MAXPOVAL"] != DBNull.Value ? Convert.ToInt32(row["MAXPOVAL"]) : 0,
                    CALLCANCELATION = row["CALLCANCELATION"] != DBNull.Value ? Convert.ToInt32(row["CALLCANCELATION"]) : 0,
                    Materialdescription = row["MATERIALDESCRIPTION"] != DBNull.Value ? Convert.ToString(row["MATERIALDESCRIPTION"]) : string.Empty,
                }).ToList();

                query = list.AsQueryable();
                dTResult.recordsTotal = ds.Tables[0].Rows.Count;
                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.CLIENTNAME).ToLower().Contains(searchBy.ToLower())
                    || Convert.ToString(w.LETTER_NO).ToLower().Contains(searchBy.ToLower())
                    );
                dTResult.recordsFiltered = query.Count();

                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
                dTResult.draw = dtParameters.Draw;
            }
            return dTResult;
        }

        public ContractEntry FindByID(int ID)
        {
            ContractEntry model = new();
            T100Contract Contract = context.T100Contracts.Find(ID);

            if (Contract == null)
                throw new Exception("Contract Not found");
            else
            {
                model.LETTER_NO = Contract.LetterNo;
                model.LETTER_DATE = Contract.LetterDate;
                model.TPFROM = Contract.Tpfrom;
                model.TPTO = Contract.Tpto;
                model.CLIENTTYPE = Contract.Clienttype;
                model.CLIENTNAME = Contract.Clientcode + " = " + Contract.Clientname;
                model.INSPFEE = Convert.ToInt32(Contract.Inspfee);
                model.MANDAYBASIS = Convert.ToInt32(Contract.Mandaybasis);
                model.LOTOFINSP = Convert.ToInt32(Contract.Lotofinsp);
                model.MATERIALVALUE = Convert.ToInt32(Contract.Materialvalue);
                model.MINPOVAL = Convert.ToInt32(Contract.Minpoval);
                model.MAXPOVAL = Convert.ToInt32(Contract.Maxpoval);
                model.CALLCANCELATION = Convert.ToInt32(Contract.Callcancelation);
                model.Materialdescription = Contract.Materialdescription;
                model.CLIENTCODE = Contract.Clientcode;
                return model;
            }
        }

        public bool Remove(int ID, int UserID)
        {
            var Contract = context.T100Contracts.Find(ID);
            if (Contract == null) { return false; }
            Contract.Isdeleted = Convert.ToByte(true);
            Contract.Updatedby = UserID;
            Contract.Updatedate = DateTime.Now;
            context.SaveChanges();
            return true;
        }
    }
}
