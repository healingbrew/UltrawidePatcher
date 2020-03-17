using DragonLib.CLI;
using JetBrains.Annotations;

namespace UltrawidePatcher
{
    [MeansImplicitUse]
    [PublicAPI]
    public class PatcherFlags : ICLIFlags
    {
        [NotNull]
        [UsedImplicitly]
        [CLIFlag("file", Positional = 0, IsRequired = true, Help = "File to mod", Category = "General Options")]
        public string File { get; set; } = "";

        [CLIFlag("fov-signature", Default = "39 8E E3 3F", Help = "Signature for FOV", Category = "Signature Options")]
        public string? FOVSignature { get; set; }
        
        [CLIFlag("fov-target", Default = "26 B4 17 40", Help = "Target bytes for FOV Adjustment", Category = "Target Options")]
        public string? FOVTarget { get; set; }
        
        [CLIFlag("fov-calculate", Default = "2560x1080", Help = "Calculate FOV from width/height", Category = "Target Options")]
        public string? FOVCalc { get; set; }
        
        [CLIFlag("ue4-pillarbox-signature", Default = "F6 41 2C ??", Help = "Signature for Unreal Engine 4 Pillarboxes", Category = "Signature Options")]
        public string? UE4PillarboxSignature { get; set; }
        
        [CLIFlag("ue4-pillarbox-target", Default = "F6 41 2C 00", Help = "Target bytes for UE4 Pillarbox removal", Category = "Target Options")]
        public string? UE4PillarboxTarget { get; set; }
        
        [CLIFlag("ue4-ratio-signature", Default = "3B 8E E3 3F", Help = "Signature for Unreal Engine 4 Ratio, uses --fov-target/--fov-calculate", Category = "Signature Options")]
        public string? UE4RatioSignature { get; set; }
        
        [CLIFlag("ue4-fov-signature", Default = "35 FA 0E 3C A4", Help = "Signature for Unreal Engine 4 YFOV", Category = "Signature Options")]
        public string? UE4YFOVSignature { get; set; }
        
        [CLIFlag("ue4-fov-target", Default = "35 FA 3E 3C A4", Help = "Target bytes for UE4 YFOV adjustment", Category = "Target Options")]
        public string? UE4YFOVTarget { get; set; }
    }
}
