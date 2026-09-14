using System.Collections.Generic;
using Core.SaveSystem;

public interface ISaveRegistration
{
    IEnumerable<ISaveable> Saveables { get; }
}