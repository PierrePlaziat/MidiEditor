using Sanford.Multimedia.Midi;
using ScoreApp.TrackLine.MvcMidi;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ScoreApp.MVC
{

    public class Control
    {

        #region CTOR

        #region CONSTANTES

        readonly int cellWidth = int.Parse(ConfigurationManager.AppSettings["cellWidth"].ToString());
        readonly int cellHeigth = int.Parse(ConfigurationManager.AppSettings["cellHeigth"].ToString());
        readonly double notesQuantity = double.Parse(ConfigurationManager.AppSettings["notesQuantity"].ToString());
        readonly double DAWhosReso = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"].ToString());
        const int offsetForNoteAppelations = 15;
        
        #endregion

        readonly Model model;
        readonly Vue vue;

        public Control(Model model,Vue vue)
        {
            this.model = model;
            this.vue = vue;
            InitModel();
            InitVue();
        }

        private void InitModel()
        {
            MidiManager.timer.Tick += Timer_Tick;
        }

        private void InitVue()
        {
            vue.Title = model.ProjectName;
            vue.positionScrollBar.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(
                    this.HandleScroll);
        }

        internal void Close()
        {
            MidiManager.Unload();
        }

        #endregion

        #region LOADING GESTION

        public void AddTrack()
        {
            MidiManager.sequence.tracks.Add(new Track());
            InitTracks();
        }

        internal void RemoveTrack()
        {
            MidiManager.sequence.tracks.RemoveAt(model.selectedTrack);
            InitTracks();
        }


        #endregion
            

        #region MENU GESTION

        public void Open(string fileName)
        {
            try
            {
                MidiManager.sequencer.Stop();
                MidiManager.playing = false;
                MidiManager.sequence.LoadAsync(fileName);

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
                MidiManager.playing = true;
                MidiManager.sequencer.Start();
                MidiManager.timer.Start();
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
                MidiManager.playing = false;
                MidiManager.sequencer.Stop();
                MidiManager.timer.Stop();
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
                MidiManager.playing = true;
                MidiManager.sequencer.Continue();
                MidiManager.timer.Start();
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
                vue.positionScrollBar.Value = Math.Min(MidiManager.sequencer.Position, vue.positionScrollBar.Maximum);
                vue.ProgressViewerBar.SetValue( Canvas.LeftProperty, 
                    offsetForNoteAppelations + cellWidth * MidiManager.sequencer.Position / DAWhosReso );
            }
        }

        internal void HandleScroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            {
                MidiManager.sequencer.Position = (int)vue.positionScrollBar.Value;
            }
        }

        #endregion

        #region MANAGE USER INTERRACTIONS

        public void EnableUserInterractions()
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

        public void InitTracks()
        {
            vue.TracksPanel.Children.Clear();
            int i = 0;
            foreach (Track track in MidiManager.sequence.tracks)
            {
                track.id = i;
                i++;
                Midi_View trackView = new Midi_View(track);
                trackView.ctrl.TrackFocused += DoFocusTrack;
                vue.TracksPanel.Children.Add(new Frame() { Content = trackView } );
            }
        }

        private void DoFocusTrack(object sender, int e)
        {
            model.selectedTrack = e;
        }

        #endregion

    }

}
