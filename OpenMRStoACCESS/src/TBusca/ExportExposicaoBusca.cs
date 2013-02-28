using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using ImportacaoOpenmrsForm.Utils;
using System.Data;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm.TExposicaoBusca
{
    public class ExportExposicaoBusca
    {
        public void exportDataExposicaoBusca(MySqlConnection source, OleDbConnection target, DateTime startDate, DateTime endDate, MySqlConnection otherSource)
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

                String sqlSelect = "Select p.nid,datacomecoufaltar.datacomecoufaltar,dataentregaactivista.dataentregaactivista,";
                sqlSelect += " pacientelocalizado.pacientelocalizado,pacientelocalizado.datalocalizacao,codmotivoabandono.codmotivoabandono";
                sqlSelect += "      From t_paciente p inner join encounter e on e.patient_id=p.patient_id";
                sqlSelect += "      left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as datacomecoufaltar";
                sqlSelect += "      FROM 	encounter e ";
                sqlSelect += "  inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "  WHERE e.encounter_type in (21) and o.concept_id=2004 and o.voided=0 and e.voided=0";
                sqlSelect += ") datacomecoufaltar on datacomecoufaltar.encounter_id=e.encounter_id";
                sqlSelect += "left join (	SELECT 	o.person_id,e.encounter_id,o.value_datetime as dataentregaactivista";
                sqlSelect += "FROM 	encounter e ";
                sqlSelect += "inner join obs o on e.encounter_id=o.encounter_id and o.person_id=e.patient_id";
                sqlSelect += "WHERE 	e.encounter_type in (21) and o.concept_id=2173 and o.voided=0 and e.voided=0";
                sqlSelect += ") dataentregaactivista on dataentregaactivista.encounter_id=e.encounter_id";
                sqlSelect += "left join (	SELECT 	o.person_id,e.encounter_id,o.obs_datetime as datalocalizacao,";
                sqlSelect += "case o.value_coded";
                sqlSelect += "when 1065 then 'TRUE'";
                sqlSelect += "when 1066 then 'FALSE'";
                sqlSelect += "else '' end as pacientelocalizado";
                sqlSelect += "FROM 	encounter e 	";
                sqlSelect += "inner join obs o on e.encounter_id=o.encounter_id ";
                sqlSelect += "WHERE 	e.encounter_type=21 and o.concept_id=2003 and o.voided=0 and e.voided=0	";
                sqlSelect += ") pacientelocalizado on pacientelocalizado.encounter_id=e.encounter_id";
                sqlSelect += "left join (	SELECT 	o.person_id,e.encounter_id,";
                sqlSelect += "case o.value_coded";
                sqlSelect += "when 2005 then 'ESQUECEU A DATA'";
                sqlSelect += "when 2006 then 'ESTA ACAMADO EM CASA'";
                sqlSelect += "when 2007 then 'DISTANCIA/DINHEIRO TRANSPORTE'";
                sqlSelect += "when 2008 then 'PROBLEMAS DE ALIMENTACAO'";
                sqlSelect += "when 2009 then 'PROBLEMAS FAMILIARES'";
                sqlSelect += "when 2010 then 'INSATISFACCAO COM SERVICO NO HDD'";
                sqlSelect += "when 2011 then 'VIAJOU'";
                sqlSelect += "when 2012 then 'DESMOTIVACAO'";
                sqlSelect += "when 2013 then 'TRATAMENTO TRADICIONAL' ";
                sqlSelect += "when 2014 then 'TRABALHO'";
                sqlSelect += "when 2015 then 'EFEITOS SECUNDARIOS ARV'";
                sqlSelect += "else '' end as codmotivoabandono";
                sqlSelect += "FROM 	encounter e ";
                sqlSelect += "inner join obs o on e.encounter_id=o.encounter_id";
                sqlSelect += "WHERE 	e.encounter_type=21 and o.concept_id in (2016,2017) and o.voided=0 and e.voided=0";
                sqlSelect += ") codmotivoabandono on codmotivoabandono.encounter_id=e.encounter_id";
                sqlSelect += "where e.encounter_type in (21) and  e.voided=0;";
                                
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

                        sqlInsert = "Insert into t_exposicaobusca(nid,datacomecoufaltar,dataentregaactivista,";
                        sqlInsert += "pacientelocalizado,datalocalizacao,codmotivoabandono) values( ";
                        sqlInsert += "'" + checkNull(readerSource, 0) + "',cdate('" + checkNull(readerSource, 1) + "'),'" + "',cdate('" + checkNull(readerSource, 2) + "'),'" + checkNull(readerSource, 3) + checkNull(readerSource, 4) + checkNull(readerSource, 5) + "')";

                        commandTarge.CommandText = sqlInsert;
                        commandTarge.ExecuteNonQuery();

                     }
                }
                
                readerSource.Close();

            }
            catch (Exception e)
            {

                MessageBox.Show("Houve erro ao Exportar tabela T_EXPOSICAOBUSCA:"+e.Message);
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
