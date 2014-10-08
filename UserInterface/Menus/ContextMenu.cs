﻿// -----------------------------------------------------------------------
// <copyright file="ContextMenu.cs" company="APSIM Initiative">
//     Copyright (c) APSIM Initiative
// </copyright>
// -----------------------------------------------------------------------
namespace UserInterface.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml;
    using Commands;
    using Interfaces;
    using Microsoft.Win32;
    using Models;
    using Models.Core;
    using Models.Factorial;
    using Models.Soils;
    
    /// <summary>
    /// This class contains methods for all context menu items that the ExplorerView exposes to the user.
    /// </summary>
    public class ContextMenu
    {
        /// <summary>
        /// Reference to the ExplorerPresenter.
        /// </summary>
        private ExplorerPresenter explorerPresenter;

        /// <summary>
        /// The command that is currently being run.
        /// </summary>
        private RunCommand command = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenu" /> class.
        /// </summary>
        /// <param name="explorerPresenter">The explorer presenter to work with</param>
        public ContextMenu(ExplorerPresenter explorerPresenter)
        {
            this.explorerPresenter = explorerPresenter;
        }

        /// <summary>
        /// User has clicked rename
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Rename", ShortcutKey = Keys.F2)]
        public void OnRename(object sender, EventArgs e)
        {
            this.explorerPresenter.Rename();
        }

        /// <summary>
        /// User has clicked Copy
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Copy", ShortcutKey = Keys.Control | Keys.C)]
        public void OnCopyClick(object sender, EventArgs e)
        {
            Model model = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as Model;
            if (model != null)
            {
                // Set the clipboard text.
                System.Windows.Forms.Clipboard.SetText(Apsim.Serialise(model));
            }
        }

        /// <summary>
        /// User has clicked Paste
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Paste", ShortcutKey = Keys.Control | Keys.V)]
        public void OnPasteClick(object sender, EventArgs e)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(System.Windows.Forms.Clipboard.GetText());
                object newModel = Utility.Xml.Deserialise(document.DocumentElement);

                // See if the presenter is happy with this model being added.
                Model parentModel = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as Model;
                AllowDropArgs allowDropArgs = new AllowDropArgs();
                allowDropArgs.NodePath = this.explorerPresenter.CurrentNodePath;
                allowDropArgs.DragObject = new DragObject()
                {
                    NodePath = null,
                    ModelType = newModel.GetType(),
                    Xml = System.Windows.Forms.Clipboard.GetText()
                };
                this.explorerPresenter.OnAllowDrop(null, allowDropArgs);

                // If it is happy then issue an AddModelCommand.
                if (allowDropArgs.Allow)
                {
                    AddModelCommand command = new AddModelCommand(System.Windows.Forms.Clipboard.GetText(), parentModel);
                    this.explorerPresenter.CommandHistory.Add(command, true);
                }
            }
            catch (Exception exception)
            {
                this.explorerPresenter.ShowMessage(exception.Message, DataStore.ErrorLevel.Error);
            }
        }

        /// <summary>
        /// User has clicked Delete
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Delete", ShortcutKey = Keys.Delete)]
        public void OnDeleteClick(object sender, EventArgs e)
        {
            Model model = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as Model;
            if (model != null && model.GetType().Name != "Simulations")
            {
                DeleteModelCommand command = new DeleteModelCommand(model);
                this.explorerPresenter.CommandHistory.Add(command, true);
            }
        }

        /// <summary>
        /// Event handler for a User interface "Run APSIM" action
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Run APSIM",
                     AppliesTo = new Type[] { typeof(Simulation),
                                              typeof(Simulations),
                                              typeof(Experiment),
                                              typeof(Folder) },
                     ShortcutKey = Keys.F5)]
        public void RunAPSIM(object sender, EventArgs e)
        {
            if (this.explorerPresenter.Save())
            {
                Model model = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as Model;
                this.command = new Commands.RunCommand(this.explorerPresenter.ApsimXFile, model, this.explorerPresenter);
                this.command.Do(null);
            }
        }

        /// <summary>
        /// A run has completed so re-enable the run button.
        /// </summary>
        /// <returns>True when APSIM is not running</returns>
        public bool RunAPSIMEnabled()
        {
            bool isRunning = this.command != null && this.command.IsRunning;
            if (!isRunning)
            {
                this.command = null;
            }

            return !isRunning;
        }

        /// <summary>
        /// Event handler for a User interface "Run APSIM" action
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Check Soil", AppliesTo = new Type[] { typeof(Soil) })]
        public void CheckSoil(object sender, EventArgs e)
        {
            Soil currentSoil = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as Soil;
            if (currentSoil != null)
            {
                string errorMessages = currentSoil.Check(false);
                if (errorMessages != string.Empty)
                {
                    this.explorerPresenter.ShowMessage(errorMessages, DataStore.ErrorLevel.Error);
                }
            }
        }

        /// <summary>
        /// Event handler for a User interface "Run APSIM" action
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Run Tests", AppliesTo = new Type[] { typeof(Tests) })]
        public void RunTests(object sender, EventArgs e)
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"Software\R-core\R", false);
            if (registryKey != null)
            {
                // Will need to make this work on 32bit machines
                string pathToR = (string)registryKey.GetValue("InstallPath", string.Empty);
                pathToR += "\\Bin\\x64\\rscript.exe";

                string binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string scriptFileName = Path.Combine(new string[] 
                    {
                        binFolder, 
                        "..", 
                        "Tests", 
                        "RTestSuite",
                        "RunTest.R"
                    });

                string workingFolder = Path.Combine(new string[] { binFolder, ".." });

                Process process = new Process();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = workingFolder;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = pathToR;
                process.StartInfo.Arguments = "\"" + scriptFileName + "\" " + "\"" + this.explorerPresenter.ApsimXFile.FileName + "\"";
                process.Start();
                process.WaitForExit();
                this.explorerPresenter.ShowMessage(process.StandardOutput.ReadToEnd(), DataStore.ErrorLevel.Information);
                this.explorerPresenter.ShowMessage(process.StandardError.ReadToEnd(), DataStore.ErrorLevel.Warning);
            }
            else
            {
                this.explorerPresenter.ShowMessage("Could not find R installation.", DataStore.ErrorLevel.Warning);
            }

            {
            string binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string apsimxFolder = Path.Combine(binFolder, "..");
            string scriptFileName = Path.Combine(new string[] 
            {
                binFolder, 
                "..", 
                "Tests", 
                "RTestSuite",
                "RunTest.Bat"
            });
            string workingFolder = apsimxFolder;
            Process process = Utility.Process.RunProcess(scriptFileName, this.explorerPresenter.ApsimXFile.FileName, workingFolder);
            string errorMessages = Utility.Process.CheckProcessExitedProperly(process);
            }
        }

        /// <summary>
        /// Event handler for adding a factor
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Add factor", AppliesTo = new Type[] { typeof(Factors) })]
        public void AddFactor(object sender, EventArgs e)
        {
            Model factors = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as Model;
            if (factors != null)
            {
                AddModelCommand command = new AddModelCommand("<Factor/>", factors);
                this.explorerPresenter.CommandHistory.Add(command, true);
            }
        }

        /// <summary>
        /// Run post simulation models.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Refresh",
                     AppliesTo = new Type[] { typeof(DataStore) })]
        public void RunPostSimulationModels(object sender, EventArgs e)
        {
            DataStore dataStore = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as DataStore;
            if (dataStore != null)
            {
                try
                {
                    // Run all child model post processors.
                    dataStore.RunPostProcessingTools();
                    this.explorerPresenter.ShowMessage("Post processing models have successfully completed", Models.DataStore.ErrorLevel.Information);
                }
                catch (Exception err)
                {
                    this.explorerPresenter.ShowMessage("Error: " + err.Message, Models.DataStore.ErrorLevel.Error);
                }
            }
        }

        /// <summary>
        /// Empty the data store
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Empty the data store",
                     AppliesTo = new Type[] { typeof(DataStore) })]
        public void EmptyDataStore(object sender, EventArgs e)
        {
            DataStore dataStore = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as DataStore;
            if (dataStore != null)
            {
                dataStore.DeleteAllTables();
            }
        }

        /// <summary>
        /// Export the data store to comma separated values
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Export to CSV",
                     AppliesTo = new Type[] { typeof(DataStore) })]
        public void ExportDataStoreToCSV(object sender, EventArgs e)
        {
            DataStore dataStore = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as DataStore;
            if (dataStore != null)
            {
                dataStore.WriteToTextFiles();
            }
        }

        /// <summary>
        /// Export the data store to EXCEL format
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Export to EXCEL",
                     AppliesTo = new Type[] { typeof(DataStore) })]
        public void ExportDataStoreToEXCEL(object sender, EventArgs e)
        {
            DataStore dataStore = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as DataStore;
            if (dataStore != null)
            {
                List<DataTable> tables = new List<DataTable>();
                foreach (string tableName in dataStore.TableNames)
                {
                    if (tableName != "Simulations" && tableName != "Messages" && tableName != "InitialConditions")
                    {
                        DataTable table = dataStore.GetData("*", tableName, true);
                        table.TableName = tableName;
                        tables.Add(table);
                    }
                }
                Cursor.Current = Cursors.WaitCursor; 
                string fileName = Path.ChangeExtension(dataStore.Filename, ".xlsx");
                Utility.Excel.WriteToEXCEL(tables.ToArray(), fileName);
                Cursor.Current = Cursors.Default; 
            }
        }

        /// <summary>
        /// Event handler for a User interface "Export" action
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        [ContextMenu(MenuName = "Export to HTML")]
        public void ExportToHTML(object sender, EventArgs e)
        {
            string destinationFolder = this.explorerPresenter.AskUserForFolder("Select folder to export to");
            if (destinationFolder != null)
            {
                Model modelClicked = Apsim.Get(this.explorerPresenter.ApsimXFile, this.explorerPresenter.CurrentNodePath) as Model;
                if (modelClicked != null)
                {
                    if (modelClicked is Simulations)
                    {
                        ExportNodeCommand command = new ExportNodeCommand(this.explorerPresenter, this.explorerPresenter.CurrentNodePath, destinationFolder);
                        this.explorerPresenter.CommandHistory.Add(command, true);
                    }
                    else
                    {
                        string fileName = Path.Combine(destinationFolder, modelClicked.Name + ".html");

                        Classes.PMFDocumentation.Go(fileName, modelClicked);
                    }
                }
            }
        }

    }
}