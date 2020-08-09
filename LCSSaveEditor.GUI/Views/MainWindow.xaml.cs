﻿using LCSSaveEditor.GUI.ViewModels;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfEssentials.Win32;

namespace LCSSaveEditor.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool m_initialized;
        private bool m_initializing;
        private LogWindow m_logWindow;
        
        public ViewModels.MainWindow ViewModel
        {
            get { return (ViewModels.MainWindow) DataContext; }
            set { DataContext = value; }
        }

        public MainWindow()
        {
            m_initializing = true;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_initialized) return;

            ViewModel.Initialize();
            ViewModel.MessageBoxRequest += ViewModel_MessageBoxRequest;
            ViewModel.FileDialogRequest += ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest += ViewModel_FolderDialogRequest;
            ViewModel.LogWindowRequest += ViewModel_LogWindowRequest;

            m_initializing = false;
            m_initialized = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ViewModel.IsDirty)
            {
                e.Cancel = true;
                ViewModel.PromptSaveChanges(ExitAppConfirmationDialog_Callback);
                return;
            }

            ViewModel.Shutdown();
            ViewModel.MessageBoxRequest -= ViewModel_MessageBoxRequest;
            ViewModel.FileDialogRequest -= ViewModel_FileDialogRequest;
            ViewModel.FolderDialogRequest -= ViewModel_FolderDialogRequest;
            ViewModel.LogWindowRequest -= ViewModel_LogWindowRequest;

            if (m_logWindow != null)
            {
                m_logWindow.HideOnClose = false;
                m_logWindow.Close();
            }

            m_initialized = false;
        }

        private void ExitAppConfirmationDialog_Callback(MessageBoxResult r)
        {
            if (r != MessageBoxResult.Cancel)
            {
                if (r == MessageBoxResult.Yes) ViewModel.SaveFile();
                ViewModel.IsDirty = false;
                ViewModel.CloseFile();
                Application.Current.Shutdown();
            }
        }

        private void ViewModel_MessageBoxRequest(object sender, MessageBoxEventArgs e)
        {
            e.Show(this);
        }

        private void ViewModel_FileDialogRequest(object sender, FileDialogEventArgs e)
        {
            e.ShowDialog(this);
        }

        private void ViewModel_FolderDialogRequest(object sender, FileDialogEventArgs e)
        {
            VistaFolderBrowserDialog d = new VistaFolderBrowserDialog();
            bool? r = d.ShowDialog(this);

            e.FileName = d.SelectedPath;
            e.Callback?.Invoke(r, e);
        }

        private void ViewModel_LogWindowRequest(object sender, EventArgs e)
        {
            if (m_logWindow != null && m_logWindow.IsVisible)
            {
                m_logWindow.Focus();
                return;
            }

            if (m_logWindow == null) m_logWindow = new LogWindow() { Owner = this };
            m_logWindow.Show();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_initializing || !(e.OriginalSource is TabControl))
            {
                return;
            }

            foreach (var item in e.AddedItems)
            {
                if (item is TabPageBase page)
                {
                    page.Update();
                }
            }
        }
    }
}
