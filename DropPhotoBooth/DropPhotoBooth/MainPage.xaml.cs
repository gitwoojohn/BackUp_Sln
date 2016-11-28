using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// 빈 페이지 항목 템플릿은 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 에 문서화되어 있습니다.

namespace DropPhotoBooth
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhotoStripGrid_DragOver( object sender, DragEventArgs e )
        {
            Debug.WriteLine( "PhotoStripGrid_DragOver" );
            //if( !App.IsSource( e.DataView ) )
            //{
            if( e.DataView.Contains( StandardDataFormats.Text ) )
            {
                e.DragUIOverride.Caption = "Drop here to add legend to photo montage";
                e.DragUIOverride.IsContentVisible = false;
                e.AcceptedOperation = DataPackageOperation.Copy;
            }
            else
            {
                e.DragUIOverride.Clear();
                e.AcceptedOperation = DataPackageOperation.None;
            }
            //}
        }

        private async void PhotoStripGrid_Drop( object sender, DragEventArgs e )
        {
            Debug.WriteLine( "PhotoStripGrid_Drop" );
            //if( !App.IsSource( e.DataView ) )
            //{
            bool hasText = e.DataView.Contains( StandardDataFormats.Text );
            if( hasText )
            {
                // We don't need to take a deferral here
                // as it will slower the feedback (keep the visual alive).
                // As we still have a reference on the DataPackage
                // we can get the DataAsynchronously anyway
                e.AcceptedOperation = DataPackageOperation.Copy;
                string text = await e.DataView.GetTextAsync();
                MessageTextBlock.Text = text;
                MessageTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                e.AcceptedOperation = DataPackageOperation.None;
            }
            //}
        }

        private async void PhotoStripGrid_DragStarting( UIElement sender, DragStartingEventArgs args )
        {
            if( ( Picture1.Picture == null ) || ( Picture2.Picture == null ) || ( Picture3.Picture == null ) || ( Picture4.Picture == null ) )
            {
                // Photo Montage is not ready
                args.Cancel = true;
            }
            else

            {
                args.Data.RequestedOperation = DataPackageOperation.Copy;
                args.Data.SetDataProvider( StandardDataFormats.Bitmap, ProvideContentAsBitmap );
                App.SetSource( args.Data );
                var deferral = args.GetDeferral();
                var rtb = new RenderTargetBitmap();
                const int width = 200;
                int height = ( int )( .5 + PhotoStripGrid.ActualHeight / PhotoStripGrid.ActualWidth * ( double )width );
                await rtb.RenderAsync( PhotoStripGrid, width, height );
                var buffer = await rtb.GetPixelsAsync();
                var bitmap = SoftwareBitmap.CreateCopyFromBuffer( buffer, BitmapPixelFormat.Bgra8, width, height, BitmapAlphaMode.Premultiplied );
                args.DragUI.SetContentFromSoftwareBitmap( bitmap );
                deferral.Complete();
            }
        }

        private async void ProvideContentAsBitmap( DataProviderRequest request )
        {
            if( request.FormatId == StandardDataFormats.Bitmap )
            {
                var deferral = request.GetDeferral();
                await Dispatcher.RunAsync( CoreDispatcherPriority.Normal, async () =>
                {
                    var rtb = new RenderTargetBitmap();
                    await rtb.RenderAsync( PhotoStripGrid );
                    var buffer = await rtb.GetPixelsAsync();
                    var memoryStream = new InMemoryRandomAccessStream();
                    var encoder = await BitmapEncoder.CreateAsync( BitmapEncoder.PngEncoderId, memoryStream );
                    var bytes = WindowsRuntimeBufferExtensions.ToArray( buffer );
                    var dpi = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().LogicalDpi;
                    encoder.SetPixelData( BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, ( uint )rtb.PixelWidth, ( uint )rtb.PixelHeight, dpi, dpi, bytes );
                    await encoder.FlushAsync();
                    request.SetData( RandomAccessStreamReference.CreateFromStream( memoryStream ) );
                    deferral.Complete();
                } );
            }
        }

    }
}
