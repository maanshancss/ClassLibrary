namespace ESBasic.Widget
{
    partial class RichTimerConfigure
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_timerType = new System.Windows.Forms.Label();
            this.numericUpDown_hours = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_mins = new System.Windows.Forms.NumericUpDown();
            this.label_time = new System.Windows.Forms.Label();
            this.numericUpDown_spanmins = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_day = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown_spanhour = new System.Windows.Forms.NumericUpDown();
            this.label_span = new System.Windows.Forms.Label();
            this.comboBox_week = new System.Windows.Forms.ComboBox();
            this.enumComboBox_timerType = new ESBasic.Widget.EnumComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_hours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_mins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_spanmins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_day)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_spanhour)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(33, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 21);
            this.label1.TabIndex = 10;
            this.label1.Text = "小时";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(175, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 21);
            this.label2.TabIndex = 11;
            this.label2.Text = "分钟";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(175, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 21);
            this.label3.TabIndex = 12;
            this.label3.Text = "星期";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_timerType
            // 
            this.label_timerType.Location = new System.Drawing.Point(3, 8);
            this.label_timerType.Name = "label_timerType";
            this.label_timerType.Size = new System.Drawing.Size(70, 20);
            this.label_timerType.TabIndex = 14;
            this.label_timerType.Text = "活动频率：";
            this.label_timerType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown_hours
            // 
            this.numericUpDown_hours.Location = new System.Drawing.Point(79, 54);
            this.numericUpDown_hours.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericUpDown_hours.Name = "numericUpDown_hours";
            this.numericUpDown_hours.Size = new System.Drawing.Size(99, 21);
            this.numericUpDown_hours.TabIndex = 17;
            // 
            // numericUpDown_mins
            // 
            this.numericUpDown_mins.Location = new System.Drawing.Point(220, 54);
            this.numericUpDown_mins.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown_mins.Name = "numericUpDown_mins";
            this.numericUpDown_mins.Size = new System.Drawing.Size(99, 21);
            this.numericUpDown_mins.TabIndex = 18;
            // 
            // label_time
            // 
            this.label_time.Location = new System.Drawing.Point(3, 32);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(70, 20);
            this.label_time.TabIndex = 19;
            this.label_time.Text = "活动时刻：";
            this.label_time.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown_spanmins
            // 
            this.numericUpDown_spanmins.Location = new System.Drawing.Point(220, 132);
            this.numericUpDown_spanmins.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown_spanmins.Name = "numericUpDown_spanmins";
            this.numericUpDown_spanmins.Size = new System.Drawing.Size(99, 21);
            this.numericUpDown_spanmins.TabIndex = 21;
            // 
            // numericUpDown_day
            // 
            this.numericUpDown_day.Location = new System.Drawing.Point(79, 81);
            this.numericUpDown_day.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDown_day.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_day.Name = "numericUpDown_day";
            this.numericUpDown_day.Size = new System.Drawing.Size(99, 21);
            this.numericUpDown_day.TabIndex = 23;
            this.numericUpDown_day.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(34, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 21);
            this.label8.TabIndex = 22;
            this.label8.Text = "日期";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(34, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 21);
            this.label9.TabIndex = 24;
            this.label9.Text = "小时";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(175, 132);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 21);
            this.label10.TabIndex = 25;
            this.label10.Text = "分钟";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown_spanhour
            // 
            this.numericUpDown_spanhour.Location = new System.Drawing.Point(79, 132);
            this.numericUpDown_spanhour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericUpDown_spanhour.Name = "numericUpDown_spanhour";
            this.numericUpDown_spanhour.Size = new System.Drawing.Size(99, 21);
            this.numericUpDown_spanhour.TabIndex = 27;
            // 
            // label_span
            // 
            this.label_span.Location = new System.Drawing.Point(2, 106);
            this.label_span.Name = "label_span";
            this.label_span.Size = new System.Drawing.Size(70, 20);
            this.label_span.TabIndex = 29;
            this.label_span.Text = "活动周期：";
            this.label_span.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_week
            // 
            this.comboBox_week.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_week.FormattingEnabled = true;
            this.comboBox_week.Items.AddRange(new object[] {
            "天",
            "一",
            "二",
            "三",
            "四",
            "五",
            "六"});
            this.comboBox_week.Location = new System.Drawing.Point(220, 82);
            this.comboBox_week.Name = "comboBox_week";
            this.comboBox_week.Size = new System.Drawing.Size(99, 20);
            this.comboBox_week.TabIndex = 30;
            // 
            // enumComboBox_timerType
            // 
            this.enumComboBox_timerType.DataSource = null;
            this.enumComboBox_timerType.Location = new System.Drawing.Point(79, 9);
            this.enumComboBox_timerType.Name = "enumComboBox_timerType";
            this.enumComboBox_timerType.SelectedValue = null;
            this.enumComboBox_timerType.Size = new System.Drawing.Size(99, 19);
            this.enumComboBox_timerType.TabIndex = 31;
            this.enumComboBox_timerType.SelectedIndexChanged += new ESBasic.CbSimple(this.enumComboBox_timerType_SelectedIndexChanged);
            // 
            // TimerConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.enumComboBox_timerType);
            this.Controls.Add(this.comboBox_week);
            this.Controls.Add(this.label_span);
            this.Controls.Add(this.numericUpDown_spanhour);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numericUpDown_day);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numericUpDown_spanmins);
            this.Controls.Add(this.label_time);
            this.Controls.Add(this.numericUpDown_mins);
            this.Controls.Add(this.numericUpDown_hours);
            this.Controls.Add(this.label_timerType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TimerConfigure";
            this.Size = new System.Drawing.Size(327, 161);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_hours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_mins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_spanmins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_day)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_spanhour)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_timerType;
        private System.Windows.Forms.NumericUpDown numericUpDown_hours;
        private System.Windows.Forms.NumericUpDown numericUpDown_mins;
        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.NumericUpDown numericUpDown_spanmins;
        private System.Windows.Forms.NumericUpDown numericUpDown_day;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDown_spanhour;
        private System.Windows.Forms.Label label_span;
        private System.Windows.Forms.ComboBox comboBox_week;
        private ESBasic.Widget.EnumComboBox enumComboBox_timerType;
    }
}
