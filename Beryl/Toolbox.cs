using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beryl
{
    public class Toolbox
    {
        // IsSystemReserved:
        // Return true if the specified ABSOLUTE path name is the name of a
        // reserved Windows directory
        public static bool IsSystemReserved(string value)
        {
            string path = value.Substring(1);

            switch (path)
            {
                case @":\System Volume Information":
                case @":\$RECYCLE.BIN":
                case @":\RECYCLER":
                    return true;

                default:
                    return false;
            }
        }

        // Find:
        // Finds all files matching the specified wildcard.
        // If 'recurse' is true, it searches all subdirectories too.
        public static string[] Find(string wildcard, bool recurse)
        {
            // set up sentinel used to as a cludge around empty directory names
            char sep = System.IO.Path.DirectorySeparatorChar;
            string cludge = "." + sep + "." + sep + "." + sep + "." + sep + ".";

            // expand wildcard and check that each file exists
            List<string> result = new List<string>();

            // handle the lame .NET case where the directory name is empty
            // this is done by using a sentinel of @".\.\.\.\."
            string directory = System.IO.Path.GetDirectoryName(wildcard);
            if (directory.Length == 0)
                directory = cludge;

            // skip system-reserved directories (to avoid errors)
            string abspath = System.IO.Path.GetFullPath(directory);
            if (IsSystemReserved(abspath))
                return new string[0];

            string filename = System.IO.Path.GetFileName(wildcard);
            string[] files = System.IO.Directory.GetFiles(directory, filename);

            // add each file to our list of files to process
            foreach (string file in files)
            {
                // skip duplicate directories (to avoid lame output)
                if (result.Contains(file))
                    continue;

                // remove the sentinel, if any
                string name = file;
                if (name.IndexOf(cludge, System.StringComparison.Ordinal) == 0)
                    name = file.Substring(cludge.Length + 1);

                // add the directory to our list of directories to process
                result.Add(name);
            }

            // recurse subdirectories, if applicable
            if (recurse)
            {
                string[] subdirs = System.IO.Directory.GetDirectories(directory, "*");

                // ... iterate over found subdirectories
                foreach (string dir in subdirs)
                {
                    string[] children = Find(dir + sep + filename, recurse);

                    // ... iterate over each found file in the current subdir
                    foreach (string child in children)
                    {
                        // remove the sentinel, if any
                        string name = child;
                        if (name.IndexOf(cludge, System.StringComparison.Ordinal) == 0)
                            name = name.Substring(cludge.Length);

                        result.Add(name);
                    }
                }
            }

            return result.ToArray();
        }

        public static string[] Find(string[] wildcards, bool recurse)
        {
            List<string> result = new List<string>();

            foreach (string wildcard in wildcards)
            {
                string[] matches = Find(wildcard, recurse);

                // report error if the wildcard didn't match anything
                if (matches.Length == 0)
                    throw new BerylError("No matches found: " + wildcard);

                // copy to our assembled list of directories
                foreach (string match in matches)
                    result.Add(match);
            }

            // convert the collection to an array of string
            return result.ToArray();
        }
    }
}
