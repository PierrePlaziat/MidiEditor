using Sanford.Multimedia.Midi;
using ScoreApp.TrackLine.MvcMidi;
using System;
using System.Windows.Media;
using System.Windows.Controls;
using TrackExtensions;
using System.Media;

namespace ScoreApp.MVC
{

    public class Control
    {
        #region CTOR

        readonly Model model;
        readonly Vue vue;

        public Control(Model model,Vue vue)
        {
            this.model = model;
            this.vue = vue;
            model.TracksPanel = vue.TracksPanel;
            InitModel();
        }

        private void InitModel()
        {
            MidiManager.Timer.Tick += Update;
        }

        public void InitVue()
        {
            vue.TracksPanel.Background = Brushes.Transparent;
            vue.TracksPanel.MouseWheel += vue.HandleWheel;
            vue.Title = model.ProjectName;
            vue.TimeScroller.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(ManualScroll);
        }

        internal void Close()
        {
            MidiManager.Unload();
        }

        #endregion
                    
        #region MENU GESTION

        public void Open(string fileName)
        {
            MidiManager.OpenFile(fileName);
        }

        #endregion

        #region PLAY GESTION

        internal void Start()
        {
            MidiManager.Start();
        }

        internal void Stop()
        {
            MidiManager.Stop();
        }

        private void Update(object sender, EventArgs e)
        {
            vue.Update();
        }
        

        internal void ManualScroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            int newScrollValue = (int) vue.TimeScroller.Value;
            MidiManager.Time = newScrollValue;
            vue.Update();
        }

        #endregion

        #region TRACK GESTION

        public void InitTracks()
        {
            vue.TracksPanel.Children.Clear();
            foreach (Track track in MidiManager.Tracks) 
            {
                track.Id(); // init track id
                MidiLineView lineView = new MidiLineView(track);
                lineView.Ctrl.TrackFocused += FocusTrack;
                vue.TracksPanel.Children.Add(new Frame() { Content = lineView } );
            }
        }

        public void AddTrack()
        {
            MidiManager.AddTrack();
            InitTracks();
        }

        internal void RemoveTrack()
        {
            MidiManager.RemoveTrack(model.SelectedTrack);
            InitTracks();
        }

        private void FocusTrack(object sender, int e)
        {
            model.SelectedTrack = e;
        }

        #endregion

        #region ZOOM

        internal void TranslateTracks(int delta)
        {
            model.XOffset+=delta;
        }

        internal void ZoomTracksX(int delta)
        {
            if (MidiManager.IsPlaying) return;
            model.XZoom += (float)delta/10;
        }

        internal void ZoomTracksY(int delta)
        {
            if (MidiManager.IsPlaying) return;
            model.YZoom += (float)delta /10;
        }

        #endregion

    }

}
