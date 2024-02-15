using Optevus.Model.Enum;
using Optevus.Model.POCO;
using Optevus.Model.Response;
using Optevus.Repositories.Diversity;
using Optevus.Services.DiversityServices.Interface;


namespace Optevus.Services.DiversityServices
{
    public class DiversityService : IDiversityService
    {
        private readonly DiversityRepository _diversityRepository;
        public DiversityService(DiversityRepository diversityRepository)
        {
            _diversityRepository = diversityRepository;
        }

        public async Task<ResponseGenderAggregate?> GetApplicantGenderAsync(int jobId)
        {
            var gender = await _diversityRepository.GetApplicantGenderAsync(jobId);

            if (gender == null)
            {
                return null;
            }

            int totalGender = gender.MaleCount + gender.FemaleCount;

            return new ResponseGenderAggregate
            {
                FemalePercentage = CalcuatePecentage(totalGender, gender.FemaleCount),
                MalePercentage = CalcuatePecentage(totalGender, gender.MaleCount)
            };
        }
        public async Task<ResponseDiversitySchool?> GetDiversitySchoolAsync(int jobId)
        {
            var diversitySchool = await _diversityRepository.GetDiversitySchoolAsync(jobId);

            if (diversitySchool == null)
            {
                return null;
            }

            int totalSchool = diversitySchool.HBCU + diversitySchool.HSI + diversitySchool.TCU + diversitySchool.Others;

            return new ResponseDiversitySchool
            {
                JobId = jobId,
                HBCU = CalcuatePecentage(totalSchool, diversitySchool.HBCU),
                HSI = CalcuatePecentage(totalSchool, diversitySchool.HSI),
                TCU = CalcuatePecentage(totalSchool, diversitySchool.TCU),
                Others = CalcuatePecentage(totalSchool, diversitySchool.Others)
            };
        }
        public async Task<List<ResponseAgeAggregate>?> GetApplicantAgeRangesAsync(int jobId)
        {
            var ages = await _diversityRepository.GetJobApplicantAgeAsync(jobId);
            if (ages == null || ages.Count == 0)
            {
                return null;
            }

            var under20Count = ages?.Count(x => x?.age <= 20) ?? 0;
            var under30Count = ages?.Count(x => x?.age >= 21 && x?.age <= 30) ?? 0;
            var under40Count = ages?.Count(x => x?.age >= 31 && x?.age <= 40) ?? 0;
            var under50Count = ages?.Count(x => x?.age >= 41 && x?.age <= 50) ?? 0;
            var under60Count = ages?.Count(x => x?.age >= 51 && x?.age <= 60) ?? 0;
            var above60 = ages?.Count(x => x?.age > 60) ?? 0;


            var totalApplicants = ages?.Count() ?? 0;


            List<ResponseAgeAggregate> responseList = new List<ResponseAgeAggregate> {
                new ResponseAgeAggregate
                {
                    JobId = jobId,
                    AgeRange = "under20",
                    Percentage = CalcuatePecentage(totalApplicants, under20Count)
                },
                new ResponseAgeAggregate
                {
                    JobId = jobId,
                    AgeRange = "21 to 30",
                    Percentage = CalcuatePecentage(totalApplicants, under30Count)
                },
                new ResponseAgeAggregate
                {
                    JobId = jobId,
                    AgeRange = "31 to 40",
                    Percentage = CalcuatePecentage(totalApplicants, under40Count)
                },
                new ResponseAgeAggregate
                {
                    JobId = jobId,
                    AgeRange = "41 to 50",
                    Percentage = CalcuatePecentage(totalApplicants, under50Count)
                },
                new ResponseAgeAggregate
                {
                    JobId = jobId,
                    AgeRange = "51 to 60",
                    Percentage = CalcuatePecentage(totalApplicants, under60Count)
                },
                new ResponseAgeAggregate
                {
                    JobId = jobId,
                    AgeRange = "60+",
                    Percentage = CalcuatePecentage(totalApplicants, above60)
                }
            };
            return responseList;
        }
        public async Task<ResponseEthnicityAggregate?> GetApplicantEthnicityAsync(int jobId)
        {
            var ethnicity_count = await _diversityRepository.GetApplicantEthnicityAsync(jobId);

            if (ethnicity_count == null)
            {
                return null;
            }

            int total = ethnicity_count.TotalCount;

            return new ResponseEthnicityAggregate
            {
                JobId = jobId,
                TotalCount = total,
                WhiteNonLatinoPercentage = (int)CalcuatePecentage(total, ethnicity_count.WhiteNonLatino),
                HispanoNonLatinooPercentage = (int)CalcuatePecentage(total, ethnicity_count.HispanoNonLatinoo),
                AsianNonLatinoPercentage = (int)CalcuatePecentage(total, ethnicity_count.AsianNonLatino),
                BlackNonLatinoPercentage = (int)CalcuatePecentage(total, ethnicity_count.BlackNonLatino),
                AmericanIndianAlaskanNativePercentage = (int)CalcuatePecentage(total, ethnicity_count.AmericanIndianAlaskanNative),
                PacificIslanderPercentage = (int)CalcuatePecentage(total, ethnicity_count.PacificIslander)

            };
        }
        public async Task<ResponseJobDiversityStatistics> GetJobApplicantAsync(int jobId)
        {

            var result = await _diversityRepository.GetJobApplicantAsync(jobId);
            if (result == null)
            {
                return null;
            }

            ResponseJobDiversityStatistics jobDiversityStatistics = new ResponseJobDiversityStatistics();

            jobDiversityStatistics.JobId = jobId;
            jobDiversityStatistics.JobTitle = result.Applicants[0].JobTitle.Trim();
            jobDiversityStatistics.NumberOfPosition = result.Applicants[0].NumberOfPosition;

            jobDiversityStatistics.Applied = result.Applicants.Count(x => x.Status == ApplicantStatus.Applied);
            jobDiversityStatistics.Prescreened = result.Applicants.Count(x => x.Status == ApplicantStatus.Prescreened);
            jobDiversityStatistics.Shortlisted = result.Applicants.Count(x => x.Status == ApplicantStatus.Shortlisted);
            jobDiversityStatistics.Assessed = result.Applicants.Count(x => x.Status == ApplicantStatus.Assessed);
            jobDiversityStatistics.Interviewed = result.Applicants.Count(x => x.Status == ApplicantStatus.Interviewed);
            jobDiversityStatistics.Offered = result.Applicants.Count(x => x.Status == ApplicantStatus.Offered);
            jobDiversityStatistics.BGV = result.Applicants.Count(x => x.Status == ApplicantStatus.BGV);
            jobDiversityStatistics.Hired = result.Applicants.Count(x => x.Status == ApplicantStatus.Hired);
            jobDiversityStatistics.Onboarded = result.Applicants.Count(x => x.Status == ApplicantStatus.Onboarded);
            jobDiversityStatistics.Rejected = result.Applicants.Count(x => x.Status == ApplicantStatus.Rejected);

            jobDiversityStatistics.TotalApplicant = result.Applicants.Count();


            jobDiversityStatistics.DiversityCandiateApplied = GetDiversityCount(ApplicantStatus.Applied, result);
            jobDiversityStatistics.DiversityCandiatePrescreened = GetDiversityCount(ApplicantStatus.Prescreened, result);
            jobDiversityStatistics.DiversityCandiateShortlisted = GetDiversityCount(ApplicantStatus.Shortlisted, result);
            jobDiversityStatistics.DiversityCandiateAssessed = GetDiversityCount(ApplicantStatus.Assessed, result);
            jobDiversityStatistics.DiversityCandiateInterviewed = GetDiversityCount(ApplicantStatus.Interviewed, result);
            jobDiversityStatistics.DiversityCandiateOffered = GetDiversityCount(ApplicantStatus.Offered, result);
            jobDiversityStatistics.DiversityCandiateBGV = GetDiversityCount(ApplicantStatus.BGV, result);
            jobDiversityStatistics.DiversityCandiateHired = GetDiversityCount(ApplicantStatus.Hired, result);
            jobDiversityStatistics.DiversityCandiateOnboarded = GetDiversityCount(ApplicantStatus.Onboarded, result);
            jobDiversityStatistics.DiversityCandiateRejected = GetDiversityCount(ApplicantStatus.Rejected, result);



            return jobDiversityStatistics;
        }
        private double CalcuatePecentage(double total, double value)
        {
            if (total == 0) { return 0; }

            return Math.Round(value / total * 100, 2, MidpointRounding.AwayFromZero);
        }
        private int GetDiversityCount(ApplicantStatus appStatus, JobDiversityStatistics jobStatistics)
        {
            int? diversityCount;
            var applicant = jobStatistics.Applicants.Where(x => x.Status == appStatus);

            var diversityScoolIds = jobStatistics.DiversitySchools.Select(x => x.ApplicantId)?.ToList();
            if (diversityScoolIds != null)
            {
                diversityCount = applicant?.Count(x => x.Gender?.ToLower() == "female" || x.Race?.ToUpper() != "W_NL" || diversityScoolIds.Contains(x.ApplicantId));
            }
            else
            {
                diversityCount = applicant?.Count(x => x.Gender?.ToLower() == "female" || x.Race?.ToUpper() != "W_NL");
            }


            return diversityCount ?? 0;

        }
    }
}
