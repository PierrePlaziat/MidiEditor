using System;
using System.ComponentModel;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace ScoreApp.MVC
{
    class Model : INotifyPropertyChanged
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
        public Sequencer sequencer = new Sequencer();
        public Sequence sequence = new Sequence();

        public OutputDevice outDevice;
        public int outDeviceID = 0;
        public OutputDeviceDialog outDialog = new OutputDeviceDialog();

        public Timer timer = new Timer();

        public bool scrolling = false;
        public bool playing = false;
        public bool closing = false;

        public OpenFileDialog openMidiFileDialog = new OpenFileDialog();

        public int Tempo
        {
            get { return sequencer.clock.Tempo; }
            set { sequencer.clock.Tempo = value; RaisePropertyChanged("Tempo"); }
        }
    }
}
