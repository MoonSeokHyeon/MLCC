using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VASFx.MLCC.Common.Model;

namespace VASFx.MLCC.Common.Theme
{
    public static class ThemeHelper
    {
        public static void SaveTheme(ThemeConfig theme)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"c:\Temp");

            if (!dirInfo.Exists)
                Directory.CreateDirectory(@"c:\Temp");

            var ini = new GSG.NET.Utils.IniUtils();
            ini.FileName = @"c:\Temp\VASFxInfomation.ini";

            ini.WriteValue("Theme", "Primary", theme.Primary);
            ini.WriteValue("Theme", "Accent", theme.Accent);
            ini.WriteValue("Theme", "IsDark", theme.IsDark.ToString());
        }

        public static ThemeConfig LoadTheme()
        {
            var ini = new GSG.NET.Utils.IniUtils();
            ini.FileName = @"c:\Temp\VASFxInfomation.ini";

            ThemeConfig themeInfo = new ThemeConfig();

            themeInfo.Primary = ini.GetString("Theme", "Primary", "indigo");
            themeInfo.Accent = ini.GetString("Theme", "Accent", "indigo");
            themeInfo.IsDark = ini.GetString("Theme", "IsDark", "false").Equals("True") ? true : false;

            if (!File.Exists(ini.FileName))
                SaveTheme(themeInfo);

            return themeInfo;
        }

        /// <summary>
        /// Defalut Theme indigo
        /// </summary>
        /// <param name="setTheme"></param>
        public static void SetTheme(ThemeConfig setTheme)
        {
            var primary = new SwatchesProvider().Swatches.ToList().FirstOrDefault(x => x.Name.Equals(setTheme.Primary));
            var accent = new SwatchesProvider().Swatches.ToList().FirstOrDefault(x => x.Name.Equals(setTheme.Accent));

            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            if (primary == null)
            {
                primary = new SwatchesProvider().Swatches.ToList().FirstOrDefault(x => x.Name.Equals("indigo"));
                accent = new SwatchesProvider().Swatches.ToList().FirstOrDefault(x => x.Name.Equals("indigo"));
            }

            theme.SetBaseTheme(setTheme.IsDark ? MaterialDesignThemes.Wpf.Theme.Dark : MaterialDesignThemes.Wpf.Theme.Light);
            theme.SetPrimaryColor(primary.ExemplarHue.Color);
            theme.SetSecondaryColor(accent.AccentExemplarHue.Color);

            paletteHelper.SetTheme(theme);
        }

    }
}
