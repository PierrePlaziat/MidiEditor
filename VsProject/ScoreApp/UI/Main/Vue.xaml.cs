using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Sanford.Multimedia.Midi.UI;

namespace ScoreApp.MVC
{

    public partial class Vue : Window
    {

        #region ATRB

        readonly int timeWidth = int.Parse(ConfigurationManager.AppSettings["cellWidth"].ToString());
        readonly double midiResolution = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"].ToString());
        const int notationOffset = 15;

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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MidiManager.Stop();
            Ctrl.Close();
        }

        #endregion

        public void MouseWheeled(object sender, MouseWheelEventArgs e)
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

        #region PLAY GESTION

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

        public void Update()
        {
            double timePosition = MidiManager.Time * timeWidth / midiResolution;
            TimeScroller.Value = Math.Min(MidiManager.Time, TimeScroller.Maximum);
            TimeBar.SetValue(Canvas.LeftProperty, timePosition + notationOffset - Model.XOffset);
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

        #region UI ACCESS

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
