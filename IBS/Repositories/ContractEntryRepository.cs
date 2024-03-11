using DocumentFormat.OpenXml.Office2010.Excel;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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

                obj.InspectionfeeType = model.InspectionfeeType;

                obj.PerBasisFlatfee = model.PerBasisFlatfee;
                obj.MandayFlatfee = model.MandayFlatfee;
                obj.LumpsumFlatfee = model.LumpsumFlatfee;

                obj.PerBasisCancellation = model.PerBasisCancellation;
                obj.MandayCancellation = model.MandayCancellation;
                obj.LumpsumCancellation = model.LumpsumCancellation;

                obj.PerBasisRejection = model.PerBasisRejection;
                obj.MandayRejection = model.MandayRejection;
                obj.LumpsumRejection = model.LumpsumRejection;

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

                Contract.InspectionfeeType = model.InspectionfeeType;

                Contract.PerBasisFlatfee = model.PerBasisFlatfee;
                Contract.MandayFlatfee = model.MandayFlatfee;
                Contract.LumpsumFlatfee = model.LumpsumFlatfee;

                Contract.PerBasisCancellation = model.PerBasisCancellation;
                Contract.MandayCancellation = model.MandayCancellation;
                Contract.LumpsumCancellation = model.LumpsumCancellation;

                Contract.PerBasisRejection = model.PerBasisRejection;
                Contract.MandayRejection = model.MandayRejection;
                Contract.LumpsumRejection = model.LumpsumRejection;


                context.SaveChanges();
                ContractId = Convert.ToInt32(Contract.Id);
            }

            var T100ContractMaterials = (from T100 in context.T100ContractMaterials where T100.ContractId == ContractId select T100).ToList();
            if (T100ContractMaterials.Count > 0 && T100ContractMaterials != null)
            {
                context.T100ContractMaterials.RemoveRange(T100ContractMaterials);
                context.SaveChanges();
            }
            if (model.lstContractEntryList != null)
            {
                foreach (var item in model.lstContractEntryList)
                {
                    T100ContractMaterial Clst = new T100ContractMaterial();
                    {
                        Clst.ContractId = ContractId;
                        Clst.PerBasis = item.PerBasis;
                        Clst.Manday = item.Manday;
                        Clst.Lumpsum = item.Lumpsum;
                        Clst.Fromrs = item.Fromrs;
                        Clst.Tors = item.Tors;

                        Clst.Userid = Convert.ToString(model.CreatedBy);
                        Clst.Createdby = Convert.ToString(model.CreatedBy);
                        Clst.Createddate = DateTime.Now.Date;
                    }
                    context.T100ContractMaterials.Add(Clst);
                    context.SaveChanges();
                }

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
            var orderAscendingDirection = false;

            if (dtParameters.Order != null)
            {
                orderCriteria = "LETTER_DATE"; // dtParameters.Columns[2].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "LETTER_DATE";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "LETTER_DATE";
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
            string ClientCode = null;
            if (!string.IsNullOrEmpty(clientname))
            {
                var Client = clientname.Split("=");
                ClientCode = Client[0].Trim();
                clientname = Client[1].Trim();
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_clienttype", OracleDbType.Varchar2, Clienttype, ParameterDirection.Input);
            par[1] = new OracleParameter("p_clientcode", OracleDbType.Varchar2, ClientCode, ParameterDirection.Input);
            par[2] = new OracleParameter("p_clientname", OracleDbType.Varchar2, clientname, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);
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

                    InspectionfeeType = Convert.ToString(row["INSPECTIONFEE_TYPE"]) == "F" ? "Flat Fee" : "Material Value Wise",
                    PerBasisFlatfee = Convert.ToString(row["INSPECTIONFEE_TYPE"]) == "F" ? Convert.ToInt32(row["PER_BASIS_FLATFEE"]) : 0,
                    MandayFlatfee = Convert.ToString(row["INSPECTIONFEE_TYPE"]) == "F" ? Convert.ToInt32(row["MANDAY_FLATFEE"]) : 0,
                    LumpsumFlatfee = Convert.ToString(row["INSPECTIONFEE_TYPE"]) == "F" ? Convert.ToInt32(row["LUMPSUM_FLATFEE"]) : 0,
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

                model.InspectionfeeType = Contract.InspectionfeeType;
                model.PerBasisFlatfee = Contract.PerBasisFlatfee;
                model.MandayFlatfee = Contract.MandayFlatfee;
                model.LumpsumFlatfee = Contract.LumpsumFlatfee;
                model.PerBasisCancellation = Contract.PerBasisCancellation;
                model.MandayCancellation = Contract.MandayCancellation;
                model.LumpsumCancellation = Contract.LumpsumCancellation;
                model.PerBasisRejection = Contract.PerBasisRejection;
                model.MandayRejection = Contract.MandayRejection;
                model.LumpsumRejection = Contract.LumpsumRejection;

                List<ContractEntryList> clst = (from T100 in context.T100ContractMaterials
                                                where T100.ContractId == ID
                                                select new ContractEntryList
                                                {
                                                    Id = Convert.ToInt32(T100.Id),
                                                    ContractId = Convert.ToInt32(T100.ContractId),
                                                    PerBasis = T100.PerBasis,
                                                    Manday = T100.Manday,
                                                    Lumpsum = T100.Lumpsum,
                                                    Fromrs = T100.Fromrs,
                                                    Tors = T100.Tors,
                                                }
                                                ).ToList();
                model.lstContractEntryList = clst;

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

        public DTResult<ContractEntryList> GetValueList(DTParameters dtParameters, List<ContractEntryList> ContractList)
        {
            DTResult<ContractEntryList> dTResult = new() { draw = 0 };
            IQueryable<ContractEntryList>? query = null;
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "PerBasis";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "IeCd";
                orderAscendingDirection = true;
            }

            query = (from u in ContractList
                     select new ContractEntryList
                     {
                         Id = u.Id,
                         PerBasis = u.PerBasis,
                         Manday = u.Manday,
                         Lumpsum = u.Lumpsum,
                         Fromrs = u.Fromrs,
                         Tors = u.Tors,
                     }).AsQueryable();

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }
    }
}
