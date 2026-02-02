using Microsoft.Data.SqlClient;
using MvcCoreLinqToSql.Models;
using System.Data;

namespace MvcCoreLinqToSql.Repositories
{
    public class RepositoryEnfermos
    {
        private SqlConnection cn;
        private SqlCommand com;
        private DataTable tablaEnfermos ;

        public RepositoryEnfermos() 
        {
            string connectionString = "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=HOSPITAL;User ID=SA;Trust Server Certificate=True";
            string sql = "select * from ENFERMO";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

            this.tablaEnfermos = new DataTable();

            ad.Fill(this.tablaEnfermos);
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in tablaEnfermos.AsEnumerable() select datos;

            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Enfermo> enfermos = new List<Enfermo>();

                var row = consulta.First();
                
                Enfermo enfermo = new Enfermo();
                enfermo.Inscripcion = row.Field<string>("INSCRIPCION");
                enfermo.Apellido = row.Field<string>("APELLIDO");
                enfermo.Direccion = row.Field<string>("DIRECCION");
                enfermo.FechaNac = row.Field<DateTime>("FECHA_NAC");
                enfermo.Sexo = row.Field<string>("S");
                enfermo.SeguridadSocial = row.Field<string>("NSS");

                enfermos.Add(enfermo);

                return enfermos;
            }
        }

        public Enfermo GetEnfermoInscripcion(string inscripcion)
        {
            var consulta = from datos in tablaEnfermos.AsEnumerable() 
                           where datos.Field<string>("INSCRIPCION") == inscripcion 
                           select datos ;

            Enfermo enfermo = new Enfermo();

            foreach (var row in consulta)
            {
                enfermo.Inscripcion = row.Field<string>("INSCRIPCION");
                enfermo.Apellido = row.Field<string>("APELLIDO");
                enfermo.Direccion = row.Field<string>("DIRECCION");
                enfermo.FechaNac = row.Field<DateTime>("FECHA_NAC");
                enfermo.Sexo = row.Field<string>("S");
                enfermo.SeguridadSocial = row.Field<string>("NSS");
            }

            return enfermo;
        }

        public void DeleteEnfermo(string inscripcion)
        {
            string sql = "delete from ENFERMO where INSCRIPCION = @Inscripcion";

            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();

            this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}