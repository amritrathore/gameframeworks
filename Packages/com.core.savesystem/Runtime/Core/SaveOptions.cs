using System;

namespace Core.SaveSystem
{
    [Flags]
    public enum SaveOptions
    {
        None = 0,

        PrettyPrint = 1 << 0,

        Backup = 1 << 1,

        Compress = 1 << 2,

        Encrypt = 1 << 3,

        AutoCreateDirectory = 1 << 4
    }
}