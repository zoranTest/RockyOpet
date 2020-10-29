using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RockyOpet.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()        // desni klik na index i add a View
        {
            return View();
        }
    }
}
