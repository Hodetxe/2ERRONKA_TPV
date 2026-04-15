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

