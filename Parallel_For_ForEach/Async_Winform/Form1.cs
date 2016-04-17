using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Async_Winform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click( object sender, EventArgs e )
        {
            // ExampleMethodAsync returns a Task.
            await ExampleMethodAsync();
            textBox1.Text = "Delay Message";
        }

        private async Task ExampleMethodAsync()
        {
            // The following line simulates a task-returning asynchronous process.
            await Task.Delay( 1000 );
        }
    }
}
