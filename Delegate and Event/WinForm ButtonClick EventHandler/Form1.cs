using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        FormTest m_FormTest = new FormTest();
        public Form1()
        {
            InitializeComponent();
            this.Cursor = new Cursor( GetType(), "arrow_rl.cur" );
            m_FormTest.ButtonClicked += new ButtonClickedEventHandler( Form1_DisplayLabel );
            m_FormTest.Show();
        }
        void Form1_DisplayLabel(string s)
        {
            lblStatus.Text = s;
        }
    }
}