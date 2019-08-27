using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreApp.TrackLine.MvcMidi
{
    class Midi_Model : INotifyPropertyChanged
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


        public Midi_Model(Track track, Sequencer sequencer)
        {
            this.track = track;
            Length = track.Length;
            this.sequencer = sequencer;
        }

        public Sequencer sequencer { get; }
        public Track track { get; }

        public int sequencerPosition = 0;

        public int GridHorizontalPosition = 0;

        public int GridVerticalPosition = 0;

        private int length;
        public int Length
        {
            get { return length; }
            set { length = value; RaisePropertyChanged("Tracks"); }
        }

    }
}
