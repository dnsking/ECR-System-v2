﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR_System_v2.ViewModel
{
    public abstract class FileSystemControlViewModel : ViewModel
    {
        private string m_selectedAction;
        private bool m_showHiddenFilesAndDirectories;
        private bool m_showSystemFilesAndDirectories;

        public string SelectedAction
        {
            get
            {
                return m_selectedAction;
            }

            set
            {
                m_selectedAction = value;

                OnPropertyChanged(nameof(SelectedAction));
            }
        }

        public bool ShowHiddenFilesAndDirectories
        {
            get
            {
                return m_showHiddenFilesAndDirectories;
            }

            set
            {
                m_showHiddenFilesAndDirectories = value;

                OnPropertyChanged(nameof(ShowHiddenFilesAndDirectories));
            }
        }

        public bool ShowSystemFilesAndDirectories
        {
            get
            {
                return m_showSystemFilesAndDirectories;
            }

            set
            {
                m_showSystemFilesAndDirectories = value;

                OnPropertyChanged(nameof(ShowSystemFilesAndDirectories));
            }
        }

        public FileSystemControlViewModel()
            : base()
        {
            m_selectedAction = null;
            m_showHiddenFilesAndDirectories = false;
            m_showSystemFilesAndDirectories = false;
        }
    }
}
