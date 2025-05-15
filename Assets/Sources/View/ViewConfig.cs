using System.Collections.Generic;
using System.Drawing;
using Sources.Core;

namespace Sources.View {
    public struct RegionTheme {
        public Color SkyColor;
        public Color WaterColor;
        public Color LightColor;
        public float LightIntensity;
    }
        
    public static class ViewConfig {
        public const float FOG_DENSITY = 0.01f;
        
        public static readonly Dictionary<RegionType, RegionTheme> regionThemesByType = new() {
            { RegionType.Aegis, new RegionTheme {
                WaterColor = Color.FromArgb(0x0f, 0xa0, 0x9f),
                SkyColor   = Color.FromArgb(0x37, 0xa0, 0xcf),
                LightColor = Color.FromArgb(0xff, 0xF9, 0xe6),
                LightIntensity = 1f,
            }},
            { RegionType.Styx, new RegionTheme {
                WaterColor = Color.FromArgb(0x02, 0x2c, 0x22),
                SkyColor   = Color.FromArgb(0x00, 0x3b, 0x2c),
                LightColor = Color.FromArgb(0x00, 0x7e, 0x49),
                LightIntensity = 0.25f,
            }},
            { RegionType.Olympia, new RegionTheme {
                WaterColor = Color.FromArgb(0xed, 0xbc, 0x98),
                SkyColor   = Color.FromArgb(0xa9, 0x7d, 0x9e),
                LightColor = Color.FromArgb(0xef, 0xbc, 0x96),
                LightIntensity = 1.5f,
            }},
            { RegionType.Hephaestus, new RegionTheme {
                WaterColor = Color.FromArgb(0xd6, 0x29, 0x02),
                SkyColor   = Color.FromArgb(0x57, 0x15, 0x07),
                LightColor = Color.FromArgb(0xfb, 0xa5, 0x01),
                LightIntensity = 0.75f,
            }},
            { RegionType.Artemis, new RegionTheme {
                WaterColor = Color.FromArgb(0x02, 0x81, 0x70),
                SkyColor   = Color.FromArgb(0x02, 0x4b, 0x4b),
                LightColor = Color.FromArgb(0x05, 0xce, 0xb5),
                LightIntensity = 0.5f,
            }},
        };
    }
}
