using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using XNASysLib.XNAKernel;
using XNASysLib.Primitives3D;
using VertexPipeline;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WinFormsContentLoading
{
    public delegate void UpdatePropertyPanal(ObjData obj);
    public class PropertyPanal : System.Windows.Forms.TableLayoutPanel
    {
        SceneEntry _entry;
        private System.Windows.Forms.TableLayoutPanel _table;
        List<ISelectable> _selsList;

        public static UpdatePropertyPanal UpdateHandler;



        public void Initialize()
        {
            _table = new System.Windows.Forms.TableLayoutPanel();
            _table.AutoSize = true;
            _table.AutoScroll = true;
            _table.SuspendLayout();
            
            this.Controls.Clear();

        }

        void Update(ObjData obj)
        {

            if (_table == null)
                this.Initialize();


            for (int i = 0; i < _table.Controls.Count; i++)
            {
                if (!(_table.Controls[i] is TextBox))
                    continue;

                TextBox textBox = (TextBox)_table.Controls[i];
                Binding b = textBox.DataBindings[0];
                b.ReadValue();

                //label.Text = property.Name;
                //label.Text = LanguagePack.GetNm(obj.GetType(), row);
                //if(label.Text=="")
                //    label.Text = LanguagePack.GetNm(obj.GetType(), row);

                //textBox.Location = new System.Drawing.Point(0, 0);
                //textBox.Name = "textBox_" + property.Name;
                //textBox.Size = new System.Drawing.Size(50, 30);

                //Binding b = new Binding("Text", obj, property.Name);
                //textBox.DataBindings.Add(b);

                //_table.Controls.Add(label, 0, row);
                //_table.Controls.Add(textBox, 1, row);
            }

            // PropertyInfo[] properties = obj.GetType().GetProperties();
            //List<PropertyInfo> showList= new List<PropertyInfo>();

            //foreach (PropertyInfo property in properties)
            //{

            //    System.Attribute[] attrs =
            //        System.Attribute.GetCustomAttributes(property);

            //    bool isShowable = false;
            //    foreach (Attribute attr in attrs)
            //        isShowable |= (attr is MyShowProperty) ?
            //                true : false;


            //    if (!isShowable)
            //        continue;


            //    if
            //    (showList.Exists(
            //        delegate(PropertyInfo matcher)
            //        {

            //            return matcher.Name == property.Name ? true : false;

            //        })
            //    )
            //        continue;


            //    showList.Add(property);

            //}
        
        }

        public PropertyPanal(SceneEntry entry)
            : base()
        {
            _entry = entry;
            //_entry.Scene.SelectorHandler += Show;

            SelectFunction.OnSelectionUpdate += OnSelectionUpdate;
            _selsList = new List<ISelectable>();
            this.Name = "PropertyPanal";
            UpdateHandler += Update;
            
        }
        void OnSelectionUpdate()
        {
            ISelectable[] sels=SelectFunction.Selection.ToArray();
            //if (sels.Length == 0)
            //{
                _selsList.RemoveAll(
                    delegate(ISelectable sel) 
                    {
                        return !sel.KeepSel; 
                    });
                this.Initialize();
            //}

            //Convert sel to root node, if it was hierachy node
            foreach (ISelectable sel in sels)
            {
                SceneNodHierachyModel node = sel as SceneNodHierachyModel;

                if (node != null)
                {
                    ISelectable rootSel = node as ISelectable;
                    if (rootSel == null)
                        throw new ArgumentNullException();
                    if (rootSel.KeepSel&&
                        !_selsList.Contains(sel))
                        _selsList.Add(rootSel);
                }
            }

            if (_selsList.Count == 0)
                Show(sels);
            else
                Show(_selsList.ToArray());
        }
        protected override void OnPaint(PaintEventArgs e)
        {


            try
            {
                base.OnPaint(e);
            }
            catch (Exception fe)
            { 
            
            }
        }


        void Show(ISelectable obj)
        {
            this.Initialize();

            PropertyInfo[] properties = obj.GetType().GetProperties();
            //row = showList.Count - 1;
           List<PropertyInfo>showList=
            ShowProperties( obj, properties);

            _table.ColumnCount = 2;
            _table.RowCount = showList.Count;
            _table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle
                (System.Windows.Forms.SizeType.Percent, 50F));
            _table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle
                (System.Windows.Forms.SizeType.Percent, 50F));

            this.Controls.Add(_table);
            this._table.ResumeLayout(false);
            this._table.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();


        }

        void Show(params ISelectable[] objs)
        {
            
            if (objs.Length == 0)
                return;

            //Only show the last obj
            ISelectable obj = objs[objs.Length - 1];
            Show(obj);
            //List<PropertyInfo> showList = new List<PropertyInfo>();
            //PropertyInfo[] properties = obj.GetType().BaseType.GetProperties();

         
            
            //int row = -1;
            //ShowProperties(row, obj, properties, showList);

            //properties = obj.GetType().GetProperties();
            //row = showList.Count-1;
            //ShowProperties(row, obj, properties, showList);



        }
        static char[] hexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
        /// <summary>
        /// Convert a .NET Color to a hex string.
        /// </summary>
        /// <returns>ex: "FFFFFF", "AB12E9"</returns>
        public static string ColorToHexString(Color color)
        {
            byte[] bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }

            string extension = "(" +color.R.ToString()+","+color.G.ToString()+","+color.B.ToString()+ ")";
            return new string(chars)+extension;
        }
        /// <summary>
        /// Extract only the hex digits from a string.
        /// </summary>
        public static string ExtractHexDigits(string input)
        {
            // remove any characters that are not digits (like #)
            Regex isHexDigit
               = new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
            string newnum = "";
            foreach (char c in input)
            {
                if (isHexDigit.IsMatch(c.ToString()))
                    newnum += c.ToString();
            }
            return newnum;
        }
        /// <summary>
        /// Convert a hex string to a .NET Color object.
        /// </summary>
        /// <param name="hexColor">a hex string: "FFFFFF", "#000000"</param>
        public static Color HexStringToColor(string hexColor)
        {
            string hc = ExtractHexDigits(hexColor);
            if (hc.Length != 6)
            {
                // you can choose whether to throw an exception
                //throw new ArgumentException("hexColor is not exactly 6 digits.");
                return Color.Empty;
            }
            string r = hc.Substring(0, 2);
            string g = hc.Substring(2, 2);
            string b = hc.Substring(4, 2);
            Color color = Color.Empty;
            try
            {
                int ri
                   = Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
                int gi
                   = Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
                int bi
                   = Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
                color = Color.FromArgb(ri, gi, bi);
            }
            catch
            {
                // you can choose whether to throw an exception
                //throw new ArgumentException("Conversion failed.");
                return Color.Empty;
            }
            return color;
        }
        private void TextBoxClick(object sender, EventArgs e)
        {

            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.


            Color col = new Color();
            TextBox textBox = (TextBox)sender;
            col = HexStringToColor(textBox.Text);
            MyDialog.Color = col;

            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = ColorToHexString(MyDialog.Color);

                textBox.ForeColor = MyDialog.Color;
            }

        }

        List<PropertyInfo> ShowProperties(ISelectable obj, PropertyInfo[] properties)
        {
            List<PropertyInfo> showList = new List<PropertyInfo>();
            foreach (PropertyInfo property in properties)
            {

                System.Attribute[] attrs =
                    System.Attribute.GetCustomAttributes(property);

                bool isShowable = false;
                foreach (Attribute attr in attrs)
                    isShowable |= (attr is MyShowProperty) ?
                            true : false;


                if (!isShowable)
                    continue;


                if//Escape when existing same property
                (showList.Exists(
                    delegate(PropertyInfo matcher)
                    {

                        return matcher.Name == property.Name ? true : false;

                    })
                )
                    continue;
                showList.Add(property);
            }

            for (int row = 0; row<showList.Count;row++)
            {
                Label label = new Label();
                TextBox textBox = new TextBox();


                label.AutoSize = true;
                label.Location = new System.Drawing.Point(0, 0);
                label.Size = new System.Drawing.Size(139, 20);
                label.Location = new System.Drawing.Point(0, 0);
                label.TabIndex = 1;

                //label.Text = property.Name;
                label.Text = LanguagePack.GetNm(obj.GetType(), row);

                textBox.Location = new System.Drawing.Point(0, 0);
                textBox.Name = "textBox_" + showList[row].Name;
                textBox.Size = new System.Drawing.Size(90, 30);
                if (showList[row].Name == "ObjColor")
                {

                    textBox.Click += this.TextBoxClick;
                }
                Binding b = new Binding("Text", obj, showList[row].Name, true, DataSourceUpdateMode.OnPropertyChanged);
                textBox.DataBindings.Add(b);
                

                

                _table.Controls.Add(label, 0, row);
                _table.Controls.Add(textBox, 1, row);

            }// end of foreach property

            return showList;
        }
    }
}
