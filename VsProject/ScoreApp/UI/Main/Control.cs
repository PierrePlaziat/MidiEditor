using Sanford.Multimedia.Midi;
using ScoreApp.TrackLine.MvcMidi;
using System;
using System.Windows.Media;
using System.Windows.Controls;
using TrackExtensions;
using System.Windows;
using System.Linq;
using ScoreApp.Managers;

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
            //MidiManager.Timer.Tick += Update;
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

        #region Scroll
        
        public void Update(object sender, EventArgs e)
        {
            vue.TimeUpdate();
        }
        

        internal void ManualScroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (MidiManager.IsPlaying) return;
            int newScrollValue = (int) vue.TimeScroller.Value;
            MidiManager.CurrentTime = newScrollValue;
            vue.TimeUpdate();
        }

        #endregion

        #region TRACK GESTION

        public void InitTracks()
        {
            vue.TracksPanel.Children.Clear();
            vue.TracksPanel.RowDefinitions.Clear();
            int i = 0;
            foreach (Track track in MidiManager.Tracks) 
            {
                MidiLineView lineView = InitTrackLine(i,track);
                AddTrackGridRow(i,lineView);
                AddSeparatorGridRow(i);
                i++;
            }
            vue.TracksPanel.RowDefinitions.Add(
               new RowDefinition()
               {
                   Height = new GridLength(400, GridUnitType.Pixel)
               }
           );

        }

        private MidiLineView InitTrackLine(int rowIndex, Track track)
        {
            track.Id();
            track.Channel = rowIndex;
            MidiLineView lineView = new MidiLineView(track);
            lineView.Ctrl.TrackFocused += FocusTrack;
            return lineView;
        }

        private void AddSeparatorGridRow(int rowIndex)
        {
            // make row
            vue.TracksPanel.RowDefinitions.Add(
                new RowDefinition()
                {
                    Height = new GridLength(3, GridUnitType.Pixel)
                }
            );
            // add separator in row
            var separator = new GridSplitter()
            {
                Height = 3,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = Brushes.Black,
            };
            vue.TracksPanel.Children.Add(separator);
            Grid.SetRow(separator, 2 * rowIndex + 1);
        }

        private void AddTrackGridRow(int rowIndex, MidiLineView lineView)
        {
            // make row
            vue.TracksPanel.RowDefinitions.Add(
                new RowDefinition()
                {
                    Height = new GridLength(UiManager.TrackHeightDefault, GridUnitType.Pixel),
                    MaxHeight = UiManager.TrackHeightMax,
                    MinHeight = UiManager.TrackHeightMin,
                }
            );
            // add trackline in row
            var trackLine = new Frame()
            {
                Content = lineView
            };
            vue.TracksPanel.Children.Add(trackLine);
            Grid.SetRow(trackLine, 2 * rowIndex);
        }

        public void AddTrack()
        {
            MidiManager.AddTrack();
            InitTracks();
        }

        internal void RemoveTrack()
        {
            if (!MidiManager.Tracks.Any()) return;
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
