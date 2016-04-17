using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_UserCancel_Button
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click( object sender, EventArgs e )
        {

        }

        private void Form1_Load( object sender, EventArgs e )
        {
            int[] source = Enumerable.Range( 1, 10000000 ).ToArray();
            CancellationTokenSource cts = new CancellationTokenSource();

            // Start a new asynchronous task that will cancel the 
            // operation from another thread. Typically you would call
            // Cancel() in response to a button click or some other
            // user interface event.
            Task.Factory.StartNew( () =>
            {
                UserClicksTheCancelButton( cts );
            } );

            int[] results = null;
            try
            {
                results = ( from num in source.AsParallel().WithCancellation( cts.Token )
                            where num % 3 == 0
                            orderby num descending
                            select num ).ToArray();

            }
            catch( OperationCanceledException ex )
            {
                MessageBox.Show( ex.Message );
            }
            catch( AggregateException ae )
            {
                if( ae.InnerExceptions != null )
                {
                    foreach( Exception ex in ae.InnerExceptions )
                        MessageBox.Show( ex.Message );
                }
            }
            finally
            {
                cts.Dispose();
            }

            if( results != null )
            {
                foreach( var v in results )
                    Debug.Write( "\t" + v.ToString() );
            }
        }
        static void UserClicksTheCancelButton( CancellationTokenSource cs )
        {
            // Wait between 150 and 500 ms, then cancel.
            // Adjust these values if necessary to make
            // cancellation fire while query is still executing.
            Random rand = new Random();
            Thread.Sleep( rand.Next( 150, 500 ) );
            cs.Cancel();
        }
    }
}
