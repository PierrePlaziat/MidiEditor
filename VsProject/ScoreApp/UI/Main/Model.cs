using System.ComponentModel;
using System.Configuration;
using System.Windows.Controls;
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

        #region State

        public int SelectedTrack { get; set; } = 0;
        public bool ManuallyScrolling { get; set; } = false;
        public bool Closing { get; set; } = false;
        public bool Playing { get; set; } = false;

        public int Tempo
        {
            get { return MidiManager.GetTempo(); }
            set { MidiManager.SetTempo(value); RaisePropertyChanged("Tempo"); }
        }

        #endregion


        #region Zoom & Offset

        public StackPanel TracksPanel { get; set; }


        private int xOffset = 0;
        public int XOffset {
            get { return xOffset; }
            set
            {
                if (value < 0) value = 0;
                xOffset = value;
                RaisePropertyChanged("XOffset");
                foreach(Frame track in TracksPanel.Children)
                {
                    ((MidiLineView)track.Content).model.XOffset = xOffset;
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
                    ((MidiLineView)track.Content).model.CellWidth = (int)(XZoom * int.Parse(ConfigurationManager.AppSettings["cellWidth"].ToString())); 
                }
            }
        }
        
        private float yZoom = 1;
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
                    ((MidiLineView)track.Content).model.CellHeigth = 
                        (int)(YZoom * int.Parse(ConfigurationManager.AppSettings["cellHeigth"].ToString()));
                }
            }
        }

        #endregion
        
    }
}
