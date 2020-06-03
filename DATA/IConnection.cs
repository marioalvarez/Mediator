using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA
{
    /// <summary>
    /// By MarioAlvarez.cl
    /// Pattern Fabric
    /// </summary>
    public interface IConnection
    {
        void Conectar();
        void Desconectar();
        DataTable ExcecuteQuery(string sql);
        void ExecuteNonQuery(string sql);
        void ExecuteSP(string name_sp, parametros p);
    }
    
    public class parametros
    {
        public enum eTypeData
        {
            eInt = 0, 
            eLong, 
            eDouble, 
            eDecimal, 
            eFloat, 
            eVarchar, 
            eDateTime, 
            eBool, 
            eByte
        }

        private List<string> _name = new List<string>();
        private List<eTypeData> _type = new List<eTypeData>();
        private List<string> _value = new List<string>();

        public void Add(string name, eTypeData type, string value)
        {
            this._name.Add(name);
            this._type.Add(type);
            this._value.Add(value);
        }

        public string GetName(int indice)
        {
            return _name[indice];
        }

        public eTypeData GetTypeData(int indice)
        {
            return _type[indice];
        }

        public string GetValue(int indice)
        {
            return _value[indice];
        }

        public string Count()
        {
            return _name.Count;
        }
    }




}
