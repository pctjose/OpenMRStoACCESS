using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using ImportacaoOpenmrsForm.Utils;

namespace ImportacaoOpenmrsForm.TContacto
{
    public class ExportTContacto
    {
        public void exportContacto(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String startDateMySQL = startDate.Year + "/" + startDate.Month + "/" + startDate.Day;
                String endDateMySQL = endDate.Year + "/" + endDate.Month + "/" + endDate.Day;

                String sqlSelect = " SELECT	p.nid,nome.nome,apelido.apelido,telefone.telefone ";
                sqlSelect += " FROM	t_paciente p ";
                sqlSelect += "		inner join (SELECT	e.patient_id, ";
                sqlSelect += "				e.encounter_id,	 ";
                sqlSelect += "				o.value_text as nome ";
                sqlSelect += "		 FROM 	encounter e  ";
                sqlSelect += "				inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "				WHERE 	e.encounter_type in (5,7) and o.concept_id=1441 and ";
                sqlSelect += "						o.voided=0 and e.voided=0  ";
                sqlSelect += "		 ) nome  ";
                sqlSelect += "		 on p.patient_id=nome.patient_id ";
                sqlSelect += "		left join (SELECT	e.patient_id, ";
                sqlSelect += "				e.encounter_id,	 ";
                sqlSelect += "				o.value_text as apelido ";
                sqlSelect += "		 FROM 	encounter e  ";
                sqlSelect += "				inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "				WHERE 	e.encounter_type in (5,7) and o.concept_id=1442 and ";
                sqlSelect += "						o.voided=0 and e.voided=0  ";
                sqlSelect += "		 ) apelido  ";
                sqlSelect += "		 on p.patient_id=apelido.patient_id ";
                sqlSelect += "		left join (SELECT	e.patient_id, ";
                sqlSelect += "				e.encounter_id,	 ";
                sqlSelect += "				o.value_text as telefone ";
                sqlSelect += "		 FROM 	encounter e  ";
                sqlSelect += "				inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "				WHERE 	e.encounter_type in (5,7) and o.concept_id=1611 and ";
                sqlSelect += "						o.voided=0 and e.voided=0  ";
                sqlSelect += "		 ) telefone  ";
                sqlSelect += "		 on p.patient_id=telefone.patient_id ";
                sqlSelect += " WHERE	p.nid is not null and p.datanasc is not null  and "; 
                sqlSelect += "          p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "'";
                

                commandTarge.Connection = target;
                commandTarge.CommandType = CommandType.Text;

                commandSource.Connection = source;
                commandSource.CommandType = CommandType.Text;
                commandSource.CommandText = sqlSelect;
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {
                    while (readerSource.Read())
                    {

                        sqlInsert = "Insert into t_contacto(nid,nome) values(";
                        sqlInsert += "'" + readerSource.GetString(0) + "','"+readerSource.GetString(1)+"')";
                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        insertUtil.updateStringValue("t_contacto", "apelido", commandTarge, readerSource, 2, "nid", readerSource.GetString(0));

                        insertUtil.updateStringValue("t_contacto", "telefone", commandTarge, readerSource, 3, "nid", readerSource.GetString(0));
                        
                        
                    }
                }
                readerSource.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_CONTACTO: " + e.Message);

            }
        }
    }
}
