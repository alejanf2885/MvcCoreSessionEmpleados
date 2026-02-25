using MvcCoreSessionEmpleados.Models;

namespace MvcCoreSessionEmpleados.Repositories
{
    public interface IRepositoryEmpleados
    {
        Task<List<Empleado>> GetEmpleadosAsync();

        Task<Empleado> FindEmpleadoAsync(int idEmpleado);

        Task<List<Empleado>> GetEmpleadosSession(List<int> idsEmpleados);

        Task<List<Empleado>> GetEmpleadosNotSession(List<int> idsEmpleados);
    } 
}
