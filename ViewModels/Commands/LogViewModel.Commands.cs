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

using LcsSaveEditor.Infrastructure;
using LcsSaveEditor.Resources;
using System;
using System.Windows.Input;

namespace LcsSaveEditor.ViewModels
{
    public partial class LogViewModel : ObservableObject
    {
        public ICommand ClearLogCommand
        {
            get { return new RelayCommand(() => LogItems.Clear()); }
        }

        public ICommand SaveLogCommand
        {
            get {
                return new RelayCommand<Action<bool?, DialogCloseEventArgs>>(
                    (x) => ShowSaveDialog(SaveLog_ResultAction));
            }
        }

        public ICommand SaveOnExitCommand
        {
            get {
                return new RelayCommand<Action<bool?, DialogCloseEventArgs>>(
                    (x) => ShowSaveDialog(SaveOnExit_ResultAction));
            }
        }

        public ICommand CloseWindowCommand
        {
            get { return new RelayCommand(() => OnDialogCloseRequested(new DialogCloseEventArgs(false))); }
        }

        private void ShowSaveDialog(Action<bool?, FileDialogEventArgs> resultAction)
        {
            OnFileDialogRequested(new FileDialogEventArgs(
                FileDialogType.SaveDialog,
                title: FrontendResources.Common_SaveAs,
                initialDirectory: Settings.Current.OtherFileDialogDirectory,
                filter: FrontendResources.FileFilter_Log,
                resultAction: resultAction));
        }
    }
}