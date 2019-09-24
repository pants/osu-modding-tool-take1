using System;
using System.Collections.Generic;
using System.Linq;
using osu_mapping.util;

namespace osu_mapping.objects
{
    public class ClassObj : ObfuscatedObject
    {
        public List<MethodObj> Methods { get; set; }
        public List<FieldObj> Fields { get; set; }
        public List<ClassObj> InnerClasses { get; set; }

        public ClassObj(string name, string obfName) : base(name, obfName)
        {
            Methods = new List<MethodObj>();
            Fields = new List<FieldObj>();
            InnerClasses = new List<ClassObj>();
        }

        public void AddMethod(string name, string obfName)
        {
            Console.WriteLine("[log] ... Added method {1} <{2}>", Name, name, obfName);
            Methods.Add(new MethodObj(name, obfName));
        }
        
        public void AddField(string name, string obfName)
        {
            Console.WriteLine("[log] ... Added field {1} <{2}>", Name, name, obfName);
            Fields.Add(new FieldObj(name, obfName));
        }

        public Optional<MethodObj> GetMethod(string methodName, bool obfuscatedName = false) =>
            Optional<MethodObj>
                .OfNullable(Methods
                    .FirstOrDefault(c => string.Equals(!obfuscatedName ? c.Name : c.ObfName, methodName, StringComparison.OrdinalIgnoreCase)));
        
        public Optional<FieldObj> GetField(string methodName, bool obfuscatedName = false) =>
            Optional<FieldObj>
                .OfNullable(Fields
                    .FirstOrDefault(c => string.Equals(!obfuscatedName ? c.Name : c.ObfName, methodName, StringComparison.OrdinalIgnoreCase)));
    }
}