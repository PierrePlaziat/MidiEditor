using System;
using Sanford.Multimedia.Midi;
using System.Windows.Forms;
using System.ComponentModel;
using ScoreApp.MVC;
using System.Configuration;
using System.Collections.Generic;

namespace ScoreApp
{

    /// Connects ScoreApp to Sanford Midi
    public static class MidiManager
    {

        #region ATRB


        // IO

        private static OutputDevice outDevice;
        public static Timer Timer { get; } = new Timer(); 

        // MIDI BUILDER

        private static ChannelMessageBuilder cmBuilder = new ChannelMessageBuilder();
        //private static SysCommonMessageBuilder scBuilder = new SysCommonMessageBuilder();

        // SEQUENCER

        private static Sequencer sequencer;
        public static bool IsPlaying { get; set; } = false;
        public static int Time
        {
            get { return sequencer.Position; } 
            set { sequencer.Position = value; }
        }

        public static int GetTempo()
        {
            return sequencer.clock.Tempo;
        }

        public static void SetTempo(int value)
        {
            if (value < 1) value = 1;
            sequencer.clock.Tempo = value;
        }

        // SEQUENCE

        private static Sequence sequence;
        public static IEnumerable<Track> Tracks
        {
            get { return sequence.tracks; }
        }

        #endregion
        
        #region CTOR

        public static Vue attachedView { get; set; }

        public static void Init(Vue vue)
        {
            attachedView = vue;
            InitSequencer();
            if (CheckMidiOutput())
                InitOutputDevice();
        }

        private static bool CheckMidiOutput()
        {
            if (OutputDevice.DeviceCount == 0)
            {
                attachedView.ErrorMessage("No MIDI output devices available.");
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
                attachedView.ErrorMessage(ex.Message);
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
        
        public static void Unload()
        {
            sequence.Dispose();
            if (outDevice != null)
            {
                outDevice.Dispose();
            }
        }

        internal static void ChangeInstrument(Track track, int instrument)
        {
            // TODO
            
            cmBuilder.Command = ChannelCommand.ProgramChange;
            if (outDevice != null)
            {
                cmBuilder.Data1 = instrument;
                cmBuilder.MidiChannel = track.Channel;
                cmBuilder.Build();
                outDevice.Send(cmBuilder.Result);
            }
        }

        #endregion

        #region MIDI MSG

        private static void HandleChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            if (attachedView.Model.Closing)
            {
                return;
            }

            MidiManager.outDevice.Send(e.Message);
            attachedView.Piano.Send(e.Message);
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
                attachedView.Piano.Send(message);
            }
        }

        private static void HandlePlayingCompleted(object sender, EventArgs e)
        {
            Stop();
        }

        #endregion

        #region  MTDS

        #region PLAY/PAUSE GESTION

        internal static void Start()
        {
            try
            {
                IsPlaying = true;
                sequencer.Start();
                Timer.Start();
            }
            catch (Exception ex)
            {
                attachedView.ErrorMessage(ex.Message);
            }
        }

        internal static void Stop()
        {
            try
            {
                IsPlaying = false;
                sequencer.Stop();
                Timer.Stop();
            }
            catch (Exception ex)
            {
                attachedView.ErrorMessage(ex.Message);
            }
        }

        internal static void Continue()
        {
            try
            {
                IsPlaying = true;
                sequencer.Continue();
                Timer.Start();
            }
            catch (Exception ex)
            {
                attachedView.ErrorMessage(ex.Message);
            }
        }

        internal static void Playback(bool v, int noteID)
        {
            if (!IsPlaying)
            {
                try
                {
                    outDevice.Send(
                        new ChannelMessage(v ? ChannelCommand.NoteOn : ChannelCommand.NoteOff, 0, noteID, 127)
                    );
                }
                catch (Exception e)
                {
                    Console.WriteLine("MidiRollDebug : " + e);
                }
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

        #region PLOT GESTION

        readonly static double DAWhosReso = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"]);

        internal static Tuple<MidiEvent, MidiEvent> CreateNote(int channel, int noteIndex, Track Track, double start, double end, int velocity)
        {
            cmBuilder.Command = ChannelCommand.NoteOn;
            cmBuilder.Data1 = noteIndex;
            cmBuilder.Data2 = velocity;
            cmBuilder.MidiChannel = channel;
            cmBuilder.Build();
            MidiEvent me1 = Track.Insert((int)(start* DAWhosReso), cmBuilder.Result);
            cmBuilder.Command = ChannelCommand.NoteOff;
            cmBuilder.Data1 = noteIndex;
            cmBuilder.Data2 = 0;
            cmBuilder.MidiChannel = channel;
            cmBuilder.Build();
            MidiEvent me2 = Track.Insert((int)(end* DAWhosReso), cmBuilder.Result);
            return new Tuple<MidiEvent, MidiEvent>(me1, me2);
        }

        #endregion

        #region DATA
        
        // TODO open file dialog to select save name
        internal static void SaveFile(string fileName)
        {
            Stop();
            try
            {
                // sequence.SaveAsync(fileName);
            }
            catch (Exception ex)
            {
                attachedView.ErrorMessage(ex.Message);
            }
            // on success
            attachedView.DisableUserInterractions();
        }

        internal static void OpenFile(string fileName)
        {
            Stop();
            try
            {
                // LOAD MIDI FILE
                sequence.LoadAsync(fileName); 
            }
            catch (Exception ex)
            {
                attachedView.ErrorMessage(ex.Message);
            }
            // on success
            attachedView.DisableUserInterractions();
        }

        public static void HandleLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            attachedView.ProgressionBar.Value = e.ProgressPercentage;
        }

        public static void HandleLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

            attachedView.EnableUserInterractions();

            attachedView.ProgressionBar.Value = 0;
            if (e.Error == null)
            {
                attachedView.TimeScroller.Value = 0;
                attachedView.TimeScroller.Maximum = MidiManager.sequence.GetLength();
            }
            else
            {
                System.Windows.MessageBox.Show(e.Error.Message);
            }
            attachedView.Model.Tempo = MidiManager.sequencer.clock.Tempo; // TODO tempo doesnt seem to be loaded from midi file that easy
            attachedView.Ctrl.InitTracks(); 
        }

        #endregion

        #endregion

    }
}
