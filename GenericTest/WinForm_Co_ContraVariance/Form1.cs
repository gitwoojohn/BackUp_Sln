using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm_Co_ContraVariance
{
    // 반공변성 예제
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // TextBox.KeyDown 이벤트가 덜 파생적으로 사용
            // TextBox.KeyDown 이벤트 사용하는 이벤트 처리기
            this.button1.KeyDown += this.MultiHandler;

            // MouseEventArgs 형식
            this.button1.MouseClick += this.MultiHandler;
        }
        /// <summary>
        /// Mouse로 버튼을 클릭하거나 키보드의 아무 글자나 입력하면 Label에 현재 시간이 표시됨.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiHandler( object sender, EventArgs e )
        {
            label1.Text = DateTime.Now.ToString();
        }
    }
}
