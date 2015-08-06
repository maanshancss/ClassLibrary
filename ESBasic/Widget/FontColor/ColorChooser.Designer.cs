namespace ESBasic.Widget
{
    partial class ColorChooser
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorChooser));
            this.button_color = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button_currentColor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_color
            // 
            this.button_color.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_color.BackgroundImage")));
            this.button_color.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button_color.FlatAppearance.BorderSize = 0;
            this.button_color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_color.Location = new System.Drawing.Point(12, 1);
            this.button_color.Name = "button_color";
            this.button_color.Size = new System.Drawing.Size(22, 21);
            this.button_color.TabIndex = 1;
            this.button_color.UseVisualStyleBackColor = true;
            this.button_color.Click += new System.EventHandler(this.button_color_Click);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            // 
            // button_currentColor
            // 
            this.button_currentColor.BackColor = System.Drawing.Color.Black;
            this.button_currentColor.FlatAppearance.BorderSize = 0;
            this.button_currentColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_currentColor.Location = new System.Drawing.Point(0, 4);
            this.button_currentColor.Name = "button_currentColor";
            this.button_currentColor.Size = new System.Drawing.Size(13, 13);
            this.button_currentColor.TabIndex = 2;
            this.button_currentColor.UseVisualStyleBackColor = false;
            this.button_currentColor.Click += new System.EventHandler(this.button_currentColor_Click);
            // 
            // ColorChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.button_currentColor);
            this.Controls.Add(this.button_color);
            this.Name = "ColorChooser";
            this.Size = new System.Drawing.Size(32, 20);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_color;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button_currentColor;
    }
}
