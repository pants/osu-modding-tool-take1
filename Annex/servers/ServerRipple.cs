using System;

namespace Annex.model
{
    public class ServerRipple : OsuServer
    {
        public ServerRipple() : base("Ripple")
        {
        }
        
        public override string ReplaceString(string s)
        {
            var returnVal = s;

            if (s.Equals("https://osu.ppy.sh/d/{0}"))
                returnVal = "https://storage.ripple.moe/d/{0}";
            else if (s.Equals("https://osu.ppy.sh/b/{0}"))
                returnVal = s;
            else if (s.Contains("osu.ppy.sh"))
                returnVal = s.Replace("https://osu.ppy.sh", "https://ripple.moe");
            else if (s.Contains("c.ppy.sh") || s.Contains("c1.ppy.sh"))
                returnVal = "https://c.ripple.moe";
            else if (s.Contains("a.ppy.sh"))
                returnVal = "https://a.ripple.moe/{0}";
           

            return returnVal;
        }
    }
}