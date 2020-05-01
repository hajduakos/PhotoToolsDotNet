﻿using System.Drawing;

namespace FilterScript.Model
{
    interface ITask
    {
        public Bitmap Execute();
        public void Clear();
    }
}
