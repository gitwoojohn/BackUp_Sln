using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public delegate void ButtonClickedEventHandler (string s);
    public partial class FormTest : Form
    {
        public event ButtonClickedEventHandler ButtonClicked;
        public FormTest()
        {
            InitializeComponent();
        }
        private void btn1_Click( object sender, EventArgs e )
        {
            if (this.ButtonClicked != null)
                this.ButtonClicked( "1번 버튼 클릭" );
        }
        private void btn2_Click( object sender, EventArgs e )
        {
            if (this.ButtonClicked != null)
                this.ButtonClicked( "2번 버튼 클릭" ); 
        }
        private void btn3_Click( object sender, EventArgs e )
        {
            if (this.ButtonClicked != null)
                this.ButtonClicked( "3번 버튼 클릭" );
        }
    }
}