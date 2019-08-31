using System;
using System.ComponentModel;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace ScoreApp.MVC
{
    public class Model : INotifyPropertyChanged
    {

        #region CONFIG

        public string ProjectName = "New Project";
        public string Author = "Anonymous";
        public string Notes = "";

        #endregion

        #region STATE

        public int selectedTrack = 0;
        public bool manuallyScrolling = false;
        public bool closing = false;
        public bool playing = false;
        public int Tempo
        {
            get { return MidiManager.Tempo;  }
            set { MidiManager.Tempo=value; RaisePropertyChanged("Tempo"); }
        }

        #endregion

        #region NotifyProperty

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

    }
}
