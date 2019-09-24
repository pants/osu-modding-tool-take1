namespace Annex.model
{
    public abstract class OsuServer
    {
        public readonly string ServerName;
        
        public OsuServer(string serverName)
        {
            ServerName = serverName;
        }
        
        public abstract string ReplaceString(string s);
    }
}