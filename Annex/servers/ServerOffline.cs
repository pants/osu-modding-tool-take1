namespace Annex.model
{
    public class ServerOffline : OsuServer
    {
        public ServerOffline() : base("Offline")
        {}
        
        public override string ReplaceString(string s)
        {
            var returnVal = s;

            if (s.Contains("osu.ppy.sh"))
                returnVal = s.Replace("https://osu.ppy.sh", "https://0.0.0.0");
            else if (s.Contains("c.ppy.sh") || s.Contains("c1.ppy.sh"))
                returnVal = "https://0.0.0.0";
            else if (s.Contains("a.ppy.sh"))
                returnVal = "https://0.0.0.0/{0}";

            return returnVal;
        }
    }
}