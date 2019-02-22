﻿#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using LcsSaveEditor.Core;
using LcsSaveEditor.Core.Helpers;
using LcsSaveEditor.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows.Input;

namespace LcsSaveEditor.ViewModels
{
    public partial class ScriptsViewModel : PageViewModel
    {
        private const string DefaultSymbolsFileName = "CustomVariables.ini";

        public ICommand InsertRowAboveCommand
        {
            get { return new RelayCommand(InsertRowAbove, () => GlobalVariablesSelectedIndex != -1); }
        }

        public ICommand InsertRowBelowCommand
        {
            get { return new RelayCommand(InsertRowBelow, () => GlobalVariablesSelectedIndex != -1); }
        }

        public ICommand DeleteRowCommand
        {
            get { return new RelayCommand(DeleteRow, () => GlobalVariablesSelectedIndex != -1); }
        }

        public ICommand LoadSymbolsCommand
        {
            get {
                return new RelayCommand<Action<bool?, FileDialogEventArgs>>(
                    (x) => MainViewModel.ShowOpenFileDialog(
                        FileDialog_ResultAction,
                        title: FrontendResources.Scripts_DialogTitle_LoadSymbols,
                        filter: FrontendResources.FileFilter_Ini,
                        initialDirectory: Settings.Current.CustomVariablesFileDialogDirectory));
            }
        }

        public ICommand SaveSymbolsCommand
        {
            get {
                return new RelayCommand<Action<bool?, FileDialogEventArgs>>(
                    (x) => MainViewModel.ShowSaveFileDialog(
                        FileDialog_ResultAction,
                        title: FrontendResources.Scripts_DialogTitle_SaveSymbols,
                        fileName: DefaultSymbolsFileName,
                        filter: FrontendResources.FileFilter_Ini,
                        initialDirectory: Settings.Current.CustomVariablesFileDialogDirectory));
            }
        }

        private void InsertRowAbove()
        {
            m_namedGlobalVariables.Insert(GlobalVariablesSelectedIndex, new NamedScriptVariable());
        }

        private void InsertRowBelow()
        {
            m_namedGlobalVariables.Insert(GlobalVariablesSelectedIndex + 1, new NamedScriptVariable());
        }

        private void DeleteRow()
        {
            m_namedGlobalVariables.RemoveAt(GlobalVariablesSelectedIndex);
        }

        public void AutoLoadCustomVariables()
        {
            GamePlatform fileType = MainViewModel.CurrentSaveData.FileType;
            string path = Settings.Current.GetCustomVariablesFile(fileType);

            if (path != null) {
                LoadCustomVariables(path, updateStatusTextOnSuccess: false);
            }
        }

        private void LoadCustomVariables(string path, bool updateStatusTextOnSuccess = true)
        {
            GamePlatform fileType = MainViewModel.CurrentSaveData.FileType;

            void ErrorHandler(Exception ex)
            {
                Settings.Current.SetCustomVariablesFile(fileType, null);
                Logger.Error(CommonResources.Error_SymbolsLoadFail);
                Logger.Error("({0})", ex.Message);
                MainViewModel.ShowErrorDialog(
                    FrontendResources.Scripts_DialogText_SymbolsLoadFail,
                    title: FrontendResources.Scripts_DialogTitle_SymbolsLoadFail,
                    exception: ex);
                MainViewModel.StatusText = CommonResources.Error_SymbolsLoadFail;
            }

            try {
                Dictionary<string, string> dict = IniHelper.ReadAllKeys(path);
                for (int i = 0; i < m_namedGlobalVariables.Count; i++) {
                    if (dict.TryGetValue(i.ToString(), out string sym)) {
                        m_namedGlobalVariables[i].Name = sym;
                    }
                }

                Settings.Current.SetCustomVariablesFile(fileType, path);

                Logger.Info(CommonResources.Info_SymbolsLoadSuccess);
                if (updateStatusTextOnSuccess) {
                    MainViewModel.StatusText = CommonResources.Info_SymbolsLoadSuccess;
                }
            }
            catch (IOException ex) {
                ErrorHandler(ex);
            }
            catch (SecurityException ex) {
                ErrorHandler(ex);
            }
            catch (UnauthorizedAccessException ex) {
                ErrorHandler(ex);
            }
        }

        private void SaveCustomVariables(string path)
        {
            GamePlatform fileType = MainViewModel.CurrentSaveData.FileType;
            string platformName = GamePlatformHelper.GetPlatformName(fileType);

            void ErrorHandler(Exception ex)
            {
                Settings.Current.SetCustomVariablesFile(fileType, null);
                Logger.Error(CommonResources.Error_SymbolsSaveFail);
                Logger.Error("({0})", ex.Message);
                MainViewModel.ShowErrorDialog(
                    FrontendResources.Scripts_DialogText_SymbolsSaveFail,
                    title: FrontendResources.Scripts_DialogTitle_SymbolsSaveFail,
                    exception: ex);
                MainViewModel.StatusText = CommonResources.Error_SymbolsSaveFail;
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < m_namedGlobalVariables.Count; i++) {
                string sym = m_namedGlobalVariables[i].Name;
                if (!string.IsNullOrWhiteSpace(sym)) {
                    dict[i.ToString()] = sym;
                }
            }

            try {
                IniHelper.WriteComment(path, string.Format(FrontendResources.Scripts_Ini_Compatibility, platformName));
                IniHelper.AppendComment(path, FrontendResources.Scripts_Ini_GeneratedBy + "\n");
                IniHelper.AppendAllKeys(path, dict);

                Settings.Current.SetCustomVariablesFile(fileType, path);

                Logger.Info(CommonResources.Info_SymbolsSaveSuccess);
                MainViewModel.StatusText = CommonResources.Info_SymbolsSaveSuccess;
            }
            catch (IOException ex) {
                ErrorHandler(ex);
            }
            catch (SecurityException ex) {
                ErrorHandler(ex);
            }
            catch (UnauthorizedAccessException ex) {
                ErrorHandler(ex);
            }
        }
    }
}