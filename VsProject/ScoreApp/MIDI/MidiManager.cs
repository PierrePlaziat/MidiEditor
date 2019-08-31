﻿using System;
using Sanford.Multimedia.Midi;
using System.Windows.Forms;
using System.ComponentModel;
using ScoreApp.MVC;
using System.Configuration;
using System.Collections.Generic;

namespace ScoreApp
{
    public static class MidiManager
    {

        #region ATRB

        // IO

        private static OutputDevice outDevice;
        public static Timer timer = new Timer(); 

        // SEQUENCER

        private static Sequencer sequencer;
        public static bool isPlaying = false;
        public static int Time
        {
            get { return sequencer.Position; } 
            set { sequencer.Position = value; }
        }
        public static int Tempo
        {
            get { return sequencer.clock.Tempo; }
            set { sequencer.clock.Tempo = value; }
        }

        // SEQUENCE

        private static Sequence sequence;
        public static IEnumerable<Track> Tracks
        {
            get { return sequence.tracks; }
        }

        #endregion
        
        #region CTOR

        public static Vue vue;

        public static void Init(Vue _vue)
        {
            vue = _vue;
            if (CheckMidiOutput())
                InitOutputDevice();
            InitSequencer();
        }

        private static bool CheckMidiOutput()
        {
            if (OutputDevice.DeviceCount == 0)
            {
                vue.ErrorMessage("No MIDI output devices available.");
                Unload();
                return false;
            }
            else return true;
        }

        private static void InitOutputDevice()
        {
            try
            {
                outDevice = new OutputDevice(
                    int.Parse(ConfigurationManager.AppSettings["outDeviceID"])
                );
            }
            catch (Exception ex)
            {
                vue.ErrorMessage(ex.Message);
                Unload();
            }
        }

        private static void InitSequencer()
        {
            // create
            sequencer = new Sequencer();
            sequence = new Sequence();
            // configure
            sequencer.Position = 0;
            sequencer.Sequence = sequence;
            sequencer.PlayingCompleted += new EventHandler(HandlePlayingCompleted);
            sequencer.ChannelMessagePlayed += new EventHandler<ChannelMessageEventArgs>(HandleChannelMessagePlayed);
            sequencer.SysExMessagePlayed += new EventHandler<SysExMessageEventArgs>(HandleSysExMessagePlayed);
            sequencer.Chased += new EventHandler<ChasedEventArgs>(HandleChased);
            sequencer.Stopped += new EventHandler<StoppedEventArgs>(HandleStopped);
            sequence.LoadCompleted += HandleLoadCompleted;
            sequence.LoadProgressChanged += HandleLoadProgressChanged;
        }
        
        // TODO maybe cause of cloing error, investigate further
        public static void Unload()
        {
            sequence.Dispose();
            if (outDevice != null)
            {
                outDevice.Dispose();
            }
        }

        #endregion

        #region MIDI
        
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

        private static void HandlePlayingCompleted(object sender, EventArgs e)
        {
            timer.Stop();
        }

        #endregion

        #region  MTDS

        // TODO move to IMusicControl
        internal static void PianoTouch(bool v, int noteID)
        {
            if (!isPlaying)
            {
                outDevice.Send(new ChannelMessage(v ? ChannelCommand.NoteOn : ChannelCommand.NoteOff, 0, noteID, 127));
            }

        }

        #region PLAY/PAUSE GESTION

        internal static void Start()
        {
            try
            {
                isPlaying = true;
                sequencer.Start();
                timer.Start();
            }
            catch (Exception ex)
            {
                vue.ErrorMessage(ex.Message);
            }
        }

        internal static void Stop()
        {
            try
            {
                isPlaying = false;
                sequencer.Stop();
                timer.Stop();
            }
            catch (Exception ex)
            {
                vue.ErrorMessage(ex.Message);
            }
        }

        internal static void Continue()
        {
            try
            {
                isPlaying = true;
                sequencer.Continue();
                timer.Start();
            }
            catch (Exception ex)
            {
                vue.ErrorMessage(ex.Message);
            }
        }

        #endregion

        #region TRACK GESTION

        internal static void AddTrack()
        {
            sequence.tracks.Add(new Track());
        }

        internal static void RemoveTrack(int selectedTrack)
        {
            sequence.tracks.RemoveAt(selectedTrack);
        }

        #endregion

        #region DATA

        // TODO
        internal static void SaveFile(string fileName)
        {
        }

        internal static void OpenFile(string fileName)
        {
            try
            {
                sequencer.Stop();
                isPlaying = false;
                sequence.LoadAsync(fileName);
                vue.DisableUserInterractions();
            }
            catch (Exception ex)
            {
                vue.ErrorMessage(ex.Message);
            }
        }

        public static void HandleLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            vue.ProgressionBar.Value = e.ProgressPercentage;
        }

        public static void HandleLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

            vue.EnableUserInterractions();

            vue.ProgressionBar.Value = 0;
            if (e.Error == null)
            {
                vue.TimeScroller.Value = 0;
                vue.TimeScroller.Maximum = MidiManager.sequence.GetLength();
            }
            else
            {
                System.Windows.MessageBox.Show(e.Error.Message);
            }
            vue.model.Tempo = MidiManager.sequencer.clock.Tempo;
            vue.control.InitTracks();
        }

        #endregion

        #endregion

    }
}
