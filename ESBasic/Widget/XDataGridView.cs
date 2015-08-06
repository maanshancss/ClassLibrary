using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ESBasic.Widget
{
    public partial class XDataGridView : DataGridView
    {
        /// <summary>
        /// 当列表中的某个对象被双击时，触发该事件。参数为选中的条目所对应的对象。
        /// </summary>
        public event CbGeneric<object> ItemDoubleClicked;

        /// <summary>
        /// 在显示快捷菜单之前，触发此事件。参数为选中的条目所对应的对象。如果参数为null，则表示将出现的快捷菜单为 XcontextMenuStrip4Blank。
        /// </summary>
        public event CbGeneric<object> BeforeShowContextMenu;


        public XDataGridView()
            : base()
        {
            this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.BackgroundColor = System.Drawing.Color.White;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize; 
            this.MultiSelect = false;          
            this.ReadOnly = true;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.RowHeadersVisible = false;
            this.RowTemplate.Height = 23;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);

            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.ColumnHeadersHeight = 30;            
        }

        #region SelectedItem
        public object SelectedItem
        {
            get
            {
                IList list = (IList)this.DataSource;
                if (list == null || this.selectedRowIndex < 0)
                {
                    return null;
                }
                if (this.selectedRowIndex >= list.Count)
                {
                    return null;
                }

                return list[this.selectedRowIndex];
            }
        }
        #endregion       

        #region ContextMenuStrip
        private ContextMenuStrip xcontextMenuStrip;
        /// <summary>
        /// 当右键点击条目处时的快捷菜单。
        /// </summary>
        public  ContextMenuStrip XContextMenuStrip
        {
            get
            {
                return this.xcontextMenuStrip;
            }
            set
            {
                this.xcontextMenuStrip = value;
            }
        }
        #endregion

        #region XcontextMenuStrip4Blank
        private ContextMenuStrip xcontextMenuStrip4Blank;
        /// <summary>
        /// 当右键点击空白处时的快捷菜单。
        /// </summary>
        public ContextMenuStrip XcontextMenuStrip4Blank
        {
            get { return xcontextMenuStrip4Blank; }
            set { xcontextMenuStrip4Blank = value; }
        } 
        #endregion

        #region SelectedRowIndex
        private int selectedRowIndex = -1;
        public int SelectedRowIndex
        {
            get { return selectedRowIndex; }
            set 
            {
                IList list = (IList)this.DataSource;
                if (list == null)
                {
                    return;
                }
                if (value >= list.Count)
                {
                    value = list.Count - 1;
                }
                selectedRowIndex = value;
                if (this.selectedRowIndex >= 0)
                {
                    this.Rows[this.selectedRowIndex].Selected = true;
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.Rows[i].Selected = false;
                    }
                }
            }
        } 
        #endregion

        #region MouseDown
        
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo info = this.HitTest(e.X, e.Y);
            this.selectedRowIndex = info.RowIndex;
            if (this.selectedRowIndex >= 0)
            {
                this.Rows[this.selectedRowIndex].Selected = true;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (this.BeforeShowContextMenu != null)
                {
                    if (info.RowIndex >= 0)
                    {
                        IList list = (IList)this.DataSource;
                        this.BeforeShowContextMenu(list[this.selectedRowIndex]);
                    }
                    else
                    {
                        this.BeforeShowContextMenu(null);
                    }
                }

                this.ContextMenuStrip = info.RowIndex < 0 ? this.xcontextMenuStrip4Blank : this.xcontextMenuStrip;
            }
            
        }
        #endregion

        #region MouseDoubleClick
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            IList list = (IList)this.DataSource;
            if (list == null || e.RowIndex < 0)
            {
                return;
            }

            if (e.RowIndex >= list.Count)
            {
                return;
            }

            if (this.ItemDoubleClicked != null)
            {
                this.ItemDoubleClicked(list[e.RowIndex]);
            }
        }
        #endregion
    }
}
