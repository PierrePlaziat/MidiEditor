using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace ScoreApp.MVC
{
    class Model
    {

        public List<Track> Tracks;
        //{
        //    get
        //    {
        //        return tracks;
        //    }
        //    set
        //    {
        //        tracks = value; RaisePropertyChanged("Tracks");
        //    }
        //}
        //private List<Track> tracks;


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

    }

}
