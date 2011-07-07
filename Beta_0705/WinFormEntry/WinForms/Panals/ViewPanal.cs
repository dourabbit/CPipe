using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using System.Reflection;
using XNASysLib.XNAKernel;
using Microsoft.Xna.Framework;

namespace WinFormsContentLoading
{
    public class ViewPanal : System.Windows.Forms.TableLayoutPanel
    {
        SceneEntry _entry;

        MenuStrip _menuStrip;

        ToolStripMenuItem _cam_Persp;
        ToolStripMenuItem _cam_Top;
        ToolStripMenuItem _cam_Front;
        ToolStripMenuItem _cam_Side;
        ToolStripMenuItem _cam_List;

        SplitContainer _viewContainerRoot;

        void IntialOneView()
        {
            this.Controls.Clear();

            //SplitContainer verticalRoot = new SplitContainer();
            _viewContainerRoot = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)
                    (_viewContainerRoot)).BeginInit();
            _viewContainerRoot.Panel1.SuspendLayout();
            _viewContainerRoot.Panel2.SuspendLayout();
            _viewContainerRoot.SuspendLayout();

            _viewContainerRoot.Dock = DockStyle.Fill;
            _viewContainerRoot.Location = new System.Drawing.Point(0, 0);
            _viewContainerRoot.Name = "VerticalRoot";
            _viewContainerRoot.Orientation = Orientation.Vertical;

            SplitContainer view1 = new ViewContainer("View1", true);


           // _viewContainerRoot.Panel1.Controls.Add(view1);
            this.Controls.Add(view1);



            this.Controls.Add(_viewContainerRoot);

