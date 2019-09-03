using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

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
                Console.WriteLine(DateTime.Now + " >>> PropertyChanged : " + property);
            }
        }

        #endregion

        #region CTOR

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
                cellWidth = value/10;
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
                cellHeigth = value/10;
                RaisePropertyChanged("cellHeigth");
            }
        }


    }
}
