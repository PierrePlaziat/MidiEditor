using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

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

        public Model model;
        public Control control;

        public Vue()
        {
            model = new Model();
            DataContext = model;
            InitializeComponent();
            control = new Control(model, this);
            Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MidiManager.Stop();
            control.Close();
        }

        #endregion

        #region MENU

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openMidiFileDialog = new OpenFileDialog();
            if (openMidiFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                control.Open(openMidiFileDialog.FileName);
            }
        }

        private void AddTrackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            control.AddTrack();
        }

        private void DeleteTrackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            control.RemoveTrack();
        }

        #endregion

        #region PLAY GESTION

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            control.Start();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MidiManager.Continue();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            control.Stop();
        }

        public void Update()
        {
            double timePosition = MidiManager.Time * timeWidth / midiResolution;
            TimeBar.SetValue(Canvas.LeftProperty, timePosition);
            TimeScroller.Value = Math.Min(MidiManager.Time, TimeScroller.Maximum);
        }

        #region PIANO WIDGET

        private void PianoControl_PianoKeyDown(object sender, PianoKeyEventArgs e)
        {

            MidiManager.PianoTouch(true, e.NoteID);
        }

        private void PianoControl_PianoKeyUp(object sender, PianoKeyEventArgs e)
        {

            MidiManager.PianoTouch(false, e.NoteID);
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
