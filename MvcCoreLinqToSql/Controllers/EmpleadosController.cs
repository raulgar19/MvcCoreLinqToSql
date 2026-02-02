using Microsoft.AspNetCore.Mvc;
using MvcCoreLinqToSql.Models;
using MvcCoreLinqToSql.Repositories;

namespace MvcCoreLinqToSql.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController()
        {
            this.repo = new RepositoryEmpleados();

        }

        public IActionResult Index()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }

        public IActionResult Details(int id)
        {
            Empleado empleado = this.repo.FindEmpleado(id);
            return View(empleado);
        }

        public IActionResult BuscadorEmpleados()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuscadorEmpleados(string oficio, int salario)
        {
            List<Empleado> empleados = this.repo.GetEmpleadosOficioSalario(oficio, salario);
            if (empleados == null)
            {
                ViewData["MENSAJE"] = "No existen empledo con oficio " + oficio + " y salario mayor a " + salario;
                return View();
            }
            return View(empleados);
        }

        public IActionResult DatosEmpleados()
        {
            List<string> oficios = this.repo.GetOficios();
            ViewData["OFICIOS"] = oficios;
            return View();
        }

        [HttpPost]
        public IActionResult DatosEmpleados(string oficio)
        {
            ResumenEmpleados model = this.repo.GetEmpleadosOficio(oficio);
            List<string> oficios = this.repo.GetOficios();
            ViewData["OFICIOS"] = oficios;
            return View(model);
        }
    }
}