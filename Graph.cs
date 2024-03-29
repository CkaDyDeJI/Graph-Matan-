﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;


namespace Graph
{
    internal class Graph
    {
        private int count_;
        private int[,] futherGraph_ = new int[8, 8];
        private List <int> queenNumbers = new List <int>();
        private List <List <int>> removedQueens = new List <List <int>>
        {
            new List <int>(), new List <int>(), new List <int>(), new List <int>(), 
            new List <int>(), new List <int>(), new List <int>(), new List <int>()
        };

        private readonly List <Vertex> mainArray_;
        private int previousCount_;


        public Graph()
        {
            mainArray_ = new List <Vertex>();
            count_ = 0;
        }


        public List <int> bfs(int minOrMax, int startFrom)
        {
            queenNumbers.Clear ();

            Queue<Vertex> mainQueue = new Queue<Vertex> ();
            mainQueue.Enqueue ( mainArray_[startFrom] );
            tryQueen( mainArray_[mainQueue.Peek ().getNumber () - 1]);

            while (true) {
                for (int i = 0; i < queenNumbers.Count; i++)
                    buildGraph ( queenNumbers[i] );

                for (int i = 0; i < mainArray_.Count; i++)
                {
                    if (!queenNumbers.Contains ( i ))
                        mainArray_[i].IsVisited = false;
                }

                if (queenNumbers.Count != minOrMax || checkFullnes() == false) {
                    if (queenNumbers.Count > 1)
                    {
                        mainQueue.Enqueue ( mainArray_[queenNumbers.Last ()] );
                        removedQueens[queenNumbers.Count - 1].Add ( queenNumbers.Last () );
                        queenNumbers.RemoveAt ( queenNumbers.Count - 1 );
                    }

                    while (mainQueue.Count != 0)
                    {
                        Vertex tempOne = mainQueue.Dequeue ();
                        tempOne.IsVisited = true;
                        for (int i = 0; i < tempOne.getList ().Count; i++)
                        {
                            if (tempOne.getList ()[i] != 0 && mainArray_[i].IsVisited == false)
                            {
                                mainQueue.Enqueue ( mainArray_[i] );
                                mainArray_[i].IsVisited = true;
                            }
                        }

                        for (int i = 0; i < 8; i++)
                        {
                            if (removedQueens[i].Contains ( tempOne.getNumber () - 1 ))
                                break;

                            if (i == 7)
                                tryQueen ( tempOne );
                        }
                    }

                    for (int i = queenNumbers.Count; i < 8; i++)
                    {
                        removedQueens[i].Clear ();
                    }

                    clearMatrix();
                } else
                    break;
            }

            clearMatrix ();
            clearRemovedQueens();
            clearAllVisited ();
            return queenNumbers;
        }


        private void clearRemovedQueens()
        {
            for (int i = 0; i < removedQueens.Count; i++)
            {
                removedQueens[i].Clear();
            }
        }


        private bool checkFullnes()
        {
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    if (futherGraph_[i, j] == 0)
                        return false;
                }
            }

