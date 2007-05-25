using System;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    /// <summary>
    ///klasa ktora przechowuje rozwiazania
    /// </summary>
    class Solution
    {
        /// <summary>
        ///prostokat bedacy rozwiazaniem
        /// </summary>
        private Rectangle rectangle;
        /// <summary>
        ///tag rozwiazania
        /// </summary>
        private string tag ="";
        /// <summary>
        ///czy ten prostokat jest prawidlowym prostokatem
        /// </summary>
        private bool correct;
        /// <summary>
        ///informacje zaczytane z pliku
        /// </summary>
        private string info=null;
        /// <summary>
        ///czas wykonania algorytmu - ustawiane przez program
        /// </summary>
        private TimeSpan ts;
        /// <summary>
        ///informacje o rozwiazaniu
        /// </summary>
        private String solutionInfo;

        public TimeSpan Ts
        {
            get { return ts; }
            set { ts = value; }
        }
        

        public String SolutionInfo
        {
            get { return solutionInfo; }
            set { solutionInfo = value; }
        }

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
