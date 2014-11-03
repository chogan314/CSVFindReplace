using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CSVFindReplace
{
    public partial class Form1 : Form
    {
        private const string CONFIG_FILE = @"config.txt";
        CSVDirectory directory;

        public Form1()
        {
            InitializeComponent();
            directory = new CSVDirectory();

            notifyIcon.Visible = false;

            AddRowToFindReplace();

            loadSettings();
            populateFileList();
        }


        // Add row to findReplace list view
        public void AddRowToFindReplace()
        {
            FindReplaceRow row = new FindReplaceRow(findReplace);
            row.addToParent();
        }


        // Causes minimize to send form to system tray.
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                Hide();
            }
            else if (WindowState == FormWindowState.Normal)
            {
                notifyIcon.Visible = false;
            }
        }


        // Restores on clicking notify icon.
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }


        // Populates list of .csv files based on source folder.
        private void populateFileList()
        {
            directory.InitDirectory(sourcePath.Text);
            csvFileList.Items.Clear();

            if (directory.PathIsValid)
            {
                foreach (string fileTag in directory.FileMap.Keys)
                {
                    csvFileList.Items.Add(fileTag, 
                        directory.NewFiles.Contains(directory.FileMap[fileTag]));
                }
            }
        }

        // Selects location unprocessed .csv files will be located
        // and then updates list of .csv files.
        private void source_Click(object sender, EventArgs e)
        {
            if (sourceBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                sourcePath.Text = sourceBrowserDialog.SelectedPath;
                destPath.Text = sourceBrowserDialog.SelectedPath;
            }

            populateFileList();
        }


        // Updates list of .csv files.
        private void refresh_Click(object sender, EventArgs e)
        {
            populateFileList();
        }


        // Selects location processed .xlsx files will be saved to.
        private void destination_Click(object sender, EventArgs e)
        {
            if (destBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                destPath.Text = destBrowserDialog.SelectedPath;
            }
        }

        
        // Reads CSV files and exports them as XLSX
        private void run()
        {
            if (directory == null || !directory.PathIsValid)
            {
                MessageBox.Show("Source path is not valid.", "Error", MessageBoxButtons.OK, 
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                return;
            }

            if (!Directory.Exists(destPath.Text))
            {
                MessageBox.Show("Destination path is not valid", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                return;
            }

            if (csvFileList.CheckedItems.Count == 0)
            {
                MessageBox.Show("No .CSV files selected", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                return;
            }

            int startRowNum = parseStartRow(startRow.Text);

            if (startRowNum < 1)
            {
                MessageBox.Show("Invalid start row number.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                return;
            }

            List<RowParser> rowParsers = new List<RowParser>();
            bool allRowsValid = true;
            int firstInvalidRow = -1;

            int rowNum = 1;
            foreach (FindReplaceRow row in FindReplaceRow.Container)
            {
                RowParser rowParser = new RowParser(row, startRow.Text);

                if (!rowParser.IsValid)
                {
                    allRowsValid = false;
                    if (firstInvalidRow < 0)
                    {
                        firstInvalidRow = rowNum;
                    }
                }

                rowParsers.Add(rowParser);
                rowNum++;
            }

            if (!allRowsValid)
            {
                RowParser invalidParser = rowParsers[firstInvalidRow - 1];

                StringBuilder builder = new StringBuilder();
                builder.Append("Value(s) for ");

                if (!invalidParser.FindStringIsValid)
                {
                    builder.Append("\"Find\" ");
                }

                if (!invalidParser.ReplaceStringIsValid)
                {
                    builder.Append("\"Replace with\" ");
                }

                if (!invalidParser.ColumnsIsValid)
                {
                    builder.Append("\"From columns\" ");
                }

                MessageBox.Show("Invalid entry for row " + firstInvalidRow + ". " + builder.ToString() + " are invalid.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                return;
            }

            string[] fileTags = new string[csvFileList.CheckedItems.Count];
            int idx = 0;

            foreach (var item in csvFileList.CheckedItems)
            {
                fileTags[idx] = Convert.ToString(item);
                idx++;
            }

            CSVReader reader = new CSVReader(directory, fileTags, @destPath.Text, 
                rowParsers, parseDelimiter(delimiter.Text), startRowNum);

            List<string> unprocessedFiles = reader.execute();
            directory.finishProcessing(fileTags, unprocessedFiles);
            populateFileList();
            saveSettings();
        }


        private void runButton_Click(object sender, EventArgs e)
        {
            run();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddRowToFindReplace();
        }


        private void loadSettings()
        {
            if (File.Exists(CONFIG_FILE))
            {
                string[] lines = File.ReadAllLines(CONFIG_FILE);
                int findReplaceRowIndex = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] split = line.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    string tag = split[0].Trim();
                    string value = split[1].Trim();

                    switch (tag)
                    {
                        case "sourcePath":
                            sourcePath.Text = value;
                            break;
                        case "oldFile":
                            directory.OldFiles.Add(value);
                            break;
                        case "destPath":
                            destPath.Text = value;
                            break;
                        case "startRow":
                            startRow.Text = value;
                            break;
                        case "delimiter":
                            delimiter.Text = value;
                            break;
                        case "findReplaceRow":
                            int numLines = int.Parse(value);
                            string[] linesForRow = new string[numLines];

                            for (int j = 0; j < numLines; j++)
                            {
                                linesForRow[j] = lines[i + 1 + j];
                            }

                            loadFindReplaceRow(linesForRow, findReplaceRowIndex);
                            findReplaceRowIndex++;
                            i += numLines;

                            break;
                        default:
                            break;
                    }
                }
            }
        }


        private void loadFindReplaceRow(string[] lines, int rowIndex)
        {
            if (rowIndex > 0)
            {
                AddRowToFindReplace();
            }

            FindReplaceRow row = FindReplaceRow.Container[rowIndex];

            foreach (string line in lines)
            {
                string[] split = line.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string tag = split[0].Trim();
                string value = split[1].Trim();

                switch (tag)
                {
                    case "find":
                        row.FindBox.Text = value;
                        break;
                    case "replace":
                        row.ReplaceBox.Text = value;
                        break;
                    case "format":
                        loadCellFormat(row.FormatBox, value);
                        break;
                    case "columns":
                        row.ColumnsBox.Text = value;
                        break;
                    default:
                        break;
                }
            }
        }


        private void loadCellFormat(ComboBox formatBox, string value)
        {
            switch (value)
            {
                case "text":
                    formatBox.SelectedIndex = 0;
                    break;
                case "number":
                    formatBox.SelectedIndex = 1;
                    break;
                case "currency":
                    formatBox.SelectedIndex = 2;
                    break;
                case "date":
                    formatBox.SelectedIndex = 3;
                    break;
                case "time":
                    formatBox.SelectedIndex = 4;
                    break;
                case "percentage":
                    formatBox.SelectedIndex = 5;
                    break;
                case "default":
                    formatBox.SelectedIndex = 6;
                    break;
                default:
                    break;
            }
        }


        private void saveSettings()
        {
            if (File.Exists(CONFIG_FILE))
            {
                File.Delete(CONFIG_FILE);
            }

            List<string> linesToWrite = new List<string>();

            if (directory.PathIsValid)
            {
                linesToWrite.Add("sourcePath | " + directory.Path);
                foreach (string oldFile in directory.OldFiles)
                {
                    linesToWrite.Add("oldFile | " + oldFile);
                }
            }

            if (destPath.Text != String.Empty)
            {
                linesToWrite.Add("destPath | " + destPath.Text);
            }

            if (startRow.Text != String.Empty)
            {
                linesToWrite.Add("startRow | " + startRow.Text);
            }

            if (delimiter.Text != String.Empty)
            {
                linesToWrite.Add("delimiter | " + delimiter.Text);
            }

            foreach (FindReplaceRow row in FindReplaceRow.Container)
            {
                int nonempty = 0;
                List<string> rowValues = new List<string>();

                if (row.FindBox.Text != String.Empty)
                {
                    nonempty++;
                    rowValues.Add("find | " + row.FindBox.Text);
                }

                if (row.ReplaceBox.Text != String.Empty)
                {
                    nonempty++;
                    rowValues.Add("replace | " + row.ReplaceBox.Text);
                }

                switch (row.FormatBox.SelectedIndex)
                {
                    case 0:
                        nonempty++;
                        rowValues.Add("format | text");
                        break;
                    case 1:
                        nonempty++;
                        rowValues.Add("format | number");
                        break;
                    case 2:
                        nonempty++;
                        rowValues.Add("format | currency");
                        break;
                    case 3:
                        nonempty++;
                        rowValues.Add("format | date");
                        break;
                    case 4:
                        nonempty++;
                        rowValues.Add("format | time");
                        break;
                    case 5:
                        nonempty++;
                        rowValues.Add("format | percentage");
                        break;
                    case 6:
                        nonempty++;
                        rowValues.Add("format | default");
                        break;
                    default:
                        break;
                }

                if (row.ColumnsBox.Text != String.Empty)
                {
                    nonempty++;
                    rowValues.Add("columns | " + row.ColumnsBox.Text);
                }

                if (nonempty > 0)
                {
                    linesToWrite.Add("findReplaceRow | " + nonempty);
                    linesToWrite.AddRange(rowValues);
                }
            }

            File.WriteAllLines(CONFIG_FILE, linesToWrite);
        }


        private void saveParameters_Click(object sender, EventArgs e)
        {
            saveSettings();
        }


        private void resetParameters_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Reset parameters to default?", "Confirm Reset", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                reset();
            }
        }


        private void reset()
        {
            if (File.Exists(CONFIG_FILE))
            {
                File.Delete(CONFIG_FILE);
            }

            sourcePath.Text = String.Empty;
            destPath.Text = String.Empty;
            startRow.Text = String.Empty;

            FindReplaceRow[] rows = new FindReplaceRow[FindReplaceRow.Container.Count];

            for (int i = 1; i < rows.Length; i++)
            {
                rows[i] = FindReplaceRow.Container[i];
            }

            for (int i = 1; i < rows.Length; i++)
            {
                rows[i].selfDestruct();
            }

            FindReplaceRow lastRow = FindReplaceRow.Container[0];
            lastRow.FindBox.Text = String.Empty;
            lastRow.ReplaceBox.Text = String.Empty;
            lastRow.FormatBox.SelectedIndex = 6;
            lastRow.ColumnsBox.Text = String.Empty;
        }


        private char parseDelimiter(string text)
        {
            if (text.Length == 0)
            {
                return ' ';
            }
            else
            {
                return text[0];
            }
        }


        private int parseStartRow(string text)
        {
            int result = 0;
            if (int.TryParse(text, out result) && !text.Equals(String.Empty))
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
    }
}
