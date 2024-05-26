using DryIoc;
using HarmonyLib;
using System.Reflection;

namespace DryIocPatcher.Patchers;

internal static class InitializablePatcher
{
    public static void Patch()
    {
        var harmony = new Harmony(typeof(InitializablePatcher).FullName);

        var method = typeof(Interpreter)
            .GetRuntimeMethods()
            .Where(m => m.Name == "TryInterpret")
            .First();
        harmony.Patch(method, null, new HarmonyMethod(Postfix));
    }

    private static readonly List<WeakReference<IInitializable>> _initializedList = [];
    public static void Postfix(ref object result)
    {
        if (result is not IInitializable initializable) return;

        _initializedList
            .Where(item => !item.TryGetTarget(out _))
            .ToList()
            .ForEach(item => _initializedList.Remove(item));
        if (_initializedList.Any(item => item.TryGetTarget(out var target) && target == initializable))
        {
            return;
        }

        _initializedList.Add(new WeakReference<IInitializable>(initializable));
        initializable.Initialize();
    }
}
