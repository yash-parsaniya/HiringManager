using HiringManager.DataAccess.Servides;
using HiringManager.DataAccess.UnitOfWork;
using HiringManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace HiringManager.Web.Controllers
{
    [Route("apply")]
    public class ApplicationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationNumberGenerator _appNumberGenerator;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(
            IUnitOfWork unitOfWork,
            IApplicationNumberGenerator appNumberGenerator,
            ILogger<ApplicationController> logger)
        {
            _unitOfWork = unitOfWork;
            _appNumberGenerator = appNumberGenerator;
            _logger = logger;
        }

        [HttpGet("personal")]
        public async Task<IActionResult> PersonalDetails()
        {
            try
            {
                var sessionId = GetOrCreateSessionId();
                var application = await _unitOfWork.Applications.GetDraftApplicationAsync(sessionId);

                if (application?.PersonalDetail != null)
                    return View(application.PersonalDetail);

                return View(new PersonalDetail());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading personal details");
                return View("Error");
            }
        }

        [HttpPost("personal")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonalDetails(PersonalDetail model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var sessionId = GetOrCreateSessionId();
                var application = await GetOrCreateDraftApplication(sessionId);

                await _unitOfWork.Applications.AddPersonalDetailsAsync(application, model);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("EducationDetails");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving personal details");
                ModelState.AddModelError("", "An error occurred while saving your details");
                return View(model);
            }
        }

        [HttpGet("education")]
        public async Task<IActionResult> EducationDetails()
        {
            try
            {
                var sessionId = GetOrCreateSessionId();
                var application = await _unitOfWork.Applications.GetDraftApplicationAsync(sessionId);

                if (application == null)
                    return RedirectToAction("PersonalDetails");

                ViewBag.EducationList = application.EducationDetails.ToList();
                return View(new EducationDetail());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading education details");
                return View("Error");
            }
        }

        [HttpPost("education")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEducation(EducationDetail model)
        {
            if (!ModelState.IsValid)
            {
                var sessionId = GetOrCreateSessionId();
                var application = await _unitOfWork.Applications.GetDraftApplicationAsync(sessionId);
                ViewBag.EducationList = application?.EducationDetails.ToList() ?? new List<EducationDetail>();
                return View("EducationDetails", model);
            }

            try
            {
                var sessionId = GetOrCreateSessionId();
                var application = await GetOrCreateDraftApplication(sessionId);

                model.ApplicationDetailId = application.Id;
                await _unitOfWork.EducationDetails.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("EducationDetails");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving education details");
                ModelState.AddModelError("", "An error occurred while saving your education");
                return View("EducationDetails", model);
            }
        }

        [HttpGet("experience")]
        public async Task<IActionResult> ExperienceDetails()
        {
            try
            {
                var sessionId = GetOrCreateSessionId();
                var application = await _unitOfWork.Applications.GetDraftApplicationAsync(sessionId);

                if (application == null)
                    return RedirectToAction("PersonalDetails");

                ViewBag.ExperienceList = application.ExperienceDetails.ToList();
                return View(new ExperienceDetail());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading experience details");
                return View("Error");
            }
        }

        [HttpPost("experience")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExperience(ExperienceDetail model)
        {
            if (!ModelState.IsValid)
            {
                var sessionId = GetOrCreateSessionId();
                var application = await _unitOfWork.Applications.GetDraftApplicationAsync(sessionId);
                ViewBag.ExperienceList = application?.ExperienceDetails.ToList() ?? new List<ExperienceDetail>();
                return View("ExperienceDetails", model);
            }

            try
            {
                var sessionId = GetOrCreateSessionId();
                var application = await GetOrCreateDraftApplication(sessionId);

                model.ApplicationDetailId = application.Id;
                await _unitOfWork.ExperienceDetails.AddAsync(model);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction("ExperienceDetails");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving experience details");
                ModelState.AddModelError("", "An error occurred while saving your experience");
                return View("ExperienceDetails", model);
            }
        }

        [HttpGet("review")]
        public async Task<IActionResult> Review()
        {
            try
            {
                var sessionId = GetOrCreateSessionId();
                var application = await _unitOfWork.Applications.GetDraftApplicationAsync(sessionId);

                if (application == null || !IsApplicationComplete(application))
                    return RedirectToAction("PersonalDetails");

                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading review page");
                return View("Error");
            }
        }

        [HttpPost("submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit()
        {
            try
            {
                var sessionId = GetOrCreateSessionId();
                var application = await _unitOfWork.Applications.GetDraftApplicationAsync(sessionId);

                if (application == null || !IsApplicationComplete(application))
                {
                    TempData["Error"] = "Please complete all sections before submitting";
                    return RedirectToAction("Review");
                }

                if (application.PersonalDetail != null &&
                    await _unitOfWork.Applications.HasExistingApplicationAsync(application.PersonalDetail.Email))
                {
                    TempData["Error"] = "An application already exists for this email address";
                    return RedirectToAction("Review");
                }

                application.ApplicationId = await _appNumberGenerator.GenerateAsync();
                await _unitOfWork.Applications.SubmitApplicationAsync(application);
                await _unitOfWork.CompleteAsync();

                HttpContext.Session.Remove("ApplicationSessionId");

                return RedirectToAction("Confirmation", new { applicationId = application.ApplicationId });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Maximum daily application limit"))
            {
                TempData["Error"] = "We've reached our maximum applications for today. Please try again tomorrow.";
                return RedirectToAction("Review");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting application");
                TempData["Error"] = "An error occurred while submitting your application";
                return RedirectToAction("Review");
            }
        }

        [HttpGet("confirmation/{applicationId}")]
        public IActionResult Confirmation(string applicationId)
        {
            ViewBag.ApplicationId = applicationId;
            return View();
        }

        private string GetOrCreateSessionId()
        {
            var sessionId = HttpContext.Session.GetString("ApplicationSessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("ApplicationSessionId", sessionId);
            }
            return sessionId;
        }

        private async Task<ApplicationDetail> GetOrCreateDraftApplication(string sessionId)
        {
            var application = await _unitOfWork.Applications.GetDraftApplicationAsync(sessionId);
            if (application == null)
            {
                application = await _unitOfWork.Applications.CreateDraftApplicationAsync(sessionId);
                await _unitOfWork.CompleteAsync();
            }
            return application;
        }

        private bool IsApplicationComplete(ApplicationDetail application)
        {
            return application.PersonalDetail != null &&
                   application.EducationDetails.Any() &&
                   application.ExperienceDetails.Any();
        }
    }
}
