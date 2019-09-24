using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Annex.forms;
using Annex.mod;
using Annex.model;
using Annex.placeholder.components;
using Annex.util;
using Microsoft.Xna.Framework;

namespace Annex
{
    public class Annex
    {
        private static string OSU_PATH = @"E:\osu!\";
        public static readonly ModManager ModManager = new ModManager();


        private static readonly List<OsuServer> _servers = new List<OsuServer>()
        {
            new ServerBanchoAnnex(),
            new ServerOffline(),
            new ServerBancho(),
            new ServerRipple()
        };

        public static readonly OsuServer Server = new ServerBanchoAnnex();//_servers[0];

        public static void Init()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerSelectorForm());

            SetWorkingDirectory();
            Logger.Debug("Loading assemblies...");
            LoadAssemblies();

            Logger.Debug("Current Directory:" + Directory.GetCurrentDirectory());

            ModManager.Initialize();
        }

        private static void SetWorkingDirectory()
        {
            Environment.CurrentDirectory = OSU_PATH;
            Directory.SetCurrentDirectory(OSU_PATH);
        }

        private static void LoadAssemblies()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                var filename = new AssemblyName(e.Name).Name;

                if (filename.Contains("resource"))
                    return Assembly.GetAssembly(e.GetType());

                var path = $@"{OSU_PATH}{filename}.dll";
                Logger.Debug("Loading: " + path);
                return Assembly.LoadFrom(path);
            };
        }

        public void empty()
        {
            var checkBox = new CompCheckbox("Hello World", 12f, Vector2.One, 0.84f, true);
            checkBox.AddToggleCallback(onClick);
        }

        private void onClick(object sender, bool status)
        {
            Console.WriteLine("Clicked!");
        }
    }
}