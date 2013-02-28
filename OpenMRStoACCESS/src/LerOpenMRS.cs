using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MySql.Data.MySqlClient;

namespace ImportacaoOpenmrsForm
{
    public class LerOpenMRS
    {
        private OpenMRSUtils openmrs;

        public ArrayList tPaciente()
        {
            openmrs = new OpenMRSUtils();
            ArrayList lista = new ArrayList();
            String query = "select e."+
                "from enconter e,patient_identifier pi,person p,obs o";
            MySqlCommand cmd = new MySqlCommand(query, openmrs.mysqlConexao());
            openmrs.abreConexao();
            try
            {
                cmd.ExecuteNonQuery();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(1);
                    lista.Add("Nome");
                }
            }
            catch
            {
            }
            openmrs.fechaConexao();
            return lista;
        }
    }
}
