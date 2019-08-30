using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfScore.MVC
{
    /// <summary>
    /// Logique d'interaction pour Editor.xaml
    /// </summary>
    public partial class Vue : Window
    {

        #region CTOR

        Model model;
        Control control;

        public Vue()
        {
            model = new Model();
            DataContext = model;
            control = new Control(model, this);
            InitializeComponent();
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
                string fileName = model.openMidiFileDialog.FileName;
                control.Open(fileName);
            }
        }

        #endregion

        #region PLAY GESTION

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

        private void PositionScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            control.HandleScroll(e);
        }

        private void HandlePlayingCompleted(object sender, EventArgs e)
        {
            model.timer.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!model.scrolling)
            {
                positionScrollBar.Value = Math.Min(model.sequencer.Position, positionScrollBar.Maximum);
            }
        }

        #endregion

        #region loading

        private void HandleLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressionBar.Value = e.ProgressPercentage;
        }

        private void HandleLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            control.HandleLoadComplete(e);
        }

        #endregion

        #region out

        private void HandleChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            if (model.closing)
            {
                return;
            }

            model.outDevice.Send(e.Message);
            //pianoControl1.Send(e.Message);
        }

        private void HandleChased(object sender, ChasedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
            {
                model.outDevice.Send(message);
            }
        }

        private void HandleSysExMessagePlayed(object sender, SysExMessageEventArgs e)
        {
            //     outDevice.Send(e.Message); Sometimes causes an exception to be thrown because the output device is overloaded.
        }

        private void HandleStopped(object sender, StoppedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
            {
                model.outDevice.Send(message);
                //pianoControl1.Send(message);
            }
        }

        #endregion

        #region piano

        private void pianoControl1_PianoKeyDown(object sender, PianoKeyEventArgs e)
        {
            #region Guard

            if (model.playing)
            {
                return;
            }

            #endregion

            model.outDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, e.NoteID, 127));
        }

        private void pianoControl1_PianoKeyUp(object sender, PianoKeyEventArgs e)
        {
            #region Guard

            if (model.playing)
            {
                return;
            }

            #endregion

            model.outDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, e.NoteID, 0));
        }

        #endregion

    }
}
