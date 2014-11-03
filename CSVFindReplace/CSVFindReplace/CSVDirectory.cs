using System.Collections.Generic;
using System.IO;

namespace CSVFindReplace
{
    public class CSVDirectory
    {
        private HashSet<string> oldFiles;
        private HashSet<string> newFiles;
        private Dictionary<string, string> fileMap;


        public string Path { get; private set; }
        public bool PathIsValid { get; private set; }


        public HashSet<string> OldFiles
        {
            get { return oldFiles; }
            private set { oldFiles = value; }
        }


        public HashSet<string> NewFiles
        {
            get { return newFiles; }
        }


        public Dictionary<string, string> FileMap
        {
            get { return fileMap; }
        }


        public CSVDirectory()
        {
            // TODO: Initialize from config file
            oldFiles = new HashSet<string>();
            newFiles = new HashSet<string>();

            fileMap = new Dictionary<string,string>();
        }


        // Set directory path to given string
        // and check which files in directory are new.
        public void InitDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Path = @path;
                PathIsValid = true;

                string[] csvFiles = Directory.GetFiles(Path, "*.csv");

                HashSet<string> oldFilesReplacement = new HashSet<string>();

                foreach (string oldFile in OldFiles)
                {
                    bool found = false;

                    foreach (string csvFile in csvFiles)
                    {
                        if (csvFile.Equals(oldFile))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        oldFilesReplacement.Add(oldFile);
                    }
                }

                OldFiles = oldFilesReplacement;

                NewFiles.Clear();

                foreach (string csvFile in csvFiles)
                {
                    if (!OldFiles.Contains(csvFile))
                    {
                        NewFiles.Add(csvFile);
                    }
                }

                GenFileTags();
            }
            else
            {
                PathIsValid = false;
                Path = "";
            }
        }


        // Create map from custom file tags to full file path
        // and return array of file tags
        private void GenFileTags()
        {
            FileMap.Clear();

            foreach (string csvFile in NewFiles)
            {
                string fileTag = "*" + csvFile.Substring(csvFile.LastIndexOf('\\') + 1);
                FileMap.Add(fileTag, csvFile);
            }

            foreach (string csvFile in OldFiles)
            {
                string fileTag = csvFile.Substring(csvFile.LastIndexOf('\\') + 1);
                FileMap.Add(fileTag, csvFile);
            }
        }


        // Move files with given tags from NewFiles to OldFiles
        public void finishProcessing(string[] fileTags, List<string> unprocessedFiles)
        {
            NewFiles.Clear();

            foreach (string fileTag in fileTags)
            {
                string fileName = FileMap[fileTag];

                if (!unprocessedFiles.Contains(fileName.ToLower()))
                {
                    OldFiles.Add(fileName);
                }
            }

        }

    }
}