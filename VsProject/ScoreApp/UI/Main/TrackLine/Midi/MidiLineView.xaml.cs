using Sanford.Multimedia.Midi;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TrackExtensions;

namespace ScoreApp.TrackLine.MvcMidi
{

    public partial class MidiLineView : Page
    {

        #region CTOR

        public MidiLineControl Ctrl { get; set; }
        public MidiLineModel Model { get; set; }

        readonly double notesQuantity = double.Parse(ConfigurationManager.AppSettings["notesQuantity"]);
        readonly Thickness SelectedBorderThickness = new Thickness(.5f);
        readonly Thickness UnselectedBorderThickness = new Thickness(0);

        public MidiLineView(Track track)
        {
            Model = new MidiLineModel(track);
            DataContext = Model;
            InitializeComponent();
            Ctrl = new MidiLineControl(Model,this);
            Model.Ctrl = Ctrl;
            Loaded += MyWindow_Loaded;
            TrackBody.MouseWheel += MouseWheeled;
            TrackHeader.MouseWheel += MouseWheeled;
        }

        private void MouseWheeled(object sender, MouseWheelEventArgs e)
        {
            MidiManager.Vue.MouseWheeled(sender, e);
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
            Ctrl.TrackGotFocus(sender, e); 
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = UnselectedBorderThickness;
        }

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
                double start = mouseDragStartPoint.X/ Model.CellWidth;
                double end = mouseDragEndPoint.X / Model.CellWidth;
                int noteIndex = (int)notesQuantity - (int)(mouseDragStartPoint.Y/Model.CellHeigth);
                Ctrl.InsertNote(start,end,noteIndex);
            }
        }

        #endregion

        private void TrackColor_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            Color color = Color.FromRgb(
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255)
            );
            Model.Track.SetColor(color);
            Model.TColor = new SolidColorBrush(color); 
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MidiManager.ChangeInstrument(Model.Track, ComboInstruments.Text);
        }
    }

}
