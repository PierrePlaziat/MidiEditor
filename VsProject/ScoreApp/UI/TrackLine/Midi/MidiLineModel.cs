using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace ScoreApp.TrackLine.MvcMidi
{
    public class MidiLineModel : INotifyPropertyChanged
    {

        #region NOTIFY PROPERTY

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
                //Console.WriteLine(DateTime.Now + " >>> PropertyChanged : " + property);
            }
        }

        #endregion

        #region CTOR

        private SolidColorBrush tColor = new SolidColorBrush(Color.FromRgb(89, 201, 119));
        public SolidColorBrush TColor
        {
            get { return tColor; }
            set { tColor = value; RaisePropertyChanged("TColor"); }
        }

        public MidiLineModel(Track track)
        {
            this.Track = track;
        }

        #endregion

        public Track Track { get; }
        public int MidiInstrument { get; internal set; }

        public Dictionary<int, Tuple<int, MidiEvent>> lastNotesOn = new Dictionary<int, Tuple<int, MidiEvent>>();


        private int cellWidth = 24;
        public int CellWidth
        {
            get
            {
                return cellWidth;
            }
            set
            {
                RaisePropertyChanged("CellWidth");
            }
        }

        private int cellHeigth = 5;
        public int CellHeigth
        {
            get
            {
                return cellHeigth;
            }
            set
            {
                RaisePropertyChanged("CellHeigth");
            }
        }


    }
}
