using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using ImportacaoOpenmrsForm.Utils;
using System.Windows.Forms;
using System.Data;

namespace ImportacaoOpenmrsForm.TMae
{
    public class ExportTMae
    {
        public void exportDataTMae(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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

                String sqlSelect = "Select p.nid as nid,nome.nome,idade.idade,vivo.vivo,doente.doente,doenca.doenca,codprofissao.codprofissao,";
                sqlSelect += "             resultadohiv.resultadohiv,emtarv.emtarv";
                sqlSelect += "      From t_paciente p inner join encounter e on e.patient_id=p.patient_id" ;                 
                sqlSelect += "      inner join ( SELECT o.person_id,e.encounter_id,o.value_text as nome";
                sqlSelect += "                  FROM 	encounter e";
                sqlSelect += "                          inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                  WHERE  e.encounter_type in (7) and o.concept_id=1477 and o.voided=0 and e.voided=0";
                sqlSelect += "                 ) nome on nome.encounter_id=e.encounter_id";               
                sqlSelect += "     left join ( SELECT o.person_id,e.encounter_id,o.value_numeric as idade";
                sqlSelect += "                 FROM 	encounter e";
                sqlSelect += "                          inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                 WHERE  e.encounter_type in (7) and o.concept_id=1478 and o.voided=0 and e.voided=0";
                sqlSelect += "                ) idade on idade.encounter_id=e.encounter_id";
                sqlSelect += "      left join ( SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                             case o.value_coded";
                sqlSelect += "                             when 1065 then 'SIM'";
                sqlSelect += "                             when 1066 then 'NAO'";
                sqlSelect += "                             when 1457 then 'Sem Informação'";
                sqlSelect += "                             else '' end as vivo";
                sqlSelect += "                   FROM 	encounter e";
                sqlSelect += "                          inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "                   WHERE  e.encounter_type=7 and o.concept_id=1479 and o.voided=0 and e.voided=0";
                sqlSelect += "                  ) vivo on vivo.encounter_id=e.encounter_id";            
                sqlSelect += "      left join ( SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                              case o.value_coded";
                sqlSelect += "                              when 1065 then 'SIM'";
                sqlSelect += "                              when 1066 then 'NAO'";
                sqlSelect += "                              when 1457 then 'Sem Informação'";
                sqlSelect += "                              else '' end as doente";
                sqlSelect += "                   FROM 	encounter e";
                sqlSelect += "                          inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "                   WHERE  e.encounter_type=7 and o.concept_id=1480 and o.voided=0 and e.voided=0";
                sqlSelect += "                  ) doente on doente.encounter_id=e.encounter_id";               
			    sqlSelect += "      left join ( SELECT o.person_id,e.encounter_id,o.value_text as doenca";
                sqlSelect += "                   FROM 	encounter e";
                sqlSelect += "                          inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                   WHERE  e.encounter_type in (7) and o.concept_id=1481 and o.voided=0 and e.voided=0";
                sqlSelect += "                  ) doenca on doenca.encounter_id=e.encounter_id";
                sqlSelect += "      left join ( SELECT o.person_id,e.encounter_id,o.value_text as codprofissao";
                sqlSelect += "                   FROM 	encounter e";
                sqlSelect += "                          inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                   WHERE  e.encounter_type in (7) and o.concept_id=1482 and o.voided=0 and e.voided=0";
                sqlSelect += "                  ) codprofissao on codprofissao.encounter_id=e.encounter_id"; 
                sqlSelect += "      left join ( SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                              case o.value_coded";
                sqlSelect += "                              when 703 then 'POSITIVO'";
                sqlSelect += "                              when 664 then 'NEGATIVO'";
                sqlSelect += "                              when 1138 then 'INDETERMINADO'";
                sqlSelect += "                              when 1118 then 'NAO FEZ'";
                sqlSelect += "                              when 1457 then 'Sem Informação'";
                sqlSelect += "                              else '' end as resultadohiv";
                sqlSelect += "                   FROM 	encounter e";
                sqlSelect += "                          inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "                   WHERE  e.encounter_type=7 and o.concept_id=1483 and o.voided=0 and e.voided=0";
                sqlSelect += "                  ) resultadohiv on resultadohiv.encounter_id=e.encounter_id";
                sqlSelect += "      left join ( SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                              case o.value_coded";
                sqlSelect += "                              when 1065 then 'SIM'";
                sqlSelect += "                              when 1066 then 'NAO'";
                sqlSelect += "                              when 1457 then 'Sem Informação'";
                sqlSelect += "                              else '' end as emtarv";
                sqlSelect += "                   FROM 	encounter e";
                sqlSelect += "                          inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "                   WHERE  e.encounter_type=7 and o.concept_id=1484 and o.voided=0 and e.voided=0";
                sqlSelect += "                  ) emtarv on emtarv.encounter_id=e.encounter_id";
                sqlSelect += " where e.encounter_type=7 and p.nid is not null and  e.voided=0 and nome.nome is not null and dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "'";

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

                        sqlInsert = "Insert into t_mae(nid) values('" + readerSource.GetString(0) + "')";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();
                        insertUtil.updateStringValue("t_mae", "nome", commandTarge, readerSource, 1, "nid", readerSource.GetString(0));
                        insertUtil.updateNumericValue("t_mae", "idade", commandTarge, readerSource, 2, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_mae", "vivo", commandTarge, readerSource, 3, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_mae", "doente", commandTarge, readerSource, 4, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_mae", "doenca", commandTarge, readerSource, 5, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_mae", "codprofissao", commandTarge, readerSource, 6, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_mae", "resultadohiv", commandTarge, readerSource, 7, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_mae", "emtarv", commandTarge, readerSource, 8, "nid", readerSource.GetString(0));
                    }
                }
                
                readerSource.Close();

            }
            catch (Exception e)
            {

                MessageBox.Show("Houve erro ao Exportar tabela T_MAE:" + e.Message);
            }
            
            

        }

        private static String checkNull(MySqlDataReader reader, Int16 index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetValue(index).ToString();
        }


    }
    
}
