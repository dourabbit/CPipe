#region File Description
//-----------------------------------------------------------------------------
// Primitives3DGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace VertexPipeline
{
    /// <summary>
    /// Define the Load and Save
    /// </summary>
    public interface ISerilization
    {
        void Load(ITransformNode Trans, IShapeNode Shape);
        void Save(string Path);
    }
    
    public interface INode
    {
        string NodeNm { get; set; }
        INode Parent { get; set; }
        NodeChildren<INode> Children
        { get; set; }
        INode Root { get; set; }
        int Index { get; set; }

    }
    public interface ITransformNode:INode
    {
        Matrix World
        { get; set; }

        Vector3 Translate
        { get; set; }
        Vector3 Rotate
        { get; set; }
        Matrix Pivot
        { get; set; }
        Vector3 Scale
        { get; set; }
        Quaternion RotQuaternion
        { get; set; }
        Matrix AbsoluteTransform { get; set; }
    }
    public interface ITransObj
    {
        TransformNode TransformNode
        { get; set; }
        bool IsMuteTransform
        { get; }
    }
    public delegate void OnDataModified();
    
    public delegate void TransformChanged();
    public class TransformNode : ITransformNode
    {
        public TransformNode GetCopy()
        {
            return (TransformNode)this.MemberwiseClone();
        }
        public string NodeNm
        { 
            get 
            { 
                return _nodeNm; 
            } 
            set
            {
                if (DataModifiedHandler != null)
                    DataModifiedHandler.Invoke();
                _nodeNm = value; 
            } 
        }
        public INode Parent
        { 
            get 
            { 
                return _parent; 
            } 
            set 
            {
                _parent = value;
                if (DataModifiedHandler != null)
                    DataModifiedHandler.Invoke();
            }
        }
        public NodeChildren<INode> Children 
        { 
            get 
            { 
                return _children; 
            } 
            set 
            {
                _children = value; 
            } 
        }
        public INode Root 
        { 
            get
            { 
                return _root; 
            } 
            set 
            {
                _root = value; 
            } 
        }
        public Matrix AbsoluteTransform 
        {
            get
            {
                return _absoluteTrans;
            }

            set
            {
                _absoluteTrans = value;
            }
        }
        Matrix _absoluteTrans;
        public int Index {  get; set; }
        public OnDataModified DataModifiedHandler;
       // public TransformChanged TransformChangedHandler;
        protected Vector3 _translate;
        protected Vector3 _rotate;
        protected Matrix _pivot;
        protected Vector3 _scale;
        protected Matrix _world;
        protected string _nodeNm;
        protected INode _parent;
        protected NodeChildren<INode> _children;
        protected INode _root;

        //protected Quaternion _rotQuaternion;


        public Matrix World
        { 
            get 
            {
               // this._world =
               //TransformHelper.RotInObjSpace
               //(Rotate, Pivot, Translate, Scale, out _rotQuaternion);
                return _world; 
            }
            set 
            {
                
                value.Decompose(out _scale, out _rotQuaternion, out _translate);

                this._rotate = TransformHelper.QuaternionToYawPitchRoll(_rotQuaternion);
                if (DataModifiedHandler != null)
                    DataModifiedHandler.Invoke();
                _world = value; 
            }
        }
        public virtual Vector3 Translate 
        {
            get 
            {
                
                return _translate; 
            }
            set 
            {
                _translate = value;
                //if (TransformChangedHandler != null)
                //    TransformChangedHandler.Invoke();
                // UpdateTransform();
               // this._world =
               //TransformHelper.RotInObjSpace
               //(Rotate, Pivot, Translate, Scale, out _rotQuaternion);



                this._world = 
                    TransformHelper.
                    RotByQuat(Pivot, Translate, Scale, RotQuaternion);

                if (DataModifiedHandler != null)
                    DataModifiedHandler.Invoke();
                
            }
        }
        public Vector3 Rotate 
        {
            get 
            { 
                return _rotate; 
            }
            set
            {
                _rotate = value;


               // this._world =
               //TransformHelper.RotInObjSpace
               //(Rotate, Pivot, Translate, Scale, out _rotQuaternion);
                //this._world =
                //    TransformHelper.
                //    RotInObjSpace(Pivot.Translation, Translate, Scale, RotQuaternion);

                //if (TransformChangedHandler != null)
                //    TransformChangedHandler.Invoke();
                //UpdateTransform();
                if (DataModifiedHandler != null)
                    DataModifiedHandler.Invoke();
                
            }
        }

        /// <summary>
        /// Relative offset of Pivot
        /// </summary>
        public Matrix Pivot
        {
            get 
            { 
                return _pivot; 
            }
            set 
            { 
                _pivot = value;
               // this._world =
               //TransformHelper.RotInObjSpace
               //(Rotate, Pivot, Translate, Scale, out _rotQuaternion);
                //this._world =
                //    TransformHelper.
                //    RotInObjSpace(Pivot.Translation, Translate, Scale, RotQuaternion);

                //if (TransformChangedHandler != null)
                //    TransformChangedHandler.Invoke();
                //UpdateTransform(); 
                if (DataModifiedHandler != null)
                    DataModifiedHandler.Invoke();
            }
        }
        public Vector3 Scale 
        {
            get 
            { 
                return _scale;
            }
            set
            {
                _scale = value;
               // this._world =
               //TransformHelper.RotInObjSpace
               //(Rotate, Pivot, Translate, Scale, out _rotQuaternion);
                this._world =
                    TransformHelper.
                    RotByQuat(Pivot, Translate, Scale, RotQuaternion);

                //if (TransformChangedHandler != null)
                //    TransformChangedHandler.Invoke();
                //UpdateTransform();
                if (DataModifiedHandler != null)
                    DataModifiedHandler.Invoke();
                
            }
        }
        Quaternion _rotQuaternion;
        public Quaternion RotQuaternion
        {
            get 
            {
                //this._world =
                //TransformHelper.RotInObjSpace
                //(Rotate, Pivot, Translate, Scale, out _rotQuaternion);
                //_rotQuaternion.Normalize();
                return _rotQuaternion; 
            }
            set 
            {
                value.Normalize();
                this.Rotate = QuaternionToEuler(value);
                _rotQuaternion = value;
                this._world = TransformHelper.RotByQuat(
                    _pivot, _translate, _scale, _rotQuaternion
                    );

                if (DataModifiedHandler != null)
                    DataModifiedHandler.Invoke();
                
            }
        }
        public void RotateInObjSpace(Vector3 rotation)
        {
            this._world = TransformHelper.RotByEuler
                (rotation,_pivot,_translate,_scale,out _rotQuaternion);
            if (DataModifiedHandler != null)
                DataModifiedHandler.Invoke();
        }


        public void RotateInArbitaryAxis(Vector3 axis, float rotation)
        {
            Quaternion rot = Quaternion.CreateFromAxisAngle(axis,MathHelper.ToRadians(rotation));
            _rotQuaternion *= rot;
            this._pivot = Matrix.CreateFromQuaternion(_rotQuaternion) 
                            * Matrix.CreateTranslation(_translate);
            this._world = TransformHelper.RotByQuat(
                    _pivot, _translate, _scale, _rotQuaternion
                    );
        }

        Vector3 QuaternionToEuler(Quaternion q)
        {
            const float Epsilon = 0.0009765625f;
            const float Threshold = 0.5f - Epsilon;

            float yaw;
            float pitch;
            float roll;

            float XY = q.X * q.Y;
            float ZW = q.Z * q.W;

            float TEST = XY + ZW;

            if (TEST < -Threshold || TEST > Threshold)
            {

                int sign = Math.Sign(TEST);

                yaw = sign * 2 * (float)Math.Atan2(q.X, q.W);

                pitch = sign * MathHelper.PiOver2;

                roll = 0;

            }
            else
            {

                float XX = q.X * q.X;
                float XZ = q.X * q.Z;
                float XW = q.X * q.W;

                float YY = q.Y * q.Y;
                float YW = q.Y * q.W;
                float YZ = q.Y * q.Z;

                float ZZ = q.Z * q.Z;

                yaw = (float)Math.Atan2(2 * YW - 2 * XZ, 1 - 2 * YY - 2 * ZZ);

                pitch = (float)Math.Atan2(2 * XW - 2 * YZ, 1 - 2 * XX - 2 * ZZ);

                roll = (float)Math.Asin(2 * TEST);

            }//if 

            return new Vector3(MathHelper.ToDegrees(pitch), MathHelper.ToDegrees(yaw), MathHelper.ToDegrees(roll)); 
        }  

        public TransformNode()
        { 
            _translate = Vector3.Zero;
            _rotate = Vector3.Zero;
            _pivot =Matrix.Identity;
            _scale = new Vector3(1,1,1);
            _world = Matrix.Identity;
            _rotQuaternion = Quaternion.Identity;
            AbsoluteTransform = Matrix.Identity;

           // this.DataModifiedHandler += UpdateTransform;
        }
        public void Initialize()
        {
            this._world =
                    TransformHelper.
                    RotByQuat(Pivot, Translate, Scale, RotQuaternion);
        }
        /// <summary>
        /// Update Transform From Translate, Rotate, Scale
        /// </summary>
        //void UpdateTransform()
        //{
        //    this._world =
        //         TransformHelper.RotInObjSpace
        //         (Rotate, Pivot, Translate, Scale, out _rotQuaternion);
        //}



        /// <summary>
        /// Hierachy Transforming, Updating the world Matrix and AbsoluteTransform Matrix
        /// </summary>
        /// <param name="parentMatrix"></param>
        //public void UpdateTransform(Matrix parentMatrix)
        //{
        //    this._world =
        //         TransformHelper.RotInObjSpace
        //         (Rotate, Pivot, Translate, Scale, out _rotQuaternion);
        //    //this._world *= parentMatrix;

        //    this.AbsoluteTransform = this.World * parentMatrix;
        //    //
        //    //RotQuaternion = Quaternion.Identity;
        //    //RotQuaternion *= Quaternion.
        //    //    CreateFromAxisAngle(Vector3.UnitX, 
        //    //                        MathHelper.ToRadians( _rotate.X));
        //    //RotQuaternion *= Quaternion.
        //    //    CreateFromAxisAngle(Vector3.UnitY,
        //    //                        MathHelper.ToRadians(_rotate.Y));
        //    //RotQuaternion *= Quaternion.
        //    //    CreateFromAxisAngle(Vector3.UnitZ,
        //    //                        MathHelper.ToRadians(_rotate.Z));

        //    //this.World = Matrix.CreateFromQuaternion(RotQuaternion) *
        //    //   Matrix.CreateTranslation(Translate) *
        //    //   Matrix.CreateScale(Scale);
        //}

    }
    public interface IShapeNode
    {
        string Name { get; set; }
        List< VertexPositionNormalTexture> Vertices
        { get; }
        DynamicVertexBuffer VertexBuffer
        {get;}
        BoundingSphere[] BoundingSpheres
        { get; set; }
        IndexBuffer IndexBuffer
        { get; }
    }
    
  
    public class NodeChildren<T> : List<T>
       where T : INode
    { 
        
    }


}
