using Optevus.Model.Enum;

namespace Optevus.Model.POCO
{
    public class ApplicantDiversitySchool
    {
        public int JobId { get; set; }
        public int ApplicantId { get; set; }
        public string? SchoolName { get; set; }
        public DiversitySchoolType DiversitySchoolType { get; set; }
    }
}
