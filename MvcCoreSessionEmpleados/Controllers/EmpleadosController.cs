using Microsoft.AspNetCore.Mvc;
using MvcCoreSessionEmpleados.Extensions;
using MvcCoreSessionEmpleados.Models;
using MvcCoreSessionEmpleados.Repositories;
using NuGet.Packaging.Signing;
using System.Threading.Tasks;

namespace MvcCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {

        private IRepositoryEmpleados repo;

        public EmpleadosController(IRepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if (salario != null)
            {
                int sumaTotal = 0;
                if (HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    sumaTotal =
                        HttpContext.Session.GetObject<int>("SUMASALARIAL");
                }
                sumaTotal += salario.Value;
                HttpContext.Session.SetObject("SUMASALARIAL", sumaTotal);
                ViewData["MENSAJE"] = "Salario almacenado: " + salario;
            }

            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);

        }


        public IActionResult SumaSalarial()
        {
            return View();
        }


        public async Task<IActionResult> SessionEmpleados
            (int? idempleado)
        {
            if (idempleado != null)
            {
                Empleado empleado =
                    await this.repo.FindEmpleadoAsync(idempleado.Value);

                List<Empleado> empleadosList;
                if (HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    empleadosList =
                     HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                }
                else
                {
                    empleadosList = new List<Empleado>();
                }

                empleadosList.Add(empleado);

                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);

                ViewData["MENSAJE"] = "Empleado " + empleado.Apellido + " almacenado correctamente";

            }

            List<Empleado> empleados =
                await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult EmpleadosAlmacenados()
        {

            return View();
        }


        public async Task<IActionResult> SessionEmpleadosObj(int? idempleado)
        {

            if (idempleado != null)
            {
                Empleado empleado =
                    await this.repo.FindEmpleadoAsync(idempleado.Value);

                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") != null)
                {
                    idsEmpleados =
                     HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                else
                {
                    idsEmpleados = new List<int>();
                }

                idsEmpleados.Add(empleado.IdEmpleado);

                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);

                ViewData["MENSAJE"] = "Empleado " + empleado.Apellido + " almacenado correctamente";

            }

            List<Empleado> empleados =
                await this.repo.GetEmpleadosAsync();


            return View(empleados);

        }


        public async Task<IActionResult> EmpleadosAlmacenadosObj()
        {
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");

            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados en session";
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSession(idsEmpleados);

                return View(empleados);
            }


            return View();
        }











        public async Task<IActionResult> SessionEmpleadosV4(int? idempleado)
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") ?? new List<int>();

            if (idempleado.HasValue && !idsEmpleados.Contains(idempleado.Value))
            {
                Empleado empleado = await this.repo.FindEmpleadoAsync(idempleado.Value);

                if (empleado != null)
                {
                    idsEmpleados.Add(empleado.IdEmpleado);
                    ViewData["MENSAJE"] = $"Empleado {empleado.Apellido} almacenado correctamente";
                }
            }

            HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);

            List<Empleado> empleados;

            if (idsEmpleados.Any())
            {
                empleados = await this.repo.GetEmpleadosNotSession(idsEmpleados);
            }
            else
            {
                empleados = await this.repo.GetEmpleadosAsync();
            }

            return View(empleados);
        }


        public async Task<IActionResult> EmpleadosAlmacenadosV4()
        {
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");

            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados en session";
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSession(idsEmpleados);

                return View(empleados);
            }


            return View();
        }
    }
}
