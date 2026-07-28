using System;

namespace Amrit.SaveSystem
{
    public interface ISaveable
    {
        string SaveKey { get; }

        object CaptureState();

        void RestoreState(object state);

        Type StateType { get; }
    }
}