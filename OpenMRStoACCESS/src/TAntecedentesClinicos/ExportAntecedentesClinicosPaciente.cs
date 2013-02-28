using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm.TAntecedentesClinicos
{
    public class ExportAntecedentesClinicosPaciente
    {
        public void exportAntecedentesClinicosPaciente(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {

            try
            {
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                //InsertUtils insertUtil = new InsertUtils();

                String startDateMySQL = startDate.Year + "/" + startDate.Month + "/" + startDate.Day;
                String endDateMySQL = endDate.Year + "/" + endDate.Month + "/" + endDate.Day;

                String  sqlSelect = " SELECT   distinct p.patient_id, ";
                        sqlSelect += "			cn.name as codantecendentes, ";
                        sqlSelect += "			p.nid, ";
                        sqlSelect += "			o.obs_datetime as datadiagnostico, ";
                        sqlSelect += "		    case o.value_coded ";
                        sqlSelect += "			     when 1065 then 'SIM' ";
                        sqlSelect += "			     when 1066 then 'NAO' ";
                        sqlSelect += "			     when 1457 then 'SEM INFORMACAO' ";
                        sqlSelect += "		    else 'SIM' end as estado ";
                        sqlSelect += " FROM	    t_paciente p ";
                        sqlSelect += "		    inner join encounter e on p.patient_id=e.patient_id ";
                        sqlSelect += "		    inner join obs o on o.encounter_id=e.encounter_id and e.patient_id=o.person_id ";
                        sqlSelect += "		    inner join concept_name cn on cn.concept_id=o.concept_id and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED' ";
                        sqlSelect += " WHERE	e.encounter_type in (5,7) and ";
                        sqlSelect += "		    o.concept_id in (42, 5042, 836, 5334, 5340, 507,1379, 1380, 1381, 5018, 5339, 5027, 1429,5030,5965,5050,204,1215) and ";
                        sqlSelect += "		    o.voided=0 and ";
                        sqlSelect += "		    cn.voided=0 and ";
                        sqlSelect += "		    e.voided=0 and ";
                        sqlSelect += "		    p.datanasc is not null and ";
                        sqlSelect += "		    p.nid is not null and e.encounter_datetime=p.dataabertura and "; 
                        sqlSelect += "          e.encounter_datetime between  '" + startDateMySQL + "' and '" + endDateMySQL + "' and ";
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

                        sqlInsert = "Insert into t_antecedentesclinicospaciente(codantecendentes,nid,datadiagnostico,Estado) values(";
                        sqlInsert += "'" + readerSource.GetString(1) + "','" + readerSource.GetString(2) + "',cdate('" + readerSource.GetMySqlDateTime(3) + "'),'" + readerSource.GetString(4) + "')";


                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();


                      }
                    
                }

                readerSource.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Houve erro ao Exportar T_ANTECEDENTESCLINICOSPACIENTE: " + e.Message);

            }
        }
    }
}
