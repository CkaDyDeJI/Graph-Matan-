using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.VisualBasic;


namespace Graph
{
    public partial class Form1 : Form
    {
        private readonly List <int> commonList = new List <int>();
        public string fileName;
        private bool forButton3;
        private bool forButton4;
        private bool forButton5;
        private bool forButton7;
        private Graphics graphics;
        private readonly Brush mainBrush = new SolidBrush (Color.Black);
        private Font mainFont;
        private readonly Graph mainGraph = new Graph();
        private readonly Pen mainPen = new Pen (Color.Black, 1);
        private Bitmap map;
        private int mouseX;
        private int mouseY;
        private readonly Pen redPen = new Pen (Color.Red, 2);
        private int selectedCount  ;
        private int vertexRadious = 10 + 50 / 5;
        private readonly Brush whiteBrush = new SolidBrush (Color.White);

        private Graphics anotherOne_;
        private Bitmap anotherBitmap_;
        private Image mainImage_ = Image.FromFile ( "crown_PNG23872.png" );


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load (object sender, EventArgs e)
        {
            map = new Bitmap (pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage (map);

            anotherBitmap_ = new Bitmap (pictureBox2.Width, pictureBox2.Height);
            anotherOne_ = Graphics.FromImage (anotherBitmap_);
        }


        private void Button1_Click (object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            fileName = openFileDialog1.FileName;
            mainGraph.buildGraph (fileName);

            button10.Enabled = true;
            button9.Enabled = true;
        }


        private void Button2_Click (object sender, EventArgs e)
        {
            setAllFalse();
            drawGraph();
            pictureBox1.Image = map;
        }


        private void drawGraph()
        {
            graphics.Clear (Color.White);
            if (mainGraph.getCount() < 5)
                vertexRadious = 10 + 50 / 5;
            else
                vertexRadious = 10 + 50 / mainGraph.getCount();

            mainFont = new Font ("Arial", vertexRadious / 2);
            var varForAngle = 0;
            var circleRadious = 500;
            var centerX = pictureBox1.Width / 2;
            var centerY = pictureBox1.Height / 2;
            int topLeftX;
            int topLeftY;

            var pen1 = new Pen (Color.Black, 3);

            for (var i = 0; i < mainGraph.getCount(); i++) {
                topLeftX = Convert.ToInt32 (
                    centerX + circleRadious * Math.Cos (varForAngle * Math.PI / mainGraph.getCount()) - vertexRadious);
                topLeftY = Convert.ToInt32 (
                    centerY + circleRadious * Math.Sin (varForAngle * Math.PI / mainGraph.getCount()) - vertexRadious);

                drawVertex (topLeftX, topLeftY, mainGraph.index (i).getNumber() - 1);
                varForAngle += 2;
            }

            for (var i = 0; i < mainGraph.getCount(); i++) drawLinesBetweenVertexes (mainGraph.index (i));
            for (var i = 0; i < mainGraph.getCount(); i++)
                drawVertex (mainGraph.index (i).getX() - vertexRadious, mainGraph.index (i).getY() - vertexRadious,
                    mainGraph.index (i).getNumber() - 1);
        }


        private void drawVertex (int topLeftX, int topLeftY, int vertexNumber)
        {
            if (mainGraph.getCount() < 5)
                vertexRadious = 10 + 50 / 5;
            else
                vertexRadious = 10 + 50 / mainGraph.getCount();

            mainFont = new Font ("Arial", vertexRadious / 2);
            mainGraph.index (vertexNumber).setCoordinates (topLeftX + vertexRadious, topLeftY + vertexRadious);

            graphics.DrawEllipse (mainPen, topLeftX, topLeftY, vertexRadious * 2, vertexRadious * 2);
            graphics.DrawString (Convert.ToString (mainGraph.index (vertexNumber).getNumber()), mainFont, mainBrush,
                topLeftX + vertexRadious - mainFont.GetHeight() / 2,
                topLeftY + vertexRadious - mainFont.GetHeight() / 2);
        }


        private void drawLinesBetweenVertexes (Vertex newVertex)
        {
            var newList = newVertex.getList();
            for (var i = 0; i < newList.Count; i++)
                if (newList[i] != 0) {
                    graphics.DrawLine (mainPen, newVertex.getX(), newVertex.getY(), mainGraph.index (i).getX(),
                        mainGraph.index (i).getY());
                    graphics.FillEllipse (whiteBrush, newVertex.getX() - vertexRadious - 2,
                        newVertex.getY() - vertexRadious - 2, (vertexRadious + 2) * 2, (vertexRadious + 2) * 2);
                    graphics.FillEllipse (whiteBrush, mainGraph.index (i).getX() - vertexRadious,
                        mainGraph.index (i).getY() - vertexRadious, vertexRadious * 2, vertexRadious * 2);
                    graphics.DrawString (Convert.ToString (newVertex.getList()[i]), mainFont, mainBrush,
                        (newVertex.getX() + mainGraph.index (i).getX()) / 2,
                        (newVertex.getY() + mainGraph.index (i).getY()) / 2);
                }
        }


        private void drawLineBetweenTwoNew (Vertex one, Vertex two, string distance)
        {
            redrawGraph();
            graphics.DrawLine (mainPen, one.getX(), one.getY(), two.getX(), two.getY());
            graphics.FillEllipse (whiteBrush, one.getX() - vertexRadious - 2, one.getY() - vertexRadious - 2,
                (vertexRadious + 2) * 2, (vertexRadious + 2) * 2);
            graphics.FillEllipse (whiteBrush, two.getX() - vertexRadious - 2, two.getY() - vertexRadious - 2,
                (vertexRadious + 2) * 2, (vertexRadious + 2) * 2);
            graphics.DrawString (distance, mainFont, mainBrush,
                (one.getX() + two.getX()) / 2, (one.getY() + two.getY()) / 2);

            drawVertex (one.getX() - vertexRadious, one.getY() - vertexRadious, one.getNumber() - 1);
            drawVertex (two.getX() - vertexRadious, two.getY() - vertexRadious, two.getNumber() - 1);
            mainGraph.addNewPath (mainGraph.index (one.getNumber() - 1), mainGraph.index (two.getNumber() - 1),
                Convert.ToInt32 (distance));
        }


        private void drawSelectedVertex (Vertex chosenOne)
        {
            graphics.FillEllipse (whiteBrush, chosenOne.getX() - vertexRadious - 2,
                chosenOne.getY() - vertexRadious - 2, (vertexRadious + 2) * 2, (vertexRadious + 2) * 2);
            graphics.DrawEllipse (redPen, chosenOne.getX() - vertexRadious, chosenOne.getY() - vertexRadious,
                vertexRadious * 2, vertexRadious * 2);
            graphics.DrawString (Convert.ToString (chosenOne.getNumber()), mainFont, mainBrush,
                chosenOne.getX() - mainFont.GetHeight() / 2, chosenOne.getY() - mainFont.GetHeight() / 2);
        }


        private void drawLineBetweenSelected (Vertex one, Vertex two)
        {
            graphics.DrawLine (redPen, one.getX(), one.getY(), two.getX(), two.getY());
            graphics.FillEllipse (whiteBrush, one.getX() - vertexRadious - 2, one.getY() - vertexRadious - 2,
                (vertexRadious + 2) * 2, (vertexRadious + 2) * 2);
            graphics.FillEllipse (whiteBrush, two.getX() - vertexRadious - 2, two.getY() - vertexRadious - 2,
                (vertexRadious + 2) * 2, (vertexRadious + 2) * 2);
            drawSelectedVertex (one);
            drawSelectedVertex (two);
        }


        private void drawNeighbors (Vertex main)
        {
            drawSelectedVertex (main);
            var tempList = main.getList();
            for (var i = 0; i < tempList.Count; i++)
                if (tempList[i] != 0)
                    drawLineBetweenSelected (main, mainGraph.index (i));
        }


        private void redrawGraph()
        {
            graphics.Clear (Color.White);
            for (var i = 0; i < mainGraph.getCount(); i++) drawLinesBetweenVertexes (mainGraph.index (i));

            for (var i = 0; i < mainGraph.getCount(); i++)
                drawVertex (mainGraph.index (i).getX() - vertexRadious, mainGraph.index (i).getY() - vertexRadious,
                    mainGraph.index (i).getNumber() - 1);
        }


        private void PictureBox1_MouseClick (object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            if (forButton3) {
                var tempInt = checkSelecting (mouseX, mouseY);
                if (tempInt == -1) {
                    mainGraph.addNewVertex();
                    mainGraph.index (mainGraph.getCount() - 1).setCoordinates (mouseX, mouseY);
                    drawVertex (mouseX - vertexRadious, mouseY - vertexRadious,
                        mainGraph.index (mainGraph.getCount() - 1).getNumber() - 1);
                }

                pictureBox1.Image = map;
            } else {
                if (forButton4) {
                    var tempInt = checkSelecting (mouseX, mouseY);
                    if (tempInt != -1) {
                        commonList.Add (tempInt);
                        drawSelectedVertex (mainGraph.index (tempInt - 1));
                        selectedCount++;
                    }

                    if (selectedCount == 2) {
                        var tempString = Interaction.InputBox ("Enter distance");
                        drawLineBetweenTwoNew (mainGraph.index (commonList[0] - 1), mainGraph.index (commonList[1] - 1),
                            tempString);
                        selectedCount = 0;
                        commonList.Clear();
                    }

                    pictureBox1.Image = map;
                } else {
                    if (forButton5) {
                        var tempInt = checkSelecting (mouseX, mouseY);
                        if (tempInt != -1) {
                            mainGraph.deleteVertex (tempInt - 1);
                            redrawGraph();
                        }

                        pictureBox1.Image = map;
                    } else {
                        if (forButton7) {
                            var tempInt = checkSelecting (mouseX, mouseY);
                            if (tempInt != -1) {
                                redrawGraph();
                                drawNeighbors (mainGraph.index (tempInt - 1));
                            } else {
                                redrawGraph();
                            }

                            pictureBox1.Image = map;
                        }
                    }
                }
            }
        }


        private int checkSelecting (int mouseX, int mouseY)
        {
            var newList = new List <int>();

            for (var i = 0; i < mainGraph.getCount(); i++)
                if (mouseX >= mainGraph.index (i).getX() - vertexRadious && mouseX <=
                                                                         mainGraph.index (i).getX() + vertexRadious
                                                                         && mouseY >= mainGraph.index (i).getY() -
                                                                         vertexRadious &&
                                                                         mouseY <= mainGraph.index (i).getY() +
                                                                         vertexRadious)
                    return mainGraph.index (i).getNumber();

            return -1;
        }


        private void Button3_Click (object sender, EventArgs e)
        {
            redrawGraph();
            pictureBox1.Image = map;
            forButton3 = true;
            forButton4 = false;
            forButton5 = false;
            forButton7 = false;
        }


        private void Button4_Click (object sender, EventArgs e)
        {
            redrawGraph();
            pictureBox1.Image = map;
            forButton3 = false;
            forButton4 = true;
            forButton5 = false;
            forButton7 = false;
        }


        private void Button5_Click (object sender, EventArgs e)
        {
            redrawGraph();
            pictureBox1.Image = map;
            forButton3 = false;
            forButton4 = false;
            forButton5 = true;
            forButton7 = false;
        }


        private void Button6_Click (object sender, EventArgs e)
        {
            clearAll();
        }


        private void Button7_Click (object sender, EventArgs e)
        {
            forButton3 = false;
            forButton4 = false;
            forButton5 = false;
            forButton7 = true;
        }


        private void Button8_Click (object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            fileName = saveFileDialog1.FileName;
            mainGraph.outputMatrixInFile (fileName);
        }


        private void clearAll()
        {
            var result = MessageBox.Show ("Are you sure that you want to clear all of this?",
                "You are to delete all elemets of the graph", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                graphics.Clear (Color.White);
                mainGraph.deleteAll();
                pictureBox1.Image = map;
            }
        }


        private void clearAllWithoutWarning()
        {
            graphics.Clear (Color.White);
            pictureBox1.Image = map;
        }


        private void setAllFalse()
        {
            forButton3 = false;
            forButton4 = false;
            forButton5 = false;
            forButton7 = false;
        }


        private void button10_Click (object sender, EventArgs e)
        {
            int startForm = Convert.ToInt32(Interaction.InputBox ("enter start position (0 - 63)"));

            anotherOne_.Clear ( Color.Transparent );

            List<int> tempOne = mainGraph.bfs (8, startForm);
            for (int i = 0; i < tempOne.Count; i++)
                anotherOne_.DrawImage ( mainImage_, 100 * (tempOne[i] % 8), 100 * (tempOne[i] / 8));

            pictureBox2.Image = anotherBitmap_;
        }

        private void button9_Click (object sender, EventArgs e)
        {
            int startForm = Convert.ToInt32 ( Interaction.InputBox ("enter start position (0 - 63)"));

            anotherOne_.Clear (Color.Transparent);

            List <int> tempOne = mainGraph.bfs (5, startForm);
            for (int i = 0; i < tempOne.Count; i++)
                anotherOne_.DrawImage ( mainImage_, 100 * (tempOne[i] % 8), 100 * (tempOne[i] / 8));

            pictureBox2.Image = anotherBitmap_;
        }
    }
}