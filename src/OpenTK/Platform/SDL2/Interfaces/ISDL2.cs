using System;
using AdvancedDLSupport;
using AdvancedDLSupport.AOT;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DLSupportDynamicAssembly")]
namespace OpenTK.Platform.SDL2.Interfaces
{
    [NativeSymbols(Prefix = "SDL_"), AOTType]
    internal interface ISDL2 : IError, IEvents, IGameController, IInit, IJoystick, IKeyboard, IMouse, IPixels, ISurface, IVideo
    {
        [NativeSymbol("free")]
        void Free(IntPtr memblock);

        void GetVersion(out Version version);
    }
}
