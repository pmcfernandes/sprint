using System;
using System.Collections.Generic;
using System.IO;

namespace Sprint.IO
{
    public class Directories
    {       /// <summary>
            /// Determines whether the specified folder path is empty.
            /// </summary>
            /// <param name="folderPath">The folder path.</param>
            /// <returns>
            ///   <c>true</c> if the specified folder path is empty; otherwise, <c>false</c>.
            /// </returns>
        public static bool IsEmpty(string folderPath)
        {
            return Directory.GetFiles(folderPath).Length <= 0 && Directory.GetDirectories(folderPath).Length <= 0;
        }

        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public static bool Exists(string folderPath)
        {
            return Directory.Exists(folderPath);
        }

        /// <summary>
        /// Creates a directory
        /// </summary>
        /// <param name="DirectoryPath">Directory to create</param>
        public static void CreateDirectory(string DirectoryPath)
        {
            try
            {
                if (Directory.Exists(DirectoryPath) == false) Directory.CreateDirectory(DirectoryPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a directory and all files found within it.
        /// </summary>
        /// <param name="DirectoryPath">Path to remove</param>
        public static void DeleteDirectory(string DirectoryPath)
        {
            try
            {
                if (Directory.Exists(DirectoryPath)) Directory.Delete(DirectoryPath, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyDirectory(string source, string destination)
        {
            CopyDirectory(source, destination, true, CopyOptions.CopyAlways);
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        public static void CopyDirectory(string source, string destination, bool recursive)
        {
            CopyDirectory(source, destination, recursive, CopyOptions.CopyAlways);
        }

        /// <summary>
        /// Copies a directory to a new location
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <param name="options">The options.</param>
        /// <exception cref="System.Exception"></exception>
        public static void CopyDirectory(string source, string destination, bool recursive, CopyOptions options)
        {
            DirectoryInfo sourceInfo = new DirectoryInfo(source);
            DirectoryInfo destinationInfo = new DirectoryInfo(destination);

            if (Exists(destination) == false)
            {
                CreateDirectory(destination);
            }

            try
            {
                foreach (FileInfo file in FileList(source))
                {
                    switch (options)
                    {
                        case CopyOptions.CopyAlways:
                            file.CopyTo(Path.Combine(destinationInfo.FullName, file.Name), true);
                            break;

                        case CopyOptions.DoNotOverwrite:
                            file.CopyTo(Path.Combine(destinationInfo.FullName, file.Name), false);
                            break;

                        case CopyOptions.CopyIfNewer:
                            if (File.Exists(Path.Combine(destinationInfo.FullName, file.Name)))
                            {
                                FileInfo fi = new FileInfo(Path.Combine(destinationInfo.FullName, file.Name));

                                if (fi.LastWriteTime.CompareTo(file.LastWriteTime) >= 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    file.CopyTo(Path.Combine(destinationInfo.FullName, file.Name), true);
                                }
                            }
                            else
                            {
                                file.CopyTo(Path.Combine(destinationInfo.FullName, file.Name), true);
                            }

                            break;
                    }
                }

                if (recursive)
                {
                    foreach (DirectoryInfo Directory in DirectoryList(sourceInfo.FullName))
                    {
                        CopyDirectory(Directory.FullName, Path.Combine(destinationInfo.FullName, Directory.Name), recursive, options);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets a list of files
        /// </summary>
        /// <param name="DirectoryPath">Directory to check for files</param>
        /// <returns>a list of files</returns>
        public static List<FileInfo> FileList(string DirectoryPath)
        {
            return FileList(DirectoryPath, false);
        }

        /// <summary>
        /// Directory listing
        /// </summary>
        /// <param name="DirectoryPath">Path to get the directories in</param>
        /// <returns>List of directories</returns>
        public static List<DirectoryInfo> DirectoryList(string DirectoryPath)
        {
            if (Exists(DirectoryPath) == false)
            {
                return (new List<DirectoryInfo>());
            }

            List<DirectoryInfo> Directories = new List<DirectoryInfo>();
            DirectoryInfo Directory = new DirectoryInfo(DirectoryPath);

            try
            {
                foreach (DirectoryInfo SubDirectory in Directory.GetDirectories())
                {
                    Directories.Add(SubDirectory);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Directories;
        }

        /// <summary>
        /// Gets a list of files
        /// </summary>
        /// <param name="DirectoryPath">Directory to check for files</param>
        /// <param name="Recursive">Determines if this is a recursive look at all directories under this one</param>
        /// <returns>a list of files</returns>
        public static List<FileInfo> FileList(string DirectoryPath, bool Recursive)
        {
            if (string.IsNullOrEmpty(DirectoryPath))
            {
                throw new ArgumentNullException("DirectoryPath");
            }

            if (Exists(DirectoryPath) == false)
            {
                return (new List<FileInfo>());
            }

            DirectoryInfo Directory = new DirectoryInfo(DirectoryPath);
            List<FileInfo> Files = new List<FileInfo>();
            Files.AddRange(Directory.GetFiles());

            if (Recursive == true)
            {
                foreach (DirectoryInfo SubDirectory in Directory.GetDirectories())
                {
                    Files.AddRange(FileList(SubDirectory.FullName, true));
                }
            }

            return Files;
        }

    }
}
