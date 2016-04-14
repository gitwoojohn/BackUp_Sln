namespace FileExplorer.ViewModel
   2: {
   3:     public abstract class ViewModelBase : INotifyPropertyChanged
   4:     {
   5:         #region INotifyPropertyChanged Members
   6:
   7:         public event PropertyChangedEventHandler PropertyChanged;
   8:
   9:         /// <summary>
  10:         /// Raises the PropertyChanged event
  11:         /// </summary>
  12:         /// <param name="propertyName">The property name</param>
  13:         protected void OnPropertyChanged(string propertyName)
  14:         {
  15:             if (PropertyChanged != null)
  16:                 PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
  17:         }
  18:
  19:         #endregion
  20:
  21:         public ViewModelBase()
  22:         {
  23:         }
  24:     }
  25: }