using System;
using System.IO;

namespace Sprint.IO
{
    public class Files
    {
        /// <summary>
        /// Make a file writteble
        /// </summary>
        /// <param name="path">File name to change</param>
        public static void MakeWritable(string path)
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
            }
        }

        /// <summary>
        /// Copy a file aways setting a file writable
        /// </summary>
        /// <param name="source">Source File name</param>
        /// <param name="target">Target File name</param>
        public static void CopyAlways(string source, string target)
        {
            if (!File.Exists(source))
            {
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(target));
            MakeWritable(target);

            if (File.Exists(source))
            {
                File.Copy(source, target, true);
            }
        }

        /// <summary>
        /// Gets the temp file path with extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public static string GetTempFilePathWithExtension(string extension)
        {
            if (extension.Substring(0, 1) != ".")
            {
                extension = "." + extension;
            }

            return Path.Combine(Path.GetTempPath(), (Guid.NewGuid() + extension));
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            return Files.GetExtension(path, true);
        }

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="returnInLowerCase">if set to <c>true</c> [return in lower case].</param>
        /// <returns></returns>
        public static string GetExtension(string path, bool returnInLowerCase)
        {
            if (returnInLowerCase == false)
            {
                return Path.GetExtension(path);
            }
            else
            {
                return Path.GetExtension(path).ToLower();
            }
        }

        /// <summary>
        /// Gets the file info.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string file)
        {
            return new FileInfo(file);
        }

        /// <summary>
        /// Opens the file stream reader.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static FileStream OpenFileStreamReader(string file)
        {
            return new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Opens the file stream writer.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static FileStream OpenFileStreamWriter(string file)
        {
            return new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read);
        }

        /// <summary>
        /// Reads all text.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static string ReadAllText(string file)
        {
            return Files.ReadAllText(file, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Reads all text.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string ReadAllText(string file, System.Text.Encoding encoding)
        {
            string text1;

            using (StreamReader reader1 = new StreamReader(file, encoding))
            {
                text1 = reader1.ReadToEnd();
            }

            return text1;
        }

        /// <summary>
        /// Writes all text.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="text">The text.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        public static void WriteAllText(string file, string text, bool append)
        {
            Files.WriteAllText(file, text, append, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Writes all text.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="text">The text.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        /// <param name="encoding">The encoding.</param>
        public static void WriteAllText(string file, string text, bool append, System.Text.Encoding encoding)
        {
            if (append && File.Exists(file))
            {
                using (StreamReader reader1 = new StreamReader(file, encoding, true))
                {
                    char[] chArray1 = new char[10];
                    reader1.Read(chArray1, 0, 10);
                    encoding = reader1.CurrentEncoding;
                }
            }

            using (StreamWriter writer1 = new StreamWriter(file, append, encoding))
            {
                writer1.Write(text);
            }
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void Delete(string fileName)
        {
            try
            {
                if (File.Exists(fileName)) File.Delete(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
