using Sanford.Multimedia.Midi;
using System.Windows;
using System.Windows.Controls;

namespace ScoreApp.TrackLine.MvcMidi
{

    public partial class Midi_View : Page
    {

        public MidiLineControl ctrl;
        MidiLineModel model;

        public Midi_View(Track track)
        {
            model = new MidiLineModel(track);
            InitializeComponent();
            ctrl = new MidiLineControl(model,this);
        }
        

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = new Thickness(.5f);
            ctrl.TrackGotFocus(sender,e);
            BodyScroll.ScrollToVerticalOffset(BodyScroll.ScrollableHeight / 2);
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = new Thickness(0);
        }
    }

}
