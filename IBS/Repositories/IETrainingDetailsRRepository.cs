using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class IETrainingDetailsRRepository : IETrainingDetailsRepository
    {
        private readonly ModelContext context;

        public IETrainingDetailsRRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<IETrainingDetailsModel> GetBills(DTParameters dtParameters, string Regin)
        {


            DTResult<IETrainingDetailsModel> dTResult = new() { draw = 0 };
            IQueryable<IETrainingDetailsModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "course_id";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "course_id";
                orderAscendingDirection = true;
            }


            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_Regin", OracleDbType.NVarchar2, Regin, ParameterDirection.Input);
            par[1] = new OracleParameter("CUR_OUT", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("Get_IETrainingDetails", par, 1);

            List<IETrainingDetailsModel> modelList = new List<IETrainingDetailsModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    IETrainingDetailsModel model = new IETrainingDetailsModel
                    {
                        course_id = Convert.ToString(row["course_id"]),
                        course_name = Convert.ToString(row["course_name"]),
                        course_institute = Convert.ToString(row["course_institute"]),
                        certificate = Convert.ToString(row["certificate"]),
                        training_category = Convert.ToString(row["training_category"]),

                    };

                    modelList.Add(model);
                }
            }

            //query = from l in context.Roles
            //        where l.Isdeleted == 0
            //        select new RoleModel
            //        {
            //            RoleId = l.RoleId,
            //            Rolename = l.Rolename,
            //            Roledescription = l.Roledescription,
            //            Issysadmin = Convert.ToBoolean(l.Issysadmin),
            //            Isactive = Convert.ToBoolean(l.Isactive),
            //            Isdeleted = l.Isdeleted,
            //            Createddate = l.Createddate,
            //            Createdby = l.Createdby,
            //            Updateddate = l.Updateddate,
            //            Updatedby = l.Updatedby
            //        };

            query = modelList.AsQueryable();

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.course_name).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.course_name).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.data = query.ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;

            //using (var dbContext = context.Database.GetDbConnection())
            //{

            //}

            //return dTResult;
        }

        public IETrainingDetailsModel IEFetchData(string Name)
        {
              
            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_IE_CD", OracleDbType.Int32, Name, ParameterDirection.Input);
                par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GetEmpIETrainingData", par, 1);

                IETrainingDetailsModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new IETrainingDetailsModel
                    {
                        DOB = Convert.ToString(row["DOB"]),
                        DOJ = Convert.ToString(row["JOIN_DT"]),
                        Discipline = Convert.ToString(row["DESCIPLINE"]),
                        EmpNo = Convert.ToString(row["EMP_NO"]),
                        Category = Convert.ToString(row["CATEGORY"]),
                        CategoryOther = Convert.ToString(row["CATEGORY_OTHER"]),
                        Qualification = Convert.ToString(row["QUALIFICATION"]),
                        QualificationOther = Convert.ToString(row["QUAL_OTHER"]),
                        Institute = Convert.ToString(row["QUAL_INSTITUTE"]),
                        Name = Convert.ToString(row["NAME"]),

                    };
                }
                else
                {
                    int ie = Convert.ToInt32(Name);
                    var IE = context.T09Ies.Find(ie);
                    if (IE == null)
                        return model;
                    else
                    {
                        DateTime? ieDob = IE.IeDob;
                        DateTime? ieDoj = IE.IeJoinDt;
                        string dob = "";
                        string doj = "";
                        if(ieDob ==null && ieDoj == null)
                        {
                            dob = "";
                            doj = "";
                        }
                        else if(ieDob != null)
                        {
                            dob = ieDob.Value.ToString("dd/MM/yyyy") ?? "";
                            //doj = ieDoj.Value.ToString("dd/MM/yyyy") ?? "";
                        }
                        else if (ieDoj != null)
                        {
                            //dob = ieDob.Value.ToString("dd/MM/yyyy") ?? "";
                            doj = ieDoj.Value.ToString("dd/MM/yyyy") ?? "";
                        }

                        model.EmpNo = IE.IeEmpNo;
                        model.DOB = dob;
                        model.DOJ = doj;
                        model.Discipline = IE.IeDepartment;
                        return model;
                    }
                }

                return model;
            }
        }
        public IETrainingDetailsModel TrainingDFetchData(string Course)
        {

            using (var dbContext = context.Database.GetDbConnection())
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_course_id", OracleDbType.NVarchar2, Course, ParameterDirection.Input);
                par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("GetTrainingCourseDetails", par, 1);

                IETrainingDetailsModel model = new();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataRow row = ds.Tables[0].Rows[0];
                    model = new IETrainingDetailsModel
                    {
                        CourseNameOther = Convert.ToString(row["COURSE_NAME"]),
                        Institue = Convert.ToString(row["COURSE_INSTITUTE"]),
                        From = Convert.ToString(row["COURSE_DUR_FR"]),
                        To = Convert.ToString(row["COURSE_DUR_TO"]),
                        certificate = Convert.ToString(row["CERTIFICATE"]),
                        Fees = Convert.ToString(row["FEES"]),
                        Validity = Convert.ToString(row["VALIDITY"]),
                        training_category = Convert.ToString(row["TRAINING_CATEGORY"]),

                    };
                }

                return model;
            }
        }
        public static string GetDateString(string sqlQuery)
        {
            ModelContext context = new ModelContext(DbContextHelper.GetDbContextOptions());
            string dateResult = null;
            try
            {
                var conn = (OracleConnection)context.Database.GetDbConnection();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;

                    context.Database.OpenConnection();

                    // Execute the SQL query and fetch the date result
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        dateResult = result.ToString();
                    }

                    context.Database.CloseConnection();
                }
            }
            catch (Exception)
            {
                context.Database.CloseConnection();
            }

            return dateResult;
        }
        public IETrainingDetailsModel FindByID(int ie)
        {

            IETrainingDetailsModel model = new();
            var IE = context.T09Ies.Find(ie);

            //var lb = (from l in context.T65LaboratoryMasters where l.LabId == LabId select l).FirstOrDefault();

            if (IE == null)
                return model;
            else
            {
                //model.ID = Convert.ToDecimal(user.Id);
                model.EmpNo = IE.IeEmpNo;
                model.DOB = IE.IeDob.ToString();
                model.DOJ = IE.IeJoinDt.ToString();
                model.Discipline = IE.IeDepartment;
                return model;
            }
        }
        public bool Save(IETrainingDetailsModel IETrainingDetailsModel)
        {
            var from = Convert.ToDateTime(IETrainingDetailsModel.From).ToString("MM/dd/yyyy");
            var to = Convert.ToDateTime(IETrainingDetailsModel.To).ToString("MM/dd/yyyy");
            string course_count;
            string sqlQuery1 = "select NVL(count(*),0) from TRAINING_COURSE_MASTER WHERE TRAINING_TYPE='" + IETrainingDetailsModel.TrainingType + "' AND TRAINING_FIELD='" + IETrainingDetailsModel.TrainingArea + "' AND REGION='" + IETrainingDetailsModel.Regin + "'";

            course_count = GetDateString(sqlQuery1);
            int ie = Convert.ToInt32(IETrainingDetailsModel.Name);
            IETrainingDetailsModel model = FindByID(ie);

            string iec;
            string Query = "select IE_CD from TRAINEE_EMPLOYEE_MASTER where IE_CD='" + IETrainingDetailsModel.Name + "'";

            iec = GetDateString(Query);
            if (iec == null)
            {
                var trainingDetail = new TraineeEmployeeMaster
                {
                    IeCd = Convert.ToInt32(IETrainingDetailsModel.Name),
                    Name = IETrainingDetailsModel.hdnName,
                    Dob = Convert.ToDateTime(IETrainingDetailsModel.DOB),
                    Doj = Convert.ToDateTime(IETrainingDetailsModel.DOJ),
                    EmpNo = model.EmpNo,
                    Descipline = IETrainingDetailsModel.Discipline,
                    Category = IETrainingDetailsModel.Category,
                    CategoryOther = IETrainingDetailsModel.CategoryOther,
                    Qualification = IETrainingDetailsModel.Qualification,
                    QualOther = IETrainingDetailsModel.QualificationOther,
                    QualInstitute = IETrainingDetailsModel.Institute,
                    Region = IETrainingDetailsModel.Regin
                };
                context.TraineeEmployeeMasters.Add(trainingDetail);
                context.SaveChanges();
            }

            if (course_count == "0" || IETrainingDetailsModel.course_name == "" || IETrainingDetailsModel.CourseNameOther == "")
            {


                string CourseId;
                string CoId;
                string sqlQuery = "Select lpad(nvl(max(to_number(nvl(substr(COURSE_ID,2,4),0))),0)+1,4,'0')  From TRAINING_COURSE_MASTER where substr(COURSE_ID,1,1)='" + IETrainingDetailsModel.Regin + "'";

                CoId = GetDateString(sqlQuery);
                CourseId = (IETrainingDetailsModel.Regin + CoId);
                try
                {

                    OracleParameter[] par = new OracleParameter[12];
                    par[0] = new OracleParameter("p_course_id", OracleDbType.Varchar2, CourseId, ParameterDirection.Input);
                    par[1] = new OracleParameter("p_training_type", OracleDbType.Varchar2, IETrainingDetailsModel.TrainingType, ParameterDirection.Input);
                    par[2] = new OracleParameter("p_training_field", OracleDbType.Varchar2, IETrainingDetailsModel.TrainingArea, ParameterDirection.Input);
                    par[3] = new OracleParameter("p_course_name", OracleDbType.Varchar2, IETrainingDetailsModel.CourseNameOther, ParameterDirection.Input);
                    par[4] = new OracleParameter("p_course_institute", OracleDbType.Varchar2, IETrainingDetailsModel.Institue, ParameterDirection.Input);
                    par[5] = new OracleParameter("p_course_dur_fr", OracleDbType.Date, from, ParameterDirection.Input);
                    par[6] = new OracleParameter("p_course_dur_to", OracleDbType.Date, to, ParameterDirection.Input);
                    par[7] = new OracleParameter("p_certificate", OracleDbType.Varchar2, IETrainingDetailsModel.certificate, ParameterDirection.Input);
                    par[8] = new OracleParameter("p_fees", OracleDbType.Varchar2, IETrainingDetailsModel.Fees, ParameterDirection.Input);
                    par[9] = new OracleParameter("p_validity", OracleDbType.Varchar2, IETrainingDetailsModel.Validity, ParameterDirection.Input);
                    par[10] = new OracleParameter("p_region", OracleDbType.Varchar2, IETrainingDetailsModel.Regin, ParameterDirection.Input);
                    par[11] = new OracleParameter("p_training_category", OracleDbType.Varchar2, IETrainingDetailsModel.training_category, ParameterDirection.Input);

                    var ds = DataAccessDB.ExecuteNonQuery("InsertTrainingCourse", par, 1);
                    int iecode = Convert.ToInt32(IETrainingDetailsModel.Name);
                    var trainingDetail = new TrainingDetail
                    {
                        IeCd = iecode,
                        CourseId = CourseId
                    };
                    context.TrainingDetails.Add(trainingDetail);
                    context.SaveChanges();
                    //string Query = "INSERT INTO TRAINING_DETAILS (IE_CD,COURSE_ID) values (:p_ie_cd, :p_course_id)";
                    //OracleParameter[] par1 = new OracleParameter[2];
                    //par1[0] = new OracleParameter("p_ie_cd", OracleDbType.Varchar2, IETrainingDetailsModel.Name, ParameterDirection.Input);
                    //par1[1] = new OracleParameter("p_course_id", OracleDbType.Varchar2, CourseId, ParameterDirection.Input);
                    //var ds1 = DataAccessDB.ExecuteNonQuery(Query, par1, 1);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    //string Query = "INSERT INTO TRAINING_DETAILS (IE_CD,COURSE_ID) values (:p_ie_cd, :p_course_id)";
                    //int iecode = Convert.ToInt32(IETrainingDetailsModel.Name);
                    //OracleParameter[] par = new OracleParameter[2];
                    //par[0] = new OracleParameter("p_ie_cd", OracleDbType.Int16, iecode, ParameterDirection.Input);
                    //par[1] = new OracleParameter("p_course_id", OracleDbType.Varchar2, IETrainingDetailsModel.course_name, ParameterDirection.Input);
                    //var ds1 = DataAccessDB.ExecuteNonQuery(Query, par, 1);

                    int iecode = Convert.ToInt32(IETrainingDetailsModel.Name);
                    var count = context.TrainingDetails
                           .Where(item =>
                               item.IeCd == iecode &&
                               item.CourseId == IETrainingDetailsModel.course_name)
                           .Count();
                    if (count == 0)
                    {
                        var trainingDetail = new TrainingDetail
                        {
                            IeCd = iecode,
                            CourseId = IETrainingDetailsModel.course_name
                        };
                        context.TrainingDetails.Add(trainingDetail);
                        context.SaveChanges();
                    }
                    else
                    {
                        IETrainingDetailsModel.MSG = "Ie Code Already Exists";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    IETrainingDetailsModel.MSG = ex.InnerException.Message.ToString();
                    return false;
                }
            }
            return true;
        }
    }
}
