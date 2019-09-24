using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using dnlib.DotNet;
using osu_mapping;
using osu_mapping.objects;
using osu_mapping_tool.mapping;
using osu_mapping_tool.mapping.maps.options;
using osu_mapping_tool.mapping.maps.options.components;

namespace osu_mapping_tool
{
    public class Program
    {
        public static ModuleDefMD osu_exe;

        private static readonly List<ClassMap> _classMaps = new List<ClassMap>()
        {
            new ClassOptions(),
            new ClassOptCheckbox(),
            new ClassSongSelection(),
            new ClassModManager(),
            new ClassOsu(),
            new ClassBoolObj(),
            new ClassBeatmapManager(),
            new ClassBeatmap(),
            new ClassScore(),
            new ClassInGame(),
            new ClassDefines(),
            new ClassHitObjectManager(),
            new ClasspWebRequest(),
            new ClassCompCheckbox(),
            new ClassGuiScreen(),
            new ClassGameMain(),
            new ClassCompRect(),
            new ClassCompLabel(),
            new ClassCompSlider(),
            new ClassUrls()
        };

        public static string DeobfuscatorInsn =>
            $"System.String {StringDeobfuscator.DeobfClassName}::{StringDeobfuscator.DeobfMainName}(System.Int32)";

        public static void Main(string[] args)
        {
            const string fileName = @"E:\osu!\osu!.exe";
            osu_exe = ModuleDefMD.Load(fileName);

            Console.WriteLine("Getting String Deobfuscator method...");
            StringDeobfuscator.Init(osu_exe);
            
            Console.WriteLine("Searching for classes...");
            foreach (var type in osu_exe.Types)
            {
                _classMaps.ForEach(c =>
                {
                    c.GetMethods(type.Name, type, type.Methods);
                    c.GetFields(type.Name, type, type.Fields);
                });
            }

            var @class = MappingManager.GetClass("StringDeobf")
                .OrElse(new ClassObj("StringDeobf", StringDeobfuscator.DeobfClassName));
            MappingManager.AddClass(@class);
            @class.AddMethod("Deobfuscate", StringDeobfuscator.DeobfMethodName);

            MappingManager.SaveMappingFile("C:/Users/Andrew/Documents/Annex/Annex/config/mapping.json", _classMaps.Count);
            MappingManager.LoadMappingFile();
        }

        //ef938618bf362d52b8aa1ea9a0620d11 C:\Users\Andrew\Documents\osu_buddy\osu_buddy.exe | osu_buddy (osu!)
        public static string ProcessReplace(string s)
        {
            if (!s.Contains(" | osu! (")) return s;
            if (cachedLine != null) return cachedLine;
            var fileInfo = new FileInfo($"{OSU_PATH}osu!.exe");
            cachedLine = $"##{GetMd5String(fileInfo.Length.ToString())} {OSU_PATH}osu!.exe | osu! {s.Substring(s.IndexOf("(", StringComparison.Ordinal))}";
            return cachedLine;
        }

        public static string GetMd5String(string instr)
        {
            return GetMd5String(utf8Encoding.GetBytes(instr));
        }

        public static string GetMd5String(byte[] data)
        {
            data = md5.ComputeHash(data);

            var str = new char[data.Length * 2];
            for (var i = 0; i < data.Length; i++)
                data[i].ToString("x2", nfi).CopyTo(0, str, i * 2, 2);

            return new string(str);
        }

        private static void listShit()
        {
            var procs = Process.GetProcesses();
            var b = new StringBuilder();

            foreach (var p in procs)
            {
                var filename = string.Empty;
                try
                {
                    filename = p.MainModule.FileName;
                    var fi = new FileInfo(filename);
                    filename = GetMd5String(fi.Length.ToString()) + @" " + filename;
                }
                catch
                {
                }
                b.AppendLine(ProcessReplace(filename + @" | " + p.ProcessName + @" (" + p.MainWindowTitle + @")"));
            }
            Console.WriteLine(b.ToString());
            Environment.Exit(0);
        }

        private static string cachedLine = null;
        private static readonly MD5 md5 = MD5.Create();
        private static readonly UTF8Encoding utf8Encoding = new UTF8Encoding();
        private static readonly NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        public static string OSU_PATH = @"E:\osu!\";
    }
}