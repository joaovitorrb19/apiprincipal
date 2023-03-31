using Microsoft.AspNetCore.Mvc;

namespace ApiPrincipal.Controllers{

    public class UsuarioController : Controller{

        public UsuarioController()
        {
            
        }
        
        [HttpGet]
        public ActionResult Login(){
            return View();
        }


    }

}