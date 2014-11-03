using System;
using System.Collections.Generic;

namespace CSVFindReplace
{
    public class RowParser
    {
        public enum eFindLocation
        {
            Start, End, Match, Anywhere, Invalid
        }

        public enum eFindMode
        {
            PrependToAll, AppendToAll, DoNothing, Normal
        }

        private FindReplaceRow Row { get; set; }

        public eFindLocation FindLocation { get; private set; }
        public string FindString { get; private set; }
        public string ReplaceString { get; private set; }
        public eCellFormat CellFormat { get; private set; }
        public HashSet<int> Columns { get; private set; }

        public eFindMode FindMode { get; private set; }
        public bool ReplaceNil { get; private set; }
        public bool NoFormat { get; private set; }
        public bool AllColumns { get; private set; }

        public bool FindStringIsValid { get; private set; }
        public bool ReplaceStringIsValid { get; private set; }
        public bool ColumnsIsValid { get; private set; }

        public bool IsValid {
            get
            {
                return (FindStringIsValid && ReplaceStringIsValid && ColumnsIsValid);
            }
        }


        public RowParser(FindReplaceRow row, string startRowCSV)
        {
            Row = row;

            FindStringIsValid = true;
            ReplaceStringIsValid = true;
            ColumnsIsValid = true;

            FindMode = eFindMode.Normal;
            ReplaceNil = false;
            NoFormat = false;
            AllColumns = false;

            FindLocation = parseFindLocation();
            FindString = parseFindString();
            ReplaceString = parseReplaceString();
            CellFormat = parseCellFormat();
            Columns = parseColumns();
        }


        private eFindLocation parseFindLocation()
        {
            string findText = Row.FindBox.Text;

            if (findText.Length == 0)
            {
                return eFindLocation.Invalid;
            }

            bool startStar = findText[0].Equals('*');
            bool endStar = findText[findText.Length - 1].Equals('*');

            if (startStar && endStar && (findText.Length == 1 || findText.Length == 2))
            {
                FindStringIsValid = false;
                return eFindLocation.Invalid;
            }

            if (startStar && endStar)
            {
                return eFindLocation.Match;
            }
            else if (startStar)
            {
                return eFindLocation.End;
            }
            else if (endStar)
            {
                return eFindLocation.Start;
            }
            else
            {
                return eFindLocation.Anywhere;
            }
        }


        private string parseFindString()
        {
            string findText = string.Copy(Row.FindBox.Text).Trim();

            switch (findText)
            {
                case "\\prepend\\":
                    FindMode = eFindMode.PrependToAll;
                    return null;
                case "\\append\\":
                    FindMode = eFindMode.AppendToAll;
                    return null;
                case "":
                    FindMode = eFindMode.DoNothing;
                    return null;
                default:
                    break;
            }

            if (findText[0].Equals('\\') && findText.Length > 1)
            {
                findText = findText.Substring(1);
            }
            else if (findText[0].Equals('*') && findText.Length > 1)
            {
                findText = findText.Substring(1);
            }

            if (findText[findText.Length - 1].Equals('\\') && findText.Length > 1)
            {
                findText = findText.Substring(0, findText.Length - 1);
            }
            else if (findText[findText.Length - 1].Equals('*') && findText.Length > 1)
            {
                findText = findText.Substring(0, findText.Length - 1);
            }

            return findText;
        }


        private string parseReplaceString()
        {
            if (Row.ReplaceBox.Text.Trim().Equals(""))
            {
                ReplaceNil = true;
                return null;
            }

            return Row.ReplaceBox.Text;
        }


        private eCellFormat parseCellFormat()
        {
            if (Row.FormatBox.SelectedItem != null)
            {
                return (eCellFormat) Row.FormatBox.SelectedItem;
            }
            else
            {
                NoFormat = true;
                return eCellFormat.Invalid;
            }
        }


        // This is an abomination
        private HashSet<int> parseColumns()
        {
            if (Row.ColumnsBox.Text.Trim().Equals(""))
            {
                AllColumns = true;
                return null;
            }

            HashSet<int> columns = new HashSet<int>();

            string[] columnRangesAndValues = Row.ColumnsBox.Text.Split(new char[] { ',' }, 
                StringSplitOptions.RemoveEmptyEntries);

            HashSet<string> columnRanges = new HashSet<string>();

            // For each comma seperated value in columns input
            foreach (string columnRAV in columnRangesAndValues)
            {
                string trimmed = columnRAV.Trim();
                int result;

                // If that value is just a number, add it to columns set
                if (int.TryParse(trimmed, out result))
                {
                    columns.Add(result);
                }
                // Else check if it's a range
                else if (trimmed.Contains("-"))
                {
                    string[] trimmedSplit = trimmed.Split(new char[] { '-' },
                        StringSplitOptions.RemoveEmptyEntries);

                    if (trimmedSplit.Length == 2)
                    {
                        int start;
                        int stop;

                        // If range start and stop are integers
                        if (int.TryParse(trimmedSplit[0].Trim(), out start) &&
                            int.TryParse(trimmedSplit[1].Trim(), out stop))
                        {
                            if (start < stop)
                            {
                                // Add all columns in range to columns set
                                for (int i = start; i <= stop; i++)
                                {
                                    columns.Add(i);
                                }
                            }
                            else
                            {
                                // Add all columns in range to columns set
                                for (int i = stop; i <= start; i++)
                                {
                                    columns.Add(i);
                                }
                            }
                        }
                    }
                }
            }

            return columns;
        }
    }
}