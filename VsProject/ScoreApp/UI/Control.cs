using Sanford.Multimedia.Midi;
using ScoreApp.TrackLine.MvcMidi;
using System;
using System.Windows.Controls;

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
            model.tracksPanel = vue.TracksPanel;
            InitModel();
            InitVue();
        }

        private void InitModel()
        {
            MidiManager.timer.Tick += Update;
        }

        private void InitVue()
        {
            vue.Title = model.ProjectName;
            vue.TimeScroller.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(
                    this.ManualScroll);
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
        }

        #endregion

        #region TRACK GESTION

        public void InitTracks()
        {
            vue.TracksPanel.Children.Clear();
            int i = 0;
            foreach (Track track in MidiManager.Tracks) 
            {
                track.id = i;
                i++;
                MidiLineView lineView = new MidiLineView(track);
                lineView.ctrl.TrackFocused += FocusTrack;
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
            MidiManager.RemoveTrack(model.selectedTrack);
            InitTracks();
        }

        private void FocusTrack(object sender, int e)
        {
            model.selectedTrack = e;
        }

        #endregion

        #region ZOOM

        internal void TranslateTracks(int delta)
        {
            model.XOffset+=delta;
            if (model.XOffset < 0) model.XOffset = 0;
            InitTracks();
        }

        internal void ZoomTracksX(int delta)
        {
            model.XZoom += (float)delta/10;
            if (model.XZoom < .1f) model.XZoom = .1f;
            InitTracks();
        }

        internal void ZoomTracksY(int delta)
        {
            model.YZoom += delta;
            if (model.YZoom < .1f) model.YZoom = .1f;
            InitTracks();
        }

        #endregion

    }

}
