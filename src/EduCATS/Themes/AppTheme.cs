﻿using EduCATS.Helpers.Settings;
using EduCATS.Themes.Interfaces;
using EduCATS.Themes.Templates;

namespace EduCATS.Themes
{
	public static class AppTheme
	{
		public const string ThemeDark = "THEME_DARK";
		public const string ThemeDefault = "THEME_DEFAULT";

		static ITheme currentTheme = new DefaultTheme();

		public static void SetCurrentTheme()
		{
			SetTheme(AppPrefs.Theme);
		}

		public static void SetTheme(string theme)
		{
			switch (theme) {
				case ThemeDark:
					currentTheme = new DarkTheme();
					break;
				default:
					currentTheme = new DefaultTheme();
					break;
			}

			Theme.Set(currentTheme);
			AppPrefs.Theme = theme;
		}
	}
}