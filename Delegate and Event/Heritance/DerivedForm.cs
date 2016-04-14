using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Heritance
{
    public partial class DerivedForm : BaseForm
    {
        public DerivedForm()
        {
            InitializeComponent();
           // Load += new EventHandler( DerivedForm_Load );
        }
        //private void DerivedForm_Load(object sender, EventArgs e)
        //{
        //    MessageBox.Show( "DerivedForm Loaded" );
        //}
        protected override void OnLoad( EventArgs e )
        {
            MessageBox.Show( "DerivedForm Loaded" );
            base.OnLoad( e );
        }
    }
}
