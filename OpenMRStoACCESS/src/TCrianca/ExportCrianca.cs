using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using ImportacaoOpenmrsForm.Utils;
using System.Data;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm.TCrianca
{
    public class ExportCrianca
    {
        public void exportDataCrianca(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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

                String sqlSelect = "Select distinct p.nid,tipoparto.tipoparto,local.local,termo.termo,pesonascimento.pesonascimento,";
                sqlSelect += "             exposicaotarvmae.exposicaotarvmae,exposicaotarvnascenca.exposicaotarvnascenca,";
                sqlSelect += "             patologianeonatal.patologianeonatal,injeccoes.injeccoes,escarificacoes.escarificacoes,";
                sqlSelect += "             extracoesdentarias.extracoesdentarias,aleitamentomaterno.aleitamentomaterno,";
                sqlSelect += "             aleitamentoexclusivo.aleitamentoexclusivo,idadedesmame.idadedesmame,pavcompleto.pavcompleto,";
                sqlSelect += "             idadecronologica.idadecronologica,bailey.bailey";
                sqlSelect += "      From t_paciente p inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "           left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1170 then 'VAGINAL'";
                sqlSelect += "                                  when 1171 then 'CESARIANA'";
                sqlSelect += "                                  else '' end as tipoparto";
                sqlSelect += "                           FROM 	encounter e ";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "                           WHERE 	e.encounter_type=7 and o.concept_id=5630 and o.voided=0 and e.voided=0";
                sqlSelect += "                       ) tipoparto on tipoparto.encounter_id=e.encounter_id";
                sqlSelect += "            left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as local";
                sqlSelect += "                          FROM 	encounter e";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                          WHERE 	e.encounter_type in (7) and o.concept_id=1505 and o.voided=0 and e.voided=0";
                sqlSelect += "                       ) local on local.encounter_id=e.encounter_id";
                sqlSelect += "            left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1065 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  else '' end as termo";
                sqlSelect += "                          FROM 	encounter e";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "                          WHERE 	e.encounter_type=7 and o.concept_id=1500 and o.voided=0 and e.voided=0";
                sqlSelect += "                        ) termo on termo.encounter_id=e.encounter_id";
                sqlSelect += "             left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as pesonascimento";
                sqlSelect += "                          FROM 	encounter e";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                          WHERE 	e.encounter_type in (7) and o.concept_id=5916 and o.voided=0 and e.voided=0";
                sqlSelect += "                        ) pesonascimento on pesonascimento.encounter_id=e.encounter_id";
                sqlSelect += "             left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1065 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  when 1457 then -99";
                sqlSelect += "                                  else 0 end as exposicaotarvmae";
                sqlSelect += "                          FROM 	encounter e ";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "                          WHERE 	e.encounter_type=7 and o.concept_id=1501 and o.voided=0 and e.voided=0";
                sqlSelect += "                        ) exposicaotarvmae on exposicaotarvmae.encounter_id=e.encounter_id";
                sqlSelect += "             left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1065 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  when 1457 then -99";
                sqlSelect += "                                  else '' end as exposicaotarvnascenca";
                sqlSelect += "                          FROM 	encounter e";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "                          WHERE 	e.encounter_type=7 and o.concept_id=1502 and o.voided=0 and e.voided=0";
                sqlSelect += "                         ) exposicaotarvnascenca on exposicaotarvnascenca.encounter_id=e.encounter_id";
                sqlSelect += "             left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as patologianeonatal";
                sqlSelect += "                          FROM 	encounter e";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                          WHERE 	e.encounter_type in (7) and o.concept_id=1506 and o.voided=0 and e.voided=0";
                sqlSelect += "                        ) patologianeonatal on patologianeonatal.encounter_id=e.encounter_id";
                sqlSelect += "             left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1065 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  when 1457 then -99";
                sqlSelect += "                                  else '' end as injeccoes";
                sqlSelect += "                          FROM 	encounter e ";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "                          WHERE 	e.encounter_type=7 and o.concept_id=1507 and o.voided=0 and e.voided=0";
                sqlSelect += "                       ) injeccoes on injeccoes.encounter_id=e.encounter_id";
                sqlSelect += "             left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1065 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  when 1457 then -99";
                sqlSelect += "                                  else '' end as escarificacoes";
                sqlSelect += "                          FROM 	encounter e";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "                          WHERE 	e.encounter_type=7 and o.concept_id=1509 and o.voided=0 and e.voided=0";
                sqlSelect += "                        ) escarificacoes on escarificacoes.encounter_id=e.encounter_id";
                sqlSelect += "              left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1065 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  when 1457 then -99";
                sqlSelect += "                                  else '' end as extracoesdentarias";
                sqlSelect += "                          FROM 	encounter e ";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "                          WHERE 	e.encounter_type=7 and o.concept_id=1508 and o.voided=0 and e.voided=0	";
                sqlSelect += "                        ) extracoesdentarias on extracoesdentarias.encounter_id=e.encounter_id";
                sqlSelect += "              left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1065 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  else '' end as aleitamentomaterno ";
                sqlSelect += "                           FROM 	encounter e ";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "                           WHERE 	e.encounter_type=7 and o.concept_id=6061 and o.voided=0 and e.voided=0	";
                sqlSelect += "                         ) aleitamentomaterno on aleitamentomaterno.encounter_id=e.encounter_id";
                sqlSelect += "              left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 5526 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  else '' end as aleitamentoexclusivo";
                sqlSelect += "                          FROM 	encounter e";
                sqlSelect += "                                   inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "                         WHERE 	e.encounter_type=7 and o.concept_id=1613 and o.voided=0 and e.voided=0";
                sqlSelect += "                       ) aleitamentoexclusivo on aleitamentoexclusivo.encounter_id=e.encounter_id";
                sqlSelect += "             left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as idadedesmame";
                sqlSelect += "                          FROM 	encounter e";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                          WHERE 	e.encounter_type in (7) and o.concept_id=1510 and o.voided=0 and e.voided=0";
                sqlSelect += "                        ) idadedesmame on idadedesmame.encounter_id=e.encounter_id";
                sqlSelect += "              left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "                                  case o.value_coded";
                sqlSelect += "                                  when 1065 then -1";
                sqlSelect += "                                  when 1066 then 0";
                sqlSelect += "                                  else '' end as pavcompleto";
                sqlSelect += "                           FROM 	encounter e ";
                sqlSelect += "                                  inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "                           WHERE 	e.encounter_type=7 and o.concept_id=1511 and o.voided=0 and e.voided=0";
                sqlSelect += "                         ) pavcompleto on pavcompleto.encounter_id=e.encounter_id";
                sqlSelect += "               left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as idadecronologica";
                sqlSelect += "                              FROM 	encounter e";
                sqlSelect += "                                      inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                              WHERE 	e.encounter_type=7 and o.concept_id=1512 and o.voided=0 and e.voided=0";
                sqlSelect += "                          ) idadecronologica on idadecronologica.encounter_id=e.encounter_id";
                sqlSelect += "               left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as bailey";
                sqlSelect += "                              FROM 	encounter e ";
                sqlSelect += "                                      inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "                              WHERE 	e.encounter_type=7 and o.concept_id=1514 and o.voided=0 and e.voided=0";
                sqlSelect += "                          ) bailey on bailey.encounter_id=e.encounter_id";

                sqlSelect += " where e.encounter_type=7 and  e.voided=0 and nid is not null and dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' group by nid;";

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

                        //sqlInsert = "Insert into t_crianca(nid,local) values('" + readerSource.GetString(0) + "','"+ checkNull(readerSource, 2) + "')";
                        sqlInsert = "Insert into t_crianca(nid) values('" + readerSource.GetString(0) + "')";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        insertUtil.updateStringValue("t_crianca", "tipoparto", commandTarge, readerSource, 1, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "[local]", commandTarge, readerSource, 2, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "termo", commandTarge, readerSource, 3, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "pesonascimento", commandTarge, readerSource, 4, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "exposicaotarvmae", commandTarge, readerSource, 5, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "exposicaotarvnascenca", commandTarge, readerSource, 6, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "patologianeonatal", commandTarge, readerSource, 7, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "injeccoes", commandTarge, readerSource, 8, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "escarificacoes", commandTarge, readerSource, 9, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "extracoesdentarias", commandTarge, readerSource, 10, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "aleitamentomaterno", commandTarge, readerSource, 11, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "aleitamentoexclusivo", commandTarge, readerSource, 12, "nid", readerSource.GetString(0));
                        insertUtil.updateNumericValue("t_crianca", "idadedesmame", commandTarge, readerSource, 13, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "pavcompleto", commandTarge, readerSource, 14, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "idadecronologica", commandTarge, readerSource, 15, "nid", readerSource.GetString(0));
                        insertUtil.updateStringValue("t_crianca", "bailey", commandTarge, readerSource, 16, "nid", readerSource.GetString(0));
                
                    }
                }

                String sqlUpdateMae = "UPDATE t_crianca,t_mae SET t_crianca.idmae=t_mae.idmae where t_crianca.nid=t_mae.nid;";
                commandTarge.CommandText = sqlUpdateMae;
                commandTarge.ExecuteNonQuery();

                String sqlUpdatePai = "UPDATE t_crianca,t_pai SET t_crianca.idpai=t_pai.idpai where t_crianca.nid=t_pai.nid;";
                commandTarge.CommandText = sqlUpdatePai;
                commandTarge.ExecuteNonQuery();
               
                
                readerSource.Close();

            }
            catch (Exception e)
            {

                MessageBox.Show("Houve erro ao Exportar tabela T_CRIANCA:" + e.Message);
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
