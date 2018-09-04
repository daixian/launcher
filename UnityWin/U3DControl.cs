using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnityWin
{
    public partial class U3DControl : UserControl
    {
        public U3DControl()
        {
            InitializeComponent();
        }

        public Panel u3dPanel { get { return panel1; } }
    }
}
