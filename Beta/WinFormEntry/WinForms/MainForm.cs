#region File Description
//-----------------------------------------------------------------------------
// MainForm.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion
#define _Debug
#region Using Statements
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.Xna.Framework.Graphics;
using XNASysLib.XNAKernel;
using Microsoft.Xna.Framework.Input;
using XNASysLib.XNATools;
using Microsoft.Xna.Framework;
using XNASysLib.Primitives3D;
using VertexPipeline;
using WinFormEntry;
#endregion


namespace WinFormsContentLoading
{
    /// <summary>
    /// Custom form provides the main user interface for the program.
    /// In this sample we used the designer to fill the entire form with a
    /// ModelViewerControl, except for the menu bar which provides the
    /// "File / Open..." option.
    /// </summary>
    public partial class MainForm : Form
    {
        //ContentBuilder contentBuilder;
        //MyContentManager contentManager;
        BottomPanal _bottomPanal;
        ViewPanal _viewPanal;
        OutlinePanel _outlinePanal;

#if _Debug

        MemStackWin _mem;
#endif
        
        /// <summary>
        /// Constructs the main form.
        /// </summary>
        public MainForm()

        {
             
            Initialize();
            
            InitializeComponent();

            Mouse.WindowHandle =
                this.Cursor.Handle;

            new Initializer();
            /*contentBuilder = new ContentBuilder();

            contentManager = new MyContentManager(_entry.Services,
                                                contentBuilder.OutputDirectory);

            */
            this.openToolStripMenuItem.Click += OpenMenuClicked;
          
            this.splitContainer2.Panel2.Controls.Add(_bottomPanal);
            this.ViewParentPanalContainer.Panel2.Controls.Add(this._propertyPanal);
            
            //this.splitContainer4.Panel1.Controls.Add(this._entry);
            this.ViewParentPanalContainer.Panel1.Controls.Add(this._viewPanal);
            this.splitContainer3.Panel1.Controls.Add(this._outlinePanal);


            
            
            System.Drawing.Point dd = this.PointToScreen(new System.Drawing.Point(0,0));

            System.Drawing.Point ss =
            this.splitContainer3.Panel2.PointToScreen(new System.Drawing.Point(0, 0));

            System.Drawing.Point ssd=
            this._viewPanal.Parent.PointToScreen(new System.Drawing.Point(0, 0));


           // System.Drawing.Point aa =
           // viewPanel1.PointToScreen(new  System.Drawing.Point(0,0));


            this.ViewParentPanalContainer.Panel1.MouseEnter += 
                delegate(object sender, EventArgs e)
            {
               // MyConsole.WriteLine(this.Name+ e.ToString());
            };



            Control result = null;

            GetChildByNm("View1", this._viewPanal.Controls[0], ref result);
            SceneEntry entry1 = (SceneEntry)result;

            entry1.MouseMove += this.MouseMoveRegister;


            //SceneEntry.Scene.UpdateScene += delegate()
            //{

            //    //this._mem.UpdateList();

            //};

            TimeMechine.History.Push(new HistoryEntry(TimeMechine.Time, "FirstEntry", null, null));
            TimeMechine.History.MemChanging += this._mem.UpdateList;
        }

        void MouseMoveRegister(object sender, MouseEventArgs e)
        {
            SceneEntry entry = (SceneEntry)sender;

            SceneEntry.Scene.ActiveViewRect = new Rectangle();
            System.Drawing.Point p =
               entry.PointToClient(new System.Drawing.Point(0, 0));
            
            foreach (SceneEntry se in SceneEntry.SceneEntries)
            {
               se.IsActivate = (se.Name == entry.Name) ? true : false;
                if (se.IsActivate)
                {
                    SceneEntry.Scene.ActiveViewRect = 
                        new Rectangle(
                            p.X,p.Y,entry.Width,entry.Height
                            );
                    SceneEntry.Scene.ActiveViewport =
                        new Viewport
                        (0,0,entry.Width,entry.Height);
                }
            }
        
        }
       

      void GetChildByNm(string Nm, Control curControl, ref Control result)
      {
          if (curControl.Name != Nm && result == null)
              foreach (Control c in curControl.Controls)
                  GetChildByNm(Nm, c, ref result);
          else if (curControl.Name == Nm)
          {
              result = curControl;
          }
          else
          {
              return;
          }

      }


