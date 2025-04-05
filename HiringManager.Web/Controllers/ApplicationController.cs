using HiringManager.DataAccess.Repository;
using HiringManager.DataAccess.Services;
using HiringManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using HiringManager.Models.ViewModel;

namespace HiringManager.Web.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IIdGeneratorService _idGeneratorService;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(
            IApplicationRepository applicationRepository,
            IIdGeneratorService idGeneratorService,
            ILogger<ApplicationsController> logger)
        {
            _applicationRepository = applicationRepository;
            _idGeneratorService = idGeneratorService;
            _logger = logger;
        }

        public async Task<IActionResult> ApplicationForm(string? sessionId, int? stage)
        {
            try
            {
                sessionId ??= Guid.NewGuid().ToString();
                var viewModel = await _applicationRepository.GetApplicationFormViewModelAsync(sessionId);

                // Override stage if explicitly requested
                if (stage.HasValue && stage.Value >= 1 && stage.Value <= 3)
                {
                    viewModel.CurrentStage = stage.Value;
                    viewModel.ApplicationDetails.StageId = stage.Value;
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading application form");
                TempData["ErrorMessage"] = "An error occurred while loading the form. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePersonalDetails(PersonalDetails personalDetails, string sessionId, string action)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await GetErrorViewModel(sessionId, personalDetails);
                return View("ApplicationForm", viewModel);
            }

            try
            {
                var hasExistingApplication = await _applicationRepository.HasExistingApplicationAsync(personalDetails.Email);
                if (hasExistingApplication && action != "SaveDraft")
                {
                    ModelState.AddModelError("PersonalDetails.Email", "An application with this email already exists.");
                    var viewModel = await GetErrorViewModel(sessionId, personalDetails);
                    return View("ApplicationForm", viewModel);
                }

                await _applicationRepository.SavePersonalDetailsAsync(personalDetails, sessionId);

                if (action == "SaveDraft")
                {
                    TempData["SuccessMessage"] = "Personal details saved as draft.";
                    return RedirectToAction("ApplicationForm", new { sessionId });
                }

                return RedirectToAction("ApplicationForm", new { sessionId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving personal details");
                TempData["ErrorMessage"] = "An error occurred while saving your information. Please try again.";
                return RedirectToAction("ApplicationForm", new { sessionId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEducationDetails(EducationDetails educationDetails, string sessionId, string action)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await GetErrorViewModel(sessionId, educationDetails: educationDetails);
                return View("ApplicationForm", viewModel);
            }

            try
            {
                await _applicationRepository.SaveEducationDetailsAsync(educationDetails, sessionId);

                if (action == "SaveDraft")
                {
                    TempData["SuccessMessage"] = "Education details saved as draft.";
                    return RedirectToAction("ApplicationForm", new { sessionId });
                }
                else if (action == "Back")
                {
                    return RedirectToAction("ApplicationForm", new { sessionId, stage = 1 });
                }

                return RedirectToAction("ApplicationForm", new { sessionId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving education details");
                TempData["ErrorMessage"] = "An error occurred while saving your information. Please try again.";
                return RedirectToAction("ApplicationForm", new { sessionId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveExperienceDetails(ExperienceDetails experienceDetails, string sessionId, string action)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await GetErrorViewModel(sessionId, experienceDetails: experienceDetails);
                return View("ApplicationForm", viewModel);
            }

            try
            {
                await _applicationRepository.SaveExperienceDetailsAsync(experienceDetails, sessionId);

                if (action == "SaveDraft")
                {
                    TempData["SuccessMessage"] = "Experience details saved as draft.";
                    return RedirectToAction("ApplicationForm", new { sessionId });
                }
                else if (action == "Back")
                {
                    return RedirectToAction("ApplicationForm", new { sessionId, stage = 2 });
                }
                else if (action == "SubmitApplication")
                {
                    return await SubmitApplication(sessionId);
                }

                return RedirectToAction("ApplicationForm", new { sessionId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving experience details");
                TempData["ErrorMessage"] = "An error occurred while saving your information. Please try again.";
                return RedirectToAction("ApplicationForm", new { sessionId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(string sessionId)
        {
            // Data is already saved in each step, so we just redirect with message
            TempData["SuccessMessage"] = "Application progress saved as draft.";
            return RedirectToAction("ApplicationForm", new { sessionId });
        }

        private async Task<IActionResult> SubmitApplication(string sessionId)
        {
            try
            {
                var application = await _applicationRepository.GetApplicationBySessionIdAsync(sessionId);

                if (application == null)
                {
                    TempData["ErrorMessage"] = "Application not found. Please start a new application.";
                    return RedirectToAction("ApplicationForm");
                }

                var errors = ValidateApplicationComplete(application);
                if (errors.Any())
                {
                    TempData["ValidationErrors"] = errors;
                    return RedirectToAction("ApplicationForm", new { sessionId });
                }

                await _applicationRepository.SubmitApplicationAsync(sessionId, _idGeneratorService);
                TempData["SuccessMessage"] = $"Application submitted successfully! Your ID: {application.ApplicationId}";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting application");
                TempData["ErrorMessage"] = "An error occurred while submitting. Please try again.";
                return RedirectToAction("ApplicationForm", new { sessionId });
            }
        }

        private async Task<ApplicationFormViewModel> GetErrorViewModel(
            string sessionId,
            PersonalDetails? personalDetails = null,
            EducationDetails? educationDetails = null,
            ExperienceDetails? experienceDetails = null)
        {
            var viewModel = await _applicationRepository.GetApplicationFormViewModelAsync(sessionId);

            if (personalDetails != null)
                viewModel.PersonalDetails = personalDetails;
            if (educationDetails != null)
                viewModel.EducationDetails = educationDetails;
            if (experienceDetails != null)
                viewModel.ExperienceDetails = experienceDetails;

            return viewModel;
        }

        private List<string> ValidateApplicationComplete(ApplicationDetails application)
        {
            var errors = new List<string>();

            if (application.PersonalDetails == null)
                errors.Add("Personal Details section is incomplete");
            if (application.EducationDetails == null)
                errors.Add("Education Details section is incomplete");
            if (application.ExperienceDetails == null)
                errors.Add("Experience Details section is incomplete");

            return errors;
        }
    }
}