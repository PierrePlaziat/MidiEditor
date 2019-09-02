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

        readonly int cellWidth = int.Parse(ConfigurationManager.AppSettings["cellWidth"].ToString());
        readonly int cellHeigth = int.Parse(ConfigurationManager.AppSettings["cellHeigth"].ToString());
        readonly double notesQuantity = double.Parse(ConfigurationManager.AppSettings["notesQuantity"].ToString());
        readonly double DAWhosReso = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"].ToString());
        readonly Thickness SelectedBorderThickness = new Thickness(.5f);
        readonly Thickness UnselectedBorderThickness = new Thickness(0);

        public MidiLineView(Track track)
        {
            model = new MidiLineModel(track);
            InitializeComponent();
            ctrl = new MidiLineControl(model,this);
            Loaded += MyWindow_Loaded;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BodyScroll.ScrollToVerticalOffset(BodyScroll.ScrollableHeight / 2);
        }

        #endregion

        #region SHOW BORDERS ON FOCUS

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = SelectedBorderThickness;
            ctrl.TrackGotFocus(sender, e); 
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = UnselectedBorderThickness;
        }

        #endregion

        #region ZOOM GESTION

        // TODO

        #endregion

        #region PLOT GESTION

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
