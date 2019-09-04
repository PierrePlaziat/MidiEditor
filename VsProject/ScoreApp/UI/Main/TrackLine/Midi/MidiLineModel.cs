using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using TrackExtensions;
using ScoreApp.Utils;

namespace ScoreApp.TrackLine.MvcMidi
{
    public class MidiLineModel : HandleBinding
    {
        
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
            Color color = track.Color();
            tColor = new SolidColorBrush(color);
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

        public int XOffset { get; internal set; }
    }
}
