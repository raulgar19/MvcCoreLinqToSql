using AspNetCoreGeneratedDocument;
using Microsoft.Data.SqlClient;
using MvcCoreLinqToSql.Models;
using System.Data;
using System.Globalization;

namespace MvcCoreLinqToSql.Repositories
{
    public class RepositoryEmpleados
    {
        private DataTable tablaEmpleados;

        public RepositoryEmpleados()
        {
            string connectionString = "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from EMP";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);

            this.tablaEmpleados = new DataTable();

            ad.Fill(this.tablaEmpleados);
        }

        public List<Empleado> GetEmpleados()
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable() select datos;

            List<Empleado> empleados = new List<Empleado>();

            foreach (var row in consulta)
            {
                Empleado empleado = new Empleado();

                empleado.IdEmpleado = row.Field<int>("EMP_NO");
                empleado.Apellido = row.Field<string>("APELLIDO");
                empleado.Oficio = row.Field<string>("OFICIO");
                empleado.Salario = row.Field<int>("SALARIO");
                empleado.IdDepartamento = row.Field<int>("DEPT_NO");

                empleados.Add(empleado);
            }

            return empleados;
        }

        public Empleado FindEmpleado(int idEmpleado)
        {
            var consulta = 
                from datos in this.tablaEmpleados.AsEnumerable()
                where datos.Field<int>("EMP_NO") == idEmpleado
                select datos;
            
            var row = consulta.First();

            Empleado empleado = new Empleado();

            empleado.IdEmpleado = row.Field<int>("EMP_NO");
            empleado.Apellido = row.Field<string>("APELLIDO");
            empleado.Oficio = row.Field<string>("OFICIO");
            empleado.Salario = row.Field<int>("SALARIO");
            empleado.IdDepartamento = row.Field<int>("DEPT_NO");

            return empleado;
        }
    }
}
