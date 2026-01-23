using System;

namespace Seacore.Common.Services
{
    [Serializable]
    //Do not use zero as a value, it will cause issues. 0 is reserved for MasterVolume in SoundSettings
    public enum SoundType
    {
        SFX = 1,
        Music = 2,
        Ambient = 3
    }
}
