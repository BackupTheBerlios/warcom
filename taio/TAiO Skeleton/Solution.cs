using System;
using System.Collections.Generic;
using System.Text;

namespace Taio
{
    //klasa ktora przechowuje rozwiazania
    class Solution
    {
        //prostokat bedacy rozwiazaniem
        private Rectangle rectangle;
        //tag rozwiazania
        private string tag ="";
        //czy ten prostokat jest prawidlowym prostokatem
        private bool correct;
        //informacje zaczytane z pliku
        private string info=null;
        //czas wykonania algorytmu - ustawiane przez program
        private TimeSpan ts;
        //informacje o rozwiazaniu
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
