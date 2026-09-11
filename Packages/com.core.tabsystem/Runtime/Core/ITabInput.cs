using System;

namespace Core.TabSystem
{
    public interface ITabInput
    {
        event Action SelectionRequested;
    }
}
