using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class VigilanceCaseMonitoringRepository : IVigilanceCaseMonitoringRepository
    {
        private readonly ModelContext context;

        public VigilanceCaseMonitoringRepository(ModelContext context)
        {
            this.context = context;
        }

        public VigilanceCasesMasterModel FindByID(int Id)
        {
            VigilanceCasesMasterModel model = new();
            T53VigilanceCasesMaster master = context.T53VigilanceCasesMasters.Find(Id);

            if (master == null)
                return model;
            else
            {
                model.Id = master.Id;
                model.RefRegNo = master.RefRegNo;
                model.RefDt = master.RefDt;
                model.RefNo = master.RefNo;
                model.RefDetails = master.RefDetails;
                model.RefReplyDt = master.RefReplyDt;
                model.PrelimInvDetails = master.PrelimInvDetails;
                model.ActionProposed = master.ActionProposed;
                model.ActionProposedDt = master.ActionProposedDt;
                model.FinalAction = master.FinalAction;
                model.FinalActionDt = master.FinalActionDt;

                return model;
            }
        }

        public DTResult<VigilanceCasesMasterModel> GetVigilanceCaseList(DTParameters dtParameters)
        {
            DTResult<VigilanceCasesMasterModel> dTResult = new() { draw = 0 };
            IQueryable<VigilanceCasesMasterModel>? query = null;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "RefRegNo";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "RefRegNo";
                orderAscendingDirection = true;
            }

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            string RefRegNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["RefRegNo"]) ? Convert.ToString(dtParameters.AdditionalValues["RefRegNo"]) : "";
            DateTime? RefDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["RefDate"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["RefDate"]) : null;
            string CaseNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]) ? Convert.ToString(dtParameters.AdditionalValues["CaseNo"]) : "";
            string BookNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BookNo"]) ? Convert.ToString(dtParameters.AdditionalValues["BookNo"]) : "";
            string SetNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["SetNo"]) ? Convert.ToString(dtParameters.AdditionalValues["SetNo"]) : "";

            query = (from t53 in context.T53VigilanceCasesMasters
                     join t54 in context.T54VigilanceCasesDetails on t53.RefRegNo equals t54.RefRegNo
                     where t53.RefRegNo.Substring(0, 1) == Region
                     && (!string.IsNullOrEmpty(RefRegNo) ? t53.RefRegNo == RefRegNo : true)
                     && (RefDate != null ? t53.RefDt == RefDate : true)
                     && (!string.IsNullOrEmpty(CaseNo) ? t54.CaseNo == CaseNo : true)
                     && (!string.IsNullOrEmpty(BookNo) ? t54.BkNo == BookNo : true)
                     && (!string.IsNullOrEmpty(SetNo) ? t54.SetNo == SetNo : true)
                     group new
                     {
                         t53.Id,
                         t53.RefRegNo,
                         t53.RefDt,
                         t53.RefDetails
                     } by new
                     {
                         t53.Id,
                         t53.RefRegNo,
                         t53.RefDt,
                         t53.RefDetails
                     } into grouped
                     orderby grouped.Key.RefRegNo
                     select new VigilanceCasesMasterModel
                     {
                         Id = grouped.Key.Id,
                         RefRegNo = grouped.Key.RefRegNo,
                         RefDt = grouped.Key.RefDt,
                         RefDetails = grouped.Key.RefDetails
                     });

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<VigilanceCasesListModel> GetVigilanceList(DTParameters dtParameters)
        {
            DTResult<VigilanceCasesListModel> dTResult = new() { draw = 0 };
            IQueryable<VigilanceCasesListModel>? query = null;

            string CaseNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]) ? Convert.ToString(dtParameters.AdditionalValues["CaseNo"]) : "";
            string RefRegNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["RefRegNo"]) ? Convert.ToString(dtParameters.AdditionalValues["RefRegNo"]) : "";

            if (string.IsNullOrEmpty(RefRegNo))
            {
                query = from t20 in context.T20Ics
                        join t22 in context.T22Bills on t20.BillNo equals t22.BillNo
                        where t20.CaseNo == CaseNo
                        orderby t20.BkNo, t20.SetNo
                        select new VigilanceCasesListModel
                        {
                            Id = 0,
                            CaseNo = t20.CaseNo,
                            BkNo = t20.BkNo,
                            SetNo = t20.SetNo,
                            ConsigneeCd = t20.ConsigneeCd,
                            BpoCd = t20.BpoCd,
                            IeCd = t20.IeCd ?? 0,
                            BillNo = t20.BillNo,
                            BillDt = t22.BillDt
                        };
            }
            else if (!string.IsNullOrEmpty(RefRegNo))
            {
                CaseNo = context.T54VigilanceCasesDetails.Where(x => x.RefRegNo == RefRegNo).Select(x => x.CaseNo).FirstOrDefault();

                query = (from t20 in context.T20Ics
                         join t22 in context.T22Bills on t20.BillNo equals t22.BillNo
                         where t20.CaseNo == CaseNo &&
                             !context.T54VigilanceCasesDetails
                                 .Any(t54 => t20.CaseNo == t54.CaseNo && t20.BkNo == t54.BkNo && t20.SetNo == t54.SetNo)
                         select new VigilanceCasesListModel
                         {
                             Id = -1,
                             CaseNo = t20.CaseNo,
                             BkNo = t20.BkNo,
                             SetNo = t20.SetNo,
                             ConsigneeCd = t20.ConsigneeCd,
                             BpoCd = t20.BpoCd,
                             IeCd = t20.IeCd ?? 0,
                             BillNo = t20.BillNo,
                             BillDt = t22.BillDt
                         }
                        )
                        .Union(
                            from t54 in context.T54VigilanceCasesDetails
                            where t54.RefRegNo == RefRegNo
                            select new VigilanceCasesListModel
                            {
                                Id = Convert.ToInt32(t54.Id),
                                CaseNo = t54.CaseNo,
                                BkNo = t54.BkNo,
                                SetNo = t54.SetNo,
                                ConsigneeCd = t54.ConsigneeCd ?? 0,
                                BpoCd = t54.BpoCd,
                                IeCd = t54.IeCd ?? 0,
                                BillNo = t54.BillNo,
                                BillDt = t54.BillDt
                            }
                        );

                query = query.OrderBy(x => x.BkNo).ThenBy(x => x.SetNo);
            }

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int SaveDetails(VigilanceCasesMasterModel model)
        {
            if (model.Id == 0)
            {
                string RefRegNo = GetVigilanceRefRegNo(model.Region, string.Format("{0:ddMMyyyy}", model.RefDt));

                if (RefRegNo == "-1") return -1;
                else model.RefRegNo = RefRegNo;

                T53VigilanceCasesMaster vigilanceCases = new()
                {
                    RefRegNo = model.RefRegNo,
                    RefNo = model.RefNo,
                    RefDt = model.RefDt,
                    RefDetails = model.RefDetails,
                    RefReplyDt = model.RefReplyDt,
                    PrelimInvDetails = model.PrelimInvDetails,
                    ActionProposed = model.ActionProposed,
                    ActionProposedDt = model.ActionProposedDt,
                    FinalAction = model.FinalAction,
                    FinalActionDt = model.FinalActionDt,
                    UserId = model.UserId,
                    Datetime = DateTime.Now.Date,
                };

                context.T53VigilanceCasesMasters.Add(vigilanceCases);
                context.SaveChanges();

                int SNo = GetMaxSno(model.RefRegNo);
                SNo += 1;

                foreach (VigilanceCasesListModel data in model.lstVigilanceCasesList)
                {
                    T54VigilanceCasesDetail details = new()
                    {
                        RefRegNo = model.RefRegNo,
                        Sno = SNo,
                        CaseNo = data.CaseNo,
                        BkNo = data.BkNo,
                        SetNo = data.SetNo,
                        ConsigneeCd = data.ConsigneeCd,
                        BpoCd = data.BpoCd,
                        IeCd = data.IeCd,
                        BillNo = data.BillNo,
                        BillDt = data.BillDt,
                        UserId = model.UserId,
                        Datetime = DateTime.Now.Date,
                    };

                    context.T54VigilanceCasesDetails.Add(details);

                    SNo += 1;
                }

                context.SaveChanges();

            }
            else
            {
                T53VigilanceCasesMaster master = context.T53VigilanceCasesMasters.Find(model.Id);

                if (master != null)
                {
                    master.RefNo = model.RefNo;
                    master.RefDetails = model.RefDetails;
                    master.RefReplyDt = model.RefReplyDt;
                    master.PrelimInvDetails = model.PrelimInvDetails;
                    master.ActionProposed = model.ActionProposed;
                    master.ActionProposedDt = model.ActionProposedDt;
                    master.FinalAction = model.FinalAction;
                    master.FinalActionDt = model.FinalActionDt;
                    master.UserId = model.UserId;
                    master.Datetime = DateTime.Now;

                    context.SaveChanges();

                    if (context.T54VigilanceCasesDetails.Any(x => x.RefRegNo == model.RefRegNo))
                    {
                        context.T54VigilanceCasesDetails.RemoveRange(context.T54VigilanceCasesDetails.Where(x => x.RefRegNo == model.RefRegNo).ToList());
                        context.SaveChanges();
                    }

                    int SNo = GetMaxSno(model.RefRegNo);
                    SNo += 1;

                    foreach (VigilanceCasesListModel data in model.lstVigilanceCasesList)
                    {
                        T54VigilanceCasesDetail details = new()
                        {
                            RefRegNo = model.RefRegNo,
                            Sno = SNo,
                            CaseNo = data.CaseNo,
                            BkNo = data.BkNo,
                            SetNo = data.SetNo,
                            ConsigneeCd = data.ConsigneeCd,
                            BpoCd = data.BpoCd,
                            IeCd = data.IeCd,
                            BillNo = data.BillNo,
                            BillDt = data.BillDt,
                            UserId = model.UserId,
                            Datetime = DateTime.Now.Date,
                        };

                        context.T54VigilanceCasesDetails.Add(details);

                        SNo += 1;
                    }

                    context.SaveChanges();
                }

            }

            return model.Id;
        }

        public string GetVigilanceRefRegNo(string Region, string RefDate)
        {
            string RefRegNo = string.Empty;

            ModelContext context = new(DbContextHelper.GetDbContextOptions());
            try
            {
                var cmd = context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GENERATE_VIGILANCE_REF_REG_NO";

                OracleParameter[] par = new OracleParameter[4];

                par[0] = new OracleParameter("IN_REGION_CD", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("IN_REF_DT", OracleDbType.Varchar2, RefDate, ParameterDirection.Input);
                par[2] = new OracleParameter("OUT_REF_REG_NO", OracleDbType.Varchar2, ParameterDirection.Output);
                par[2].Size = 7;
                par[3] = new OracleParameter("OUT_ERR_CD", OracleDbType.Decimal, ParameterDirection.Output);
                par[3].DbType = DbType.Int32;

                cmd.Parameters.AddRange(par);

                context.Database.OpenConnection();
                cmd.ExecuteNonQuery();
                context.Database.CloseConnection();

                if (Convert.ToInt32(cmd.Parameters["OUT_ERR_CD"].Value) == -1)
                {
                    RefRegNo = "-1";
                }
                else
                {
                    RefRegNo = cmd.Parameters["OUT_REF_REG_NO"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                context.Database.CloseConnection();
            }

            return RefRegNo;
        }

        public int GetMaxSno(string RefRegNo)
        {
            int SNo = 0;

            using ModelContext context = new(DbContextHelper.GetDbContextOptions());
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = command.Connection.State == ConnectionState.Open;
                if (!wasOpen) command.Connection.Open();
                try
                {
                    command.CommandText = "SELECT NVL(MAX(SNO), 0) FROM T54_VIGILANCE_CASES_DETAILS WHERE REF_REG_NO='" + RefRegNo + "'";
                    SNo = Convert.ToInt32(command.ExecuteScalar());
                }
                finally
                {
                    if (!wasOpen) command.Connection.Close();
                }
            }
            return SNo;
        }
    }

}

