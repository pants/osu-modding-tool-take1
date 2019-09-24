namespace Annex.model
{
    public class ServerBancho : OsuServer
    {
        public ServerBancho() : base("Bancho")
        {
        }
        
        public override string ReplaceString(string s)
        {
            return s;
        }
    }
}