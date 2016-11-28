using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.DragDrop;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DropPhotoBooth
{
    public sealed partial class PhotoHolder : UserControl
    {
        #region Dependency Properties
        public ImageSource Picture
        {
            get { return ( ImageSource )GetValue( PictureProperty ); }
            set { SetValue( PictureProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for HasPicture.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PictureProperty =
            DependencyProperty.Register( "Picture", typeof( ImageSource ), typeof( PhotoHolder ), new PropertyMetadata( null ) );
        #endregion

        bool _acceptData = false;
        RandomAccessStreamReference _bitmapSource = null;
        StorageFile _fileSource = null;

        public PhotoHolder()
        {
            InitializeComponent();
        }

        private async void DropGrid_Drop( object sender, DragEventArgs e )
        {
            //if( !App.IsSource( e.DataView ) )
            //{
            bool forceMove = ( ( e.Modifiers & DragDropModifiers.Shift ) == DragDropModifiers.Shift );

            if( e.DataView.Contains( StandardDataFormats.Bitmap ) )
            {
                // We need to take a deferral as reading the data is asynchronous
                var def = e.GetDeferral();

                // Get the data
                _bitmapSource = await e.DataView.GetBitmapAsync();

                var imageStream = await _bitmapSource.OpenReadAsync();
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync( imageStream );

                // Display it
                Picture = bitmapImage;

                // Notify the source
                e.AcceptedOperation = forceMove ? DataPackageOperation.Move : DataPackageOperation.Copy;

                e.Handled = true;
                def.Complete();
                Debug.WriteLine( "PhotoHolder: accepted bitmap: " + e.AcceptedOperation );
            }
            else if( e.DataView.Contains( StandardDataFormats.StorageItems ) )
            {
                e.AcceptedOperation = DataPackageOperation.None;

                // We need to take a deferral as reading the data is asynchronous
                var def = e.GetDeferral();

                var items = await e.DataView.GetStorageItemsAsync();
                foreach( var item in items )
                {
                    // We might not be able to read all files
                    try
                    {
                        StorageFile file = item as StorageFile;
                        if( ( file != null ) && file.ContentType.StartsWith( "image/" ) )
                        {
                            // Get the data
                            _fileSource = file;
                            var stream = await file.OpenReadAsync();
                            var bitmapImage = new BitmapImage();
                            await bitmapImage.SetSourceAsync( stream );
                            // Display it
                            Picture = bitmapImage;
                            // Notify the source
                            e.AcceptedOperation = ( forceMove ? DataPackageOperation.Move : DataPackageOperation.Copy );
                            Debug.WriteLine( "PhotoHolder: accepted file: " + e.AcceptedOperation );
                            break;
                        }
                    }
                    catch( Exception ex )
                    {
                        Debug.WriteLine( ex.Message );
                    }
                }
                e.Handled = true;
                def.Complete();
                Debug.WriteLine( "Completing deferral" );
            }
            //}
            _acceptData = false;
        }

        private async void DropGrid_DragEnter( object sender, DragEventArgs e )
        {
            e.AcceptedOperation = DataPackageOperation.Copy;

            bool forceMove = ( ( e.Modifiers & DragDropModifiers.Shift ) == DragDropModifiers.Shift );
            if( e.DataView.Contains( StandardDataFormats.Bitmap ) )
            {
                _acceptData = true;
                e.AcceptedOperation = ( forceMove ? DataPackageOperation.Move : DataPackageOperation.Copy );
                e.DragUIOverride.Caption = "Drop the image to show it in this area";
                e.Handled = true;
            }
            else if( e.DataView.Contains( StandardDataFormats.StorageItems ) )
            {
                // Notify XAML that the end of DropGrid_Enter does
                // not mean that we have finished to handle the event
                var def = e.GetDeferral();
                _acceptData = false;
                e.AcceptedOperation = DataPackageOperation.None;
                var items = await e.DataView.GetStorageItemsAsync();
                foreach( var item in items )
                {
                    try
                    {
                        StorageFile file = item as StorageFile;
                        if( ( file != null ) && file.ContentType.StartsWith( "image/" ) )
                        {
                            _acceptData = true;
                            e.AcceptedOperation = ( forceMove ? DataPackageOperation.Move : DataPackageOperation.Copy );
                            e.DragUIOverride.Caption = "Drop the image to show it in this area";
                            break;
                        }
                    }
                    catch( Exception ex )
                    {
                        Debug.WriteLine( ex.Message );
                    }
                }
                e.Handled = true;

                // Notify XAML that now we are done
                def.Complete();
            }
            // Else we let the event bubble on a possible parent target
        }

        private void DropGrid_DragOver( object sender, DragEventArgs e )
        {
            Debug.WriteLine( "DropGrid_DragOver" );
            if( _acceptData )
            {
                bool forceMove = ( ( e.Modifiers & DragDropModifiers.Shift ) == DragDropModifiers.Shift );
                e.AcceptedOperation = ( forceMove ? DataPackageOperation.Move : DataPackageOperation.Copy );
                e.DragUIOverride.Caption = "Drop the image to show it in this area";
                e.Handled = true;
            }
        }

        private void DropGrid_DragStarting( UIElement sender, DragStartingEventArgs args )
        {
            if( Picture == null )
            {
                args.Cancel = true;
            }
            else if( _fileSource != null )
            {
                args.Data.SetStorageItems( new IStorageItem[] { _fileSource } );
                args.Data.RequestedOperation = DataPackageOperation.Copy | DataPackageOperation.Move;
                args.DragUI.SetContentFromDataPackage();
            }
            else if( _bitmapSource != null )
            {
                args.Data.SetBitmap( _bitmapSource );
                args.Data.RequestedOperation = DataPackageOperation.Copy | DataPackageOperation.Move;
                args.DragUI.SetContentFromDataPackage();
            }
        }

        private void DropGrid_DropCompleted( UIElement sender, DropCompletedEventArgs args )
        {
            if( args.DropResult == DataPackageOperation.Move )
            {
                // Move means that we should clear our own image
                Picture = null;
                _bitmapSource = null;
                _fileSource = null;
            }
        }
    }
}
