using Sanford.Multimedia.Midi;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace ScoreApp.TrackLine.MvcMidi
{

    public partial class MidiLineView : Page
    {

        #region CTOR

        public MidiLineControl ctrl;
        MidiLineModel model;

        public MidiLineView(Track track)
        {
            model = new MidiLineModel(track);
            InitializeComponent();
            ctrl = new MidiLineControl(model,this);
        }

        readonly int cellWidth = int.Parse(ConfigurationManager.AppSettings["cellWidth"].ToString());
        readonly int cellHeigth = int.Parse(ConfigurationManager.AppSettings["cellHeigth"].ToString());
        readonly double notesQuantity = double.Parse(ConfigurationManager.AppSettings["notesQuantity"].ToString());
        readonly double DAWhosReso = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"].ToString());

        #endregion
        

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = new Thickness(.5f);
            ctrl.TrackGotFocus(sender, e);
            BodyScroll.ScrollToVerticalOffset(BodyScroll.ScrollableHeight / 2);
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = new Thickness(0);
        }

        #region Mouse Gestion

        Point mouseDragStartPoint;
        Point mouseDragEndPoint;
        bool draggingNoteOnStave = false;

        private void TrackBody_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mouseDragStartPoint =  e.GetPosition((Canvas)sender);
            draggingNoteOnStave = true;
        }

        private void TrackBody_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (draggingNoteOnStave)
            {
                draggingNoteOnStave = false;
                mouseDragEndPoint = e.GetPosition((Canvas)sender);
                double start = mouseDragStartPoint.X/cellWidth;
                double end = mouseDragEndPoint.X / cellWidth;
                int noteIndex = (int)notesQuantity - (int)( mouseDragStartPoint.Y/cellHeigth );
                ctrl.InsertNote(start,end,noteIndex);
            }
        }

        #endregion

    }

}
