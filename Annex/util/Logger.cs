using System;

namespace Annex.util
{
    public static class Logger
    {
        public static void Log(string s) => PrintLn("Log", s);
        public static void Debug(string s) => PrintLn("Debug", s);
        public static void Warn(string s) => PrintLn("Warn", s);
        public static void Error(string s) => PrintLn("ERR", s);
        
        //Todo: Make this save to a file
        public static void PrintLn(string prefix, string s) =>
            Console.WriteLine($"[Annex][{prefix}] {s}");
    }
}