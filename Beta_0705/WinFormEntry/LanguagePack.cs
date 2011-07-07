#region File Description
//-----------------------------------------------------------------------------
// Program.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using XNASysLib.XNAKernel;
using XNABuilder;
using Microsoft.Xna.Framework.Content;
using XNASysLib.Primitives3D;
#endregion

namespace WinFormsContentLoading
{
    public static class LanguagePack 
    {
        static string[] pipe = new string[]
        {
            "名称",
            "东西位置-x",
            "南北位置-y",
            "深度-z",
            "俯仰旋转角度-x",
            "水平旋转角度-y",
            "滚动旋转角度-z",
            "管径",
            "材质",
            "颜色"
        };
        static string[] commPipe = new string[]
        {
            "名称",
             "东西位置-x",
             "南北位置-y",
             "深度-z",
             "俯仰旋转角度-x",
            "水平旋转角度-y",
             "滚动旋转角度-z",
             "截面长度",
              "截面宽度",	
             "管长",
             "孔的数目",
             "颜色"
        };

        static string[] valve = new string[]
        {
            "名称",
            "东西位置-x",
            "南北位置-y",
            "深度-z",
            "俯仰旋转角度-x",
            "水平旋转角度-y",
            "滚动旋转角度-z",
            "颜色"
        };

        static string[] chamber = new string[]
        {
           "东西位置-x",
            "南北位置-y",
            "深度-z",
            "俯仰旋转角度-x",
            "水平旋转角度-y",
            "滚动旋转角度-z",
            "颜色"
        
        };
        static string[] well = new string[]
        {
           "东西位置-x",
            "南北位置-y",
            "深度-z",
            "俯仰旋转角度-x",
            "水平旋转角度-y",
            "滚动旋转角度-z",
            "直径",
            "深度",
            "颜色"
        
        };
        static string[] holeRect = new string[]
        {
           "东西位置-x",
            "南北位置-y",
            "深度-z",
            "俯仰旋转角度-x",
            "水平旋转角度-y",
            "滚动旋转角度-z",
            "长",
            "宽",
            "高",
            "颜色"
        
        };
        static string[] holeEllipse = new string[]
        {
            "东西位置-x",
            "南北位置-y",
            "深度-z",
            "俯仰旋转角度-x",
            "水平旋转角度-y",
            "滚动旋转角度-z",
            "长半轴",
            "短半轴",
            "扁率",
            "颜色"
        
        };


        /// <summary>
        /// Get Language 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetNm(Type type, int index)
        {

            if (type == typeof(Pipe))
                return pipe[index];
            else if (type == typeof(Valve))
                return valve[index];
            else if (type == typeof(Chamber))
                return chamber[index];
            else if (type == typeof(CommPipe))
                return commPipe[index];
            else if (type == typeof(HoleEllipse))
                return holeEllipse[index];
            else if (type == typeof(HoleRect))
                return holeRect[index];
            else if (type == typeof(Well))
                return well[index];

            return "";
        }
    
    
    
    }
}
