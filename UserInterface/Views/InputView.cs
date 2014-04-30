﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace UserInterface.Views
{
    interface IInputView
    {
        /// <summary>
        /// Invoked when a browse button is clicked.
        /// </summary>
        event EventHandler<OpenDialogArgs> BrowseButtonClicked;

        /// <summary>
        /// Property to provide access to the filename label.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Property to provide access to the grid.
        /// </summary>
        IGridView GridView { get; }
    }

    public partial class InputView : UserControl, Views.IInputView
    {
        /// <summary>
        /// Invoked when a browse button is clicked.
        /// </summary>
        public event EventHandler<OpenDialogArgs> BrowseButtonClicked;

        /// <summary>
        /// Constructor
        /// </summary>
        public InputView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Property to provide access to the grid.
        /// </summary>
        public IGridView GridView { get { return Grid; } }

        /// <summary>
        /// Property to provide access to the filename label.
        /// </summary>
        public string FileName
        {
            get
            {
                return FileNameLabel.Text;
            }
            set
            {
                FileNameLabel.Text = value;
            }
        }

        /// <summary>
        /// Browse button was clicked - send event to presenter.
        /// </summary>
        private void OnBrowseButtonClick(object sender, EventArgs e)
        {
            if (BrowseButtonClicked != null && OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenDialogArgs args = new OpenDialogArgs();
                args.FileNames = OpenFileDialog.FileNames;

                string curdir = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 3);
                for (int i=0; i< args.FileNames.Length; i++)
                {
                    args.FileNames[i] = args.FileNames[i].Replace(curdir, "");
                }

                BrowseButtonClicked.Invoke(this, args);
            }
        }
    }

    /// <summary>
    /// A class for holding info about a begin drag event.
    /// </summary>
    public class OpenDialogArgs : EventArgs
    {
        public string[] FileNames;
    }
}
