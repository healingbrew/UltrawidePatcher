using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using DragonLib;
using DragonLib.CLI;
using DragonLib.IO;
using JetBrains.Annotations;

namespace UltrawidePatcher
{
    [PublicAPI]
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logger.PrintVersion("WSGF");
            var flags = CommandLineFlags.ParseFlags<PatcherFlags>(CommandLineFlags.PrintHelp, args);
            if (flags == null) return;

            
            var fovTarget = flags.FOVTarget?.ToHexOctetsB();
            if (!string.IsNullOrWhiteSpace(flags.FOVCalc))
            {
                var parts = flags.FOVCalc.Split('x', ':');
                if (parts.Length >= 2 && float.TryParse(parts[0], out var width) && float.TryParse(parts[1], out var height))
                {
                    fovTarget = BitConverter.GetBytes(width / height);
                }
            }

            var buffer = File.ReadAllBytes(flags.File);
            File.WriteAllBytes(Path.ChangeExtension(flags.File, ".bak"), buffer.ToArray());

            ReplaceSignature(buffer, "FOV", flags.FOVSignature, fovTarget);
            ReplaceSignature(buffer, "Unreal Engine 4 Aspect Ratio", flags.UE4RatioSignature, fovTarget);
            ReplaceSignature(buffer, "Unreal Engine 4 Pillarbox", flags.UE4PillarboxSignature, flags.UE4PillarboxTarget?.ToHexOctetsB());
            ReplaceSignature(buffer, "Unreal Engine 4 FOV", flags.UE4YFOVSignature, flags.UE4YFOVTarget?.ToHexOctetsB());
            
            File.WriteAllBytes(flags.File, buffer.ToArray());
        }

        private static void ReplaceSignature(Span<byte> buffer, string name, string? signature, Span<byte> target)
        {
            if (string.IsNullOrWhiteSpace(signature) || target.Length == 0) return;
            var cursor = 0;
            var signatureLength = signature.ToHexOctetsA().Length;
            if (signatureLength > buffer.Length) return;
            Logger.Log(ConsoleSwatch.COLOR_RESET, null, false, Console.Out, "WSGF", null, "Searching for " + name + " signature ");
            Logger.Log(ConsoleSwatch.XTermColor.Fuchsia, true, Console.Out, null, null, signature);
            while (cursor + signatureLength < buffer.Length && cursor > -1)
            {
                Logger.Debug("WSGF", $"Searching at 0x{cursor:X2}");
                var tmp = buffer.Slice(cursor).FindPointerFromSignature(signature);
                if (tmp == -1) break;
                cursor += tmp;
                Logger.Log(ConsoleSwatch.COLOR_RESET, null, false, Console.Out, "WSGF", null, "Found signature at offset ");
                Logger.Log(ConsoleSwatch.XTermColor.OrangeRed, false, Console.Out, null, null, $"0x{cursor:X2} ");
                Logger.Log(ConsoleSwatch.XTermColor.HotPink, true, Console.Out, null, null, $"{buffer.Slice(cursor, signatureLength).ToHexString()}");
                target.CopyTo(buffer.Slice(cursor));
                cursor += signatureLength;
            }
        }
    }
}
