using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using System.ComponentModel;
using System.Windows;

namespace ScoreApp.MVC
{

    public partial class Vue : Window
    {

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
            control.Close();
        }

        #endregion

        #region MENU

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (model.openMidiFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                control.Open(model.openMidiFileDialog.FileName);
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

        #region BUTTONS

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            control.Start();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            control.Continue();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            control.Stop();
        }

        #endregion

        #region PIANO

        private void PianoControl_PianoKeyDown(object sender, PianoKeyEventArgs e)
        {
            #region Guard

            if (model.playing)
            {
                return;
            }

            #endregion

            MidiManager.outDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, e.NoteID, 127));
        }

        private void PianoControl_PianoKeyUp(object sender, PianoKeyEventArgs e)
        {
            #region Guard

            if (model.playing)
            {
                return;
            }

            #endregion

            MidiManager.outDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, e.NoteID, 0));
        }

        #endregion

    }

}
