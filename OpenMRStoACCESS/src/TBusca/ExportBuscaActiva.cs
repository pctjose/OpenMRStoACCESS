using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data .Types;
using System.Data.OleDb;
using ImportacaoOpenmrsForm.Utils;
using System.Data;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm.TBuscaActiva
{
    public class ExportBuscaActiva
    {
       

        public void exportDataBuscaActiva(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate)
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
                sqlSelect += "		datacomecoufaltar.datacomecoufaltar, ";
                sqlSelect += "		dataentregaactivista.dataentregaactivista, ";
                sqlSelect += "		pacientelocalizado.pacientelocalizado, ";
                sqlSelect += "		pacientelocalizado.datalocalizacao, ";
                sqlSelect += "		codmotivoabandono.codmotivoabandono, ";
                sqlSelect += "		codreferencia.codreferencia, ";
                sqlSelect += "		entregueconvite.entregueconvite, ";
                sqlSelect += "		confidenteidentificado.confidenteidentificado, ";
                sqlSelect += "		codinformacaodadapor.codinformacaodadapor, ";
                sqlSelect += "		codservicorefere.codservicorefere, ";
                sqlSelect += "		e.encounter_datetime, ";
                sqlSelect += "		observacao.observacao ";
                sqlSelect += " FROM	t_paciente p  ";
                sqlSelect += "		inner join encounter e on e.patient_id=p.patient_id ";
                sqlSelect += "		inner join ";
                sqlSelect += "		(   SELECT 	o.person_id,e.encounter_id,o.value_datetime as datacomecoufaltar ";
				sqlSelect += "	        FROM 	encounter e ";
                sqlSelect += "			        inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "	        WHERE 	e.encounter_type=21 and o.concept_id=2004 and o.voided=0 and e.voided=0 ";
                sqlSelect += "      ) datacomecoufaltar on e.encounter_id=datacomecoufaltar.encounter_id ";
                sqlSelect += "		left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataentregaactivista ";
                sqlSelect += "					FROM 	encounter e  ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2173 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) dataentregaactivista on dataentregaactivista.encounter_id=e.encounter_id ";
                sqlSelect += "		left join (	SELECT 	o.person_id, ";
                sqlSelect += "							e.encounter_id, ";
                sqlSelect += "							if(o.value_coded=1065,o.obs_datetime,null) as datalocalizacao, ";
                sqlSelect += "							case o.value_coded ";
                sqlSelect += "								when 1065 then 'SIM' ";
                sqlSelect += "								when 1066 then 'NAO' ";
                sqlSelect += "							else null end as pacientelocalizado ";
                sqlSelect += "					FROM 	encounter e  ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2003 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) pacientelocalizado on pacientelocalizado.encounter_id=e.encounter_id ";
                sqlSelect += "		left join (	SELECT 	o.person_id, ";
                sqlSelect += "							e.encounter_id, ";
                sqlSelect += "							case o.value_coded ";
                sqlSelect += "								when 2005 then 'ESQUECEU A DATA' ";
                sqlSelect += "								when 2006 then 'ESTA ACAMADO EM CASA' ";
                sqlSelect += "								when 2007 then 'DISTANCIA/DINHEIRO TRANSPORTE' ";
                sqlSelect += "								when 2008 then 'PROBLEMAS DE ALIMENTACAO' ";
                sqlSelect += "								when 2009 then 'PROBLEMAS FAMILIARES' ";
                sqlSelect += "								when 2010 then 'INSATISFACCAO COM SERVICO NO HDD' ";
                sqlSelect += "								when 2011 then 'VIAJOU' ";
                sqlSelect += "								when 2012 then 'DESMOTIVACAO' ";
                sqlSelect += "								when 2013 then 'TRATAMENTO TRADICIONAL' ";
                sqlSelect += "								when 2014 then 'TRABALHO' ";
                sqlSelect += "								when 2015 then 'EFEITOS SECUNDARIOS ARV' ";
                sqlSelect += "								when 2017 then 'OUTRO' ";
                sqlSelect += "							else o.value_coded end as codmotivoabandono ";
                sqlSelect += "					FROM 	encounter e 	 ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2016 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) codmotivoabandono on codmotivoabandono.encounter_id=e.encounter_id ";
                sqlSelect += "		left join (	SELECT 	o.person_id, ";
                sqlSelect += "							e.encounter_id, ";
                sqlSelect += "							case o.value_coded ";
                sqlSelect += "								when 1797 then 'Encaminhado para a US' ";
                sqlSelect += "								when 1977 then 'Encaminhado para os grupos de apoio' ";
                sqlSelect += "								when 5488 then 'Orientado sobre a toma correcta dos ARV' ";
                sqlSelect += "								when 2159 then 'Familiar foi referido para a US' ";
                sqlSelect += "							else 'OUTRO' end as codreferencia ";
                sqlSelect += "					FROM 	encounter e  ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=1272 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) codreferencia on codreferencia.encounter_id=e.encounter_id ";
                sqlSelect += "		left join (	SELECT 	o.person_id, ";
                sqlSelect += "							e.encounter_id, ";
                sqlSelect += "							if(o.value_datetime is not null,'SIM','NAO') as entregueconvite ";
                sqlSelect += "					FROM 	encounter e  ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2179 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) entregueconvite on entregueconvite.encounter_id=e.encounter_id   ";
                sqlSelect += "		left join (	SELECT 	o.person_id, ";
                sqlSelect += "							e.encounter_id,	 ";
                sqlSelect += "							case o.value_coded ";
                sqlSelect += "								when 1065 then 'SIM' ";
                sqlSelect += "								when 1066 then 'NAO' ";
                sqlSelect += "							else null end as confidenteidentificado ";
                sqlSelect += "					FROM 	encounter e  ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=1739 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) confidenteidentificado on confidenteidentificado.encounter_id=e.encounter_id ";
                sqlSelect += "		left join (	SELECT 	o.person_id, ";
                sqlSelect += "							e.encounter_id, ";
                sqlSelect += "							case o.value_coded ";
                sqlSelect += "								when 2034 then 'Vizinho' ";
                sqlSelect += "								when 2033 then 'Confidente' ";
                sqlSelect += "								when 2035 then 'Familiar' ";
                sqlSelect += "								when 2036 then 'Secretário do Bairro' ";
                sqlSelect += "							else 'OUTRO' end as codinformacaodadapor ";
                sqlSelect += "					FROM 	encounter e  ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2037 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) codinformacaodadapor on codinformacaodadapor.encounter_id=e.encounter_id ";
                sqlSelect += "		left join (	SELECT 	o.person_id, ";
                sqlSelect += "							e.encounter_id, ";
                sqlSelect += "							case o.value_coded ";
                sqlSelect += "								when 2175 then 'TARV Adulto' ";
                sqlSelect += "								when 2174 then 'TARV Pediatrico' ";
                sqlSelect += "								when 1414 then 'PNCT' ";
                sqlSelect += "								when 1598 then 'PTV' ";
                sqlSelect += "							else 'OUTRO' end as codservicorefere ";
                sqlSelect += "					FROM 	encounter e  ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2176 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) codservicorefere on codservicorefere.encounter_id=e.encounter_id ";
                sqlSelect += "		left join (	SELECT 	o.person_id, ";
                sqlSelect += "							e.encounter_id, ";
                sqlSelect += "							o.value_text as observacao ";                
                sqlSelect += "					FROM 	encounter e  ";
                sqlSelect += "							inner join obs o on e.encounter_id=o.encounter_id  ";
                sqlSelect += "					WHERE 	e.encounter_type=21 and o.concept_id=2041 and o.voided=0 and e.voided=0 ";
                sqlSelect += "				  ) observacao on observacao.encounter_id=e.encounter_id ";
                sqlSelect += " WHERE    e.encounter_type=21 and  e.voided=0 and p.nid is not null and p.datanasc is not null and ";
                sqlSelect += "          p.dataabertura between '"+startDateMySQL+"' and '"+endDateMySQL+"' and e.encounter_datetime between '"+startDateMySQL+"' and '"+endDateMySQL+"'";

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

                        
                        sqlInsert = "Insert into t_buscaactiva(nid,datacomecoufaltar) values( ";
                        sqlInsert += "'" + readerSource.GetString(0) + "',cdate('" + readerSource.GetDateTime(1) + "'))";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                        //data primeira tentativa
                        if (!readerSource.IsDBNull(11))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set dataprimeiratentativa=cdate('" + readerSource.GetDateTime(11) + "') where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }


                        //Data Entrega
                        if (!readerSource.IsDBNull(2))
                        {
                            commandTarge.CommandText="Update t_buscaactiva set dataentregaactivista=cdate('"+readerSource.GetDateTime(2)+"') where nid='"+readerSource.GetString(0)+"' and datacomecoufaltar=cdate('"+readerSource.GetDateTime(1)+"')";
                            commandTarge.ExecuteNonQuery();
                        }

                        //localizado
                        if (!readerSource.IsDBNull(3))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set pacientelocalizado='" + readerSource.GetString(3) + "' where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }

                        //Data Localizado
                        if (!readerSource.IsDBNull(4))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set datalocalizacao=cdate('" + readerSource.GetDateTime(4) + "') where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }

                        //Cod Motivo Abadono
                        if (!readerSource.IsDBNull(5))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set codmotivoabandono='" + readerSource.GetString(5) + "' where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }

                        //Cod Referencia
                        if (!readerSource.IsDBNull(6))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set codreferencia='" + readerSource.GetString(6) + "' where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }

                        //Entregue o convite
                        if (!readerSource.IsDBNull(7))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set entregueconvite='" + readerSource.GetString(7) + "' where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }

                        //confidente identificado
                        if (!readerSource.IsDBNull(8))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set confidenteidentificado='" + readerSource.GetString(8) + "' where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }

                        //Cod informacao dada por
                        if (!readerSource.IsDBNull(9))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set confidenteidentificado='" + readerSource.GetString(9) + "' where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }
                        //Cod servico que refere
                        if (!readerSource.IsDBNull(10))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set Codservicoquerefere='" + readerSource.GetString(10) + "' where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }

                        

                        if (!readerSource.IsDBNull(12))
                        {
                            commandTarge.CommandText = "Update t_buscaactiva set Observacao='" + readerSource.GetString(12) + "' where nid='" + readerSource.GetString(0) + "' and datacomecoufaltar=cdate('" + readerSource.GetDateTime(1) + "')";
                            commandTarge.ExecuteNonQuery();
                        }
                        

                    }
                }

                readerSource.Close();

            //}
            //catch (Exception e)
            //{

            //    MessageBox.Show("Houve erro ao Exportar tabela T_EXPOSICAOBUSCA:" + e.Message);
            //}
        }


       
    }
}
