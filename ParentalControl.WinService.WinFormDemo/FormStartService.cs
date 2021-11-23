using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParentalControl.WinService.WinFormDemo
{
    public partial class FormStartService : Form
    {
        public FormStartService()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            var engine = new WinServiceEngine();
            WinServiceEngine x = (WinServiceEngine)engine;
            x.Start();
        }
    }
}
