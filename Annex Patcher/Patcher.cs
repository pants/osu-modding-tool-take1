using System;
using System.Dynamic;
using Annex_Patcher.injection;
using Annex_Patcher.managers;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using osu_mapping;

namespace Annex_Patcher
{
    public class Patcher
    {
        public static string osu_path
        {
            get { return _PathToOsu; }
        }

        private static string _PathToOsu = @"E:\osu!\";

        public static ModuleDefMD osu_exe;
        public static ModuleDefMD api_dll;

        private PlaceholderHandler _placeholderHandler;
        private InjectionManager _injectionManager;

        // ReSharper disable once MemberCanBePrivate.Global
        public void Patch()
        {
            _placeholderHandler = new PlaceholderHandler();
            _injectionManager = new InjectionManager();

            MappingManager.LoadMappingFile("C:/Users/Andrew/Documents/Annex/Annex/config/mapping.json");
            Console.WriteLine(MappingManager.Classes.Count);

            osu_exe = ModuleDefMD.Load(@"E:\osu!\osu!.exe");
            api_dll = ModuleDefMD.Load(@"C:\Users\Andrew\Documents\Annex\Annex\Annex.dll");

            CopyClasses();
            Console.WriteLine("Replacing placeholder classes with actual classes...");
            _placeholderHandler.ReplacePlaceholders();
            Console.WriteLine("Injecting code...");
            _injectionManager.InjectCode();

            WriteFile();
        }

        private void WriteFile()
        {
            Console.WriteLine("\nPress enter to write file..");
            Console.ReadLine();

            //osu_exe.Assembly.Name = "osu_buddy";
            osu_exe.Name = "annex!.exe";
            var options = new ModuleWriterOptions(osu_exe);
            //options.Logger = DummyLogger.NoThrowInstance;
            //osu_exe.Write(@"E:\osu!\osu_buddy.exe", options);


            //osu_exe.Assembly.Name = "osu_buddy";
            //osu_exe.Name = "osu_buddy.exe";
            osu_exe.Write(@"C:\Users\Andrew\Documents\Annex\Annex\annex!.exe", options);

            Console.WriteLine("\nPress any key to exit.");
        }

        private void CopyClasses()
        {
            Console.WriteLine("Copying classes.. ({0} Classes)", api_dll.Types.Count);
            for (var i = 0; i < api_dll.Types.Count; i++)
            {
                var type = api_dll.Types[i];

                if (!type.ToString().StartsWith("Annex")) continue;

                CopyClass(type);
                Console.WriteLine("Copied: '" + type + "'");
                i--;
            }
        }

        private void CopyClass(TypeDef type)
        {
            api_dll.Types.Remove(type);
            osu_exe.Types.Add(type);
        }
    }
}