using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace WpfScore.MVC
{
    class Model
    {

        public string ProjectName;

        public Sequencer sequencer;
        public Sequence sequence;
        public Timer timer; // change from winforms to other?

        public OutputDevice outDevice;
        public int outDeviceID = 0;
        public OutputDeviceDialog outDialog = new OutputDeviceDialog();

        public bool scrolling = false;
        public bool playing = false;
        public bool closing = false;

        public OpenFileDialog openMidiFileDialog;

    }

}
