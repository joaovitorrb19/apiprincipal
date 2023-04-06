using ApiPrincipal.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Controllers {
    public class HomeController : Controller {
        
        public HomeController()
        {
            
        }

        [Authorize]
        public ActionResult Index(){

            return View();
        }
    }
}