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
    public static class TransformHelper
    {
        /*/// <summary>
        /// Calculate the rotation in object space
        /// </summary>
        /// <param name="rotDegree"></param>
        /// <param name="pivot"></param>
        /// <param name="scale"></param>
        /// <param name="rotQuaternion"></param>
        /// <param name="world"></param>
        public static void RotInObjSpace(Vector3 rotDegree,
            Vector3 pivot,Vector3 translate, Vector3 scale,
            ref Quaternion rotQuaternion, out Matrix world)
        {
            Vector3 radianAngle = (rotDegree / 360) * 2 * MathHelper.Pi;
            rotQuaternion = Quaternion.Identity;
            rotQuaternion = Quaternion.Normalize(
                    Quaternion.CreateFromYawPitchRoll
                    (radianAngle.Y, radianAngle.X, radianAngle.Z)
                );

            world = Matrix.Identity;
            world *= Matrix.Invert(Matrix.CreateTranslation(pivot));
            world *= Matrix.CreateScale(scale);
            world *= Matrix.CreateFromQuaternion(rotQuaternion);
            world *= Matrix.CreateTranslation(pivot);
            world *= Matrix.CreateTranslation(translate);

        }
*/


        public static Vector3 QuaternionToYawPitchRoll(Quaternion q)
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

            return new Vector3(
                MathHelper.ToDegrees(pitch),
                MathHelper.ToDegrees(yaw),
                MathHelper.ToDegrees(roll));
 

        }//method 
        /// <summary>
        /// Rotate the object in its object space,
        /// Then return the world matrix and rotation quaternion
        /// </summary>
        /// <param name="rotDegree">Rotation in degree</param>
        /// <param name="pivot">Position of pivot</param>
        /// <param name="translate">Translation value of obj</param>
        /// <param name="scale">Scale value of obj</param>
        /// <param name="rotQuaternion">output rotational quaternion</param>
        /// <returns></returns>
        public static Matrix RotByEuler(Vector3 rotDegree, 
            Matrix pivot, Vector3 translate, Vector3 scale,
           out Quaternion rotQuaternion)
        {

            Matrix world = Matrix.Identity;
            world *= Matrix.Invert(Matrix.CreateTranslation(pivot.Translation));
            world *= Matrix.CreateScale(scale);

            Matrix rotMatrix = Matrix.Identity;
            rotMatrix *= Matrix.CreateFromAxisAngle(pivot.Right, MathHelper.ToRadians(rotDegree.X));
            rotMatrix *= Matrix.CreateFromAxisAngle(pivot.Up, MathHelper.ToRadians(rotDegree.Y));
            rotMatrix *= Matrix.CreateFromAxisAngle(pivot.Backward, MathHelper.ToRadians(rotDegree.Z));
            rotQuaternion = Quaternion.CreateFromRotationMatrix(rotMatrix);

            world *= rotMatrix;
            world *= Matrix.CreateTranslation(pivot.Translation);
            world *= Matrix.CreateTranslation(translate);

            return world;

        }

        /// <summary>
        /// Rotate the Obj in objspace by quaternion
        /// </summary>
        /// <param name="rotDegree"></param>
        /// <param name="pivot"></param>
        /// <param name="translate"></param>
        /// <param name="scale"></param>
        /// <param name="rotQuaternion"></param>
        /// <returns></returns>
        public static Matrix RotByQuat(
           Matrix pivot, Vector3 translate, Vector3 scale,
           Quaternion rotQuaternion)
        {
           
            Matrix world = Matrix.Identity;
            
            //Transforming back to Obj Space
            world *= Matrix.Invert(Matrix.CreateTranslation(pivot.Translation));
            world *= Matrix.CreateScale(scale);
            world *= Matrix.CreateFromQuaternion(rotQuaternion);
            //world *= Matrix.CreateTranslation(Vector3.Transform(pivot, rotQuaternion));
            //world *= Matrix.CreateTranslation(pivot.Translation);
            world *= Matrix.CreateTranslation(translate);

            return world;

        }
        /// <summary>
        /// Rotate the object in its object space,
        /// Then return the world matrix and rotation quaternion
        /// </summary>
        /// <param name="rotDegree">Rotation in degree</param>
        /// <param name="pivot">Position of pivot</param>
        /// <param name="translate">Translation value of obj</param>
        /// <param name="scale">Scale value of obj</param>
        /// <param name="rotQuaternion">output rotational quaternion</param>
        /// <returns></returns>
        public static Matrix RotInObjSpace(Vector3 rotDegree,
           Vector3 pivot, Vector3 translate, Vector3 scale)
        {
            Vector3 radianAngle = (rotDegree / 360) * 2 * MathHelper.Pi;
            Quaternion rotQuaternion = 
                Quaternion.Normalize(Quaternion.CreateFromYawPitchRoll
                    (radianAngle.Y, radianAngle.X, radianAngle.Z));

            Matrix world = Matrix.Identity;
            world *= Matrix.Invert(Matrix.CreateTranslation(pivot));
            world *= Matrix.CreateScale(scale);
            world *= Matrix.CreateFromQuaternion(rotQuaternion);
            world *= Matrix.CreateTranslation(pivot);
            world *= Matrix.CreateTranslation(translate);

            return world;

        }
    }


}