        void Initialize()
        {
#if _Debug
            _mem = new MemStackWin();
            _mem.Show();
#endif
            // 
            // _entry
            // 
            _bottomPanal = new BottomPanal(_entry);
            _propertyPanal = new PropertyPanal(_entry);
            _viewPanal = new ViewPanal(_entry);
            _outlinePanal = new OutlinePanel(_entry);

            //
            //_outlinePanal
            //
            this._propertyPanal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._outlinePanal.AutoSize = true;
            this._outlinePanal.Location = new System.Drawing.Point(0,0);
            this._outlinePanal.Name = "_outlinePanal";

            // 
            // _propertyPanal
            // 
            this._propertyPanal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._propertyPanal.Location = new System.Drawing.Point(0, 0);
            this._propertyPanal.Name = "_propertyPanal";
            this._propertyPanal.Size = new System.Drawing.Size(400, 100);
            this._propertyPanal.TabIndex = 0;
            this._propertyPanal.AutoSize = true;
            
            //
            //ViewPanal
            //
            this._viewPanal.ColumnStyles.
                Add(new System.Windows.Forms.ColumnStyle());
            this._viewPanal.Location = new System.Drawing.Point(0, 0);
            this._viewPanal.Name = "ViewPanal";
            this._viewPanal.Size = new System.Drawing.Size(400, 400);
            this._viewPanal.TabIndex = 1;
            this._viewPanal.AutoSize = true;

            this.SizeChanged += _viewPanal.scaleView;
        }
        

        /// <summary>
        /// Event handler for the Exit menu option.
        /// </summary>
        void ExitMenuClicked(object sender, EventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Event handler for the Open menu option.
        /// </summary>
        void OpenMenuClicked(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "..\\Content");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Model";

            fileDialog.Filter = "Model Files (*.fbx;*.x)|*.fbx;*.x|" +
                                "FBX Files (*.fbx)|*.fbx|" +
                                "X Files (*.x)|*.x|" +
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

               new SysEvn(0,this, OBJTYPE.Building, SYSEVN.Import,fileDialog.FileName);
               //LoadModel(fileDialog.FileName);
            }

        }

        private void MoveBtn_Click(object sender, EventArgs e)
        {

            if (SelectFunction.Selection.Count == 0)
                return;

            ISelectable scomp = SelectFunction.Selection[0];

            //if (scomp is PipeBase)
            //    new MoveTool(SceneEntry.Scene, ((ISelectable)((SceneNodHierachyModel)scomp).Root));
            //else
            new MoveTool(SceneEntry.Scene, scomp);

            //SceneNodHierachyModel test = new SceneNodHierachyModel(SceneEntry.Scene);
            //test.ShapeNode = null;
            //test.TransformNode = new TransformNode();
            //test.TransformNode.Translate = new Vector3(-10, 0, 5);
            //test.TransformNode.Pivot = Matrix.CreateTranslation
            //                        (test.TransformNode.Translate);
            //new MoveTool(SceneEntry.Scene, test);
        }

        private void ExtrudeBtn_Click(object sender, EventArgs e)
        {
            if (SelectFunction.Selection.Count == 0)
                return;

            ISelectable scomp = SelectFunction.Selection[0];
            IDrawableComponent dcomp = ((IDrawableComponent)scomp);
            if (!(dcomp is PipeBase))
                return;
            PipeBase pipe=(PipeBase)dcomp;
            
            new ExtrudeTool(SceneEntry.Scene, pipe);
           
        
        }
        
        private void PipeABtn_Click(object sender, EventArgs e)
        {

            new SysEvn(0, this, OBJTYPE.Pipe, SYSEVN.Import, "_PipeA");
        }

        private void PipeBBtn_Click(object sender, EventArgs e)
        {
            //NodCreator newScene =
            //NodCreator.CreateNode(SceneEntry.Scene, "_PipeB", typeof(Pipe));
            new SysEvn(0, this, OBJTYPE.Pipe, SYSEVN.Import, "_PipeB");
        }

