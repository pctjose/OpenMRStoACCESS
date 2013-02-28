using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using ImportacaoOpenmrsForm.Utils;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm.TAdulto
{
    public class ExportTAdulto
    {
        public void exportDataAdulto(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {
            exportDataAdultoA(source, target, startDate, endDate);
            exportDataAdultoB(source, target, startDate, endDate);

            String sqlCell = "Select nid,value ";
            sqlCell += " from t_paciente p inner join person_attribute pa on p.patient_id=pa.person_id ";
            sqlCell+=" where pa.person_attribute_type_id=9 and value is not null and value<>'';";
            MySqlCommand comadoCell = new MySqlCommand(sqlCell, source);
            MySqlDataReader reader = comadoCell.ExecuteReader();
            OleDbCommand commandTarge = new OleDbCommand();
            commandTarge.Connection = target;
            commandTarge.CommandType = CommandType.Text;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    commandTarge.CommandText = "update t_adulto set telefone='" + reader.GetString(1) + "' where nid='" + reader.GetString(0) + "'";
                    commandTarge.ExecuteNonQuery();
                    
                }
                reader.Close();
            }
        }
        
        private void exportDataAdultoA(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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

                String sqlSelect = " SELECT	DISTINCT	p.nid, ";
                sqlSelect += "					codprofissao.codprofissao, ";
                sqlSelect += "					codnivel.codnivel, ";
                sqlSelect += "					nrconviventes.nrconviventes, ";
                sqlSelect += "					codestadocivil.codestadocivil, ";
                sqlSelect += "					nrconjuges.nrconjuges, ";
                sqlSelect += "					serologiaHivconjuge.serologiaHivconjuge, ";
                sqlSelect += "					Nrprocesso.Nrprocesso, ";
                sqlSelect += "					outrosparceiros.outrosparceiros, ";
                sqlSelect += "					nrfilhos.nrfilhos,  ";
                sqlSelect += "					nrfilhostestados.nrfilhostestados, ";
                sqlSelect += "					nrfilhoshiv.nrfilhoshiv, ";
                sqlSelect += "					tabaco.tabaco, ";
                sqlSelect += "					alcool.alcool, ";
                sqlSelect += "					droga.droga, ";
                sqlSelect += "					nrparceiros.nrparceiros,  ";
                sqlSelect += "					antecedentesgenelogicos.antecedentesgenelogicos, ";
                sqlSelect += "					datamestruacao.datamestruacao, ";
                sqlSelect += "					aborto.aborto, ";
                sqlSelect += "					ptv.ptv, ";
                sqlSelect += "					gravida.gravida,  ";
                sqlSelect += "					semanagravidez.semanagravidez, ";
                sqlSelect += "					dataprevistoparto.dataprevistoparto, ";
                sqlSelect += "					dataparto.dataparto, ";
                sqlSelect += "					tipoaleitamento.tipoaleitamento,  ";
                sqlSelect += "					Alergiamedicamentos.Alergiamedicamentos, ";
                sqlSelect += "					Alergiasquais.Alergiasquais, ";
                sqlSelect += "					Antecedentestarv.Antecedentestarv, ";
                sqlSelect += "					antecedentesquais.antecedentesquais,  ";
                sqlSelect += "					exposicaoacidental.exposicaoacidental  ";
                sqlSelect += " FROM		t_paciente p  ";
                sqlSelect += "			inner join encounter e on e.patient_id=p.patient_id  ";
                sqlSelect += "left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as codprofissao ";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1459 and o.voided=0 and e.voided=0";
                sqlSelect +="           ) codprofissao on codprofissao.encounter_id=e.encounter_id";
                sqlSelect += " left join (	SELECT 	o.person_id,e.encounter_id, ";
                sqlSelect +="                       case o.value_coded ";
                sqlSelect +="                       when 1445 then 'NENHUM'";
                sqlSelect +="                       when 1446 then 'PRIMARIO'";
                sqlSelect +="                       when 1447 then 'SECUNDARIO BASICO'";
                sqlSelect +="                       when 6124 then 'TECNICO BASICO'";
                sqlSelect +="                       when 1444 then 'SECUNDARIO MEDIO'";
                sqlSelect +="                       when 6125 then 'TECNICO MEDIO'";
                sqlSelect +="                       when 1448 then 'UNIVERSITARIO'";
                sqlSelect +="                       else 'OUTRO' end as codnivel";
                sqlSelect +="               FROM 	encounter e";
                sqlSelect += "                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1443 and o.voided=0 and e.voided=0	";
                sqlSelect +="           ) codnivel on codnivel.encounter_id=e.encounter_id ";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrconviventes ";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1656 and o.voided=0 and e.voided=0 ";
                sqlSelect +="           ) nrconviventes on nrconviventes.encounter_id=e.encounter_id ";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="                       case o.value_coded";
                sqlSelect +="                       when 1057 then 'S'";
                sqlSelect +="                       when 5555 then 'C'";
                sqlSelect +="                       when 1059 then 'V'";
                sqlSelect +="                       when 1060 then 'U'";
                sqlSelect +="                       when 1056 then 'SEPARADO'";
                sqlSelect +="                       when 1058 then 'DIVORCIADO'";
                sqlSelect +="                       else 'OUTRO' end as codestadocivil";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect += "                      inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1054 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) codestadocivil on codestadocivil.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrconjuges";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=5557 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) nrconjuges on nrconjuges.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="                       case o.value_coded";
                sqlSelect +="                       when 1169 then 'POSETIVO'";
                sqlSelect +="                       when 1066 then 'NEGATIVO'";
                sqlSelect +="                       when 1457 then 'SEM INFORMACAO'";
                sqlSelect +="                       else '' end as serologiaHivconjuge";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect += "                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1449 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) serologiaHivconjuge on serologiaHivconjuge.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as outrosparceiros";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1451 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) outrosparceiros on outrosparceiros.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhos";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=5573 and o.voided=0 and e.voided=0";
                sqlSelect +="           ) nrfilhos on nrfilhos.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhostestados";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1452 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) nrfilhostestados on nrfilhostestados.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as nrfilhoshiv";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1453 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) nrfilhoshiv on nrfilhoshiv.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="                       case o.value_coded";			
                sqlSelect +="                       when 1065 then true";			
                sqlSelect +="                       when 1066 then false";			
                sqlSelect +="                       else false end as tabaco";			
                sqlSelect +="                       FROM 	encounter e ";
                sqlSelect += "                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";			
                sqlSelect +="                       WHERE 	e.encounter_type=5 and o.concept_id=1388 and o.voided=0 and e.voided=0	";			
                sqlSelect +="            ) tabaco on tabaco.encounter_id=e.encounter_id";			
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";			
                sqlSelect +="                       case o.value_coded";			
                sqlSelect +="                       when 1065 then true";			
                sqlSelect +="                       when 1066 then false";			
                sqlSelect +="                       else false end as alcool";		
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect += "                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1603 and o.voided=0 and e.voided=0	";
                sqlSelect +="            ) alcool on alcool.encounter_id=e.encounter_id ";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="                       case o.value_coded";
                sqlSelect +="                       when 1065 then true";
                sqlSelect +="                       when 1066 then false";
                sqlSelect +="                       else false end as droga";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect += "                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=105 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) droga on droga.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="                       case o.value_coded";
                sqlSelect +="                       when 1662 then 1";
                sqlSelect +="                       when 1663 then 2";
                sqlSelect +="                       when 1664 then 3";
                sqlSelect +="                       else 4 end as nrparceiros";
                sqlSelect +="               FROM 	encounter e";
                sqlSelect += "                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1666 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) nrparceiros on nrparceiros.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as antecedentesgenelogicos";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1394 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) antecedentesgenelogicos on antecedentesgenelogicos.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as datamestruacao";
                sqlSelect +="               FROM 	encounter e";
                sqlSelect +="                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1465 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) datamestruacao on datamestruacao.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id, ";
                sqlSelect +="                       case o.value_coded ";
                sqlSelect +="                       when 50 then true ";
                sqlSelect +="                       when 1066 then false ";
                sqlSelect +="                       else false end as aborto ";
                sqlSelect +="               FROM 	encounter e ";
                sqlSelect += "                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1667 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) aborto on aborto.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="                       case o.value_coded";
                sqlSelect +="                       when 1065 then true";
                sqlSelect +="                       when 1066 then false";
                sqlSelect +="                       else false end as ptv";
                sqlSelect +="               FROM 	encounter e";
                sqlSelect += "                       inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1466 and o.voided=0 and e.voided=0";
                sqlSelect +="            ) ptv on ptv.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="					    case o.value_coded";
                sqlSelect +="				    	when 44 then true ";
                sqlSelect +="				    	when 1066 then false ";
                sqlSelect +="				        else false end as gravida";
                sqlSelect +="			    FROM 	encounter e ";
                sqlSelect += "			    		inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1982 and o.voided=0 and e.voided=0	";
                sqlSelect +="	         ) gravida on gravida.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as semanagravidez";
                sqlSelect +="			    FROM 	encounter e ";
                sqlSelect +="					    inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="			    WHERE 	e.encounter_type=5 and o.concept_id=1279 and o.voided=0 and e.voided=0";
                sqlSelect +="			 ) semanagravidez on semanagravidez.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataprevistoparto";
                sqlSelect +="			    FROM 	encounter e ";
                sqlSelect +="				    	inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="			    WHERE 	e.encounter_type=5 and o.concept_id=1600 and o.voided=0 and e.voided=0";
                sqlSelect +="			 ) dataprevistoparto on dataprevistoparto.encounter_id=e.encounter_id ";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataparto";
                sqlSelect +="			    FROM 	encounter e ";
                sqlSelect +="				    	inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect +="			    WHERE 	e.encounter_type=5 and o.concept_id=5599 and o.voided=0 and e.voided=0";
                sqlSelect +="			 ) dataparto on dataparto.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="				    	case o.value_coded";
                sqlSelect +="					    when 5526 then 'MATERNO'";
                sqlSelect +="				    	when 5254 then 'ARTIFICIAL'";
                sqlSelect +="                       when 6046 then 'MISTO'";
                sqlSelect +="					    else 'OUTRO' end as tipoaleitamento";
                sqlSelect +="			    FROM 	encounter e ";
                sqlSelect += "				    	inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1151 and o.voided=0 and e.voided=0";
                sqlSelect +="		     ) tipoaleitamento on tipoaleitamento.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id, ";
                sqlSelect +="					    case o.value_coded ";
                sqlSelect +="			    		when 1065 then 'SIM' ";
                sqlSelect +="					    when 1066 then 'NAO' ";
                sqlSelect +="                       when 1067 then 'NAO SABE' ";
                sqlSelect +="					    else 'OUTRO' end as Alergiamedicamentos ";
                sqlSelect +="		    	FROM 	encounter e ";
                sqlSelect += "			    		inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1601 and o.voided=0 and e.voided=0	";
                sqlSelect +="		     ) Alergiamedicamentos on Alergiamedicamentos.encounter_id=e.encounter_id";
                sqlSelect += " left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as Alergiasquais ";
                sqlSelect +="			    FROM 	encounter e ";
                sqlSelect +="					    inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1517 and o.voided=0 and e.voided=0";
                sqlSelect +="		     ) Alergiasquais on Alergiasquais.encounter_id=e.encounter_id";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="					    case o.value_coded";
                sqlSelect +="					    when 1065 then true ";
                sqlSelect +="					    when 1066 then false ";
                sqlSelect +="					    else false end as Antecedentestarv";
                sqlSelect +="			    FROM 	encounter e ";
                sqlSelect += "					    inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1192 and o.voided=0 and e.voided=0";
                sqlSelect +="		      ) Antecedentestarv on Antecedentestarv.encounter_id=e.encounter_id";
                sqlSelect +=" left join  (  SELECT  o.person_id,o.encounter_id,cn.name as antecedentesquais";
                sqlSelect +="               FROM    obs o   inner join concept_name cn on cn.concept_id=o.value_coded and cn.locale='pt' and cn.concept_name_type='FULLY_SPECIFIED'";
                sqlSelect +="                               inner join encounter e on e.encounter_id=o.encounter_id and o.voided=0 and cn.voided=0 and e.voided=0";
                sqlSelect +="               WHERE   e.encounter_type=5 and o.concept_id=1087 and o.voided=0 and cn.voided=0 and e.voided=0 ";
                sqlSelect +="           ) antecedentesquais on antecedentesquais.encounter_id=e.encounter_id ";
                sqlSelect +=" left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect +="					    case o.value_coded";
                sqlSelect +="					    when 1443 then -1";
                sqlSelect +="					    when 1066 then 0";
                sqlSelect += "					    when 1457 then -99";
                sqlSelect +="					    else '' end as exposicaoacidental";
                sqlSelect +="			    FROM 	encounter e ";
                sqlSelect +="					    inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect +="               WHERE 	e.encounter_type=5 and o.concept_id=1687 and o.voided=0 and e.voided=0";
                sqlSelect +="		     ) exposicaoacidental on exposicaoacidental.encounter_id=e.encounter_id";
                sqlSelect += " left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as Nrprocesso ";
                sqlSelect += "			    FROM 	encounter e ";
                sqlSelect += "					    inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "               WHERE 	e.encounter_type=5 and o.concept_id=1450 and o.voided=0 and e.voided=0";
                sqlSelect += "		     ) Nrprocesso on Nrprocesso.encounter_id=e.encounter_id ";
                sqlSelect += " where    e.encounter_type=5 and  e.voided=0 and e.encounter_datetime between  '" + startDateMySQL + "' and '" + endDateMySQL + "'";
                sqlSelect += "          and p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' and p.nid is not null and p.datanasc is not null ";

                commandTarge.Connection = target;
                commandTarge.CommandType = CommandType.Text;

                commandSource.Connection = source;
                commandSource.CommandType = CommandType.Text;
                commandSource.CommandText = sqlSelect;
                //MessageBox.Show(sqlSelect);
                readerSource = commandSource.ExecuteReader();

                if (readerSource.HasRows)
                {
                    while (readerSource.Read())
                    {

                        String nid = readerSource.GetString(0);

                        sqlInsert = "Insert into t_adulto(nid) values('" + nid + "')";


                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        insertUtil.updateStringValue("t_adulto", "codprofissao", commandTarge, readerSource, 1, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "codnivel", commandTarge, readerSource, 2, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "nrconviventes", commandTarge, readerSource, 3, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "codestadocivil", commandTarge, readerSource, 4, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "nrconjuges", commandTarge, readerSource, 5, "nid", nid);


                        insertUtil.updateStringValue("t_adulto", "serologiaHivconjuge", commandTarge, readerSource, 6, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "nrprocesso", commandTarge, readerSource, 7, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "outrosparceiros", commandTarge, readerSource, 8, "nid", nid);

                        insertUtil.updateNumericValue("t_adulto", "nrfilhos", commandTarge, readerSource, 9, "nid", nid);

                        insertUtil.updateNumericValue("t_adulto", "nrfilhostestados", commandTarge, readerSource, 10, "nid", nid);

                        insertUtil.updateNumericValue("t_adulto", "nrfilhoshiv", commandTarge, readerSource, 11, "nid", nid);

                        insertUtil.updateBooleanValue("t_adulto", "tabaco", commandTarge, readerSource, 12, "nid", nid);

                        insertUtil.updateBooleanValue("t_adulto", "alcool", commandTarge, readerSource, 13, "nid", nid);

                        insertUtil.updateBooleanValue("t_adulto", "droga", commandTarge, readerSource, 14, "nid", nid);

                        insertUtil.updateNumericValue("t_adulto", "nrparceiros", commandTarge, readerSource, 15, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "antecedentesgenelogicos", commandTarge, readerSource, 16, "nid", nid);

                        insertUtil.updateDateValue("t_adulto", "datamestruacao", commandTarge, readerSource, 17, "nid", nid);

                        insertUtil.updateBooleanValue("t_adulto", "aborto", commandTarge, readerSource, 18, "nid", nid);

                        insertUtil.updateBooleanValue("t_adulto", "ptv", commandTarge, readerSource, 19, "nid", nid);                        

                        insertUtil.updateBooleanValue("t_adulto", "gravida", commandTarge, readerSource, 20, "nid", nid);

                        insertUtil.updateNumericValue("t_adulto", "semanagravidez", commandTarge, readerSource, 21, "nid", nid);

                        insertUtil.updateDateValue("t_adulto", "dataprevistoparto", commandTarge, readerSource, 22, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "dataparto", commandTarge, readerSource, 23, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "tipoaleitamento", commandTarge, readerSource, 24, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "Alergiamedicamentos", commandTarge, readerSource, 25, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "Alergiasquais", commandTarge, readerSource, 26, "nid", nid);

                        insertUtil.updateBooleanValue("t_adulto", "Antecedentestarv", commandTarge, readerSource, 27, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "antecedentesquais", commandTarge, readerSource, 28, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "exposicaoacidental", commandTarge, readerSource, 29, "nid", nid);

                                                
                    }
                    commandTarge.CommandText = "update t_adulto set gravida=true where semanagravidez is not null";
                    commandTarge.ExecuteNonQuery();

                    commandTarge.CommandText = "Update t_adulto set puerpera=true where dataparto is not null";
                    commandTarge.ExecuteNonQuery();
                    
                }

                
                readerSource.Close();

            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Houve erro ao Exportar T_ADULTO A: " + e.Message);

            //}
        }

        private void exportDataAdultoB(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
        {

            //try
            //{
                MySqlCommand commandSource = new MySqlCommand();
                OleDbCommand commandTarge = new OleDbCommand();
                MySqlDataReader readerSource;

                //String sqlInsert;

                InsertUtils insertUtil = new InsertUtils();

                String startDateMySQL = startDate.Year + "/" + startDate.Month + "/" + startDate.Day;
                String endDateMySQL = endDate.Year + "/" + endDate.Month + "/" + endDate.Day;

                String  sqlSelect = " SELECT	p.nid, ";
                        sqlSelect += "		    historiaactual.historiaactual, ";
                        sqlSelect += "		    hipotesedediagnostico.hipotesedediagnostico, ";
                        sqlSelect += "		    codkarnosfsky.codkarnosfsky, ";
                        sqlSelect += "		    geleira.geleira, ";
                        sqlSelect += "		    electricidade.electricidade, ";
                        sqlSelect += "		    sexualidade.sexualidade ";
                        sqlSelect += " FROM	t_paciente p  ";
                        sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id ";
                        sqlSelect += "		left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as historiaactual ";
                        sqlSelect += "					FROM 	encounter e  ";
                        sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                        sqlSelect += "					WHERE 	e.encounter_type=1 and o.concept_id=1671 and o.voided=0 and e.voided=0 ";
                        sqlSelect += "				  ) historiaactual on historiaactual.encounter_id=e.encounter_id ";
                        sqlSelect += "		left join (	SELECT 	o.person_id,e.encounter_id,o.value_text as hipotesedediagnostico ";
                        sqlSelect += "					FROM 	encounter e  ";
                        sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                        sqlSelect += "					WHERE 	e.encounter_type=1 and o.concept_id=1649 and o.voided=0 and e.voided=0 ";
                        sqlSelect += "				  ) hipotesedediagnostico on hipotesedediagnostico.encounter_id=e.encounter_id ";
                        sqlSelect += "		left join (	SELECT 	o.person_id,e.encounter_id,o.value_numeric as codkarnosfsky ";
                        sqlSelect += "					FROM 	encounter e  ";
                        sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                        sqlSelect += "					WHERE 	e.encounter_type=1 and o.concept_id=5283 and o.voided=0 and e.voided=0 ";
                        sqlSelect += "				  ) codkarnosfsky on codkarnosfsky.encounter_id=e.encounter_id ";
                        sqlSelect += "		left join (	SELECT 	o.person_id,e.encounter_id, ";
                        sqlSelect += "							case o.value_coded ";
                        sqlSelect += "								when 1065 then true ";
                        sqlSelect += "								when 1066 then false ";
                        sqlSelect += "							else false end as geleira ";
                        sqlSelect += "					FROM 	encounter e 	 ";
                        sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                        sqlSelect += "					WHERE 	e.encounter_type=5 and o.concept_id=1455 and o.voided=0 and e.voided=0	 ";
                        sqlSelect += "				 ) geleira on geleira.encounter_id=e.encounter_id ";
                        sqlSelect += "		left join (	SELECT 	o.person_id,e.encounter_id, ";
                        sqlSelect += "							case o.value_coded ";
                        sqlSelect += "								when 1065 then true ";
                        sqlSelect += "								when 1066 then false ";
                        sqlSelect += "							else false end as electricidade ";
                        sqlSelect += "					FROM 	encounter e 		 ";
                        sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                        sqlSelect += "					WHERE 	e.encounter_type=5 and o.concept_id=5609 and o.voided=0 and e.voided=0 ";
                        sqlSelect += "				  ) electricidade on electricidade.encounter_id=e.encounter_id ";
                        sqlSelect += "		left join (	SELECT 	o.person_id,e.encounter_id, ";
                        sqlSelect += "							case o.value_coded ";
                        sqlSelect += "								when 1376 then 'HETEROSSEXUAL' ";
                        sqlSelect += "								when 1377 then 'HOMOSSEXUAL' ";
                        sqlSelect += "								when 1378 then 'BISSEXUAL' ";
                        sqlSelect += "							else 'OUTRO' end as sexualidade ";
                        sqlSelect += "					FROM 	encounter e  ";
                        sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                        sqlSelect += "					WHERE 	e.encounter_type=5 and o.concept_id=1375 and o.voided=0 and e.voided=0 ";
                        sqlSelect += "				 ) sexualidade on sexualidade.encounter_id=e.encounter_id ";
                        sqlSelect += " WHERE	e.encounter_type in (1,5) and  e.voided=0 and p.nid is not null and p.datanasc is not null and ";
                        sqlSelect += "		    p.dataabertura between '" + startDateMySQL + "' and '" + endDateMySQL + "' and e.encounter_datetime between '" + startDateMySQL + "' and '" + endDateMySQL + "'";

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

                        //sqlInsert = "Insert into t_adulto(nid) values(";
                        //sqlInsert += "'" + readerSource.GetString(1) + "','" + readerSource.GetString(2) + "','" + readerSource.GetString(3) + "',cdate('" + readerSource.GetMySqlDateTime(4) + "'))";


                        //commandTarge.CommandText = sqlInsert;
                        //commandTarge.ExecuteNonQuery();

                        String nid = readerSource.GetString(0);


                        insertUtil.updateStringValue("t_adulto", "historiaactual", commandTarge, readerSource, 1, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "hipotesedediagnostico", commandTarge, readerSource, 2, "nid", nid);

                        insertUtil.updateNumericValue("t_adulto", "codkarnosfsky", commandTarge, readerSource, 3, "nid", nid);

                        insertUtil.updateBooleanValue("t_adulto", "geleira", commandTarge, readerSource, 4, "nid", nid);

                        insertUtil.updateBooleanValue("t_adulto", "electricidade", commandTarge, readerSource, 5, "nid", nid);

                        insertUtil.updateStringValue("t_adulto", "sexualidade", commandTarge, readerSource, 6, "nid", nid);

                    }
                    
                }

                readerSource.Close();

            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Houve erro ao Exportar T_ADULTO B: " + e.Message);

            //}
        }




    }
}
