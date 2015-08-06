using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESBasic.Helpers;

namespace ESBasic.Widget
{
    /// <summary>
    /// EnumComboBox 用于将Enum绑定到ComboBox
    /// </summary>
    public partial class EnumComboBox : UserControl
    {
        public event CbSimple SelectedIndexChanged;

        public EnumComboBox()
        {
            InitializeComponent();
            this.SelectedIndexChanged += delegate { };
        }

        #region UseEnumDescription
        private bool useEnumDescription = false;
        public bool UseEnumDescription
        {
            get { return useEnumDescription; }
            set { useEnumDescription = value; }
        } 
        #endregion

        #region DataSource
        private Type dataSource;
        /// <summary>
        /// DataSource 数据源必须是一个Enum类型
        /// </summary>
        public Type DataSource
        {
            get 
            { 
                return this.dataSource; 
            }
            set 
            {
                if (value == null)
                {
                    this.dataSource = null;
                    this.comboBox1.DataSource = null;
                    return;
                }

                if (!value.IsEnum)
                {
                    throw new Exception("DataSource Must be Enum Type !");
                }
               
                this.dataSource = value;
                this.comboBox1.DataSource = this.useEnumDescription ? (object)EnumHelper.ConvertEnumToFieldDescriptionList(this.dataSource) : Enum.GetValues(value); 
                this.comboBox1.SelectedIndex = 0;
            }
        } 
        #endregion

        #region SelectedValue
        /// <summary>
        /// SelectedValue 获取或设置选中的Enum枚举值
        /// </summary>
        public object SelectedValue
        {
            get
            {
                if (this.comboBox1.SelectedItem == null)
                {
                    return null;
                }

                return this.useEnumDescription ? EnumHelper.ParseEnumValue(this.dataSource, this.comboBox1.SelectedItem.ToString()) : Enum.Parse((Type)this.dataSource, this.comboBox1.SelectedItem.ToString());
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (value.GetType() != (Type)this.dataSource)
                {
                    return;
                }

                if (this.useEnumDescription)
                {
                    string desc = EnumDescription.GetFieldText(value);
                    if (desc == null)
                    {
                        this.comboBox1.Text = value.ToString();
                    }
                    else
                    {
                        this.comboBox1.Text = desc;
                    }
                }
                else
                {
                    this.comboBox1.Text = value.ToString();
                }
            }
        } 
        #endregion

        #region SelectedIndexChanged
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedIndexChanged();
        } 
        #endregion       
       
    }
}
