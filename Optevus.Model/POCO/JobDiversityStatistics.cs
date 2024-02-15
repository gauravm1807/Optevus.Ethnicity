namespace Optevus.Model.POCO
{
    public class JobDiversityStatistics
    {
        public JobDiversityStatistics()
        {
            Applicants = new List<JobApplicant>();
            DiversitySchools = new List<ApplicantDiversitySchool>();
        }
        public List<JobApplicant> Applicants { get; set; }
        public List<ApplicantDiversitySchool> DiversitySchools { get; set; }
    }
}
