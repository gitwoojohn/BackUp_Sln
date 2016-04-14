namespace FileExplorer.ViewModel
   3: {
   4:     public class FileExplorerViewModel : ViewModelBase
   5:     {
   6:         #region // Private fields
   7:         private ExplorerWindowViewModel _evm;
   8:         private DirInfo _currentTreeItem;
   9:         private IList<DirInfo> _sysDirSource;
  10:         #endregion
  11:
  12:         #region // Public properties
  13:         /// <summary>
  14:         /// list of the directories 
  15:         /// </summary>
  16:         public IList<DirInfo> SystemDirectorySource
  17:         {
  18:             get { return _sysDirSource; }
  19:             set
  20:             {
  21:                 _sysDirSource = value;
  22:                 OnPropertyChanged("SystemDirectorySource");
  23:             }
  24:         }
  25:
  26:         /// <summary>
  27:         /// Current selected item in the tree
  28:         /// </summary>
  29:         public DirInfo CurrentTreeItem
  30:         {
  31:             get { return _currentTreeItem; }
  32:             set
  33:             {
  34:                 _currentTreeItem = value;
  35:                 _evm.CurrentDirectory = _currentTreeItem;
  36:             }
  37:         }
  38:         #endregion
  39:
  40:         #region // .ctor
  41:         /// <summary>
  42:         /// ctor
  43:         /// </summary>
  44:         /// <param name="evm"></param>
  45:         public FileExplorerViewModel(ExplorerWindowViewModel evm)
  46:         {
  47:             _evm = evm;
  48:
  49:             //create a node for "my computer"
  50:             // this will be the root for the file system tree
  51:             DirInfo rootNode = new DirInfo(Resources.My_Computer_String);
  52:             rootNode.Path = Resources.My_Computer_String;
  53:             _evm.CurrentDirectory = rootNode; //make root node as the current directory
  54:
  55:             SystemDirectorySource = new List<DirInfo> { rootNode };
  56:         }
  57:         #endregion
  58:
  59:         #region // public methods
  60:         /// <summary>
  61:         /// 
  62:         /// </summary>
  63:         /// <param name="curDir"></param>
  64:         public void ExpandToCurrentNode(DirInfo curDir)
  65:         {
  66:             //expand the current selected node in tree 
  67:             //if this is an ancestor of the directory we want to navigate or "My Computer" current node 
  68:             if (CurrentTreeItem != null && (curDir.Path.Contains(CurrentTreeItem.Path) || CurrentTreeItem.Path == Resources.My_Computer_String))
  69:             {
  70:                 // expand the current node
  71:                 // If the current node is already expanded then first collapse it n then expand it
  72:                 CurrentTreeItem.IsExpanded = false;
  73:                 CurrentTreeItem.IsExpanded = true;
  74:             }
  75:         }
  76:         #endregion
  77:     }
  78: }