            return true;
        }


        private void clearMatrix()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++) {
                    futherGraph_[i, j] = 0;
                }
            }
        }


        private void buildGraph (int numberOfVertex)
        {
            for (int i = 0; i < 8; i++) {
                futherGraph_[i, numberOfVertex % 8] = 1;
            }

            for (int i = 0; i < 8; i++) {
                futherGraph_[numberOfVertex / 8, i] = 1;
            }

            int row = numberOfVertex / 8;
            int column = numberOfVertex % 8;
            while (row >= 0 && column >= 0) {
                futherGraph_[row, column] = 1;
                row--;
                column--;
            }

            row = numberOfVertex / 8;
            column = numberOfVertex % 8;
            while (row <= 7 && column >= 0)
            {
                futherGraph_[row, column] = 1;
                row++;
                column--;
            }

            row = numberOfVertex / 8;
            column = numberOfVertex % 8;
            while (row <= 7 && column <= 7)
            {
                futherGraph_[row, column] = 1;
                row++;
                column++;
            }

            row = numberOfVertex / 8;
            column = numberOfVertex % 8;
            while (row >= 0 && column <= 7)
            {
                futherGraph_[row, column] = 1;
                row--;
                column++;
            }
        }
        //public List <int> bfs()
        //{
        //    queenNumbers.Clear();

        //    Queue <Vertex> mainQueue = new Queue <Vertex>();
        //    mainQueue.Enqueue(mainArray_[0]);
        //    queenNumbers.Add (mainQueue.Peek().getNumber() - 1);

        //    while (true) {
        //        for (int i = 0; i < mainArray_.Count; i++)
        //        {
        //            if (!queenNumbers.Contains (i))
        //                mainArray_[i].IsVisited = false;
        //        }

        //        if (queenNumbers.Count != 8) {
        //            if (queenNumbers.Count > 1)
        //            {
        //                mainQueue.Enqueue ( mainArray_[queenNumbers.Last ()] );
        //                removedQueens[queenNumbers.Count - 1].Add ( queenNumbers.Last () );
        //                queenNumbers.RemoveAt ( queenNumbers.Count - 1 );
        //            }

        //            while (mainQueue.Count != 0) {
        //                Vertex tempOne = mainQueue.Dequeue ();
        //                tempOne.IsVisited = true;
        //                for (int i = 0; i < tempOne.getList ().Count; i++)
        //                {
        //                    if (tempOne.getList()[i] != 0 && mainArray_[i].IsVisited == false) {
        //                        mainQueue.Enqueue ( mainArray_[i] );
        //                        mainArray_[i].IsVisited = true;
        //                    } 
        //                }

        //                for (int i = 0; i < 8; i++) {
        //                    if (removedQueens[i].Contains ( tempOne.getNumber () - 1 ))
        //                        break;

        //                    if (i == 7)
        //                        tryQueen ( tempOne );
        //                }
        //            }

        //            for (int i = queenNumbers.Count; i < 8; i++) {
        //                removedQueens[i].Clear();
        //            }
        //        } else
        //            break;
        //    }

        //    clearAllVisited();
        //    return  queenNumbers;
        //}


        private void clearAllVisited()
        {
            for (int i = 0; i < count_; i++) {
                mainArray_[i].IsVisited = false;
            }
        }


        private bool tryQueen(Vertex whereToTryVertex)
        {
            if (hasNeighbor (whereToTryVertex) == false) {
                //whereToTryVertex.setQueen ();
                queenNumbers.Add (whereToTryVertex.getNumber() - 1);
                buildGraph (whereToTryVertex.getNumber() - 1);

                return true;
            } else {
                return  false;
            }
                
        }


        private bool hasNeighbor(Vertex whereNeighbor)
        {
            for (int i = 0; i < queenNumbers.Count; i++) {
                if (whereNeighbor.getList ()[queenNumbers[i]] == 1)
                    return true;
            }

            return  false;
        }


        public Vertex this [int index] => mainArray_[index];


        public void buildGraph (string fileName)
        {
            //mainArray_ = new List<Vertex>();
            string tempString;
            List <int> newList;
            previousCount_ = count_;

            using (var reader = new StreamReader (fileName)) {
                if (count_ == 0)
                    while (true) {
                        tempString = reader.ReadLine();

                        if (tempString == null)
                            break;

                        newList = fromArrayToList (tempString.Split (' '));
                        //Vertex newOne = new Vertex(count_ + 1, newList);
                        addNewVertex (newList);
                    }
                else
                    while (true) {
                        tempString = reader.ReadLine();

                        if (tempString == null)
                            break;

                        newList = fromArrayToList (tempString.Split (' '));
                        //Vertex newOne = new Vertex(count_ + 1, newList);
                        addNewVertex (newList);
                    }
            }
        }


        public void addNewVertex (List <int> newList)
        {
            var finalList = new List <int>();
            for (var i = 0; i < previousCount_; i++) finalList.Insert (0, 0);
            for (var i = 0; i < newList.Count; i++) finalList.Add (newList[i]);

            var newOne = new Vertex (count_ + 1, finalList);
            mainArray_.Add (newOne);
            for (var i = 0; i < previousCount_; i++) mainArray_[i].addLastToList (0);
            count_++;
        }


        public void addNewVertex()
        {
            var newList = new List <int>();
            for (var i = 0; i < count_ + 1; i++) newList.Add (0);
            var newOne = new Vertex (count_ + 1, newList);
            mainArray_.Add (newOne);
            for (var i = 0; i < count_; i++) mainArray_[i].addLastToList (0);
            count_++;
        }


        public void addNewPath (Vertex begin, Vertex end, int distance)
        {
            var tempList = begin.getList();
            tempList[end.getNumber() - 1] = distance;
            begin.setList (tempList);

            tempList = end.getList();
            tempList[begin.getNumber() - 1] = distance;
            end.setList (tempList);
        }


        public void deleteVertex (int vertexNumber)
        {
            mainArray_.RemoveAt (vertexNumber);
            count_--;

            var tempList = new List <int>();
            for (var i = 0; i < count_; i++) {
                mainArray_[i].setNumber (i + 1);
                tempList = mainArray_[i].getList();
                tempList.RemoveAt (vertexNumber);
                mainArray_[i].setList (tempList);
            }
        }


        public void deleteAll()
        {
            mainArray_.Clear();
            count_ = 0;
        }


        private List <int> fromArrayToList (string[] stringArray)
        {
            var tempList = new List <int>();
            for (var i = 0; i < stringArray.Length; i++) tempList.Add (Convert.ToInt32 (stringArray[i]));

            return tempList;
        }


        public void outputMatrixInFile (string fileName)
        {
            var tempString = "";
            if (count_ > 0)
                using (var writer = new StreamWriter (fileName, false)) {
                    var newList = new List <int>();
                    for (var i = 0; i < count_; i++) {
                        newList = mainArray_[i].getList();
                        for (var j = 0; j < count_; j++) tempString += newList[j] + " ";
                        writer.WriteLine (tempString);
                        tempString = "";
                    }
                }
        }


        public List <Vertex> getArray()
        {
            return mainArray_;
        }


        public int getCount()
        {
            return count_;
        }


        public Vertex index (int itemNumber)
        {
            if (count_ == 0) {
                var newList = new List <int> (1) {0};
                var newOne = new Vertex (1, newList);

                return newOne;
            }

            return mainArray_[itemNumber];
        }
    }
}