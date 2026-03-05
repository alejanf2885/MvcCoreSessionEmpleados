using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private IMemoryCache cache;

        public EmpleadosController(IRepositoryEmpleados repo, IMemoryCache cache)
        {
            this.repo = repo;
            this.cache = cache;
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


        public async Task<IActionResult> SessionEmpleadosV5
           (int? idempleado, int? idfavorito)
        {

            if(idfavorito != null)
            {
                List<Empleado> empleadosFavoritos;
                if(this.cache.Get("FAVORITOS") == null)
                {
                    empleadosFavoritos = new List<Empleado>();
                }
                else
                {
                    empleadosFavoritos =
                        this.cache.Get<List<Empleado>>("FAVORITOS");
                }
                Empleado empleadoFavorito =
                    await this.repo.FindEmpleadoAsync(idfavorito.Value);
                empleadosFavoritos.Add(empleadoFavorito);
                this.cache.Set("FAVORITOS", empleadosFavoritos);
            }


            if (idempleado != null)
            {
                List<int> idsEmpleadosList;
                if (HttpContext.Session.GetObject<List<int>>
                    ("IDSEMPLEADOS") != null)
                {
                    //RECUPERAMOS LA COLECCION
                    idsEmpleadosList =
                        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                else
                {
                    //CREAMOS LA COLECCION
                    idsEmpleadosList = new List<int>();
                }
                //ALMACENAMOS EL ID DEL EMPLEADO
                idsEmpleadosList.Add(idempleado.Value);
                //ALMACENAMOS EN SESSION LOS DATOS
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleadosList);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                    + idsEmpleadosList.Count;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
           
        }




        public async Task<IActionResult> EmpleadosAlmacenadosV5(int? ideliminar)
        {
            //RECUPERAMOS LA COLECCION DE SESSION
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados en Session";
                return View();
            }
            else
            {

                if(ideliminar != null)
                {
                    idsEmpleados.Remove(ideliminar.Value);
                    if(idsEmpleados.Count() == 0)
                    {
                        HttpContext.Session.Remove("IDSEMPLEADOS");

                    }
                    else
                    {
                        HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);

                    }
                }

                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSession(idsEmpleados);
                return View(empleados);
            }
        }

        public IActionResult EmpleadosFavoritos()
        {
            return View();
            
        }

        public IActionResult DeleteEmpleadoFavorito(int idempleado)
        {

            List<Empleado> empleados = this.cache.Get<List<Empleado>>("FAVORITOS");
            Empleado empleado = empleados.Find(z => z.IdEmpleado == idempleado);
            empleados.Remove(empleado);

            if(empleados.Count() == 0)
            {
                this.cache.Remove("FAVORITO");
            }
            else
            {
                this.cache.Set("FAVORITOS", empleados);
            }

                return RedirectToAction("EmpleadosFavoritos");
        }
    }
}
