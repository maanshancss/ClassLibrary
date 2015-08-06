using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ESBasic.Widget
{
    public partial class ColorChooser : UserControl
    {
        public event CbGeneric<Color> ColorSelected;
        public event CbGeneric<Color> CurrentColorClicked;

        public ColorChooser()
        {
            InitializeComponent();
            this.ColorSelected += delegate { };
            this.CurrentColorClicked += delegate { };
        }

        #region CurrentColor
        public Color CurrentColor
        {
            get
            {
                return this.button_currentColor.BackColor;
            }
            set
            {
                this.button_currentColor.BackColor = value;
            }
        } 
        #endregion        

        private void button_color_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = this.CurrentColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.button_currentColor.BackColor = this.colorDialog1.Color;

                this.ColorSelected(this.colorDialog1.Color);
            }
        }

        private void button_currentColor_Click(object sender, EventArgs e)
        {
            this.CurrentColorClicked(this.button_currentColor.BackColor);
        }


    }
}
