using _1Erronka_API.DTOak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoBideTPV.DTOak
{
    public class LoginErantzunaDto
    {
        public bool Ok { get; set; }
        public string Code { get; set; } = "";
        public string Message { get; set; } = "";
        public LangileaDto? Data { get; set; }
    }
}

