using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class Mediaciones_dto
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public int Id_id { get; set; }
        public DateTime Fecha_doc { get; set; }
        public string MIME_TYPE { get; set; }
        public string FILE_NAME { get; set; }
        public Byte[] FILE_DATA { get; set; }
        public string FILE_PATH { get; set; }
        public string Nota { get; set; }
        public int FILE_SIZE { get; set; }
        public string Num_doc { get; set; }
        public int Estado { get; set; }
        public string Visibilidad { get; set; }
        public DateTime Fecha_creacion { get; set; }
        public string Otro_doc { get; set; }
        public int FILE_NUM_PAG { get; set; }
        public string Tp_descripcion { get; set; } //REF: Tabla tipo documentos
        public int Cod_empresa { get; set; }
        public string Usuario_rut { get; set; }
        public string Nombre_completo { get; set; }
    }
}
