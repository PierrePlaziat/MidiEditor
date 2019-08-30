using System;
using Sanford.Multimedia.Midi;
using System.Windows.Forms;
using Sanford.Multimedia.Midi.UI;
using Sanford.Multimedia.Midi;
using System.ComponentModel;
using ScoreApp.MVC;

namespace ScoreApp
{
    public static class MidiManager
    {

        public static OutputDevice outDevice;
        public static int outDeviceID = 0;
        public static OutputDeviceDialog outDialog = new OutputDeviceDialog();
        public static Sequencer sequencer = new Sequencer();
        public static Sequence sequence = new Sequence();
        public static Timer timer = new Timer();
        public static bool playing = false;

        public static void InitMidi()
        {
            if (OutputDevice.DeviceCount == 0)
            {
                MessageBox.Show(
                    "No MIDI output devices available.", 
                    "Error!",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Stop);
                Unload();
            }
            else
            {
                try
                {
                    outDevice = new OutputDevice(outDeviceID);
                    sequence.LoadCompleted += HandleLoadCompleted;
                    sequence.LoadProgressChanged += HandleLoadProgressChanged;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    Unload();
                }
            }
            sequencer.Position = 0;
            sequencer.Sequence = sequence;

        }

        static Vue vue = null;
        public static void LinkMidiManagerToView(Vue _vue)
        {
            vue = _vue;
            sequencer.PlayingCompleted += new EventHandler(HandlePlayingCompleted);
            sequencer.ChannelMessagePlayed += new EventHandler<ChannelMessageEventArgs>(HandleChannelMessagePlayed);
            sequencer.SysExMessagePlayed += new EventHandler<SysExMessageEventArgs>(HandleSysExMessagePlayed);
            sequencer.Chased += new EventHandler<ChasedEventArgs>(HandleChased);
            sequencer.Stopped += new EventHandler<StoppedEventArgs>(HandleStopped);
        }

        private static void HandlePlayingCompleted(object sender, EventArgs e)
        {
            timer.Stop();
        }

        public static void HandleLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            vue.ProgressionBar.Value = e.ProgressPercentage;
        }
        public static void HandleLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

           vue.control.EnableUserInterractions();

            vue.ProgressionBar.Value = 0;
            if (e.Error == null)
            {
                vue.positionScrollBar.Value = 0;
                vue.positionScrollBar.Maximum = MidiManager.sequence.GetLength();
            }
            else
            {
                System.Windows.MessageBox.Show(e.Error.Message);
            }
            vue.model.Tempo = MidiManager.sequencer.clock.Tempo;
            vue.control.InitTracks();
        }

        internal static void Unload()
        {
            sequence.Dispose();
            if (outDevice != null)
            {
                outDevice.Dispose();
            }
            outDialog.Dispose();
        }
        #region MIDI GESTION


        private static void HandleChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            if (vue.model.closing)
            {
                return;
            }

            MidiManager.outDevice.Send(e.Message);
            vue.Piano.Send(e.Message);
        }

        private static void HandleChased(object sender, ChasedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
            {
                MidiManager.outDevice.Send(message);
            }
        }

        private static void HandleSysExMessagePlayed(object sender, SysExMessageEventArgs e)
        {
            //     outDevice.Send(e.Message); Sometimes causes an exception to be thrown because the output device is overloaded.
        }

        private static void HandleStopped(object sender, StoppedEventArgs e)
        {
            foreach (ChannelMessage message in e.Messages)
            {
                MidiManager.outDevice.Send(message);
                vue.Piano.Send(message);
            }
        }

        #endregion
    }
}