            view1.Panel1.ResumeLayout(false);
            view1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)
                    view1).EndInit();
            view1.ResumeLayout(false);


            _viewContainerRoot.Panel1.ResumeLayout(false);
            _viewContainerRoot.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)
                    _viewContainerRoot).EndInit();
            _viewContainerRoot.ResumeLayout(false);

            this.Name = "ViewPanal";

            this.InitLayout();

        }
        //SplitContainer getView(string ViewNm)
        //{ 
        //    SplitContainer result;
        //    ToolStripMenuItem cam_List = new ToolStripMenuItem();
        //    ToolStripMenuItem cam_Front = new ToolStripMenuItem();
        //    ToolStripMenuItem cam_Persp = new ToolStripMenuItem();
        //    ToolStripMenuItem cam_Side = new ToolStripMenuItem();
        //    ToolStripMenuItem cam_Top = new ToolStripMenuItem();

        //    MenuStrip menuStrip = new MenuStrip();
        //    menuStrip.SuspendLayout();

        //    //
        //    //StripMenuItem: Cam_Persp
        //    //
        //    cam_Persp.Name = "Cam_Persp";
        //    cam_Persp.Size = new System.Drawing.Size(152, 22);
        //    cam_Persp.Text = "Persp";

        //    //
        //    //StripMenuItem: Cam_Top
        //    //
        //    cam_Top.Name = "Cam_Top";
        //    cam_Top.Size = new System.Drawing.Size(152, 22);
        //    cam_Top.Text = "Top";

        //    //
        //    //StripMenuItem: Cam_Side
        //    //
        //    cam_Side.Name = "Cam_Side";
        //    cam_Side.Size = new System.Drawing.Size(152, 22);
        //    cam_Side.Text = "Side";

        //    //
        //    //StripMenuItem: Cam_Front
        //    //
        //    cam_Front.Name = "Cam_Front";
        //    cam_Front.Size = new System.Drawing.Size(152, 22);
        //    cam_Front.Text = "Front";

        //    //
        //    //StripMenuItem: CamList
        //    //
        //    cam_List.DropDownItems.AddRange(new ToolStripMenuItem[]
        //        {
        //            cam_Front,
        //            cam_Persp,
        //            cam_Side,
        //            cam_Top
        //        });
        //    cam_List.Name = "CamListStripMenu";
        //    cam_List.Size = new System.Drawing.Size(37, 20);
        //    cam_List.Text = "Cams";

        //    //
        //    //MenuStrip
        //    //
        //    menuStrip.Items.AddRange(new ToolStripItem[]
        //        {
        //            cam_List
        //        });
        //    menuStrip.Location = new System.Drawing.Point(0, 0);
        //    menuStrip.Name = "ViewMenuStrip";
        //    menuStrip.Size = new System.Drawing.Size(650, 25);
        //    menuStrip.TabIndex = 3;
        //    menuStrip.Text = "ViewMenuStrip";



            

        //    result = new SplitContainer();
        //    ((System.ComponentModel.ISupportInitialize)
        //            (result)).BeginInit();
        //    result.Panel1.SuspendLayout();
        //    result.Panel2.SuspendLayout();
        //    result.SuspendLayout();


        //    result.Dock = DockStyle.Fill;
        //    result.Location = new System.Drawing.Point(0, 0);
        //    result.Name = "SplitView";
        //    result.Orientation = Orientation.Horizontal;

        //    result.Panel1.Controls.Add(menuStrip);

        //    SceneEntry scene;
        //    //a = _entry;
        //    //result.Panel2.Controls.Add(_entry);
        //    scene = new WinFormsContentLoading.SceneEntry();
        //    scene.Dock = System.Windows.Forms.DockStyle.Fill;
        //    scene.Location = new System.Drawing.Point(0, 0);
        //    scene.Name = ViewNm;
        //    scene.Size = new System.Drawing.Size(642, 407);
        //    scene.TabIndex = 11;
        //    scene.Text = "modelViewerControl";
            

        //    result.Panel2.Controls.Add(scene);

        //    result.Size = new System.Drawing.Size(640, 480);
        //    result.SplitterDistance = 10;
        //    result.TabIndex = 3;
        //    result.AutoScaleMode = AutoScaleMode.Font;


        //    menuStrip.ResumeLayout(false);
        //    menuStrip.PerformLayout();

        //    return result;
        //}
        void IntialTwoView()
        {
            this.Controls.Clear();

            //SplitContainer verticalRoot = new SplitContainer();
            _viewContainerRoot = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)
                    (_viewContainerRoot)).BeginInit();
            _viewContainerRoot.Panel1.SuspendLayout();
            _viewContainerRoot.Panel2.SuspendLayout();
            _viewContainerRoot.SuspendLayout();

            _viewContainerRoot.Dock = DockStyle.Fill;
            _viewContainerRoot.Location = new System.Drawing.Point(0, 0);
            _viewContainerRoot.Name = "VerticalRoot";
            _viewContainerRoot.Orientation = Orientation.Vertical;

            SplitContainer view1 = new ViewContainer("View1", true); //getView("View1");
            SplitContainer view2 = new ViewContainer("View2", false); //getView("View2");

            _viewContainerRoot.Panel1.Controls.Add(view1);
            _viewContainerRoot.Panel2.Controls.Add(view2);

            


            this.Controls.Add(_viewContainerRoot);
            
            view1.Panel1.ResumeLayout(false);
            view1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)
                    view1).EndInit();
            view1.ResumeLayout(false);

            view2.Panel1.ResumeLayout(false);
            view2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)
                    view2).EndInit();
            view2.ResumeLayout(false);

            _viewContainerRoot.Panel1.ResumeLayout(false);
            _viewContainerRoot.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)
                    _viewContainerRoot).EndInit();
            _viewContainerRoot.ResumeLayout(false);

            this.Name = "ViewPanal";

            this.InitLayout();

           // Rectangle a=
           // ((SceneEntry)((SplitContainer)_viewContainerRoot.Panel1.Controls[0]).Panel2.Controls[0]).ActiveRegion;

        }
        public ViewPanal(SceneEntry entry)
            : base()
        {
            //_entry = entry;
            this.IntialOneView();
           
            //this.IntialTwoView();
           
        }

      
        /*
        void ViewPanal_MouseEnter(object sender, EventArgs e)
        {
            DataReactor dataR=
            (DataReactor)SceneEntry.Scene.Services.
            GetService(typeof(DataReactor));

            dataR.RetrieveCursorHandler.Invoke();
        }
        void ViewPanal_MouseLeave(object sender, EventArgs e)
        {

            DataReactor dataR =
            (DataReactor)SceneEntry.Scene.Services.
            GetService(typeof(DataReactor));

            dataR.LostCursorHandler.Invoke();
        }*/
        public void scaleView(object sender, EventArgs e)
        {
            int height =(int)(((MainForm)sender).ClientSize.Height*0.75);
            int width = (int)(((MainForm)sender).ClientSize.Width*0.75);
           _viewContainerRoot.Size = new System.Drawing.Size(width,height);
            //this._entry.Size = new System.Drawing.Size(width, height-50);
        }
    }
}
      

