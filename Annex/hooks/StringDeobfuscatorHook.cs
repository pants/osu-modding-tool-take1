namespace Annex.hooks
{
    public class StringDeobfuscatorHook
    {
        public static string ReplaceString(string s)
        {
            s = Annex.Server.ReplaceString(s);

            return s;
        }
    }
}