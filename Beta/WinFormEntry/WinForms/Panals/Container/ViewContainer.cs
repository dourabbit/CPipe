using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using System.Reflection;
using XNASysLib.XNAKernel;
using Microsoft.Xna.Framework;
using VertexPipeline;

namespace WinFormsContentLoading
{
    public class ViewContainer : System.Windows.Forms.SplitContainer
    {

        ToolStripMenuItem cam_List = new ToolStripMenuItem();
        ToolStripMenuItem cam_Contr = new ToolStripMenuItem();

        public SceneEntry SceneEntry;

        public ViewContainer(string ViewNm, bool isEnabled)
        { 
            

            MenuStrip menuStrip = new MenuStrip();
            menuStrip.SuspendLayout();

            //
            //StripMenuItem: Cam_Persp
            //
            //cam_Persp.Name = "Cam_Persp";
            //cam_Persp.Size = new System.Drawing.Size(152, 22);
            //cam_Persp.Text = "Persp";

            //
            //StripMenuItem: Cam_Top
            //
            //cam_Top.Name = "Cam_Top";
            //cam_Top.Size = new System.Drawing.Size(152, 22);
            //cam_Top.Text = "Top";

            //
            //StripMenuItem: Cam_Side
            //
            //cam_Side.Name = "Cam_Side";
            //cam_Side.Size = new System.Drawing.Size(152, 22);
            //cam_Side.Text = "Side";

            //
            //StripMenuItem: Cam_Front
            //
            //cam_Front.Name = "Cam_Front";
            //cam_Front.Size = new System.Drawing.Size(152, 22);
            //cam_Front.Text = "Front";

            //
            //StripMenuItem: CamList
            //
            //cam_List.DropDownItems.AddRange(new ToolStripMenuItem[]
                //{
              //      cam_Front,
                //    cam_Persp,
                  //  cam_Side,
                    //cam_Top
                //});
            cam_List.Name = "CamsList";
            cam_List.Size = new System.Drawing.Size(37, 20);
            cam_List.Text = "视图列表";

            cam_List.Click+=cam_List_Click;
            
            //
            //StripMenuItem:camContr
            //
            cam_Contr.Name = "CamContr";
            cam_Contr.Text = "视图控制";
            cam_Contr.Size=new System.Drawing.Size(37,20);
            ToolStripMenuItem newCam = new ToolStripMenuItem();
            newCam.Name = "New Camera";
            newCam.Text = "新建视图";
            newCam.Size = new System.Drawing.Size(37, 20);
            newCam.Click += new EventHandler(delegate(object sender, EventArgs e)
                {
                    new dCamera(SceneEntry.Scene);   
                });
            cam_Contr.DropDownItems.AddRange(
                new ToolStripMenuItem[]{newCam}
                );
            //
            //MenuStrip
            //
            menuStrip.Items.AddRange(new ToolStripItem[]
                {
                    cam_List,
                    cam_Contr
                });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "ViewMenuStrip";
            menuStrip.Size = new System.Drawing.Size(650, 25);
            menuStrip.TabIndex = 3;
            menuStrip.Text = "视图";


            ((System.ComponentModel.ISupportInitialize)
                    this).BeginInit();
            this.Panel1.SuspendLayout();
            this.Panel2.SuspendLayout();
            this.SuspendLayout();


            this.Dock = DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "SplitView";
            this.Orientation = Orientation.Horizontal;

            this.Panel1.Controls.Add(menuStrip);

            //a = _entry;
            //result.Panel2.Controls.Add(_entry);
            SceneEntry = new WinFormsContentLoading.SceneEntry();
            SceneEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            SceneEntry.Location = new System.Drawing.Point(0, 0);
            SceneEntry.Name = ViewNm;
            SceneEntry.Size = new System.Drawing.Size(642, 407);
            SceneEntry.TabIndex = 11;
            SceneEntry.Text = "modelViewerControl";
            SceneEntry.IsActivate = isEnabled;

            this.Panel2.Controls.Add(SceneEntry);

            this.Size = new System.Drawing.Size(640, 480);
            this.SplitterDistance = 10;
            this.TabIndex = 3;
            this.AutoScaleMode = AutoScaleMode.Font;


            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();

        }



        void cam_List_Click(object sender, EventArgs e)
        {

            this.cam_List.DropDownItems.Clear();

            List<ISelectable> cams=
            SelectFunction.Select(
                delegate(IUpdatableComponent matcher)
                {
                    Type[] types= matcher.GetType().GetInterfaces();
                    bool checker = false;
                    
                    foreach (Type type in types)
                        checker |= type == typeof(ICamera) ? true : false;

                    return checker;
                });

            ToolStripMenuItem[] camItems = new ToolStripMenuItem[cams.Count];
               
            for(int i=0;i<cams.Count;i++)
            {
                ToolStripMenuItem camItem=new ToolStripMenuItem();
                
                camItem.Name = cams[i].ID;
                camItem.Size = new System.Drawing.Size(152, 22);
                camItem.Text = cams[i].ID;
                camItem.Click+=new EventHandler(camItem_Click);
                camItems[i] = camItem;
                //cam_List.DropDownItems.Add(camItem);
            }

            cam_List.DropDownItems.AddRange(camItems);
        
        }

        void camItem_Click(object sender, EventArgs e)
        {
           // this.SceneEntry.Cam;
            IUpdatableComponent ucomp=
            SelectFunction.Select(((ToolStripMenuItem)sender).Name);

            this.SceneEntry.Cam = (ICamera)ucomp;
            SelectFunction.DeSelect((ISelectable)ucomp);

            SceneEntry.Scene.Services.DelService(typeof(ICamera));
            SceneEntry.Scene.Services.AddService<ICamera>((ICamera)ucomp);
        }
    }
}
      