        private void PipeCBtn_Click(object sender, EventArgs e)
        {
            // NodCreator newScene =
            //NodCreator.CreateNode(SceneEntry.Scene, "_PipeC", typeof(Pipe));
            new SysEvn(0, this, OBJTYPE.Pipe, SYSEVN.Import, "_PipeC");
        }

        private void PipeDBtn_Click(object sender, EventArgs e)
        {
            //NodCreator newScene =
            //     new NodCreator(SceneEntry.Scene, "_PipeD", typeof(Pipe));
            new SysEvn(0, this, OBJTYPE.Pipe, SYSEVN.Import, "_PipeD");
        }

        private void PipeEBtn_Click(object sender, EventArgs e)
        {
            //NodCreator newScene =
            //     new NodCreator(SceneEntry.Scene, "_PipeE", typeof(Pipe));
            new SysEvn(0, this, OBJTYPE.Pipe, SYSEVN.Import, "_PipeE");
        }
        private void ChamberBtn_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.Chamber, SYSEVN.Import, "_Chamber");
        }
        private void ValveBtn_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.Valve, SYSEVN.Import, "_Valve");

        }

        private void WellBtn_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.Well, SYSEVN.Import, "_Well");
        }

        private void HoleEllipse_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.HoleEllipse, SYSEVN.Import, "_HoleEllipse");
        }

        private void HoleBox_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.HoleRect, SYSEVN.Import, "_HoleBox");
        }


        private void BreakPointBtn_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.NULL, SYSEVN.Import, "_Flag");
        }


        private void BuildingBtn1_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.Building, SYSEVN.Import, "_Building1");
        }

        private void BuildingBtn2_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.Building, SYSEVN.Import, "_Building2");
        }

        private void BuildingBtn3_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.Building, SYSEVN.Import, "_Building3");
        }

        private void RoadBtn_Click(object sender, EventArgs e)
        {
         
            new SysEvn(0, this, OBJTYPE.Building, SYSEVN.Import, "_Road");
        }

        private void subwayBtn_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.Building, SYSEVN.Import, "_SubwayStation");
        }

        private void BridgeBtn_Click(object sender, EventArgs e)
        {
            new SysEvn(0, this, OBJTYPE.Building, SYSEVN.Import, "_Bridge");
        }
        private void UndoBtn_Click(object sender, EventArgs e)
        {
            if (TimeMechine.LastEntry == null)
                return;

            try
            {
                //if (TimeMechine.History.CurIndex == TimeMechine.History.Count - 1)
                //{

                //    TimeMechine.History.PushLast();
                //}



               

                //if(entry.ToolNm=="aCTool")
                //if (entry == null || entry.Target == null || entry.SnapShot.Trans==null)
                //{
                //    MyConsole.WriteLine("LastUndo");
                //    return;
                //}
                HistoryEntry entry = TimeMechine.History.CurObj;
                if (entry.ToolNm == "FirstEntry")
                    return;

                switch (entry.ToolNm)
                { 
                    case "aCTool":
                        entry.Target.ShapeNode = entry.SnapShot.Before.Shape;
                        entry.Target.TransformNode = entry.SnapShot.Before.Trans;
                        entry.Target.TransformNode.DataModifiedHandler.Invoke();
                        break;

                    case "FirstEntry":
                        break;

                    case "New":
                        SceneEntry.Scene.Components.Remove(entry.Target);
                        break;
                    case "PropertyPanal":
                        entry.Target.TransformNode = entry.SnapShot.Before.Trans;
                        entry.Target.TransformNode.DataModifiedHandler.Invoke();
                        break;
                    default:
                        break;
                
                
                }
                TimeMechine.History.Undo();
            }
            catch (MemoryException f)
            {
                MyConsole.WriteLine("Undo::"+f.Message);
            }
        }
        private void RedoBtn_Click(object sender, EventArgs e)
        {
            try
            {
                HistoryEntry entry = TimeMechine.History.Redo();
                if (entry == null||entry.Target == null)
                {
                    MyConsole.WriteLine("LastRedo");
                    return;
                }

                switch (entry.ToolNm)
                {
                    case "aCTool":
                        entry.Target.ShapeNode = entry.SnapShot.After.Shape;
                        entry.Target.TransformNode = entry.SnapShot.After.Trans;
                        entry.Target.TransformNode.DataModifiedHandler.Invoke();
                        break;

                    case "FirstEntry":
                        break;

                    case "New":
                        //SceneEntry.Scene.Components.Remove(entry.Target);
                        SceneEntry.Scene.Components.Add(entry.Target);
                        break;
                    case "PropertyPanal":
                        entry.Target.TransformNode = entry.SnapShot.Before.Trans;
                        entry.Target.TransformNode.DataModifiedHandler.Invoke();
                        break;
                    default:
                        break;


                }

            }
            catch (MemoryException f)
            {
                MyConsole.WriteLine("Redo::"+f.Message);
            }
        }
        private void SnapBtn_Click(object sender, EventArgs e)
        {
            if (SelectFunction.Selection.Count == 0)
                return;

            ISelectable scomp = SelectFunction.Selection[0];
            IDrawableComponent dcomp = ((IDrawableComponent)scomp);
            if (!(dcomp is PipeBase))
                return;
            PipeBase pipe = (PipeBase)dcomp;

            new CombineTool(SceneEntry.Scene, pipe);

        }

        

        void HoleBox_MouseLeave(object sender, System.EventArgs e)
        {
            for (int i = 0; i < this.ValveBtn.Parent.Controls.Count; i++)
                if (this.ValveBtn.Parent.Controls[i].Name == "tmp")
                    this.ValveBtn.Parent.Controls.RemoveAt(i);
        }

        void HoleBox_MouseEnter(object sender, System.EventArgs e)
        {
            Label tmp = new Label();
            tmp.Name = "tmp";
            tmp.Text = "·½ÐÎ¿Õ¶´";
            tmp.BackColor = System.Drawing.Color.FromArgb(0, 20, 10, 20);
            tmp.Width = 60;
            tmp.Location = this.ValveBtn.Parent.PointToClient(new System.Drawing.Point(MousePosition.X + 5, MousePosition.Y + 5));

            this.ValveBtn.Parent.Controls.Add(tmp);
            tmp.BringToFront();
        }

        void HoleEllipse_MouseLeave(object sender, System.EventArgs e)
        {
            for (int i = 0; i < this.ValveBtn.Parent.Controls.Count; i++)
                if (this.ValveBtn.Parent.Controls[i].Name == "tmp")
                    this.ValveBtn.Parent.Controls.RemoveAt(i);
        }

        void HoleEllipse_MouseEnter(object sender, System.EventArgs e)
        {
            Label tmp = new Label();
            tmp.Name = "tmp";
            tmp.Text = "ÍÖÔ²¿Õ¶´";
            tmp.BackColor = System.Drawing.Color.FromArgb(0, 20, 10, 20);
            tmp.Width = 60;
            tmp.Location = this.ValveBtn.Parent.PointToClient(new System.Drawing.Point(MousePosition.X+5,MousePosition.Y+5));

            this.ValveBtn.Parent.Controls.Add(tmp);
            tmp.BringToFront();
        }

        void WellBtn_MouseLeave(object sender, System.EventArgs e)
        {
            for (int i = 0; i < this.ValveBtn.Parent.Controls.Count; i++)
                if (this.ValveBtn.Parent.Controls[i].Name == "tmp")
                    this.ValveBtn.Parent.Controls.RemoveAt(i);
        }

        void WellBtn_MouseEnter(object sender, System.EventArgs e)
        {
            Label tmp = new Label();
            tmp.Name = "tmp";
            tmp.Text = "¾®";
            tmp.BackColor = System.Drawing.Color.FromArgb(0, 20, 10, 20);
            tmp.Width = 40;
            tmp.Location = this.ValveBtn.Parent.PointToClient(new System.Drawing.Point(MousePosition.X + 5, MousePosition.Y + 5));

            this.ValveBtn.Parent.Controls.Add(tmp);
            tmp.BringToFront();
        }
        void ChamberBtn_MouseLeave(object sender, System.EventArgs e)
        {
            for (int i = 0; i < this.ValveBtn.Parent.Controls.Count; i++)
                if (this.ValveBtn.Parent.Controls[i].Name == "tmp")
                    this.ValveBtn.Parent.Controls.RemoveAt(i);
        }
        void ChamberBtn_MouseEnter(object sender, System.EventArgs e)
        {
            Label tmp = new Label();
            tmp.Name = "tmp";
            tmp.Text = "Ð¡ÊÒ";
            tmp.BackColor = System.Drawing.Color.FromArgb(0, 20, 10, 20);
            tmp.Width = 40;
            tmp.Location = this.ValveBtn.Parent.PointToClient(new System.Drawing.Point(MousePosition.X + 5, MousePosition.Y + 5));

            this.ValveBtn.Parent.Controls.Add(tmp);
            tmp.BringToFront();
        }

        void ValveBtn_MouseLeave(object sender, System.EventArgs e)
        {
            for (int i = 0; i < this.ValveBtn.Parent.Controls.Count; i++)
                if (this.ValveBtn.Parent.Controls[i].Name == "tmp")
                    this.ValveBtn.Parent.Controls.RemoveAt(i);
        }

        void ValveBtn_MouseEnter(object sender, System.EventArgs e)
        {
            Label tmp = new Label();
            tmp.Name = "tmp";
            tmp.Text = "·§ÃÅ";
            tmp.BackColor = System.Drawing.Color.FromArgb(0, 20, 10, 20);
            tmp.Width = 40;
            tmp.Location = this.ValveBtn.Parent.PointToClient(new System.Drawing.Point(MousePosition.X + 5, MousePosition.Y + 5));

            this.ValveBtn.Parent.Controls.Add(tmp);
            tmp.BringToFront();

        }

        private void RotateBtn_Click(object sender, EventArgs e)
        {
            //SceneNodHierachyModel test = new SceneNodHierachyModel(SceneEntry.Scene);
            //test.ShapeNode = null;
            //test.Children = new NodeChildren<INode>();

            //test.TransformNode = new TransformNode();
            //test.TransformNode.Translate = new Vector3(-10, 0, 5);
            //test.TransformNode.Pivot = Matrix.CreateTranslation(Vector3.Zero);

            //SceneNodHierachyModel testChild = new SceneNodHierachyModel(SceneEntry.Scene);
            //testChild.ShapeNode = null;
            //testChild.Children = new NodeChildren<INode>();

            //testChild.TransformNode = new TransformNode();
            //testChild.TransformNode.Translate = new Vector3(0, 0, 0);
            //testChild.TransformNode.Pivot = Matrix.CreateTranslation(Vector3.Zero);
            //testChild.Parent = test;

            //test.Children.Add(testChild);
            //test.Initialize();
            //new RotateTool(SceneEntry.Scene, test);

            if (SelectFunction.Selection.Count == 0)
                return;



            ISelectable scomp = SelectFunction.Selection[0];
            new RotateTool(SceneEntry.Scene, scomp);
        }

        private void CopyBtn_Click(object sender, EventArgs e)
        {
            if (SelectFunction.Selection.Count == 0)
                return;
            ISelectable scomp = SelectFunction.Selection[0];
            SceneNodHierachyModel model = scomp as SceneNodHierachyModel;
            if (model == null)
                return;
            //NodCreator newObj = new NodCreator(SceneEntry.Scene, model);
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SerializerHelper.Load();
            OpenFileDialog fileDialog = new OpenFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "..\\Saves");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Model";

            fileDialog.Filter = "CPipe Files (*.fbx;)|*.cpipe;|" +
                                
                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                //new SysEvn(0, this, OBJTYPE.Building, SYSEVN.Import, fileDialog.FileName);
                //LoadModel(fileDialog.FileName);
                SerializerHelper.Load(fileDialog.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            //SerializerHelper.Save();
            SaveFileDialog fileDialog = new SaveFileDialog();

            // Default to the directory which contains our content files.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string relativePath = Path.Combine(assemblyLocation, "..\\Saves");
            string contentPath = Path.GetFullPath(relativePath);

            fileDialog.InitialDirectory = contentPath;

            fileDialog.Title = "Load Model";

            fileDialog.Filter = "CPipe Files (*.fbx;)|*.cpipe;|" +

                                "All Files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                //new SysEvn(0, this, OBJTYPE.Building, SYSEVN.Import, fileDialog.FileName);
                //LoadModel(fileDialog.FileName);
                SerializerHelper.Save(fileDialog.FileName, Scene.Scenes[0].Components);
            }
        }




    

        
    }
}
