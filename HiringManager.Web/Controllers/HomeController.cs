using HiringManager.DataAccess.Repository;
using HiringManager.Models;
using HiringManager.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HiringManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IApplicationRepository applicationRepository, ILogger<HomeController> logger)
        {
            _applicationRepository = applicationRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var applications = await _applicationRepository.GetAllSubmittedApplicationsAsync();
            var viewModel = new ApplicationListViewModel
            {
                Applications = applications
            };
            return View(viewModel);
        }

        public async Task<IActionResult> EditApplication(string applicationId)
        {
            try
            {
                var viewModel = await _applicationRepository.GetSubmittedApplicationForEditAsync(applicationId);
                if (viewModel == null)
                {
                    TempData["ErrorMessage"] = "Application not found";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("ApplicationForm", "Applications", new
                {
                    sessionId = viewModel.ApplicationDetails.SessionId,
                    stage = 1
                });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading application";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> ViewApplicationDetails(string id)
        {
            var application = await _applicationRepository.GetApplicationBySessionIdAsync(id);
            if (application == null || !application.IsSubmitted)
            {
                return NotFound();
            }

            return PartialView("_ApplicationDetails", application);
        }

        public async Task<IActionResult> ViewApplication(string id)
        {
            var application = await _applicationRepository.GetApplicationForViewAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View("ViewApplication", application);
        }

        [HttpGet]
        public async Task<IActionResult> ViewApplicationPartial(string id)
        {
            try
            {
                var application = await _applicationRepository.GetApplicationForViewAsync(id);
                if (application == null) return NotFound();

                return PartialView("_ViewApplication", application);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateApplication(string applicationId)
        {
            try
            {
                var result = await _applicationRepository.DeactivateApplicationAsync(applicationId);
                if (result)
                {
                    TempData["SuccessMessage"] = "Application deactivated successfully";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "Application not found";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating application");
                TempData["ErrorMessage"] = "Error deactivating application";
                return RedirectToAction("Index");
            }
        }
    }
}
