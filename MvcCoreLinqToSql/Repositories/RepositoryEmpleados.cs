using AspNetCoreGeneratedDocument;
using Microsoft.Data.SqlClient;
using MvcCoreLinqToSql.Models;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;

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

        public List<Empleado> GetEmpleadosOficioSalario(string oficio, int salario)
        {
            var consulta = from datos in 
                              this.tablaEmpleados.AsEnumerable() 
                          where datos.Field<string>("OFICIO") == oficio 
                          && datos.Field<int>("SALARIO") >= salario 
                          select datos;

            if (consulta.Count() == 0) {
                return null;
            }
            else
            {
                List<Empleado> empleados = new List<Empleado>();

                foreach(var row in consulta)
                {
                    Empleado empleado = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO"),
                    };

                    empleados.Add(empleado);
                }

                return empleados;
            }
        }

        public ResumenEmpleados GetEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;

            if(consulta.Count() == 0)
            {
                ResumenEmpleados model = new ResumenEmpleados();
                model.Personas = 0;
                model.MaximoSalario = 0;
                model.MediaSalarial = 0;
                model.Empleados = null;

                return model;
            }
            else
            {
                consulta = consulta.OrderBy(z => z.Field<int>("SALARIO"));

                int personas = consulta.Count();
                int maximo = consulta.Max(x => x.Field<int>("SALARIO"));
                double media = consulta.Average(x => x.Field<int>("SALARIO"));

                List<Empleado> empleados = new List<Empleado>();

                foreach (var row in consulta)
                {
                    Empleado empleado = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };

                    empleados.Add(empleado);
                }

                ResumenEmpleados model = new ResumenEmpleados();
                model.Personas = personas;
                model.MaximoSalario = maximo;
                model.MediaSalarial = media;
                model.Empleados = empleados;

                return model;
            }
        }

        public List<string> GetOficios()
        {
            var consulta = (from datos in 
                                this.tablaEmpleados.AsEnumerable()
                            select datos.Field<string>("OFICIO")).Distinct();

            return consulta.ToList();
        }
    }
}
