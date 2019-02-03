﻿using Sprint.Modules;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Sprint.Module.SmashImages
{
    [Module("Smash", ModuleType.AssetsFile) ]
    public class Smash : IModuleInfo
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title => "Smash Images";

        /// <summary>
        /// Executes the specified parameters.
        /// </summary>
        /// <param name="params">The parameters.</param>
        public void Execute(IModuleParameter @params)
        {
            FileInfo fileInfo = new FileInfo(@params.Filename);

            if (fileInfo.Extension == ".jpg" || 
                fileInfo.Extension == ".gif" || 
                fileInfo.Extension == ".png")
            {
                System.Console.Write("Smashing image " + fileInfo.FullName);

                Bitmap img = new Bitmap(fileInfo.FullName);
                img.SetResolution(150, 150);

                switch (fileInfo.Extension)
                {
                    case ".jpg":
                        img.Save(fileInfo.FullName, ImageFormat.Jpeg);
                        break;
                    case ".gif":
                        img.Save(fileInfo.FullName, ImageFormat.Gif);
                        break;
                    case ".png":
                        img.Save(fileInfo.FullName, ImageFormat.Png);
                        break;
                    default:
                        break;
                }
                
                System.Console.WriteLine("Done.");
            }                        
        }
    }
}
