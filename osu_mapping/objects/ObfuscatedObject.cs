namespace osu_mapping.objects
{
    public abstract class ObfuscatedObject
    {
        public string ObfName { get; set; }
        public string Name { get; set; }

        protected ObfuscatedObject(string name, string obfName)
        {
            ObfName = obfName;
            Name = name;
        }

        public enum Modifiers
        {
            Public, Static
        }
    }
}