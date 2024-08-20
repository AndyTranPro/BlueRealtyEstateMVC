using BSR.Models;
using BSR.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BSR.Controllers;

public class HomesController: Controller
{
    private readonly HomeService _homeService;
    private readonly AddressService _addressService;
    private readonly ILogger<HomesController> _logger;

    public HomesController(HomeService homeService, AddressService addressService, ILogger<HomesController> logger)
    {
        _homeService = homeService;
        _addressService = addressService;
        _logger = logger;
    }

    public async Task<IActionResult> GetCities(string state)
    {
        var cities = await _addressService.GetCitiesInState(state);
        return Ok(cities);
    }

    [HttpGet]
    public async Task<IActionResult> AddHomeView()
    {
        var statesResult = await _addressService.GetAmericanStates();

        var addHomeViewModel = new AddHomeViewModel
        {
            States = statesResult,
            Cities = new List<string>()
        };

        return View(addHomeViewModel);
    }

    public async Task<IActionResult> Index(int? minPrice, int? maxPrice, int? minArea, int? maxArea, int? minBath, int? minCar, int? minBed, string? state, string? city, int pageNumber = 1, int pageSize = 10)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var homesViewModel = new HomesViewModel();

        try
        {
            //modified
            var homes = _homeService.GetHomes(minPrice, maxPrice, minArea, maxArea, minBath, minCar, minBed, state, city);

            int totalItems = homes.Count();
            homes = homes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            //new
            homesViewModel.States = await _addressService.GetAmericanStates();
            homesViewModel.Homes = homes;
            homesViewModel.PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            };

            ViewBag.HomesCount = totalItems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching homes from the database: {ex.Message}");
            TempData["ErrorMessage"] = $"Error fetching homes from the database: {ex.Message}";
        }

        homesViewModel.MinPrice = minPrice;
        homesViewModel.MaxPrice = maxPrice;
        homesViewModel.MinArea = minArea;
        homesViewModel.MaxArea = maxArea;
        homesViewModel.MinBathrooms = minBath; //new
        homesViewModel.MinGarage = minCar; //new
        homesViewModel.MinBedrooms = minBed;//new
        homesViewModel.State = state;//new
        homesViewModel.City = city;//new

        stopwatch.Stop();

        ViewBag.LoadTestTime = stopwatch.Elapsed.TotalSeconds.ToString("F4");

        return View(homesViewModel);
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
            _logger.LogInformation("Home added successfully");
            return RedirectToAction("Index", "Homes"); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding home: {ex.Message}");
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
            _logger.LogInformation("Home updated successfully");
            return RedirectToAction("Index", "Homes");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating home: {ex.Message}");
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
            _logger.LogInformation("Home deleted successfully");
            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting home: {ex.Message}");
            TempData["ErrorMessage"] = $"Error deleting home: {ex.Message}";
            return BadRequest(new { message = ex.Message });
        }
    }
}
