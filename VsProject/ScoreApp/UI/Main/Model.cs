using System.ComponentModel;
using System.Configuration;
using System.Windows.Controls;
using ScoreApp.Managers;
using ScoreApp.TrackLine.MvcMidi;
using ScoreApp.Utils;

namespace ScoreApp.MVC
{
    public class Model : HandleBinding
    {

        #region Project Values

        public string ProjectPath { get; set; } = "D:/";
        public string ProjectName { get; set; } = "New Project";
        public string Author { get; set; } = "Anonymous";
        public string Notes { get; set; } = "";

        #endregion

        #region config 

        public int timeWidth = int.Parse(ConfigurationManager.AppSettings["cellWidth"]);
        public double midiResolution = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"]);

        #endregion

        #region State

        public int SelectedTrack { get; set; } = 0;
        public bool ManuallyScrolling { get; set; } = false;
        public bool Closing { get; set; } = false;
        public bool Playing { get; set; } = false;

        public int Tempo
        {
            get { return MidiManager.Tempo; }
            set { MidiManager.Tempo=value; RaisePropertyChanged("Tempo"); }
        }

        #endregion

        #region Zoom & Offset

        public Grid TracksPanel { get; set; }


        private double xOffset = 0;
        public double XOffset {
            get { return xOffset; }
            set
            {
                //if (value < 0) value = 0;
                xOffset = value;
                RaisePropertyChanged("XOffset");
                foreach(var track in TracksPanel.Children)
                {
                    try
                    {
                        ((MidiLineView)track).Model.XOffset = xOffset;
                    }
                    catch
                    {

                    }
                }
            }
        } 
        
        private float xZoom = 1;
        public float XZoom {
            get { return xZoom; }
            set
            {
                if (value < .1f) value = .1f;
                xZoom = value;
                RaisePropertyChanged("XZoom");
                foreach (Frame track in TracksPanel.Children)
                {
                    ((MidiLineView)track.Content).Model.CellWidth = 
                        (int)(XZoom * int.Parse(ConfigurationManager.AppSettings["cellWidth"])); 
                }
                UiManager.mainWindow.HandleTimeBar();
            }
        }
        
        private float yZoom = 1;
        internal double marginPercent = 0.25;
        internal double absoluteTimePosition = 0;
        internal double relativeTimePosition = 0;


        public const int touchOffset = 15;

        public float YZoom
        {
            get { return yZoom; }
            set
            {
                if (value < .1f) value = .1f;
                yZoom = value;
                RaisePropertyChanged("YZoom");
                foreach (Frame track in TracksPanel.Children)
                {
                    ((MidiLineView)track.Content).Model.CellHeigth = 
                        (int)(YZoom * int.Parse(ConfigurationManager.AppSettings["cellHeigth"]));
                }
            }
        }

        public double Headerwidth { get; internal set; } = 200;




        #endregion

        private double plotDivider=4;
        public double PlotDivider
        {
            get { return plotDivider; }
            set
            {
                plotDivider = value;
                RaisePropertyChanged("PlotDivider");
            }
        }


        private int plotVelocity = 100;
        public int PlotVelocity
        {
            get { return plotVelocity; }
            set
            {
                plotVelocity = value;
                RaisePropertyChanged("PlotVelocity");
            }
        }




    }
}
