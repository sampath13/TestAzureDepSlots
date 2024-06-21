using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestAzureDepSlots.Data;
using TestAzureDepSlots.Models;

namespace TestAzureDepSlots.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Repository _repository;

    public HomeController(ILogger<HomeController> logger, Repository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IActionResult Index()
    {
        ViewBag.Employees = _repository.Employees.ToList();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult Register(Employee emp)
    {
        _repository.Employees.Add(emp);
        _repository.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}