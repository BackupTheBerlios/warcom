using System;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    class Solution
    {
        private List<Rectangle> rectangles;
        private string tag;

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public List<Rectangle> Rectangles
        {
            get { return rectangles; }
            set { rectangles = value; }
        }
    }
}
