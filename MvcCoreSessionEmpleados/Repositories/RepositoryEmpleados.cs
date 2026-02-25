using Microsoft.EntityFrameworkCore;
using MvcCoreSessionEmpleados.Data;
using MvcCoreSessionEmpleados.Models;

namespace MvcCoreSessionEmpleados.Repositories
{
    public class RepositoryEmpleados : IRepositoryEmpleados
    {

        private HospitalContext _context;

        public RepositoryEmpleados(HospitalContext _context)
        {
            this._context = _context;
        }


        public async Task<Empleado> FindEmpleadoAsync(int idEmpleado)
        {
            var consulta = from datos in this._context.Empleados
                           where datos.IdEmpleado == idEmpleado
                           select datos;

            return await consulta.FirstOrDefaultAsync();
        }

     

        public Task<List<Empleado>> GetEmpleadosAsync()
        {
            var consulta = from datos in this._context.Empleados
                           select datos;

            return  consulta.ToListAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosNotSession(List<int> idsEmpleados)
        {
            var consulta = from datos in this._context.Empleados
                           where !(idsEmpleados.Contains(datos.IdEmpleado))
                           select datos;

            return await consulta.ToListAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosSession(List<int> idsEmpleados)
        {
            var consulta = from datos in this._context.Empleados
                           where idsEmpleados.Contains(datos.IdEmpleado)
                           select datos;

            return await consulta.ToListAsync();
        }
    }
}
