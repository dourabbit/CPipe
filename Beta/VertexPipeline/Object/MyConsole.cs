#region File Description
//-----------------------------------------------------------------------------
// BloomComponent.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
#endregion

namespace VertexPipeline
{
    public delegate void OutputHandler(string[] output);
    public class MyConsole
    {
        public static OutputHandler OnOutput;
        static int lineCount = 0;
        static Queue<string> _output=new Queue<string>();
        public static string Output
        {
            get { return _output.Peek(); }
            set 
            { 
                _output.Enqueue( value);
                if (_output.Count > 3)
                    _output.Dequeue();

                if (OnOutput != null)
                    OnOutput.Invoke(_output.ToArray());
            }
        }
        public static void WriteLine(string output)
        {

            Output = Interlocked.Increment(ref lineCount).ToString()+": "+ output;
        }
        
    }
    
}
