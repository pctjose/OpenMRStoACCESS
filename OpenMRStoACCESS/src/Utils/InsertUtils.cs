using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ImportacaoOpenmrsForm.Utils
{
    public class InsertUtils
    {
        public void updateDateValue(String tabela, String campo, OleDbCommand comandoTarget, MySqlDataReader reader, Int16 index,String campoId,String valorId)
        {
            if (!reader.IsDBNull(index))
            {
                comandoTarget .CommandText="Update "+tabela +" set "+campo +"=cdate('"+reader.GetMySqlDateTime(index)+"') where "+campoId+" = '"+valorId+"'";
                comandoTarget.ExecuteNonQuery();

            }
        }
        public void updateStringValue(String tabela, String campo, OleDbCommand comandoTarget, MySqlDataReader reader, Int16 index, String campoId, String valorId)
        {
            if (!reader.IsDBNull(index))
            {
                String s = reader.GetString(index);
                s = s.Replace("'", "");

                String campo1 = campo.ToLower();
                s = s.Replace("'", "");
                s = s.Trim();
                if (s.Length > 0)
                {
                    if (campo1.Equals("observacao"))
                    {
                        comandoTarget.CommandText = "Update " + tabela + " set " + campo + "='" + s + "' where " + campoId + " = '" + valorId + "'";
                        comandoTarget.ExecuteNonQuery();
                    }
                    else
                    {
                        if (s.Length <= 50)
                        {
                            comandoTarget.CommandText = "Update " + tabela + " set " + campo + "='" + s + "' where " + campoId + " = '" + valorId + "'";
                            comandoTarget.ExecuteNonQuery();
                        }
                        else
                        {
                            String reducedValue = s.Substring(0, 45);
                            reducedValue += " ...";
                            comandoTarget.CommandText = "Update " + tabela + " set " + campo + "='" + reducedValue + "' where " + campoId + " = '" + valorId + "'";
                            comandoTarget.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        public void updateBooleanValue(String tabela, String campo, OleDbCommand comandoTarget, MySqlDataReader reader, Int16 index, String campoId, String valorId)
        {
            if (!reader.IsDBNull(index))
            {
                comandoTarget.CommandText = "Update " + tabela + " set " + campo + "=" + reader.GetBoolean(index) + " where " + campoId + " = '" + valorId + "'";
                comandoTarget.ExecuteNonQuery();


            }
        }
        public void updateNumericValue(String tabela, String campo, OleDbCommand comandoTarget, MySqlDataReader reader, Int16 index, String campoId, String valorId)
        {
            if (!reader.IsDBNull(index))
            {
                comandoTarget.CommandText = "Update " + tabela + " set " + campo + "=" + reader.GetInt32(index) + " where " + campoId + " = '" + valorId + "'";
                comandoTarget.ExecuteNonQuery();


            }
        }

        public  String checkNull(MySqlDataReader reader, Int16 index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetValue(index).ToString();
        }

        public Int32 getMaxID(OleDbConnection connection, String tabela, String campoID)
        {
            OleDbCommand cmm = new OleDbCommand();
            cmm.Connection = connection;
            cmm.CommandType = CommandType.Text;
            cmm.CommandText = "Select max(" + campoID + ") from " + tabela;
            return Convert.ToInt32( cmm.ExecuteScalar());
        }


        public void insertHDD(OleDbConnection connection, ComboBox cboDistrito)
        {
            OleDbCommand cmm = new OleDbCommand();
            cmm.Connection = connection;
            cmm.CommandType = CommandType.Text;

            String sqlHdd = "";
            String sqlMutepua = "";
            switch (cboDistrito.SelectedIndex)
            {
                case 0:
                    sqlHdd = "Insert into t_hdd(HdD,designacao,[local],Provincia,Distrito) values('040607','H.D. Ile','Sede Errego','Zambézia','Ile')";
                    break;
                case 1:
                    sqlHdd = "Insert into t_hdd(HdD,designacao,[local],Provincia,Distrito) values('040906','H.D. Maganja','Maganja-sede','Zambézia','Maganja da Costa')";
                    break;
                case 2:
                    sqlHdd = "Insert into t_hdd(HdD,designacao,[local],Provincia,Distrito) values('041209','H.D. Mopeia','Mopeia-sede','Zambézia','Mopeia')";
                    break;
                case 3:
                    sqlHdd = "Insert into t_hdd(HdD,designacao,[local],Provincia,Distrito) values('041306','H.R. Morrumbala','Morrumbala-sede','Zambézia','Morrumbala')";
                    break;
                case 4:
                    sqlHdd = "Insert into t_hdd(HdD,designacao,[local],Provincia,Distrito) values('041706','C.S Pebane','Pebane-sede','Zambézia','Pebane')";
                    break;
                case 5:
                    //sqlHdd = "Insert into t_hdd(HdD,designacao,[local],Provincia,Distrito) values('041507','C.S Namarroi','Namarroi-sede','Zambézia','Namarroi')";
                    //sqlMutepua = "Insert into t_hdd(HdD,designacao,[local],Provincia,Distrito) values('041508','C.S Mutepua','Namarroi','Zambézia','Namarroi')";
                    sqlHdd = "Insert into t_hdd(HdD,designacao,[local],Provincia,Distrito) values('040307','C.S Chinde','Chinde-sede','Zambézia','Chinde')";
                    
                    break;
                default:
                    break;
            }
            cmm.CommandText = "Delete from t_hdd";
            cmm.ExecuteNonQuery();
            if (!String.IsNullOrEmpty(sqlHdd))
            {
                cmm.CommandText = sqlHdd;
                cmm.ExecuteNonQuery();
            }
            //if (!String.IsNullOrEmpty(sqlMutepua))
            //{
            //    cmm.CommandText = sqlMutepua;
            //    cmm.ExecuteNonQuery();
            //}
        }

        public String  getHDDID(OleDbConnection connection)
        {
            OleDbCommand cmm = new OleDbCommand();
            cmm.Connection = connection;
            cmm.CommandType = CommandType.Text;
            cmm.CommandText = "Select HdD from t_hdd";
            return Convert.ToString(cmm.ExecuteScalar());
        }

        public void emptyTPaciente(OleDbConnection connection)
        {
            OleDbCommand cmm = new OleDbCommand();
            cmm.Connection = connection;
            cmm.CommandType = CommandType.Text;
            cmm.CommandText = "delete from t_paciente ";
            cmm.ExecuteNonQuery();
        }

        public void callFillTPacienteTable(MySqlConnection connection,String dataFinal,String hdd,String dd)
        {
            MySqlCommand cmm = new MySqlCommand();
            cmm.Connection = connection;
            cmm.CommandType = CommandType.Text;
            cmm.CommandText = "CALL FillTPacienteTable('" + dataFinal + "','"+hdd+"','"+dd+"')";
            cmm.ExecuteNonQuery();
        }

        public void updateDateValue(String tabela, String campo, OleDbCommand comandoTarget, MySqlDataReader reader, Int16 index, String campoId, Int32 valorId)
        {
            if (!reader.IsDBNull(index))
            {
                comandoTarget.CommandText = "Update " + tabela + " set " + campo + "=cdate('" + reader.GetMySqlDateTime(index) + "') where " + campoId + " = " + valorId; ;
                comandoTarget.ExecuteNonQuery();

            }
        }
        public void updateStringValue(String tabela, String campo, OleDbCommand comandoTarget, MySqlDataReader reader, Int16 index, String campoId, Int32 valorId)
        {

            if (!reader.IsDBNull(index))
            {
                String s = reader.GetString(index);
                s = s.Replace("'", "");
                s = s.Trim();
                if (s.Length > 0)
                {

                    String campo1 = campo.ToLower();
                    if (campo1.Equals("observacao"))
                    {
                        comandoTarget.CommandText = "Update " + tabela + " set " + campo + "='" + s + "' where " + campoId + " = " + valorId;
                        comandoTarget.ExecuteNonQuery();
                    }
                    else
                    {
                        if (s.Length <= 50)
                        {
                            comandoTarget.CommandText = "Update " + tabela + " set " + campo + "='" + s + "' where " + campoId + " = " + valorId;
                            comandoTarget.ExecuteNonQuery();
                        }
                        else
                        {
                            String reducedValue = s.Substring(0, 45);
                            reducedValue += " ...";
                            comandoTarget.CommandText = "Update " + tabela + " set " + campo + "='" + reducedValue + "' where " + campoId + " = " + valorId;
                            comandoTarget.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        public void updateBooleanValue(String tabela, String campo, OleDbCommand comandoTarget, MySqlDataReader reader, Int16 index, String campoId, Int32 valorId)
        {
            if (!reader.IsDBNull(index))
            {
                comandoTarget.CommandText = "Update " + tabela + " set " + campo + "=" + reader.GetBoolean(index) + " where " + campoId + " = " + valorId ;
                comandoTarget.ExecuteNonQuery();


            }
        }
        public void updateNumericValue(String tabela, String campo, OleDbCommand comandoTarget, MySqlDataReader reader, Int16 index, String campoId, Int32 valorId)
        {
            if (!reader.IsDBNull(index))
            {
                comandoTarget.CommandText = "Update " + tabela + " set " + campo + "=" + reader.GetInt32(index) + " where " + campoId + " = " + valorId ;
                comandoTarget.ExecuteNonQuery();


            }
        }

        public DataTable deidenfyDataBase(OleDbConnection connection,OleDbConnection other)
        {
            OleDbCommand cmm = new OleDbCommand();
            OleDbCommand comand = new OleDbCommand();
            
            cmm.Connection = connection;
            cmm.CommandType = CommandType.Text;

            comand.Connection = other;
            comand.CommandType = CommandType.Text;

            
            cmm.CommandText = "Select hdd,nid from t_paciente";
            OleDbDataReader readerTarget = cmm.ExecuteReader();
            String hdd = "";
            String nid = "";
            String uuid = "";
            
            DataTable dt = new DataTable("t_paciente");
            dt.Columns.Add("hdd", typeof(string));
            dt.Columns.Add("nid", typeof(string));
            dt.Columns.Add("uuid", typeof(string));

            if (readerTarget.HasRows)
            {
                while (readerTarget.Read())
                {
                    hdd = readerTarget.GetString(0);
                    nid = readerTarget.GetString(1);
                    uuid = Guid.NewGuid().ToString();

                    dt.Rows.Add(hdd, nid, uuid);

                    comand.CommandText = "update t_paciente set nid='" + uuid + "' where nid='" + nid + "'";
                    comand.ExecuteNonQuery();
                }
            }
            readerTarget.Close();
            return dt;
        }

        public void insertBairro(OleDbConnection accessConection, MySqlConnection openmrsConnection)
        {
            MySqlCommand sourceBairro = new MySqlCommand("select distinct subregion,county_district from person_address where subregion is not null and subregion<>''", openmrsConnection);
            MySqlDataReader sourceReader = sourceBairro.ExecuteReader();

            if (sourceReader.HasRows)
            {
                OleDbCommand targetBairro = new OleDbCommand();
                targetBairro.Connection = accessConection;
                targetBairro.CommandText = "Delete from t_bairro";
                targetBairro.ExecuteNonQuery();

                while (sourceReader.Read())
                {
                    targetBairro.CommandText = "insert into t_bairro(codbairro,designacao,coddistrito,Activo) values('" + sourceReader.GetString(0) + "','" + sourceReader.GetString(0) + "','" + sourceReader.GetString(1)+"',true)";
                    targetBairro.ExecuteNonQuery();
                }
            }
            sourceReader.Close();           

        }
        public void insertFuncionario(OleDbConnection accessConection, MySqlConnection openmrsConnection)
        {
            MySqlCommand sourceBairro = new MySqlCommand("Select 	provedor.provider_id as person_id,concat(given_name,' ',middle_name) as nome,family_name from 	person_name inner join (Select distinct provider_id from encounter) provedor on person_name.person_id=provedor.provider_id;", openmrsConnection);
            MySqlDataReader sourceReader = sourceBairro.ExecuteReader();
            OleDbCommand targetBairro = new OleDbCommand();
            if (sourceReader.HasRows)
            {
                
                targetBairro.Connection = accessConection;
                targetBairro.CommandText = "Delete from t_funcionario";
                targetBairro.ExecuteNonQuery();

                while (sourceReader.Read())
                {
                    targetBairro.CommandText = "insert into t_funcionario (codfuncionario,nome,apelido,cargo,Activo) values(" + sourceReader.GetString(0) + ",'" + sourceReader.GetString(1) + "','" + sourceReader.GetString(2) + "','Clinico',true)";
                    targetBairro.ExecuteNonQuery();
                }
            }
            sourceReader.Close();
            sourceBairro.CommandText = "Select 	provedor.creator,concat(given_name,' ',middle_name) as nome,family_name from 	person_name inner join (Select distinct creator from encounter) provedor on person_name.person_id=provedor.creator;";
            sourceReader = sourceBairro.ExecuteReader();
            while (sourceReader.Read())
            {
                targetBairro.CommandText = "insert into t_funcionario (codfuncionario,nome,apelido,cargo,Activo) values(" + sourceReader.GetString(0) + ",'" + sourceReader.GetString(1) + "','" + sourceReader.GetString(2) + "','Gestor de Dados',true)";
                targetBairro.ExecuteNonQuery();
            }
            sourceReader.Close();

        }
    }
}
