using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Optevus.Services.DiversityServices.Interface;

namespace Optevus.Ethnicity.Controllers
{
    [Route("jobs")]
    [ApiController]
    public class ControllerApi : ControllerBase
    {
        private readonly IDiversityService _diversityService;
        public ControllerApi(IDiversityService ethnicityService)
        {
            _diversityService = ethnicityService;
        }

        [HttpGet]
        [Route("gender")]

        public async Task<IActionResult> GetApplicantGenderAsync(int jobId)
        {

            try
            {
                // Call the service method asynchronously
                var result = await _diversityService.GetApplicantGenderAsync(jobId);
                if (result is null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Handle exceptions and return a BadRequest result
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetDetailsByJobIdAsync(int jobId)
        {
            try
            {
                // Call the service method asynchronously
                var result = await _diversityService.GetJobApplicantAsync(jobId);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a BadRequest result
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpGet]
        [Route("ethnicity")]
        public async Task<IActionResult> GetEthnicityByJobIdAsync(int jobId)
        {
            try
            {
                // Call the service method asynchronously
                var result = await _diversityService.GetApplicantEthnicityAsync(jobId);
                if (result is null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Handle exceptions and return a BadRequest result
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("school")]
        public async Task<IActionResult> GetDiversitySchoolAsync(int jobId)
        {
            try
            {
                // Call the service method asynchronously
                var result = await _diversityService.GetDiversitySchoolAsync(jobId);
                if (result is null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Handle exceptions and return a BadRequest result
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("Age")]
        public async Task<IActionResult> GetAgeAsync(int jobId)
        {
            try
            {
                var result = await _diversityService.GetApplicantAgeRangesAsync(jobId);
                if (result is null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
