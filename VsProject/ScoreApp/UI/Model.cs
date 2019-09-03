using System.ComponentModel;
using System.Configuration;
using System.Windows.Controls;
using ScoreApp.TrackLine.MvcMidi;

namespace ScoreApp.MVC
{
    public class Model : INotifyPropertyChanged
    {

        #region Notify Property Implem

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        #region Project Values

        public string ProjectPath = "D:/";
        public string ProjectName = "New Project";
        public string Author = "Anonymous";
        public string Notes = "";

        #endregion

        #region State

        public int SelectedTrack { get; set; } = 0;
        public bool manuallyScrolling = false;
        public bool closing = false;
        public bool playing = false;

        #endregion


        
        //public int Tempo
        //{
        //    get { return MidiManager.Tempo;  }
        //    set { MidiManager.Tempo=value; RaisePropertyChanged("Tempo"); }
        //} = 120;



        #region Zoom & Offset

        public StackPanel tracksPanel;


        private int xOffset = 0;
        public int XOffset {
            get { return xOffset; }
            set
            {
                if (value < 0) value = 0;
                xOffset = value;
                RaisePropertyChanged("XOffset");
                foreach(Frame track in tracksPanel.Children)
                {
                    // TODO
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
                foreach (Frame track in tracksPanel.Children)
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
                foreach (Frame track in tracksPanel.Children)
                {
                    ((MidiLineView)track.Content).model.CellHeigth = 
                        (int)(YZoom * int.Parse(ConfigurationManager.AppSettings["cellHeigth"].ToString()));
                }
            }
        }

        #endregion
        
    }
}
