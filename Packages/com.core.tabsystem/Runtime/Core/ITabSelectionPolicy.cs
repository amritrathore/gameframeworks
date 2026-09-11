namespace Core.TabSystem
{
    public interface ITabSelectionPolicy<in TKey>
    {
        bool CanSelect(TKey key);
    }
}
