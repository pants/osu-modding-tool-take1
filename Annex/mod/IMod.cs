namespace Annex.mod
{
    public interface IMod
    {
        string Name { get; }
        string Description { get; }
        
        void OnInit();
    }
}