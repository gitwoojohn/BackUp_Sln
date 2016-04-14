 using System;
   2: using System.Collections.Generic;
   3: using System.Linq;
   4: using System.Text;
   5: using System.IO;
   6: using System.Diagnostics;
   7:
   8: namespace FileExplorer.Model
   9: {
  10:     /// <summary>
  11:     /// Class to get file system information
  12:     /// </summary>
  13:     public class FileSystemExplorerService
  14:     {
  15:         /// <summary>
  16:         /// Gets the list of files in the directory Name passed
  17:         /// </summary>
  18:         /// <param name="directory">The Directory to get the files from</param>
  19:         /// <returns>Returns the List of File info for this directory.
  20:         /// Return null if an exception is raised</returns>
  21:         public static IList<FileInfo> GetChildFiles(string directory)
  22:         {
  23:             try
  24:             {
  25:                 return (from x in Directory.GetFiles(directory)
  26:                         select new FileInfo(x)).ToList();
  27:             }
  28:             catch (Exception e){
  29:                 Trace.WriteLine(e.Message);
  30:             }
  31:
  32:             return new List<FileInfo>();
  33:         }
  34:
  35:
  36:         /// <summary>
  37:         /// Gets the list of directories 
  38:         /// </summary>
  39:         /// <param name="directory">The Directory to get the files from</param>
  40:         /// <returns>Returns the List of directories info for this directory.
  41:         /// Return null if an exception is raised</returns>
  42:         public static IList<DirectoryInfo> GetChildDirectories(string directory)
  43:         {
  44:             try
  45:             {
  46:                 return (from x in Directory.GetDirectories(directory)
  47:                         select new DirectoryInfo(x)).ToList();
  48:             }
  49:             catch (Exception e)
  50:             {
  51:                 Trace.WriteLine(e.Message);
  52:             }
  53:
  54:             return new List<DirectoryInfo>();
  55:         }
  56:
  57:         /// <summary>
  58:         /// Gets the root directories of the system
  59:         /// </summary>
  60:         /// <returns>Return the list of root directories</returns>
  61:         public static IList<DriveInfo> GetRootDirectories()
  62:         {
  63:             return (from x in DriveInfo.GetDrives() select x).ToList();
  64:         }
  65:     }