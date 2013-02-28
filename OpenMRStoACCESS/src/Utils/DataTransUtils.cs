using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportacaoOpenmrsForm.Utils
{
    public class DataTransUtils
    {
        public static String getTipoSessaoAconselhamento(Int32 valueCoded)
        {
            switch (valueCoded)
            {
                case 1725:
                    return "GRUPO";
                case 1726:
                    return "INDIVIDUAL";
                default:
                    return "";
            }
        }
        public static String getNivelEscolaridade(Int32 valueCoded)
        {
            switch (valueCoded)
            {
                case 1445:
                    return "NENHUM";
                case 1446:
                    return "Nivel Primario";
                case 1447:
                    return "Nivel Secundario Basico";
                case 1444:
                    return "Nivel Secundario Medio";
                case 6124:
                    return "Nivel Tecnico Basico";
                case 6125:
                    return "Nivel Tecnico Medio";
                case 1448:
                    return "Nivel Universitario";
                default:
                    return "";
            }
        }

        public static String getEstadoCivil(Int32 valueCoded)
        {
            switch (valueCoded)
            {
                case 1057:
                    return "S";
                case 5555:
                    return "C";
                case 1059:
                    return "V";
                case 1060:
                    return "U";
                case 1056:
                    return "SEPARADO";
                case 1058:
                    return "DIVORCIADO";
                default:
                    return "O";
            }
        }

        public static Boolean getYesNoAsBoolean(Int32 valueCoded)
        {
            switch (valueCoded)
            {
                case 1065:
                    return true;
                case 1066:
                    return false;
                default:
                    return false;
            }
        }

        public static String getYesNoAsString(Int32 valueCoded)
        {
            switch (valueCoded)
            {
                case 1065:
                    return "SIM";
                case 1066:
                    return "NAO";
                default:
                    return "";
            }
        }


    }
}
