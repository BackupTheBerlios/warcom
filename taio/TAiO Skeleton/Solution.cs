using System;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    class Solution
    {
        private Rectangle rectangle;
        private string tag ="";
        private bool correct;
        private string info;

        public string Info
        {
            get { return info; }
            set { info = value; }
        }

        public bool Correct
        {
            get { return correct; }
            set { correct = value; }
        }

        public Solution()
        {
            correct = true;
        }

        public Solution(string tag, Rectangle rectangle) : this()
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
