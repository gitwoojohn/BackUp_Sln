 public class GetFileSysemInformationConverter : IValueConverter
   2:    {
   3:        #region IValueConverter Members
   4:
   5:        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
   6:        {
   7:            try {
   8:                DirInfo nodeToExpand = value as DirInfo;
   9:                if (nodeToExpand == null)
  10:                    return null;
  11:
  12:                 //return the subdirectories of the Current Node
  13:                 if ((ObjectType)nodeToExpand.DirType == ObjectType.MyComputer)
  14:                 {
  15:                     return (from sd in FileSystemExplorerService.GetRootDirectories()
  16:                                     select new DirInfo(sd)).ToList();
  17:                 }
  18:                 else
  19:                 {
  20:                     return (from dirs in FileSystemExplorerService.GetChildDirectories(nodeToExpand.Path)
  21:                             select new DirInfo(dirs)).ToList();
  22:                 }
  23:
  24:            }
  25:            catch { return null; }
  26:        }
  27:
  28:        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
  29:        {
  30:            throw new NotImplementedException();
  31:        }
  32:
  33:        #endregion
  34:    }