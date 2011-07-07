using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VertexPipeline;
using VertexPipeline.Data;
using Microsoft.Xna.Framework;

namespace XNASysLib.Primitives3D.Base.Loader
{
	static class LoadHelper
	{
        public static void ReGroupNodes
            (TransformNode curNod, NodesGrp data,
            int index, ref TransformNode root, List<TransformNode> nodes)
        {
            string nm = data.TransData.NameGrp[index];
            int parentIndex = data.TransData.ParentIndex[index];
            Matrix relativeMat = data.TransData.RelativeMatrixGrp[index];

            curNod.Children = new NodeChildren<INode>();
            curNod.NodeNm = nm;
            if (parentIndex == -1)
            {
                root = curNod;
            }
            if (parentIndex >= 0 && parentIndex < nodes.Count)
            {
                curNod.Parent = nodes[parentIndex];
                nodes[parentIndex].Children.Add(curNod);
            }
            curNod.World = relativeMat;
        }
        public static void ProcessSceneNod(IGame game,SceneNodHierachyModel curSceneNod,
          TransformNode transNod, NodesGrp shapeGrp, ref SceneNodHierachyModel root,
          ref int curIndex)
        {

            curIndex++;
            curSceneNod.NodeNm = transNod.NodeNm;
            curSceneNod.ID = transNod.NodeNm;
            curSceneNod.Children = new NodeChildren<INode>();
            curSceneNod.Root = root;
            curSceneNod.TransformNode = transNod;
            curSceneNod.TransformNode.AbsoluteTransform = Matrix.Identity;

            //If only one obj, no child at all. New a dummy group
            if (curSceneNod == root && transNod.Children.Count == 0)
            {
                SceneNodHierachyModel childSceneH = new SceneNodHierachyModel(game);
                curSceneNod.Children.Add(childSceneH);
                childSceneH.Root = curSceneNod;
                childSceneH.Children = new NodeChildren<INode>();
                childSceneH.TransformNode = transNod;
                childSceneH.TransformNode.AbsoluteTransform = Matrix.Identity;
                childSceneH.ShapeNode = curSceneNod.ShapeNode;
                childSceneH.Parent = curSceneNod;
                childSceneH.ID = transNod.NodeNm;
                curSceneNod.ShapeNode = null;
                curSceneNod.ID = "root";
                curSceneNod.TransformNode = new TransformNode();
            }



            foreach (ShapeNode shape in shapeGrp)
            {
                if (shape.ParentIndex == curIndex)
                {
                    if (curSceneNod.ShapeNode == null)
                    {
                        if (curSceneNod.ID != "root")
                            curSceneNod.ShapeNode = shape;
                        else
                        {
                            SceneNodHierachyModel child =
                                curSceneNod.Children[0] as SceneNodHierachyModel;
                            if (child != null)
                                child.ShapeNode = shape;
                        }
                    }
                    else
                    {

                        //SceneNodHierachyModel sibling = new SceneNodHierachyModel(_game);
                        //sibling.NodeNm = transNod.NodeNm;
                        //sibling.Children = new NodeChildren<INode>();
                        //sibling.Root = root;
                        //TransformNode newTransNod = new TransformNode();
                        //sibling.TransformNode = newTransNod;
                        //sibling.TransformNode.AbsoluteTransform = Matrix.Identity;
                        //sibling.ShapeNode = shape;
                        //curSceneNod.Children.Add(sibling);
                        ShapeNode.CombineShape(shape, curSceneNod.ShapeNode);
                    }
                }
            }
            // Recurse over any child nodes.
            foreach (TransformNode childTransNod in transNod.Children)
            {
                SceneNodHierachyModel childSceneH = new SceneNodHierachyModel(game);
                //new Pipe(_game);
                curSceneNod.Children.Add(childSceneH);
                childSceneH.Parent = curSceneNod;
                ProcessSceneNod(game,childSceneH, childTransNod, shapeGrp, ref root, ref curIndex);
            }
        }
	}
}
