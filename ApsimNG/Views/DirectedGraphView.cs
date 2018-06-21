﻿// -----------------------------------------------------------------------
// <copyright file="DirectedGraphView.cs" company="APSIM Initiative">
//     Copyright (c) APSIM Initiative
// </copyright>
// -----------------------------------------------------------------------
namespace UserInterface.Views
{
    using ApsimNG.Classes.DirectedGraph;
    using Cairo;
    using Gtk;
    using Models.Graph;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A view that contains a graph and click zones for the user to allow
    /// editing various parts of the graph.
    /// </summary>
    public class DirectedGraphView : ViewBase
    {
        private DGObject selectedObject;
        private bool mouseDown = false;
        private DrawingArea drawable;
        private PointD lastPos;
        private List<DGNode> Nodes = new List<DGNode>();
        private List<DGArc> Arcs = new List<DGArc>();

        /// <summary>Initializes a new instance of the <see cref="DirectedGraphView" /> class.</summary>
        public DirectedGraphView(ViewBase owner = null) : base(owner)
        {
            drawable = new DrawingArea();
            
            drawable.AddEvents(
            (int)Gdk.EventMask.PointerMotionMask
            | (int)Gdk.EventMask.ButtonPressMask
            | (int)Gdk.EventMask.ButtonReleaseMask);

            drawable.ExposeEvent += OnDrawingAreaExpose;
            drawable.ButtonPressEvent += OnMouseButtonPress;
            drawable.ButtonReleaseEvent += OnMouseButtonRelease;
            drawable.MotionNotifyEvent += OnMouseMove;

            ScrolledWindow scroller = new ScrolledWindow(new Adjustment(0, 0, 100, 1, 1, 1), new Adjustment(0, 0, 100, 1, 1, 1))
            {
                HscrollbarPolicy = PolicyType.Always,
                VscrollbarPolicy = PolicyType.Always
            };
            
            scroller.AddWithViewport(drawable);

            _mainWidget = scroller;
            drawable.ModifyBg(StateType.Normal, new Gdk.Color(255, 255, 255));
        }

        /// <summary>The description (nodes & arcs) of the directed graph.</summary>
        public DirectedGraph DirectedGraph 
        {
            get
            {
                DirectedGraph graph = new DirectedGraph();
                Nodes.ForEach(node => graph.Nodes.Add(node.ToNode()));
                Arcs.ForEach(arc => graph.Arcs.Add(arc.ToArc()));
                return graph;
            }
            set
            {
                value.Nodes.ForEach(node => Nodes.Add(new DGNode(node)));
                value.Arcs.ForEach(arc => Arcs.Add(new DGArc(arc, Nodes)));
            }
        }

        /// <summary>Export the view to the image</summary>
        public System.Drawing.Image Export()
        {
            int width;
            int height;
            MainWidget.GdkWindow.GetSize(out width, out height);
            Gdk.Pixbuf screenshot = Gdk.Pixbuf.FromDrawable(MainWidget.GdkWindow, MainWidget.Colormap, 0, 0, 0, 0, width, height);
            byte[] buffer = screenshot.SaveToBuffer("png");
            MemoryStream stream = new MemoryStream(buffer);
            System.Drawing.Bitmap bitmap = new Bitmap(stream);
            return bitmap;
        }

        /// <summary>The drawing canvas is being exposed to user.</summary>
        private void OnDrawingAreaExpose(object sender, ExposeEventArgs args)
        {
            DrawingArea area = (DrawingArea)sender;

            Cairo.Context context = Gdk.CairoHelper.Create(area.GdkWindow);

            foreach (DGArc tmpArc in Arcs)
                tmpArc.Paint(context);
            foreach (DGNode tmpNode in Nodes)
                tmpNode.Paint(context);

            ((IDisposable)context.Target).Dispose();
            ((IDisposable)context).Dispose();
        }

        /// <summary>Mouse button has been pressed</summary>
        private void OnMouseButtonPress(object o, ButtonPressEventArgs args)
        {
            // Get the point clicked by the mouse.
            PointD clickPoint = new PointD(args.Event.X, args.Event.Y);

            // Delselect existing object
            if (selectedObject != null)
                selectedObject.Selected = false;

            // Look through nodes for the click point
            selectedObject = Nodes.FindLast(node => node.HitTest(clickPoint));

            // If not found, look through arcs for the click point
            if (selectedObject == null)
                selectedObject = Arcs.FindLast(arc => arc.HitTest(clickPoint));

            // If found object, select it.
            if (selectedObject != null)
            {
                selectedObject.Selected = true;
                mouseDown = true;
                lastPos = clickPoint;
            }

            // Redraw area.
            (o as DrawingArea).QueueDraw();
        }

        /// <summary>Mouse has been moved</summary>
        private void OnMouseMove(object o, MotionNotifyEventArgs args)
        {
            // Get the point clicked by the mouse.
            PointD movePoint = new PointD(args.Event.X, args.Event.Y);

            // If an object is under the mouse then move it
            if (mouseDown && selectedObject != null)
            {
                lastPos.X = movePoint.X;
                lastPos.Y = movePoint.Y;
                selectedObject.Location = movePoint;
                // Redraw area.
                (o as DrawingArea).QueueDraw();
            }
        }

        /// <summary>Mouse button has been released</summary>
        private void OnMouseButtonRelease(object o, ButtonReleaseEventArgs args)
        {
            mouseDown = false;

            // If part of the selected object is now off the screen, 
            // double the height or width of the drawing area.
            DGNode rightMostNode = Nodes.Aggregate((node1, node2) => node1.Location.X > node2.Location.X ? node1 : node2);
            DGNode bottomMostNode = Nodes.Aggregate((node1, node2) => node1.Location.Y > node2.Location.Y ? node1 : node2);
            if (rightMostNode.Location.X + rightMostNode.Width >= drawable.Allocation.Width)
                drawable.WidthRequest = 2 * drawable.Allocation.Width;
            // I Assume that the nodes are circles such that width = height.
            if (bottomMostNode.Location.Y + bottomMostNode.Width >= drawable.Allocation.Height)
                drawable.HeightRequest = 2 * drawable.Allocation.Height;
        }
    }
}
