 namespace FileExplorer.ViewModel
   2: {
   3:     public class ExplorerWindowViewModel : ViewModelBase
   4:     {
   5:         #region // Private Members
   6:         private DirInfo _currentDirectory;
   7:         private FileExplorerViewModel _fileTreeVM;
   8:         private DirectoryViewerViewModel _dirViewerVM;
   9:         private IList<DirInfo> _currentItems;
  10:         private bool _showDirectoryTree = true;
  11:         private ICommand _showTreeCommand;
  12:         #endregion
  13:
  14:         #region // .ctor
  15:         public ExplorerWindowViewModel()
  16:         {
  17:             FileTreeVM = new FileExplorerViewModel(this);
  18:             DirViewVM = new DirectoryViewerViewModel(this);
  19:             ShowTreeCommand = new RelayCommand(param => this.DirectoryTreeHideHandler());
  20:         }
  21:         #endregion
  22:
  23:         #region // Public Properties
  24:         /// <summary>
  25:         /// Name of the current directory user is in
  26:         /// </summary>
  27:         public DirInfo CurrentDirectory
  28:         {
  29:             get { return _currentDirectory; }
  30:             set
  31:             {
  32:                 _currentDirectory = value;
  33:                 RefreshCurrentItems();
  34:                 OnPropertyChanged("CurrentDirectory");
  35:             }
  36:         }
  37:
  38:         /// <summary>
  39:         /// Tree View model
  40:         /// </summary>
  41:         public FileExplorerViewModel FileTreeVM
  42:         {
  43:             get { return _fileTreeVM; }
  44:             set
  45:             {
  46:                 _fileTreeVM = value;
  47:                 OnPropertyChanged("FileTreeVM");
  48:             }
  49:         }
  50:
  51:
  52:         /// <summary>
  53:         /// Visibility of the 
  54:         /// </summary>
  55:         public bool ShowDirectoryTree
  56:         {
  57:             get { return _showDirectoryTree; }
  58:             set
  59:             {
  60:                 _showDirectoryTree = value;
  61:                 OnPropertyChanged("ShowDirectoryTree");
  62:             }
  63:         }
  64:
  65:
  66:         /// <summary>
  67:         /// 
  68:         /// </summary>
  69:         public ICommand ShowTreeCommand
  70:         {
  71:             get { return _showTreeCommand; }
  72:             set
  73:             {
  74:                 _showTreeCommand = value;
  75:                 OnPropertyChanged("ShowTreeCommand");
  76:             }
  77:         }
  78:
  79:         /// <summary>
  80:         /// Tree View model
  81:         /// </summary>
  82:         public DirectoryViewerViewModel DirViewVM
  83:         {
  84:             get { return _dirViewerVM; }
  85:             set
  86:             {
  87:                 _dirViewerVM = value;
  88:                 OnPropertyChanged("DirViewVM");
  89:             }
  90:         }
  91:
  92:         /// <summary>
  93:         /// Children of the current directory to show in the right pane
  94:         /// </summary>
  95:         public IList<DirInfo> CurrentItems
  96:         {
  97:             get
  98:             {
  99:                 if (_currentItems == null)
 100:                 {
 101:                     _currentItems = new List<DirInfo>();
 102:                 }
 103:                 return _currentItems;
 104:             }
 105:             set
 106:             {
 107:                 _currentItems = value;
 108:                 OnPropertyChanged("CurrentItems");
 109:             }
 110:         }
 111:         #endregion
 112:
 113:         #region // methods
 114:         private void DirectoryTreeHideHandler()
 115:         {
 116:             ShowDirectoryTree = false;
 117:         }
 118:
 119:         /// <summary>
 120:         /// this method gets the children of current directory and stores them in the CurrentItems Observable collection
 121:         /// </summary>
 122:         protected void RefreshCurrentItems()
 123:         {
 124:             IList<DirInfo> childDirList = new List<DirInfo>();
 125:             IList<DirInfo> childFileList = new List<DirInfo>();
 126:
 127:             //If current directory is "My computer" then get the all logical drives in the system
 128:             if (CurrentDirectory.Name.Equals(Resources.My_Computer_String))
 129:             {
 130:                 childDirList = (from rd in FileSystemExplorerService.GetRootDirectories()
 131:                                 select new DirInfo(rd)).ToList();
 132:             }
 133:             else
 134:             {
 135:                 //Combine all the subdirectories and files of the current directory
 136:                 childDirList = (from dir in FileSystemExplorerService.GetChildDirectories(CurrentDirectory.Path)
 137:                                 select new DirInfo(dir)).ToList();
 138:
 139:                 childFileList = (from fobj in FileSystemExplorerService.GetChildFiles(CurrentDirectory.Path)
 140:                                  select new DirInfo(fobj)).ToList();
 141:
 142:                 childDirList = childDirList.Concat(childFileList).ToList();
 143:             }
 144:
 145:             CurrentItems = childDirList;
 146:         }
 147:         #endregion
 148:     }
 149: }