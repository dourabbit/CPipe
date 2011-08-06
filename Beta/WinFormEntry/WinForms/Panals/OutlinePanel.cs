using System;
using System.Drawing;
using System.Windows.Forms;
using VertexPipeline;
using XNASysLib.Primitives3D;
using XNASysLib.XNAKernel;
using System.Collections.Generic;

namespace WinFormsContentLoading
{
    class MyNode : TreeNode
    { 
       public SceneNodHierachyModel Obj {get;set;}
       public MyNode(string nm)
           : base(nm)
       { 
       
       }
       public MyNode()
           : base()
       {

       }
 
    }
    public class OutlinePanel : System.Windows.Forms.TableLayoutPanel
    {


        TreeView _treeView;
        MyNode _pipeRoot;
        MyNode _valveRoot;
        MyNode _chamberRoot;
        MyNode _wellRoot;
        MyNode _holeRoot;
        MyNode _buildingRoot;
        Button _lockPipeBtn;
        Button _lockBuildingBtn;
        Scene _game;
        public OutlinePanel( SceneEntry scene)
            : base()
        {
            _game = SceneEntry.Scene;
            Initialize();
         
            SysCollector.Singleton.Listener += sysEvent;
           
        }
        void Add(MyNode parent, SceneNodHierachyModel obj)
        {
            MyNode child = new MyNode(obj.ID);
            child.Obj = obj;
            parent.Nodes.Add(child);
            //parent.Obj = obj;
            if(obj.Children.Count>0)
            {
               
                foreach(SceneNodHierachyModel childModel in obj.Children)
                    Add(child, childModel);
            }
        }
        void newEvn(object sender)
        {
            if (!(sender is SceneNodHierachyModel))
                return;

            SceneNodHierachyModel obj = (SceneNodHierachyModel)sender;
            obj.AfterInitializedHandler += delegate()
            {
                if (obj.Type == null)
                    Add(_buildingRoot, obj);



                else
                    switch (obj.Type.Name)
                    {
                        case "Pipe":
                            Add(_pipeRoot, obj);
                            break;
                        case "Valve":
                            Add(_valveRoot, obj);
                            break;
                        case "Chamber":
                            Add(_chamberRoot, obj);
                            break;
                        case "Well":
                            Add(_wellRoot, obj);
                            break;
                        case "HoleEllipse":
                            Add(_holeRoot, obj);
                            break;


                        default:
                            Add(_buildingRoot, obj);
                            break;
                    }

            };
        }
        void sysEvent(ISysData gameData)
        {
            ISysEnv evn = gameData as ISysEnv;
            if (evn == null)
                return;
            object sender = evn.Sender;
            SYSEVN e = evn.Event;
            
            if (e == SYSEVN.Initial)
                this.newEvn(sender);

            else if (e == SYSEVN.Select)
            {
                string fullNm="";
                foreach (string nm in evn.Params)
                    fullNm += nm+"|";
                MyConsole.WriteLine(fullNm);

                select(evn.ObjType, evn.Params);
            }

        }
        void select(MyNode node, List<string> nodeNms)
        {
            if (node.Name == nodeNms[0])
                node.TreeView.SelectedNode=node;
        }
        void select(OBJTYPE type, object[] nodeNms)
        {
            if (nodeNms == null || nodeNms.Length == 0)
                _treeView.SelectedNode = null;

            List<string> nodenms = new List<string>();
            foreach (string nm in nodeNms)
                nodenms.Add(nm);
            MyNode root;
            switch (type)
            { 
                case OBJTYPE.Pipe:
                    root = _pipeRoot;
                    break;
                case OBJTYPE.Building:
                    root = _buildingRoot;
                    break;
                case OBJTYPE.Chamber:
                    root=_chamberRoot;
                    break;
                case OBJTYPE.Valve:
                    root=_valveRoot;
                    break;
                case OBJTYPE.Well:
                    root=_wellRoot;
                    break;
                case OBJTYPE.HoleEllipse:
                    root = _holeRoot;
                    break;
                case OBJTYPE.HoleRect:
                    root = _holeRoot;
                    break;
                default:
                    root = null;
                    break;
            }
            TreeNode node=null;
            findTreeNod(root, nodenms, ref node);
            _treeView.SelectedNode = node;
            
        }

