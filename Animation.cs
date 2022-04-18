using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Management_2._1
{
    public partial class Animation : Form
    {
        public Animation()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel2.Width += 3;

            if (panel2.Width >=599)
            {
                timer1.Stop();

                ClientManagementForm cFrm = new ClientManagementForm();
                
                this.Hide();
                cFrm.Show();              
            }
               
        }
    }
}
