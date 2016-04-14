 using System;
   2: using System.Collections.Generic;
   3: using System.Linq;
   4: using System.Text;
   5: using System.Windows;
   6: using System.Windows.Controls;
   7: using System.Windows.Input;
   8: using FileExplorer.Model;
   9:
  10: namespace FileExplorer.ViewModel
  11: {
  12:     /// <summary>
  13:     /// View model for the right side pane
  14:     /// </summary>
  15:     public class DirectoryViewerViewModel : ViewModelBase
  16:     {
  17:         #region // Private variables
  18:         private ExplorerWindowViewModel _evm;
  19:         private DirInfo _currentItem;
  20:         #endregion
  21:
  22:         #region // .ctor
  23:         public DirectoryViewerViewModel(ExplorerWindowViewModel evm)
  24:         {
  25:             _evm = evm;
  26:         }
  27:         #endregion
  28:
  29:         #region // Public members
  30:         /// <summary>
  31:         /// Indicates the current directory in the Directory view pane
  32:         /// </summary>
  33:         public DirInfo CurrentItem
  34:         {
  35:             get { return _currentItem; }
  36:             set { _currentItem = value; }
  37:         }
  38:         #endregion
  39:
  40:         #region // Public Methods
  41:         /// <summary>
  42:         /// processes the current object. If this is a file then open it or if it is a directory then return its subdirectories
  43:         /// </summary>
  44:         public void OpenCurrentObject()
  45:         {
  46:             int objType = CurrentItem.DirType; //Dir/File type
  47:
  48:             if ((ObjectType)CurrentItem.DirType == ObjectType.File)
  49:             {
  50:                 System.Diagnostics.Process.Start(CurrentItem.Path);
  51:             }
  52:             else
  53:             {
  54:                 _evm.CurrentDirectory = CurrentItem;
  55:                 _evm.FileTreeVM.ExpandToCurrentNode(_evm.CurrentDirectory);
  56:             }
  57:         }
  58:         #endregion
  59:     }
  60: }