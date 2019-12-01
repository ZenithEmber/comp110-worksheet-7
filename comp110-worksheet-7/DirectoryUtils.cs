using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp110_worksheet_7
{
    public static class DirectoryUtils
    {
        // Return the size, in bytes, of the given file
        public static long GetFileSize(string directory)
        {
            return new FileInfo(directory).Length;
        }

        // Return true if the given path points to a directory, false if it points to a file
        public static bool IsDirectory(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        // Return the total size, in bytes, of all the files below the given directory
        public static long GetTotalSize(string directory)
        {
            String[] files; //Make an array for the files.
            long fileSize = 0; //Create a variable to store the size of each file.

            files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories); //Populates the files array with the files in the directory.
            foreach (string file in files) //Looks at each file in the files array.
            {
                fileSize += GetFileSize(file); //And then adds them to the fileSize variable.
            }
            return fileSize; //Returns the total fileSize to the log.
        }

        // Return the number of files (not counting directories) below the given directory
        public static int CountFiles(string directory)
        {
            int numberOfFiles = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).Length; //Initialises an integer variable called numberOfFiles and looks through all the paths in the directory.
            return numberOfFiles; //Returns an integer displaying how many files are in the directory.
        }

        // Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
        public static int GetDepth(string directory)
        {
            string[] path = Directory.GetDirectories(directory); //Creates a string array for the list of directories.
            int depth = 0; //initialises integer variable called depth.
            int elementDepth; //initialises integer variable called elementDepth.

            foreach (string element in path) //Runs the code in the curly brackets for every element in the directory.
            {
                elementDepth = GetDepth(element) + 1; //Adds 1 to the integer variable tount depth. 
                if (elementDepth > depth)
                {
                    depth = elementDepth;
                }
            }
            return depth;
        }

        // Get the path and size (in bytes) of the smallest file below the given directory
        public static Tuple<string, long> GetSmallestFile(string directory)
        {
            string[] elementsInDirectory = Directory.GetFileSystemEntries(directory); //Creates a string array for the elements in a directory.
            Tuple<string, long> smallestFile = new Tuple<string, long>(String.Empty, 0); //Sorts each file in a directory.
            Tuple<string, long> smallestFileInSubDir;
            long currentFileSize;

            foreach (string element in elementsInDirectory)
            {
                if (IsDirectory(element))
                {
                    smallestFileInSubDir = GetSmallestFile(element);
                    if (smallestFile.Item2 > smallestFileInSubDir.Item2 || smallestFile.Item2 == 0) //Tuple has made the Items from the elements inside the directory.
                    {
                        smallestFile = smallestFileInSubDir;
                    }
                }
                else
                {
                    currentFileSize = GetFileSize(element);
                    if (smallestFile.Item2 > currentFileSize || smallestFile.Item2 == 0)
                    {
                        smallestFile = new Tuple<string, long>(element, currentFileSize);
                    }
                }
            }

            return smallestFile;
        }

        // Get the path and size (in bytes) of the largest file below the given directory
        public static Tuple<string, long> GetLargestFile(string directory)
        {
            //this code is exactly the same as the GetSmallestFile however there are variable name changes and the less greater than symbols has been reversed.
            string[] elementsInDirectory = Directory.GetFileSystemEntries(directory);
            Tuple<string, long> largestFile = new Tuple<string, long>(String.Empty, 0);
            Tuple<string, long> largestFileInSubDir;
            long currentFileSize;

            foreach (string element in elementsInDirectory)
            {
                if (IsDirectory(element))
                {
                    largestFileInSubDir = GetLargestFile(element);
                    if (largestFile.Item2 < largestFileInSubDir.Item2 || largestFile.Item2 == 0)
                    {
                        largestFile = largestFileInSubDir;
                    }
                }
                else
                {
                    currentFileSize = GetFileSize(element);
                    if (largestFile.Item2 < currentFileSize || largestFile.Item2 == 0)
                    {
                        largestFile = new Tuple<string, long>(element, currentFileSize);
                    }
                }
            }

            return largestFile;
        }


        // Get all files whose size is equal to the given value (in bytes) below the given directory
        public static IEnumerable<string> GetFilesOfSize(string directory, long size)
        {
            long fileSize;
            List<string> list = new List<string>();
            foreach (FileInfo file in new DirectoryInfo(directory).GetFiles())
            {
                fileSize = GetFileSize(file);
                if (fileSize == size)
                { 
                    list.Add(file.FullName);
                }
            }
            return list;
        }
    }
}
