﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHampson.LcsSaveEditor.Helpers;

namespace WHampson.LcsSaveEditor.ViewModels
{
    public class PageViewModel : ObservableObject
    {
        private string _header;

        public PageViewModel()
            : this(null)
        { }

        public PageViewModel(string header)
        {
            _header = header;
        }

        public string Header
        {
            get { return _header; }
            set { _header = value; OnPropertyChanged(); }
        }
    }
}
