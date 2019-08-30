using System;
using System.ComponentModel;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace ScoreApp.MVC
{
    public class Model : INotifyPropertyChanged
    {

        #region NOTIFY PROPERTY

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
                Console.WriteLine(DateTime.Now + " Lm Widget Part >>> PropertyChanged : " + property);
            }
        }

        #endregion

        public int selectedTrack = 0;

        public string ProjectName = "New Project";


        public bool scrolling = false;
        public bool closing = false;
        public bool playing = false;

        public OpenFileDialog openMidiFileDialog = new OpenFileDialog();

        public int Tempo
        {
            get { return MidiManager.sequencer.clock.Tempo; }
            set { MidiManager.sequencer.clock.Tempo = value; RaisePropertyChanged("Tempo"); }
        }
    }
}