        void findTreeNod(TreeNode parentNod, List<string> nodenms, ref TreeNode result)
        {
            foreach (TreeNode nod in parentNod.Nodes)
            {
                if (nodenms.Count == 0)
                    return;
                if (nod.Text == nodenms[0])
                {
                    nodenms.RemoveAt(0);
                    if (nodenms.Count == 0)
                    {
                        result = nod;
                        return;
                    }
                    else
                    {
                        findTreeNod(nod,nodenms,ref result);
                    }
                }
            }
            return;
         
        }
        void afterCheck(object sender, TreeViewEventArgs e)
        {
            Type t = sender.GetType();

            try
            {
                MyNode node = (MyNode)e.Node;


                if (node.Obj == null)
                    return;
                node.Obj.Lock = node.Checked;
            }
            catch (Exception ef)
            {
                MyConsole.WriteLine("Cannot select such data");
            }
        }
        void afterSelectHandler(object sender, TreeViewEventArgs e)
        {
            Type t = sender.GetType();

            try
            {
                MyNode node = (MyNode)e.Node;


                if (node.Obj == null)
                    return;
                string id = node.Text;

                //SelectFunction.Select(id);
                SelectFunction.Select(node.Obj);
                node.Obj.Data.SelectionHandler.Invoke(node.Obj,true);
            }
            catch (Exception ef)
            {
                MyConsole.WriteLine("Cannot select such data");
            }
        }
        //void _treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
            
        //    Type t= sender.GetType();

        //    try
        //    {
        //        MyNode node = (MyNode)e.Node;


        //        if (node.Obj == null)
        //            return;
        //        string id = node.Text;

        //        //SelectFunction.Select(id);
        //        SelectFunction.Select(node.Obj);
        //    }
        //    catch (Exception ef)
        //    {
        //        MyConsole.WriteLine("Cannot select such data");
        //    }
        //}
        void Initialize()
        {

            _treeView = new TreeView();
            _lockPipeBtn = new Button();
            _lockBuildingBtn = new Button();

            this.SuspendLayout();

            // Initialize treeView1.
            _treeView.Location = new System.Drawing.Point(0, 25);
            _treeView.Size = new Size(292, 248);
            _treeView.Anchor = AnchorStyles.Top | AnchorStyles.Left |
                AnchorStyles.Bottom | AnchorStyles.Right;
            _treeView.CheckBoxes = true;
            //_treeView.NodeMouseClick+=_treeView_NodeMouseClick;
            _treeView.AfterSelect += new TreeViewEventHandler(afterSelectHandler);
            
            _treeView.AfterCheck += new TreeViewEventHandler(afterCheck);
            _pipeRoot = new MyNode("供水管道");
            _valveRoot = new MyNode("阀门");
            _chamberRoot = new MyNode("热力小室");
            _wellRoot =new MyNode("井");//new System.Windows.Forms.TreeNode("井");
            _holeRoot = new MyNode("空洞");
            _buildingRoot = new MyNode("地面");
            _treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
                        _pipeRoot,_valveRoot,
                        _chamberRoot,_wellRoot,
                        _holeRoot, _buildingRoot});
            /*
            // Add nodes to treeView1.
            TreeNode node;
            for (int x = 0; x < 3; ++x)
            {
                // Add a root node.
                node = _treeView.Nodes.Add(String.Format("Node{0}", x * 4));
                for (int y = 1; y < 4; ++y)
                {
                    // Add a node as a child of the previously added node.
                    node = node.Nodes.Add(String.Format("Node{0}", x * 4 + y));
                }
            }

            // Set the checked state of one of the nodes to
            // demonstrate the showCheckedNodesButton button behavior.
            _treeView.Nodes[1].Nodes[0].Nodes[0].Checked = true;
            */
            // Initialize showCheckedNodesButton.
            _lockPipeBtn.Size = new Size(144, 24);
            _lockPipeBtn.Text = "锁定管道";
            _lockPipeBtn.Click +=
                new EventHandler(showCheckedNodesButton_Click);

