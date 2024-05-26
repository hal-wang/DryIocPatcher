using DryIocPatcher.Patchers;

namespace DryIocPatcher;

public static class Patcher
{
    public static void PatchInitializable()
    {
        InitializablePatcher.Patch();
    }
}
