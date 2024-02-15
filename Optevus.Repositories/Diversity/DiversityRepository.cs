using Microsoft.Extensions.Configuration;
using Optevus.Model.Enum;
using Optevus.Model.POCO;
using Optevus.Repositories.Diversity.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Optevus.Repositories.Diversity
{
    public class DiversityRepository : IDiversityRepository
    {
        private readonly string connectionString = "Data Source=optevus-mssqlserver-staging.database.windows.net;Initial Catalog=Optevus-DB-Recruiter-002-staging;User ID=optevus-admin;Password=Stagingteam123";

        public DiversityRepository(IConfiguration configuration)
        {

        }
        protected IDbConnection OpenConnection()
        {
            IDbConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public async Task<CandidateGender?> GetApplicantGenderAsync(int jobId)
        {
            CandidateGender gender = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Applicant_Gender", conn))
                {
                    conn.Open();
                    SqlParameter param = new SqlParameter("JobId", SqlDbType.Int);
                    param.Value = jobId;

                    cmd.Parameters.Add(param);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        gender = new CandidateGender();
                        if (reader.Read())
                        {
                            gender.JobId = Convert.ToInt32(reader["JobId"].ToString());
                            gender.MaleCount = Convert.ToInt32(reader["MaleCount"].ToString());
                            gender.FemaleCount = Convert.ToInt32(reader["FemaleCount"].ToString());
                            gender.TotalCount = Convert.ToInt32(reader["TotalCount"].ToString());
                        }
                    }
                }
            }
            return gender;
        }
        public async Task<EthnicityModel?> GetApplicantEthnicityAsync(int jobId)
        {
            EthnicityModel model = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Applicant_Ethnicity", conn))
                {
                    conn.Open();
                    SqlParameter param = new SqlParameter("JobId", SqlDbType.Int);
                    param.Value = jobId;

                    cmd.Parameters.Add(param);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        model = new EthnicityModel();
                        if (reader.Read())
                        {
                            model.JobId = Convert.ToInt32(reader["JobId"].ToString());
                            model.TotalCount = Convert.ToInt32(reader["TotalCount"].ToString());
                            model.WhiteNonLatino = Convert.ToInt32(reader["WhiteNonLatino"].ToString());
                            model.HispanoNonLatinoo = Convert.ToInt32(reader["HispanoNonLatinoo"].ToString());
                            model.AsianNonLatino = Convert.ToInt32(reader["AsianNonLatino"].ToString());
                            model.BlackNonLatino = Convert.ToInt32(reader["BlackNonLatino"].ToString());
                            model.AmericanIndianAlaskanNative = Convert.ToInt32(reader["AmericanIndianAlaskanNative"].ToString());
                            model.PacificIslander = Convert.ToInt32(reader["PacificIslander"].ToString());
                        }
                    }
                }
            }
            return model;
        }
        public async Task<DiversitySchoolAggregate?> GetDiversitySchoolAsync(int jobId)
        {
            DiversitySchoolAggregate diversitySchool = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSchoolCountByJob", conn))
                {
                    conn.Open();
                    SqlParameter param = new SqlParameter("JobId", SqlDbType.Int);
                    param.Value = jobId;

                    cmd.Parameters.Add(param);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        diversitySchool = new DiversitySchoolAggregate();
                        if (reader.Read())
                        {
                            diversitySchool.JobId = Convert.ToInt32(reader["JobId"].ToString());
                            diversitySchool.HBCU = Convert.ToInt32(reader["HBCU"].ToString());
                            diversitySchool.HSI = Convert.ToInt32(reader["HSI"].ToString());
                            diversitySchool.Others = Convert.ToInt32(reader["Others"].ToString());
                        }
                    }
                }
            }
            return diversitySchool;
        }
        public async Task<JobDiversityStatistics?> GetJobApplicantAsync(int jobId)
        {
            JobDiversityStatistics jobDiversityStatistics = null;
            List<JobApplicant> applicants = null;
            JobApplicant applicant = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetApplicantByJobId", conn))
                {
                    conn.Open();
                    SqlParameter param = new SqlParameter("JobId", SqlDbType.Int);
                    param.Value = jobId;

                    cmd.Parameters.Add(param);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        jobDiversityStatistics = new JobDiversityStatistics();
                        applicants = new List<JobApplicant>();

                        while (reader.Read())
                        {
                            applicant = new JobApplicant();

                            applicant.JobId = Convert.ToInt32(reader["JobId"].ToString());
                            applicant.JobTitle = reader["JobTitle"].ToString();
                            applicant.NumberOfPosition = Convert.ToInt32(reader["NumberOfPosition"]);
                            applicant.ApplicantId = Convert.ToInt32(reader["ApplicantId"].ToString());
                            applicant.Status = Enum.Parse<ApplicantStatus>(reader["StatusId"].ToString());
                            applicant.Gender = reader["Gender"].ToString();
                            applicant.Race = reader["Race"].ToString();

                            applicants.Add(applicant);
                        }
                        jobDiversityStatistics.Applicants = applicants;
                        if (reader.NextResult() && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                jobDiversityStatistics.DiversitySchools.Add(
                                    new ApplicantDiversitySchool
                                    {
                                        ApplicantId = Convert.ToInt32(reader["ApplicantID"]),
                                        SchoolName = reader["HBC"].ToString(),
                                        DiversitySchoolType = DiversitySchoolType.HBCU
                                    });
                            }
                        }
                        if (reader.NextResult() && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                jobDiversityStatistics.DiversitySchools.Add(
                                     new ApplicantDiversitySchool
                                     {
                                         ApplicantId = Convert.ToInt32(reader["ApplicantID"]),
                                         SchoolName = reader["HSI"].ToString(),
                                         DiversitySchoolType = DiversitySchoolType.HSI
                                     });
                            }
                        }
                        if (reader.NextResult() && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                jobDiversityStatistics.DiversitySchools.Add(
                                     new ApplicantDiversitySchool
                                     {
                                         ApplicantId = Convert.ToInt32(reader["ApplicantID"]),
                                         SchoolName = reader["TCU"].ToString(),
                                         DiversitySchoolType = DiversitySchoolType.TCU
                                     });
                            }
                        }
                    }
                }
            }
            return jobDiversityStatistics;
        }
        public async Task<List<CandidateAge?>> GetJobApplicantAgeAsync(int jobId)
        {
            CandidateAge age = null;
            List<CandidateAge> ages = null;
            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Applicant_Age", conn))
                    {
                        conn.Open();
                        SqlParameter param = new SqlParameter("jobId", SqlDbType.Int);
                        param.Value = jobId;

                        cmd.Parameters.Add(param);

                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                ages = new List<CandidateAge>();
                                while (reader.Read())
                                {
                                    if (Convert.ToInt32(reader["Age"].ToString()) > 0)
                                    {
                                        age = new CandidateAge();
                                        age.JobId = jobId;
                                        age.ApplicantResumeId = Convert.ToInt32(reader["ApplicantResumeId"].ToString());

                                        age.age = Convert.ToInt32(reader["Age"].ToString());

                                        ages.Add(age);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ages;
        }
    }
}