            _lockBuildingBtn.Size = new Size(144, 24);
            _lockBuildingBtn.Text = "锁定建筑";
            _lockBuildingBtn.Click +=
                new EventHandler(showCheckedNodesButton_Click);

            // Initialize the form.
            this.ClientSize = new Size(292, 273);
            this.Controls.AddRange(new Control[] { _lockPipeBtn,_lockBuildingBtn, _treeView });

            this.ResumeLayout(false);
        }

        void lockPipes(object sender, EventArgs e)
        {
            // Disable redrawing of treeView1 to prevent flickering 
            // while changes are made.
            _treeView.BeginUpdate();

            // Collapse all nodes of treeView1.
            _treeView.ExpandAll();

            // Add the checkForCheckedChildren event handler to the BeforeExpand event.
            _treeView.BeforeCollapse += CheckLockChildrenHandler;

            // Expand all nodes of treeView1. Nodes without checked children are 
            // prevented from expanding by the checkForCheckedChildren event handler.
            _treeView.CollapseAll();

            // Remove the checkForCheckedChildren event handler from the BeforeExpand 
            // event so manual node expansion will work correctly.
            _treeView.BeforeCollapse -= CheckLockChildrenHandler;

            // Enable redrawing of treeView1.
            _treeView.EndUpdate();

        }
        void _treeView_BeforeSelect(object sender,
           TreeViewCancelEventArgs e)
        { 
            
        }
        void CheckLockChildrenHandler(object sender,
           TreeViewCancelEventArgs e)
        {
            if (CheckedLock(e.Node)) e.Cancel = true;
        }


        bool CheckedLock(TreeNode node)
        {
            
            if (node.Nodes.Count == 0) return false;
            foreach (TreeNode childNode in node.Nodes)
            {


                MyNode myNod = childNode as MyNode;
                if (childNode.Checked)
                {
                    myNod.Obj.Lock = true;
                    return true;
                }
                else
                    myNod.Obj.Lock = false;
                // Recursively check the children of the current child node.
                if (CheckedLock(childNode)) return true;
            }
            return false;
        }



        void showCheckedNodesButton_Click(object sender, EventArgs e)
        {
            // Disable redrawing of treeView1 to prevent flickering 
            // while changes are made.
            _treeView.BeginUpdate();

            // Collapse all nodes of treeView1.
            _treeView.ExpandAll();

            // Add the checkForCheckedChildren event handler to the BeforeExpand event.
            _treeView.BeforeCollapse += CheckForCheckedChildrenHandler;

            // Expand all nodes of treeView1. Nodes without checked children are 
            // prevented from expanding by the checkForCheckedChildren event handler.
            _treeView.CollapseAll();

            // Remove the checkForCheckedChildren event handler from the BeforeExpand 
            // event so manual node expansion will work correctly.
            _treeView.BeforeCollapse -= CheckForCheckedChildrenHandler;
            
            // Enable redrawing of treeView1.
            _treeView.EndUpdate();
        }

        // Prevent collapse of a node that has checked child nodes.
        void CheckForCheckedChildrenHandler(object sender,
            TreeViewCancelEventArgs e)
        {
            if (HasCheckedChildNodes(e.Node)) e.Cancel = true;
        }

        // Returns a value indicating whether the specified 
        // TreeNode has checked child nodes.
        private bool HasCheckedChildNodes(TreeNode node)
        {
            if (node.Nodes.Count == 0) return false;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Checked) return true;
                // Recursively check the children of the current child node.
                if (HasCheckedChildNodes(childNode)) return true;
            }
            return false;
        }
    
    
    }
   
}
      

