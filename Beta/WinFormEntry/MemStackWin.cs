using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XNASysLib.XNAKernel;

namespace WinFormEntry
{
    public partial class MemStackWin : Form
    {
        private System.Windows.Forms.ListBox _listBox;
        public MemStackWin()
        {
            //InitializeComponent();

            // 
            // MemStack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
           this.Name = "MemStack";
            this.Text = "MemStack";
            
            
            this._listBox = new System.Windows.Forms.ListBox();
           
            // 
            // listBox1
            // 
            this._listBox.FormattingEnabled = true;
            this._listBox.Location = new System.Drawing.Point(0, 0);
            this._listBox.Name = "listBox1";
            this._listBox.Size = new System.Drawing.Size(287, 264);
            this.SizeChanged += delegate(object sender, EventArgs e)
            {
                this._listBox.Size = this.Size;
            };

            _listBox.Items.Add("MemStack");
            this._listBox.TabIndex = 0;
            this._listBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);


            this.Controls.Add(this._listBox);
            
            this.SuspendLayout();

            this.ResumeLayout(false);
         
        }

        public void UpdateList(int curIndex, object stack)
        {

            this._listBox.Items.Clear();
            foreach (HistoryEntry entry in TimeMechine.History)
                if (entry.Target != null&&entry.ToolNm!=null)
                    this._listBox.Items.Add("ToolNm:" + entry.ToolNm + "\t" +
                                            "ToolTarget:" + entry.Target.Name);
                else
                    this._listBox.Items.Add("SysInitial");
            this._listBox.SelectedIndex=curIndex;
           // this._listBox.Update();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
