using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Sanford.Multimedia.Midi.UI;
using ScoreApp.TrackLine.MvcMidi;

namespace ScoreApp.MVC
{

    public partial class Vue : Window
    {
        
        #region CTOR

        public Control Ctrl { get; set; }
        public Model Model { get; set; }

        public Vue()
        {
            Model = new Model();
            DataContext = Model;
            Ctrl = new Control(Model, this);
            Show();
        }

        internal void Initialize()
        {
            InitializeComponent();
            Ctrl.InitVue();
            Model.TracksPanel = TracksPanel;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MidiManager.Stop();
            Ctrl.Close();
        }

        #endregion

        public void HandleWheel(object sender, MouseWheelEventArgs e)
        {
            int value = e.Delta / 120;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                Ctrl.TranslateTracks(value);
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Ctrl.ZoomTracksX(value);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                Ctrl.ZoomTracksY(value);
            }
        }
        
        #region MENU

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openMidiFileDialog = new OpenFileDialog();
            if (openMidiFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Ctrl.Open(openMidiFileDialog.FileName);
            }
        }

        private void AddTrackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Ctrl.AddTrack();
        }

        private void DeleteTrackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Ctrl.RemoveTrack();
        }

        #endregion

        #region UPDATE
        
        /// Continuously called when played
        public void TimeUpdate()
        {
            // Guard
            //if (Model.TracksPanel.Children.Count > 0) return;
            Model.absoluteTimePosition = MidiManager.Time * Model.timeWidth / Model.midiResolution ;
            Model.relativeTimePosition = HandleTrackSlide();
            HandleTimeScroller();
            HandleTimeBar();
            UpdateLayout();
        }

        #region Private

        // TODO debug
        private double HandleTrackSlide()
        {
            Canvas canvas = (
                (MidiLineView)(
                    (Frame)TracksPanel.Children[0]
                ).Content
            ).TrackBody;
            double width = canvas.ActualWidth + canvas.Margin.Left;
            double margin = width * Model.marginPercent;
            double relativeTimePosition = Model.absoluteTimePosition - Model.XOffset;

            Console.WriteLine(": " + width + " : " + margin);

            #region Slide

            //left blocked
            if (Model.absoluteTimePosition < margin)
            {
                Model.XOffset = 0;
                return Model.absoluteTimePosition;
            }

            // right to left slide
            if (relativeTimePosition < margin)
            {
                double delta = margin - relativeTimePosition;
                Model.XOffset -= delta;
                relativeTimePosition =  margin;
            }

            // left to right slide
            if (relativeTimePosition > width - margin)
            {
                double delta = relativeTimePosition - (width - margin);
                Model.XOffset += delta;
                relativeTimePosition = width - margin;
            }

            #endregion

            return relativeTimePosition;
        }

        private void HandleTimeBar()
        {
            TimeBar.SetValue(Canvas.LeftProperty, Model.relativeTimePosition+Model.touchOffset );
        }

        // TODO
        private void HandleTimeScroller()
        {
            TimeScroller.Value = Math.Min(
                Model.absoluteTimePosition,
                TimeScroller.Maximum
            );
        }

        #endregion

        #endregion

        #region PLAY Palette

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Ctrl.Start();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MidiManager.Continue();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Ctrl.Stop();
        }

        #region PIANO WIDGET

        private void PianoControl_PianoKeyDown(object sender, PianoKeyEventArgs e)
        {

            MidiManager.Playback(true, e.NoteID);
        }

        private void PianoControl_PianoKeyUp(object sender, PianoKeyEventArgs e)
        {

            MidiManager.Playback(false, e.NoteID);
        }

        #endregion

        #endregion

        #region User Interractions ON/OFF

        public void EnableUserInterractions()
        {
            Cursor = System.Windows.Input.Cursors.Arrow;
            startButton.IsEnabled = true;
            continueButton.IsEnabled = true;
            stopButton.IsEnabled = true;
            OpenMenuItem.IsEnabled = true;
        }

        public void DisableUserInterractions()
        {
            Cursor = System.Windows.Input.Cursors.Wait;
            startButton.IsEnabled = false;
            continueButton.IsEnabled = false;
            stopButton.IsEnabled = false;
            OpenMenuItem.IsEnabled = false;
        }

        public void ErrorMessage(string message)
        {
            System.Windows.Forms.MessageBox.Show(
                       message,
                       "Error!",
                       MessageBoxButtons.OK);
        }

        #endregion

    }

}
