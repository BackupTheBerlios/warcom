using System;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    class Solution
    {
        private Rectangle rectangle;
        private string tag;

        public Solution()
        {
        }

        public Solution(string tag, Rectangle rectangle)
        {
            this.tag = tag;
            this.rectangle = rectangle;
        }

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
    }
}
