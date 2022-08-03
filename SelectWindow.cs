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
    public partial class SelectWindow : Form
    {
        public SelectWindow()
        {
            InitializeComponent();
        }

        private void BtnExtract_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxName_Click(object sender, EventArgs e)
        {
            if (checkBoxName.Checked)
            {
                listBox1.Items.Add(checkBoxName.Text);
            }
            else
            {
                listBox1.Items.Remove(checkBoxName.Text);
            }

        }

        private void checkBoxPhone_Click(object sender, EventArgs e)
        {
            if (checkBoxPhone.Checked)
            {
                listBox1.Items.Add(checkBoxPhone.Text);
            }
            else
            {
                listBox1.Items.Remove(checkBoxPhone.Text);
            }
        }

        private void checkBoxModel_Click(object sender, EventArgs e)
        {
            if (checkBoxModel.Checked)
            {
                listBox1.Items.Add(checkBoxModel.Text);
            }
            else
            {
                listBox1.Items.Remove(checkBoxModel.Text);
            }
        }

        private void checkBoxVin_Click(object sender, EventArgs e)
        {
            if (checkBoxVin.Checked)
            {
                listBox1.Items.Add(checkBoxVin.Text);
            }
            else
            {
                listBox1.Items.Remove(checkBoxVin.Text);
            }
        }

        private void checkBoxEngine_Click(object sender, EventArgs e)
        {
            if (checkBoxEngine.Checked)
            {
                listBox1.Items.Add(checkBoxEngine.Text);
            }
            else
            {
                listBox1.Items.Remove(checkBoxEngine.Text);
            }
        }

        private void checkBoxPlate_Click(object sender, EventArgs e)
        {
            if (checkBoxPlate.Checked)
            {
                listBox1.Items.Add(checkBoxPlate.Text);
            }
            else
            {
                listBox1.Items.Remove(checkBoxPlate.Text);
            }
        }
    }
}
