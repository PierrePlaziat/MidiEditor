using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScoreApp.TrackLine.MvcMidi
{

    public partial class Midi_View : Page
    {

        Midi_Control ctrl;
        Midi_Model model;

        public Midi_View(Track track,Sequencer sequencer)
        {
            model = new Midi_Model(track,sequencer);
            InitializeComponent();
            ctrl = new Midi_Control(model,this);
        }
    }

}
