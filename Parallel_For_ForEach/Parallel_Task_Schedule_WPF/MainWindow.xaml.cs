using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Parallel_Task_Schedule_WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public static double SumRootN( int root )
        {
            double result = 0;
            for( int i = 1; i < 10000000; i++ )
            {
                result += Math.Exp( Math.Log( i ) / root );
            }
            return result;
        }
        private void button_Click( object sender, RoutedEventArgs e )
        {
            textBlock.Text = "";
            label.Content = "Milliseconds: ";

            var watch = Stopwatch.StartNew();
            List<Task> tasks = new List<Task>();
            for( int i = 2; i < 20; i++ )
            {
                int j = i;
                var t = Task.Factory.StartNew( () =>
                {
                    var result = SumRootN( j );
                    this.Dispatcher.BeginInvoke( new Action( () =>
                           textBlock.Text += "root " + j.ToString() + " " +
                                             result.ToString() +
                                             Environment.NewLine )
                    , null );
                } );
                tasks.Add( t );
            }
            Task.Factory.ContinueWhenAll( tasks.ToArray(),
                  result =>
                  {
                      var time = watch.ElapsedMilliseconds;
                      this.Dispatcher.BeginInvoke( new Action( () =>
                            label.Content += time.ToString() ) );
                  } );
        }
    }
}
