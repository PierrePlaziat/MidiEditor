using Sanford.Multimedia.Midi;
using System;
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

        public MidiLineView(Track track)
        {
            Model = new MidiLineModel(track);
            DataContext = Model;
            InitializeComponent();
            Ctrl = new MidiLineControl(Model,this);
            Model.Ctrl = Ctrl;
            Loaded += MyWindow_Loaded;
            TrackBody.MouseWheel += MouseWheel;
            //TrackHeader.MouseWheel += MouseWheeled;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BodyScroll.ScrollToVerticalOffset(BodyScroll.ScrollableHeight / 2);
        }

        #endregion

        #region SHOW BORDERS ON FOCUS

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = Model.SelectedBorderThickness;
            Ctrl.TrackGotFocus(sender, e); 
        }

        private void Grid_LostFocus(object sender, RoutedEventArgs e)
        {
            Border.BorderThickness = Model.UnselectedBorderThickness;
        }

        #endregion

        #region MOUSE GESTION
        // TODO better plot (ableton style)
        
        private void TrackBody_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Model.mouseDragStartPoint =  e.GetPosition((Canvas)sender);

            Model.isDragging = true;
        }

        private void TrackBody_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Model.isDragging)
            {
                // update  
                Model.isDragging = false;
                Model.mouseDragEndPoint = e.GetPosition((Canvas)sender);
                double start = Model.mouseDragStartPoint.X/ Model.CellWidth;
                double end = Model.mouseDragEndPoint.X / Model.CellWidth;
                // TODO work here
                int noteIndex = 127 - (int)(Model.mouseDragStartPoint.Y/Model.CellHeigth);
                Ctrl.InsertNote(start,end,noteIndex);
            }
        }

        private void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MidiManager.attachedView.HandleWheel(sender, e);
        }

        #endregion

        #region HEADER

        // TODO color picker
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

        // TODO midi instrument selection
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MidiManager.ChangeInstrument(Model.Track, ComboInstruments.Text);
        }

        #endregion

    }

}
