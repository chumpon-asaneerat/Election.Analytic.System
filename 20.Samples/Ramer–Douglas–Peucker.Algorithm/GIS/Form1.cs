using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GIS
{
    public partial class Form1 : Form
    {
        private Boolean _calculateDouglasPeuckerReduction = false;
        private List<GIS.Point> _amerenPoints = new List<GIS.Point>();

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            List<System.Drawing.Point> drawingPoints = new List<System.Drawing.Point>();
            foreach (GIS.Point point in _amerenPoints)
            {
                drawingPoints.Add(new System.Drawing.Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y)));
            }
            if (drawingPoints.Count > 2)
            {
                e.Graphics.DrawLines(new Pen(Brushes.Black, 2), drawingPoints.ToArray());
                lblOriginal.Text = drawingPoints.Count.ToString();

                if (_calculateDouglasPeuckerReduction)
                {
                    List<GIS.Point> points = Utility.DouglasPeuckerReduction(_amerenPoints, Convert.ToDouble(nudTolerance.Value));

                    drawingPoints = new List<System.Drawing.Point>();
                    foreach (GIS.Point point in points)
                    {
                        drawingPoints.Add(new System.Drawing.Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y)));
                    }

                    e.Graphics.DrawLines(new Pen(Brushes.Red, 2), drawingPoints.ToArray());
                    lblSimplified.Text = drawingPoints.Count.ToString();
                }
            }

            base.OnPaint(e);

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _amerenPoints.Add(new GIS.Point(e.X, e.Y));
                this.Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //DO the calculation
            _calculateDouglasPeuckerReduction = true;
            this.Invalidate();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _amerenPoints.Clear();
                _calculateDouglasPeuckerReduction = false;
            }
        }

        private void nudTolerance_ValueChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}