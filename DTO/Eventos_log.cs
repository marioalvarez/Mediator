using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class Eventos_log
    {
        public int id { get; set; }
        public int tipo_evento { get; set; }
        public DateTime fecha_evento { get; set; }
        public string detalle_evento { get; set; }
        public string usuario_rut { get; set; }
        public string folio { get; set; }
        public int empresa_rut { get; set; }
        public int id_id { get; set; }
    }
}
