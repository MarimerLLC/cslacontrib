using System;
using System.Collections;
using System.IO;

namespace Csla.NHibernate
{
    /// <summary>
    /// NFS file utilities.
    /// </summary>
    public static class File
    {
        /// <summary>
        /// gets the true working directory where the application root is
        /// </summary>
        /// <remarks>if a web service or site is hosted in IIS, then 
        /// the working directory is system32 and any non-pathed file lookups go
        /// to that directory</remarks>
        public static string TrueWorkingDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary>Gets a list of all the assemblies (*.DLLs) from AppDomain.CurrentDomain.BaseDirectory including sub-directories</summary>
        /// <param name="returnPath">should the string[] contain the path to each file</param>
        /// <returns>a string array of file names optionally including their paths</returns>
        public static string[] GetAssemblyListInBaseDirectory(bool returnPath)
        {
            return GetFileListInBaseDirectory("*.dll", returnPath);
        }

        /// <summary>Gets a list of NFS assemblies (Nfs*.dll) from AppDomain.CurrentDomain.BaseDirectory including sub-directories</summary>
        /// <param name="returnPath">should the string[] contain the path to each file</param>
        /// <returns>a string array of file names optionally including their paths</returns>
        public static string[] GetNFSAssemblyListInBaseDirectory(bool returnPath)
        {
            return GetFileListInBaseDirectory("Nfs*.dll", returnPath);
        }

        /// <summary>
        /// Gets a list of NFS config files (Nfs*.xml;Nfs*.config) from AppDomain.CurrentDomain.BaseDirectory including sub-directories
        /// </summary>
        /// <param name="returnPath">should the string[] contain the path to each file</param>
        /// <returns>a string array of file names optionally including their paths</returns>    
        public static string[] GetNFSConfigFilesInBaseDirectory(bool returnPath)
        {
            ArrayList configFiles = new ArrayList();
            configFiles.AddRange(GetFileListInBaseDirectory("Nfs*.xml", returnPath));
            configFiles.AddRange(GetFileListInBaseDirectory("Nfs*.config", returnPath));
            configFiles.TrimToSize();

            return (string[]) configFiles.ToArray(typeof (string));
        }

        /// <summary>
        /// Gets a list of files from AppDomain.CurrentDomain.BaseDirectory including sub-directories
        /// </summary>
        /// <param name="pattern">the pattern to search for e.g. *.dll</param>
        /// <param name="returnPath">should the files that are returned contain their path</param>
        /// <returns>a string array of file names optionally including their paths</returns>
        public static string[] GetFileListInBaseDirectory(string pattern, bool returnPath)
        {
			if (String.IsNullOrEmpty(pattern))
				throw new ArgumentOutOfRangeException("Pattern cannot be null or empty");

            string[] nameArray;

            // Create a reference to the current directory.
            DirectoryInfo di = new DirectoryInfo(TrueWorkingDirectory);

            // Create an array representing the files in the current directory.
            FileInfo[] fileInfoArray = di.GetFiles(pattern, SearchOption.AllDirectories);

            // Create the array to hold the names
            nameArray = new string[fileInfoArray.Length];

            int index = 0;

            // Copy the names of the files into the array
            foreach (FileInfo fileInfo in fileInfoArray)
            {
                if (returnPath)
                {
                    nameArray[index] = fileInfo.FullName;
                }
                else
                {
                    nameArray[index] = fileInfo.Name;
                }
                index++;
            }

            return nameArray;
        }

        /// <summary>
        /// Gets a namded NFS config file (Nfs*.xml or Nfs*.config) from the true working directory
        /// </summary>
        /// <param name="configFile">the config file to search for</param>
        /// <returns>a string containing the fully qualified path to the config</returns>
        /// <exception cref="FileNotFoundException">If the specified config file is not found</exception>
        public static string GetNFSConfigFile(string configFile)
        {
            string[] files = GetFileListInBaseDirectory(configFile, true);
            if (files.Length < 1)
                throw new FileNotFoundException(String.Format("File '{0}' not found in folder '{1}'", configFile, TrueWorkingDirectory));

            return files[0];
        }
    }
}