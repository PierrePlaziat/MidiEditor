using Sanford.Multimedia.Midi;
using ScoreApp.TrackLine.MvcMidi;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace ScoreApp.MVC
{

    class Control
    {

        #region CTOR

        readonly Model model;
        readonly Vue vue;

        public Control(Model model,Vue vue)
        {
            this.model = model;
            this.vue = vue;
            vue.Title = model.ProjectName;
            MidiInit();
            model.timer.Tick += Timer_Tick;
            vue.positionScrollBar.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(
                    this.HandleScroll);
        }

        internal void Close()
        {
            model.sequence.Dispose();
            if (model.outDevice != null)
            {
                model.outDevice.Dispose();
            }
            model.outDialog.Dispose();
        }

        #endregion

        #region LOADING GESTION

        private void HandleLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            vue.ProgressionBar.Value = e.ProgressPercentage;
        }

        private void HandleLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

            EnableUserInterractions();

            vue.ProgressionBar.Value = 0;
            if (e.Error == null)
            {
                vue.positionScrollBar.Value = 0;
                vue.positionScrollBar.Maximum = model.sequence.GetLength();
            }
            else
            {
                System.Windows.MessageBox.Show(e.Error.Message);
            }
            InitTracks();
        }

        #endregion

        #region MIDI GESTION

        public void MidiInit()
        {
            // do work here <- ?
            if (OutputDevice.DeviceCount == 0)
            {
                System.Windows.Forms.MessageBox.Show("No MIDI output devices available.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
            else
            {
                try
                {
                    model.outDevice = new OutputDevice(model.outDeviceID);
                    model.sequence.LoadCompleted += HandleLoadCompleted;
                    model.sequence.LoadProgressChanged += HandleLoadProgressChanged;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    Close();
                }
            }
            model.sequencer.Position = 0;
            model.sequencer.Sequence = model.sequence;
            model.sequencer.PlayingCompleted += new EventHandler(HandlePlayingCompleted);
            model.sequencer.ChannelMessagePlayed += new EventHandler<ChannelMessageEventArgs>(HandleChannelMessagePlayed);
            model.sequencer.SysExMessagePlayed += new EventHandler<SysExMessageEventArgs>(HandleSysExMessagePlayed);
            model.sequencer.Chased += new EventHandler<ChasedEventArgs>(HandleChased);
            model.sequencer.Stopped += new EventHandler<StoppedEventArgs>(HandleStopped);
        }

        private void HandleChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            if (model.closing)
            {
                return;
            }

            model.outDevice.Send(e.Message);
            vue.Piano.Send(e.Message);
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
                vue.Piano.Send(message);
            }
        }

        private void HandlePlayingCompleted(object sender, EventArgs e)
        {
            model.timer.Stop();
        }

        #endregion

        #region MENU GESTION

        public void Open(string fileName)
        {
            try
            {
                model.sequencer.Stop();
                model.playing = false;
                model.sequence.LoadAsync(fileName);

                DisableUserInterractions();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        #endregion

        #region PLAY GESTION

        internal void Start()
        {
            try
            {
                model.playing = true;
                model.sequencer.Start();
                model.timer.Start();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        internal void Stop()
        {
            try
            {
                model.playing = false;
                model.sequencer.Stop();
                model.timer.Stop();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        internal void Continue()
        {
            try
            {
                model.playing = true;
                model.sequencer.Continue();
                model.timer.Start();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!model.scrolling)
            {
                vue.positionScrollBar.Value = Math.Min(model.sequencer.Position, vue.positionScrollBar.Maximum);
            }
        }

        internal void HandleScroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            {
                model.sequencer.Position = (int)vue.positionScrollBar.Value;
            }
        }

        #endregion

        #region MANAGE USER INTERRACTIONS

        private void EnableUserInterractions()
        {
            vue.Cursor = System.Windows.Input.Cursors.Arrow;
            vue.startButton.IsEnabled = true;
            vue.continueButton.IsEnabled = true;
            vue.stopButton.IsEnabled = true;
            vue.OpenMenuItem.IsEnabled = true;
        }

        private void DisableUserInterractions()
        {
            vue.Cursor = System.Windows.Input.Cursors.Wait;
            vue.startButton.IsEnabled = false;
            vue.continueButton.IsEnabled = false;
            vue.stopButton.IsEnabled = false;
            vue.OpenMenuItem.IsEnabled = false;
        }

        #endregion

        #region TRACK GESTION

        private void InitTracks()
        {
            foreach (Track track in model.sequence.tracks)
            {
                vue.TracksPanel.Children.Add(new Frame() { Content = new Midi_View(track, model.sequencer) } );
            }
        }

        #endregion

    }

}
