using System.Collections.Generic;


namespace Graph
{
    internal class Vertex
    {
        private List <int> adjacencyList_;
        private int coordinateX_  ;
        private int coordinateY_;
        private bool isVisited_;

        private int numberOfVertex_;


        public bool IsVisited
        {
            get { return isVisited_; }
            set { isVisited_ = value; }
        }


        public Vertex (int number, List <int> newList)
        {
            numberOfVertex_ = number;
            adjacencyList_ = newList;
        }


        public void addLastToList (int value)
        {
            adjacencyList_.Add (value);
        }


        public int getNumber()
        {
            return numberOfVertex_;
        }


        public void setCoordinates (int x, int y)
        {
            coordinateX_ = x;
            coordinateY_ = y;
        }


        public int getX()
        {
            return coordinateX_;
        }


        public int getY()
        {
            return coordinateY_;
        }


        public List <int> getList()
        {
            return adjacencyList_;
        }


        public void setList (List <int> newList)
        {
            adjacencyList_ = newList;
        }


        public void setNumber (int newNumber)
        {
            numberOfVertex_ = newNumber;
        }
    }
}