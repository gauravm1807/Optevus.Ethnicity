using Optevus.Model.Enum;

namespace Optevus.Model.POCO
{
    public class JobApplicant
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public int NumberOfPosition { get; set; }
        public int ApplicantId { get; set; }
        public ApplicantStatus Status { get; set; }
        public string? Gender { get; set; }
        public string? Race { get; set; }
    }
}
