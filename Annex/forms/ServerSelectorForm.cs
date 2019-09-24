using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Annex.forms
{
    public partial class ServerSelectorForm : Form
    {
        public ServerSelectorForm()
        {
            InitializeComponent();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ServerSelectorForm_Load(object sender, EventArgs e)
        {
            serverDropdown.SelectedIndex = 0;
        }
    }
}
