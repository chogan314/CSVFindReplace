using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml.Table;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;

namespace CSVFindReplace
{
    public class CSVReader
    {
        private string OutputDirectory { get; set; }
        private List<RowParser> RowParsers { get; set; }
        private string[] FilePaths { get; set; }
        private int StartingRow { get; set; }
        private char TextDelimiter { get; set; }


        public CSVReader(CSVDirectory sourceDirectory, string[] fileTags, string outputDirectory, 
            List<RowParser> rowParsers, char delimiter, int startingRow)
        {
            OutputDirectory = outputDirectory;
            RowParsers = rowParsers;
            FilePaths = initFilePaths(sourceDirectory, fileTags);
            TextDelimiter = delimiter;
            StartingRow = startingRow;
        }


        private string[] initFilePaths(CSVDirectory directory, string[] fileTags)
        {
            string[] paths = new string[fileTags.Length];

            for (int i = 0; i < fileTags.Length; i++)
            {
                paths[i] = directory.FileMap[fileTags[i]];
            }

            return paths;
        }


        public List<string> execute()
        {
            List<string> alreadyExist = new List<string>();

            foreach (string file in FilePaths)
            {
                FileInfo newFile = new FileInfo(OutputDirectory + "\\" + parseFileName(file) + ".xlsx");

                if (newFile.Exists)
                {
                    alreadyExist.Add(newFile.FullName);
                    continue;
                }

                using (ExcelPackage package = new ExcelPackage())
                {
                    if (TextDelimiter == ' ')
                    {
                        LoadFile(package, file);
                    }
                    else
                    {
                        string tempFileName = parseTextWithDelimiter(file);
                        LoadFile(package, tempFileName);

                        if (File.Exists(tempFileName))
                        {
                            File.Delete(tempFileName);
                        }
                    }

                    package.SaveAs(newFile);
                }
            }

            if (!(alreadyExist.Count == 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("These files already exist in the given output directory.\nThe .CSV files with the same names were not processed.\n\n");
                foreach (string existingFile in alreadyExist)
                {
                    builder.Append(parseFileName(existingFile) + ".xlsx\n");
                }

                MessageBox.Show(builder.ToString(), "Warning", MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            List<string> toCSV = new List<String>();

            foreach (string xlsxFile in alreadyExist)
            {
                string toAdd = (xlsxFile.Substring(0, xlsxFile.LastIndexOf('.')) + ".CSV").ToLower();
                toCSV.Add(toAdd);
            }

            return toCSV;
        }


        private string parseTextWithDelimiter(string file)
        {
            string[] lines = File.ReadAllLines(file);

            List<string> linesToWrite = new List<string>();

            foreach (string line in lines)
            {
                StringBuilder builder = new StringBuilder();
                bool literal = false;

                foreach (char c in line)
                {
                    if (literal)
                    {
                        if (c == TextDelimiter)
                        {
                            literal = false;
                        }
                        else
                        {
                            builder.Append(c);
                        }
                    }
                    else
                    {
                        if (c == TextDelimiter)
                        {
                            literal = true;
                        }
                        else if (c == ',')
                        {
                            builder.Append('|');
                        }
                        else
                        {
                            builder.Append(c);
                        }
                    }
                }

                linesToWrite.Add(builder.ToString());
            }

            string tempFileName = parseFileName(file) + "_temp.CSV";

            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }

            File.WriteAllLines(tempFileName, linesToWrite);
            return tempFileName;
        }


        private void LoadFile(ExcelPackage package, string file)
        {
            //Create the Worksheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Csv1");

            ExcelTextFormat format = new ExcelTextFormat();
            format.Delimiter = '|';

            worksheet.Cells["A1"].LoadFromText(new FileInfo(file), format);

            foreach (RowParser parser in RowParsers)
            {
                if (parser.AllColumns)
                {
                    HashSet<int> columnNumbers = new HashSet<int>();
                    int numColumns = worksheet.Dimension.End.Column;

                    for (int i = 1; i <= numColumns; i++)
                    {
                        columnNumbers.Add(i);
                    }

                    processColumns(worksheet, parser, columnNumbers);
                }
                else
                {
                    processColumns(worksheet, parser, parser.Columns);
                }
            }
        }


        private void processColumns(ExcelWorksheet worksheet, RowParser parser, HashSet<int> columnNumbers)
        {
            foreach (int columnNumber in columnNumbers)
            {
                int startRow = StartingRow;
                int numRows = worksheet.Dimension.End.Row;
                ExcelRange cells = worksheet.Cells[startRow, columnNumber, numRows, columnNumber];

                if (!parser.ReplaceNil)
                {
                    switch (parser.FindMode)
                    {
                        case RowParser.eFindMode.PrependToAll:
                            foreach (ExcelRangeBase cell in cells)
                            {
                                string strValue = parser.ReplaceString + cell.Value;
                                changeCellValue(parser, cell, strValue);
                            }
                            break;
                        case RowParser.eFindMode.AppendToAll:
                            foreach (ExcelRangeBase cell in cells)
                            {
                                string strValue = cell.Value + parser.ReplaceString;
                                changeCellValue(parser, cell, strValue);
                            }
                            break;
                        case RowParser.eFindMode.DoNothing:
                            break;
                        case RowParser.eFindMode.Normal:
                            findAndReplace(parser, cells);
                            break;
                        default:
                            break;
                    }
                }

                if (!(parser.CellFormat == eCellFormat.Default))
                {
                    cells.Style.Numberformat.Format = getFormatString(parser.CellFormat);
                }
            }
        }


        private void findAndReplace(RowParser parser, ExcelRange cells)
        {
            foreach (ExcelRangeBase cell in cells)
            {
                switch (parser.FindLocation)
                {
                    case RowParser.eFindLocation.Start:
                        findAndReplacePrefix(parser, cell);
                        break;
                    case RowParser.eFindLocation.End:
                        findAndReplaceSuffix(parser, cell);
                        break;
                    case RowParser.eFindLocation.Match:
                        findAndReplaceMatch(parser, cell);
                        break;
                    case RowParser.eFindLocation.Anywhere:
                        findAndReplaceAnywhere(parser, cell);
                        break;
                    case RowParser.eFindLocation.Invalid:
                        break;
                    default:
                        break;
                }
            }
        }


        private void findAndReplacePrefix(RowParser parser, ExcelRangeBase cell)
        {
            string strVal = cell.Value.ToString();
            string findString = parser.FindString;
            string replaceString = parser.ReplaceString;

            if (strVal.Length >= findString.Length && strVal.Substring(0, findString.Length).Equals(findString))
            {
                string replaced = replaceString + strVal.Substring(findString.Length);
                changeCellValue(parser, cell, replaced);
            }
        }


        private void findAndReplaceSuffix(RowParser parser, ExcelRangeBase cell)
        {
            string strVal = cell.Value.ToString();
            string findString = parser.FindString;
            string replaceString = parser.ReplaceString;

            if (strVal.Length >= findString.Length && strVal.Substring(strVal.Length - findString.Length).Equals(findString))
            {
                string replaced = strVal.Substring(findString.Length) + replaceString;
                changeCellValue(parser, cell, replaced);
            }
        }


        private void findAndReplaceMatch(RowParser parser, ExcelRangeBase cell)
        {
            string strVal = cell.Value.ToString();
            string findString = parser.FindString;
            string replaceString = parser.ReplaceString;

            if (strVal.Equals(findString))
            {
                changeCellValue(parser, cell, replaceString);
            }
        }


        private void findAndReplaceAnywhere(RowParser parser, ExcelRangeBase cell)
        {
            string strVal = cell.Value.ToString();
            string findString = parser.FindString;
            string replaceString = parser.ReplaceString;

            if (strVal.Contains(findString))
            {
                string replaced = strVal.Replace(findString, replaceString);
                changeCellValue(parser, cell, replaced);
            }
        }


        private string parseFileName(string filePath)
        {
            string file = filePath.Substring(filePath.LastIndexOf('\\') + 1);
            return file.Substring(0, file.Length - (file.Length - file.LastIndexOf('.')));
        }


        private string getFormatString(eCellFormat format)
        {
            switch (format)
            {
                case eCellFormat.Currency:
                    return "$#,##0.00";
                case eCellFormat.Date:
                    return "mm/dd/yyyy";
                case eCellFormat.Number:
                    return "#";
                case eCellFormat.Percentage:
                    return "0.00%";
                case eCellFormat.Text:
                    return "@";
                case eCellFormat.Time:
                    return "hh:mm";
                default:
                    // ERROR ERROR
                    Debug.WriteLine("ERROR -- CELL FORMAT INVALID");
                    Debug.WriteLine("ERROR -- CELL FORMAT INVALID");
                    Debug.WriteLine("ERROR -- CELL FORMAT INVALID");
                    Debug.WriteLine("ERROR -- CELL FORMAT INVALID");
                    Debug.WriteLine("ERROR -- CELL FORMAT INVALID");
                    return null;
            }
        }


        private void changeCellValue(RowParser parser, ExcelRangeBase cell, string strValue)
        {
            double numValue;
            if (double.TryParse(strValue, out numValue) && parser.CellFormat != eCellFormat.Text)
            {
                cell.Value = numValue;
            }
            else
            {
                cell.Value = strValue;
            }
        }
    }
}