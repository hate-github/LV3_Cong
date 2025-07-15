using LV3_Cong.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LV3_Cong.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(model);
        }
    }
}
