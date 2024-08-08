using Microsoft.AspNetCore.Mvc;
using RealEstateApplication.Models;
using RealEstateApplication.Services;

namespace RealEstateApplication.Controllers
{
    public class HomesController : Controller
    {
        private readonly HomeService _homeService;

        public HomesController(HomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            var homesViewModel = new HomesViewModel();

            try
            {
                homesViewModel.Homes = _homeService.GetHomes();
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error getting homes from the database: {ex.Message}";
            }
            return View(homesViewModel);
        }

        [HttpGet]
        public IActionResult AddHomeView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddHome(Home newHome)
        {
            if (!ModelState.IsValid)
            {
                return View("AddHomeView", newHome);
            }

            try
            {
                _homeService.AddHome(newHome);
                TempData["SuccessMessage"] = "Home added successfully!";
                return RedirectToAction("Index", "Homes");
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding home: {ex.Message}";
                return View("AddHomeView", newHome);
            }
        }

        [HttpGet]
        public IActionResult HomeDetailView(int id)
        {
            var home = _homeService.GetHomeById(id);
            return View(home);
        }

        [HttpPost]
        public IActionResult Update(Home updatedHome)
        {
            if (!ModelState.IsValid)
            {
                return View("HomeDetailView", updatedHome);
            }

            try
            {
                _homeService.UpdateHome(updatedHome);
                TempData["SuccessMessage"] = "Home updated successfully!";
                return RedirectToAction("Index", "Homes");
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating home: {ex.Message}";
                return View("HomeDetailView", updatedHome);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _homeService.DeleteHome(id);
                TempData["SuccessMessage"] = "Home deleted successfully!";
                return new OkResult();
            } catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting home: {ex.Message}";
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
