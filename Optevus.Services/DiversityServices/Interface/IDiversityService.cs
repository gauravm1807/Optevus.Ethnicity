using Optevus.Model.Response;

namespace Optevus.Services.DiversityServices.Interface
{
    public interface IDiversityService
    {
        Task<ResponseGenderAggregate?> GetApplicantGenderAsync(int jobId);
        Task<ResponseJobDiversityStatistics> GetJobApplicantAsync(int jobId);
        Task<ResponseDiversitySchool?> GetDiversitySchoolAsync(int jobId);
        Task<List<ResponseAgeAggregate>?> GetApplicantAgeRangesAsync(int jobId);
        Task<ResponseEthnicityAggregate?> GetApplicantEthnicityAsync(int jobId);
    }
}
