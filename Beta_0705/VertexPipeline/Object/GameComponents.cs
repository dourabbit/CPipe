
#region File Description
//-----------------------------------------------------------------------------
// ModelViewerControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;


#endregion

namespace VertexPipeline
{
     public class GameComponents<T> : List<T>
        where T : IUpdatableComponent
    {
        Scene _game;

        public GameComponents(Scene game):base()
        {
            _game= game;
        }
        public  void Add(IUpdatableComponent comp)
        {
            if (comp is IDrawableComponent)
            {
                //if (_game.SysEvnHandler != null)
                //    _game.SysEvnHandler.Invoke(comp, SYSEVN.New);

                new SysEvn(0, comp, OBJTYPE.Building, SYSEVN.New, null);
            }

            base.Add((T)comp);
        }
         /// <summary>
         /// Get name recursively
         /// </summary>
         /// <param name="name"></param>
         /// <param name="output"></param>
        public void GetNm(string name, out string output)
        {
            T result=
            this.Find(delegate(T matcher)
            {
                return
                (matcher.ID == name) ?
                true : false;
            });

            if (result != null)
            {
                int ending = 0;

                string[] buffer=result.ID.Split('_');
                if (buffer.Length!=1&&
                    buffer[buffer.Length-1]!=String.Empty&&
                    buffer[buffer.Length - 1] != null)
                {
                    try
                    {
                        int digits = Convert.ToInt32(buffer[buffer.Length - 1]);
                        ending = digits + 1;
                        //name = buffer[0];
                        name = "";
                        for (int i = 0; i < buffer.Length-1; i++)
                            if(buffer[i]!=string.Empty)
                                name += '_'+ buffer[i];
                    }
                    catch
                    {
                        //output = result.ID + '_' + ending.ToString();
                        name = result.ID;
                    }
                }


                GetNm(name+'_'+ending.ToString(), out output );
            }
            else
            {
                output = name;
                return ;
            }
        
        }
    }

 

}
