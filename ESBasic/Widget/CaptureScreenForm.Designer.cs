namespace ESBasic.Widget
{
    partial class CaptureScreenForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // CaptureScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 559);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CaptureScreenForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;           
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CaptureScreenForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CaptureScreenForm_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CaptureScreenForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CaptureScreenForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CaptureScreenForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
    }
}