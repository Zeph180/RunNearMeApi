using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

public class Authentication : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}