﻿using GTASaveData;
using GTASaveData.LCS;
using LCSSaveEditor.Core;
using LCSSaveEditor.GUI.Utils;
using LCSSaveEditor.GUI.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LCSSaveEditor.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string Name => "GTA:LCS Save Editor";
        public static string Author => "Wes Hampson";
        public static string AuthorContact => "thehambone93@gmail.com";
        public static string Copyright => $"Copyright (C) 2016-2020 {Author}";
        public static string SettingsPath => "settings.json";

        public static Uri GxtResourceUri => new Uri(@"pack://application:,,,/Resources/ENGLISH.GXT");
        public static Uri CarcolsResourceUri => new Uri(@"pack://application:,,,/Resources/carcols.dat");

        public static string Version => Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;

        public static string AssemblyName => Assembly
            .GetExecutingAssembly()
            .GetName()
            .Name;

        public static Version AssemblyVersion => new Version(
            Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyFileVersionAttribute>()
            .Version);

        private static readonly StringWriter LogWriter;
        public static string LogText => LogWriter.ToString();

        public static MainWindow TheWindow { get; private set; }

        static App()
        {
            LogWriter = new StringWriter();
        }

        public static void ExitApp()
        {
            Application.Current.Shutdown();
        }

        public static byte[] LoadResource(Uri resource)
        {
            using (MemoryStream m = new MemoryStream())
            {
                GetResourceStream(resource).Stream.CopyTo(m);
                return m.ToArray();
            }
        }

        private static void LoadSettings()
        {
            if (File.Exists(SettingsPath))
            {
                Settings.TheSettings.LoadSettings(App.SettingsPath);
            }
        }

        private static void SaveSettings()
        {
            Settings.TheSettings.SaveSettings(SettingsPath);
        }

        private static void LoadCarcols()
        {
            byte[] carcols = LoadResource(CarcolsResourceUri);
            Carcols.TheCarcols.Load(carcols);
        }

        private static void LoadGxt()
        {
            byte[] gxt = LoadResource(GxtResourceUri);
            Gxt.TheText.Load(gxt);
        }

        private static void CleanupAfterUpdate()
        {
            UpdaterSettings s = Settings.TheSettings.Updater;
            if (s.CleanupAfterUpdate)
            {
                Log.Info("Cleaning up old version files...");
                s.CleanupAfterUpdate = false;
                while (s.CleanupList.Count > 0)
                {
                    try
                    {
                        File.Delete(s.CleanupList[0]);
                    }
                    catch (Exception e)
                    {
                        if (e is DirectoryNotFoundException ||
                            e is UnauthorizedAccessException)
                        {
                            Log.Exception(e);
                            continue;
                        }

                        throw;
                    }

                    s.CleanupList.RemoveAt(0);
                }
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += Application_UnhandledException;

            Log.InfoStream = LogWriter;
            Log.ErrorStream = LogWriter;

            Log.Info($"{Name} {Version}");
            Log.Info($"{Copyright}");

            string gtaLibVer = Assembly.GetAssembly(typeof(SaveData)).GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            string lcsLibVer = Assembly.GetAssembly(typeof(LCSSave)).GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            Log.Info($"Save Editor EXE version = {AssemblyVersion}");
            Log.Info($"GTASaveData.Core version = {gtaLibVer}");
            Log.Info($"GTASaveData.LCS version = {lcsLibVer}");

#if DEBUG
            Log.Info($"DEBUG build.");
#endif
#if STANDALONE
            Settings.TheSettings.Updater.StandaloneRing = true;
            Log.Info("Standalone build.");
#endif

            LoadSettings();
            LoadCarcols();
            LoadGxt();

            CleanupAfterUpdate();

            TheWindow = new MainWindow();
            TheWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.Info("Exiting...");
            SaveSettings();
        }

        private void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Log.Error(ex);

            if (Debugger.IsAttached)
            {
                // Let the debugger handle the exception
                return;
            }

            Log.Info($"A catastrophic error has occurred. Please report this issue to {AuthorContact}.");

            string logFile = $"crash-dump_{DateTime.Now:yyyyMMddHHmmss}.log";
            File.WriteAllText(logFile, LogText);

            TheWindow.ViewModel.ShowError(
                $"An unhandled exception has occurred. The program will close and you will lose all unsaved changes.\n" +
                $"\n" +
                $"{ex.GetType().Name}: {ex.Message}\n" +
                $"\n" +
                $"A log file has been created: {logFile}. " +
                $"Please report this issue to {AuthorContact} and include this log file with your report.",
                "Unhandled Exception");
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }
    }
}
