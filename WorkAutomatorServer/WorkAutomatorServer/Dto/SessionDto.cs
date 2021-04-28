using WorkAutomatorServer.Dto.Attributes;

namespace WorkAutomatorServer.Dto
{
    public class SessionDto : DtoBase
    {
        [HeaderAutoWired("UID")]
        public int UserId { get; set; }

        [HeaderAutoWired("SESSION_TOKEN_SALTED")]
        public string SessionTokenSalted { get; set; }

        [HeaderAutoWired("SALT")]
        public string Salt { get; set; }
    }
}
