using System;

namespace Annex.model
{
    public class ServerBanchoAnnex : OsuServer
    {
        public ServerBanchoAnnex() : base("Bancho Annex")
        {
        }

        public override string ReplaceString(string s)
        {
            var returnVal = s;

            if (s.Equals("https://osu.ppy.sh/d/{0}"))
                returnVal = s;
            else if (s.Equals("https://osu.ppy.sh/b/{0}"))
                returnVal = s;
            else if (s.Contains("osu.ppy.sh"))
                returnVal = s.Replace("https://osu.ppy.sh", "https://annex.net");
            else if (s.Contains("c.ppy.sh") || s.Contains("c1.ppy.sh"))
                returnVal = "https://c.annex.net";
            else if (s.Contains("a.ppy.sh"))
                returnVal = "https://a.annex.net/{0}";

            if(!s.Equals(returnVal))
                Console.WriteLine("SERVER: " + returnVal);
            
            return returnVal;
        }
    }
}