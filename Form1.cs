using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mankala
{
    public partial class menuForm : Form
    {
        public menuForm()
        {
            InitializeComponent();
        }

        private void menuForm_Load(object sender, EventArgs e)
        {

        }

        private void button1vs1_Click(object sender, EventArgs e)
        {
            PvPForm pvpForm = new PvPForm();
            this.Hide();
            pvpForm.ShowDialog();
        }

        private void buttonAI_Click(object sender, EventArgs e)
        {
            AIForm aiForm = new AIForm();
            this.Hide();
            aiForm.ShowDialog();
        }
    }
}
