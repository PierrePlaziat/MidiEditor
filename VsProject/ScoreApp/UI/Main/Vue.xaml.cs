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

        #region ATRB

        const int touchOffset = 15;

        #endregion

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
            if (Model.TracksPanel.Children.Count > 0) return;
            Model.absoluteTimePosition = MidiManager.Time * Model.timeWidth / Model.midiResolution;
            Model.relativeTimePosition = HandleXOffset();
            HandleTimeScroller();
            HandleTimeBar();
        }

        #region Private

        // remember to XOffset the track

        // TODO
        private double HandleXOffset()
        {
            double width = ((MidiLineView)Model.TracksPanel.Children[0]).Width;
            double margin = width / Model.marginPercent;
            double relativeTimePosition = Model.absoluteTimePosition - Model.XOffset;
            if (relativeTimePosition > width - margin)
            {
                double delta = relativeTimePosition - width - margin;
                Model.XOffset += delta;
                relativeTimePosition = width - margin;
            }
            //if (absoluteTimePosition > width + touchOffset - margin)
            //{
            //    Model.XOffset = 0;

            //}
            return relativeTimePosition;
        }

        private void HandleTimeBar()
        {
            double timeBarPosition = Model.absoluteTimePosition + touchOffset - Model.XOffset;
            TimeBar.SetValue(Canvas.LeftProperty, timeBarPosition);
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
