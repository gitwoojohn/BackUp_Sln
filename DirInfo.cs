using System;
   2: using System.Collections.Generic;
   3: using System.Linq;
   4: using System.Text;
   5: using System.Collections.ObjectModel;
   6: using System.IO;
   7: using FileExplorer.Properties;
   8: using System.Windows;
   9: using System.Collections;
  10:
  11:
  12: namespace FileExplorer.ViewModel
  13: {
  14:     /// <summary>
  15:     /// Enum to hold the Types of different file objects
  16:     /// </summary>
  17:     public enum ObjectType
  18:     {
  19:         MyComputer = 0,
  20:         DiskDrive = 1,
  21:         Directory = 2,
  22:         File = 3
  23:     }
  24:
  25:     /// <summary>
  26:     /// Class for containing the information about a Directory/File
  27:     /// </summary>
  28:     public class DirInfo : DependencyObject
  29:     {
  30:         #region // Public Properties
  31:         public string Name { get; set; }
  32:         public string Path { get; set; }
  33:         public string Root { get; set; }
  34:         public string Size { get; set; }
  35:         public string Ext { get; set; }
  36:         public int DirType { get; set; }
  37:         #endregion
  38:
  39:         #region // Dependency Properties
  40:         public static readonly DependencyProperty propertyChilds = DependencyProperty.Register("Childs", typeof(IList<DirInfo>), typeof(DirInfo));
  41:         public IList<DirInfo> SubDirectories
  42:         {
  43:             get { return (IList<DirInfo>)GetValue(propertyChilds); }
  44:             set { SetValue(propertyChilds, value); }
  45:         }
  46:
  47:         public static readonly DependencyProperty propertyIsExpanded = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(DirInfo));
  48:         public bool IsExpanded
  49:         {
  50:             get { return (bool)GetValue(propertyIsExpanded); }
  51:             set { SetValue(propertyIsExpanded, value); }
  52:         }
  53:
  54:         public static readonly DependencyProperty propertyIsSelected = DependencyProperty.Register("IsSelected", typeof(bool), typeof(DirInfo));
  55:         public bool IsSelected
  56:         {
  57:             get { return (bool)GetValue(propertyIsSelected); }
  58:             set { SetValue(propertyIsSelected, value); }
  59:         }
  60:         #endregion
  61:
  62:         #region // .ctor(s)
  63:         public DirInfo()
  64:         {
  65:             SubDirectories = new List<DirInfo>();
  66:             SubDirectories.Add(new DirInfo("TempDir"));
  67:         }
  68:
  69:         public DirInfo(string directoryName)
  70:         {
  71:             Name = directoryName;
  72:         }
  73:
  74:         public DirInfo(DirectoryInfo dir)
  75:             : this()
  76:         {
  77:             Name = dir.Name;
  78:             Root = dir.Root.Name;
  79:             Path = dir.FullName;
  80:             DirType = (int)ObjectType.Directory;
  81:         }
  82:
  83:         public DirInfo(FileInfo fileobj)
  84:         {
  85:             Name = fileobj.Name;
  86:             Path = fileobj.FullName;
  87:             DirType = (int)ObjectType.File;
  88:             Size = (fileobj.Length / 1024).ToString() + " KB";
  89:             Ext = fileobj.Extension + " File";
  90:         }
  91:
  92:         public DirInfo(DriveInfo driveobj)
  93:             : this()
  94:         {
  95:             if (driveobj.Name.EndsWith(@"\"))
  96:                 Name = driveobj.Name.Substring(0, driveobj.Name.Length - 1);
  97:             else
  98:                 Name = driveobj.Name;
  99:
 100:             Path = driveobj.Name;
 101:             DirType = (int)ObjectType.DiskDrive;
 102:         }
 103:         #endregion
 104:     }
 105: }