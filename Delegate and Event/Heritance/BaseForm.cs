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
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
          //  Load += new EventHandler( BaseForm_Load );
        }
        //private void BaseForm_Load( object sender, EventArgs e )
        //{
        //    MessageBox.Show( "BaseForm Loaded" );
        //}
        protected override void OnLoad( EventArgs e )
        {
           // base.OnLoad(e);
            MessageBox.Show( " BaseForm Loaded : " + e.ToString() );
        }
    }
}
