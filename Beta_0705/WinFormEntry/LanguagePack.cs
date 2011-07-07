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
            "����",
            "����λ��-x",
            "�ϱ�λ��-y",
            "���-z",
            "������ת�Ƕ�-x",
            "ˮƽ��ת�Ƕ�-y",
            "������ת�Ƕ�-z",
            "�ܾ�",
            "����",
            "��ɫ"
        };
        static string[] commPipe = new string[]
        {
            "����",
             "����λ��-x",
             "�ϱ�λ��-y",
             "���-z",
             "������ת�Ƕ�-x",
            "ˮƽ��ת�Ƕ�-y",
             "������ת�Ƕ�-z",
             "���泤��",
              "������",	
             "�ܳ�",
             "�׵���Ŀ",
             "��ɫ"
        };

        static string[] valve = new string[]
        {
            "����",
            "����λ��-x",
            "�ϱ�λ��-y",
            "���-z",
            "������ת�Ƕ�-x",
            "ˮƽ��ת�Ƕ�-y",
            "������ת�Ƕ�-z",
            "��ɫ"
        };

        static string[] chamber = new string[]
        {
           "����λ��-x",
            "�ϱ�λ��-y",
            "���-z",
            "������ת�Ƕ�-x",
            "ˮƽ��ת�Ƕ�-y",
            "������ת�Ƕ�-z",
            "��ɫ"
        
        };
        static string[] well = new string[]
        {
           "����λ��-x",
            "�ϱ�λ��-y",
            "���-z",
            "������ת�Ƕ�-x",
            "ˮƽ��ת�Ƕ�-y",
            "������ת�Ƕ�-z",
            "ֱ��",
            "���",
            "��ɫ"
        
        };
        static string[] holeRect = new string[]
        {
           "����λ��-x",
            "�ϱ�λ��-y",
            "���-z",
            "������ת�Ƕ�-x",
            "ˮƽ��ת�Ƕ�-y",
            "������ת�Ƕ�-z",
            "��",
            "��",
            "��",
            "��ɫ"
        
        };
        static string[] holeEllipse = new string[]
        {
            "����λ��-x",
            "�ϱ�λ��-y",
            "���-z",
            "������ת�Ƕ�-x",
            "ˮƽ��ת�Ƕ�-y",
            "������ת�Ƕ�-z",
            "������",
            "�̰���",
            "����",
            "��ɫ"
        
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
