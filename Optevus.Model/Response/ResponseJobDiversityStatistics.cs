namespace Optevus.Model.Response
{
    public class ResponseJobDiversityStatistics
    {
        public int JobId { get; set; }
        public string? JobTitle { get; set; }
        public int NumberOfPosition { get; set; }

        public int TotalApplicant { get; set; }
        public int Applied { get; set; }
        public int Prescreened { get; set; }
        public int Shortlisted { get; set; }
        public int Assessed { get; set; }
        public int Interviewed { get; set; }
        public int Offered { get; set; }
        public int BGV { get; set; }
        public int Hired { get; set; }
        public int Onboarded { get; set; }
        public int Rejected { get; set; }


        public int DiversityCandiateApplied { get; set; }
        public int DiversityCandiatePrescreened { get; set; }
        public int DiversityCandiateShortlisted { get; set; }
        public int DiversityCandiateAssessed { get; set; }
        public int DiversityCandiateInterviewed { get; set; }
        public int DiversityCandiateOffered { get; set; }
        public int DiversityCandiateBGV { get; set; }
        public int DiversityCandiateHired { get; set; }
        public int DiversityCandiateOnboarded { get; set; }
        public int DiversityCandiateRejected { get; set; }
    }
}
