using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using osu_mapping.objects;
using osu_mapping.util;

namespace osu_mapping
{
    public static class MappingManager
    {
        public static List<ClassObj> Classes = new List<ClassObj>();

        public static void AddClass(ClassObj classObj)
        {
            GetClass(classObj.Name).IfNotPresent(() =>
            {
                Console.WriteLine("[log] Added Class: {0} <{1}>", classObj.Name, classObj.ObfName);
                Classes.Add(classObj);
            });
        }

        public static void AddMethod(ClassObj @class, MethodObj obfuscatedObject)
        {
            @class.Methods.Add(obfuscatedObject);
        }
        
        public static Optional<ClassObj> GetClass(string className, bool obfuscatedName = false)
        {
            return Optional<ClassObj>
                .OfNullable(Classes
                    .FirstOrDefault(c => string.Equals(!obfuscatedName ? c.Name : c.ObfName, className, StringComparison.OrdinalIgnoreCase)));
        }

        public static void LoadMappingFile(string location = "config/mapping.json")
        {
            if(!File.Exists(location))
                return;
            
            var json = JsonConvert.DeserializeObject<List<ClassObj>>(File.ReadAllText(location));
            Classes = json;
        }

        public static void SaveMappingFile(string location = "config/mapping.json", int totalClasses = 0)
        {
            if (!Directory.Exists(location.Substring(0, location.LastIndexOf('/'))))
                Directory.CreateDirectory(location.Substring(0, location.LastIndexOf('/')));
                
            if(File.Exists(location))
                File.Delete(location);
            
            Console.WriteLine("Found Classes: {0} | Total: {1}", Classes.Count, totalClasses);
            
            var json = JsonConvert.SerializeObject(Classes, Formatting.Indented);
            File.AppendAllText(location, json);
        }
    }
}