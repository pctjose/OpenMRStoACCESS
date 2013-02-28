using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImportacaoOpenmrsForm.Utils;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using System.Data;

namespace ImportacaoOpenmrsForm.TTratamentoTB
{
    public class ExportTTratamentoTB
    {
        public void exportDataTB(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            exportDataTratamentoTB(source, target, startDate, endDate);
            exportDataQuestionarioTB(source, target, startDate, endDate);
        }
        private void exportDataTratamentoTB(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            //try
            //{
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String startDateMySQL = startDate.Year + "/" + startDate.Month + "/" + startDate.Day;
                String endDateMySQL = endDate.Year + "/" + endDate.Month + "/" + endDate.Day;

                String sqlSelect = " SELECT	distinct p.nid, ";
                sqlSelect += "		dataInicio.dataInicio ";
                sqlSelect += " FROM	t_paciente p  ";
                sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id ";
                sqlSelect += "        inner join  ";
                sqlSelect += "        (	SELECT	o.person_id,e.encounter_id,o.value_datetime as dataInicio ";
                sqlSelect += "			FROM 	encounter e ";
                sqlSelect += "					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "			WHERE 	o.concept_id=1113  and o.voided=0 and e.voided=0 ";
                sqlSelect += "			UNION ";
                sqlSelect += "			SELECT	o.person_id,e.encounter_id,o.obs_datetime as dataInicio ";
                sqlSelect += "			FROM 	encounter e ";
                sqlSelect += "					inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "			WHERE 	o.concept_id=1268 and o.value_coded=1256 and o.voided=0 and e.voided=0 ";
                sqlSelect += "         ) dataInicio on dataInicio.encounter_id=e.encounter_id ";
                sqlSelect += " WHERE	";
                sqlSelect += "		e.voided=0 and p.nid is not null and  ";
                sqlSelect += "		p.datanasc is not null and dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "'";
                sqlSelect += " GROUP BY nid,dataInicio ";
                

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

                        sqlInsert = "Insert into t_tratamentoTB(nid,data) values('" + readerSource.GetString(0) + "','" + readerSource.GetString(1) + "')";

                        // sqlInsert = "Insert into t_tratamentoTB(nid,data) values('" + readerSource.GetString(0) + "','" + readerSource.GetString(1) + "ON DUPLICATE KEY UPDATE datafim='" + readerSource.GetString(2) + "')";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        //insertUtil.updateNumericValue("t_tratamentoTB", "data", commandTarge, readerSource, 1, "nid", readerSource.GetString(0));
                        //insertUtil.updateStringValue("t_tratamentoTB", "datafim", commandTarge, readerSource, 2, "nid", readerSource.GetString(0));
                    }
                }

                readerSource.Close();
                
                sqlSelect = "SELECT	p.nid, ";
                sqlSelect += "				datafim.datafim  ";
                sqlSelect += " FROM	t_paciente p   ";
                sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id  ";
                sqlSelect += "        inner join   ";
                sqlSelect += "		(	SELECT 	o.person_id,e.encounter_id,o.value_datetime as datafim ";
                sqlSelect += "            FROM 	encounter e  ";
                sqlSelect += "                    inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "            WHERE 	e.encounter_type in (6,9) and o.concept_id=6120 and o.voided=0 and e.voided=0 ";
                sqlSelect += "         ) datafim on datafim.encounter_id=e.encounter_id ";

                commandSource.CommandText = sqlSelect;
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {
                    while (readerSource.Read())
                    {
                        insertUtil.updateDateValue("t_tratamentoTB", "datafim", commandTarge, readerSource, 1, "nid", readerSource.GetString(0));
                    }
                }
                readerSource.Close();
            //}
            //catch (Exception e)
            //{

            //    MessageBox.Show("Houve erro ao Exportar tabela T_TRATAMENTOTB:" + e.Message);
            //}

        }

        private void exportDataQuestionarioTB(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            //try
            //{
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String startDateMySQL = startDate.Year + "/" + startDate.Month + "/" + startDate.Day;
                String endDateMySQL = endDate.Year + "/" + endDate.Month + "/" + endDate.Day;

                String sqlSelect = "Select distinct codopcao.codopcao, ";
                sqlSelect += "              if(codopcao.codopcao is not null,TRUE,FALSE) as estadoopcao,";
                sqlSelect += "              p.nid," ;
                sqlSelect += "              e.encounter_datetime as data";
                sqlSelect += "      From t_paciente p inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "      inner join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                  case o.value_coded";
                sqlSelect += "                  when 1760 then 'Tosse há mais de 3 Semanas?'";
                sqlSelect += "                  when 1761 then 'Tosse com sangue? '";
                sqlSelect += "                  when 1762 then 'Suores á noite por mais de 3 semanas?'";
                sqlSelect += "                  when 1763 then 'Febre por mais de 3 semanas?'";
                sqlSelect += "                  when 1764 then 'Perdeu peso (mais de 3 Kg no ultimo mês)?'";
                sqlSelect += "                  when 1765 then 'Alguém na família está tratando a TB?'";
                sqlSelect += "                  else null end as codopcao";
                sqlSelect += "                FROM 	encounter e";
                sqlSelect += "                      inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "               WHERE 	e.encounter_type=20 and o.concept_id=1766 and o.voided=0 and e.voided=0	 ";
                sqlSelect += "                      ) codopcao on codopcao.encounter_id=e.encounter_id";
                sqlSelect += "       where e.encounter_type=20 and  e.voided=0 and p.nid is not null and p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' and ";
                sqlSelect += "              e.encounter_datetime between '" + startDateMySQL + "' and '" + endDateMySQL + "' and p.datanasc is not null ";

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

                        sqlInsert = "Insert into t_questionarioTB(codopcao,nid,data) values('" + readerSource.GetString(0) + "','" + readerSource.GetString(2) + "',cdate('" + readerSource.GetMySqlDateTime(3) + "'))";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        insertUtil.updateBooleanValue("t_questionarioTB", "estadoopcao", commandTarge, readerSource, 1, "nid", readerSource.GetString(2));
                    }
                }

                readerSource.Close();

            //}
            //catch (Exception e)
            //{

            //    MessageBox.Show("Houve erro ao Exportar tabela T_QUESTIONARIOTB:" + e.Message);
            //}
            

        }
    }
}
