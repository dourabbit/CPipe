﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using System.Reflection;
using XNASysLib.XNAKernel;
using VertexPipeline;

namespace WinFormsContentLoading
{
    public class BottomPanal : System.Windows.Forms.TableLayoutPanel
    {
        SceneEntry _entry;
        Label _label;

       
        
        void Initialize()
        {
            this.Controls.Clear();
            
            this.AutoSize = true;
            this.ColumnCount = 1;
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Console";
            this.RowCount =1;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 100));
            this.Size = new System.Drawing.Size(350, 300);
            this.TabIndex = 0;

            this._label.Location = new System.Drawing.Point(0, 0);
            this._label.Name = "label1";
            this._label.Size = new System.Drawing.Size(2000, 500);
            this._label.TabIndex = 0;
            this._label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._label.AutoSize = true;

            this.Controls.Add(_label, 0, 0);
            this.InitLayout();

        }
        public BottomPanal(SceneEntry entry)
            : base()
        {
            _entry = entry;
            this._label = new Label();
            MyConsole.OnOutput += this.Console;
            this.Initialize();
        }
        void Console(string[] outputArray)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in outputArray)
            {
                builder.Append(value);
                builder.Append("\r\n");
            }
            _label.Text = builder.ToString();
            this.InitLayout();
        }
    }
      
}
