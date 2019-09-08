using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using TrackExtensions;
using ScoreApp.Utils;
using System.Configuration;
using System.Windows;

namespace ScoreApp.TrackLine.MvcMidi
{
    public class MidiLineModel : HandleBinding
    {


        #region CTOR

        public MidiLineControl Ctrl { get; set; }
        public Track Track { get; }

        public MidiLineModel(Track track)
        {
            this.Track = track;
            Color color = track.Color();
            tColor = new SolidColorBrush(color);
            LastNotesOn = new Dictionary<int, Tuple<int, MidiEvent>>();
        }

        #endregion


        public readonly Thickness SelectedBorderThickness = new Thickness(.5f);
        public readonly Thickness UnselectedBorderThickness = new Thickness(0);
        public Point mouseDragStartPoint;
        public Point mouseDragEndPoint;
        public bool isDragging = false;

        #region ATRB

        private SolidColorBrush tColor;
        public SolidColorBrush TColor
        {
            get { return tColor; }
            set { tColor = value; RaisePropertyChanged("TColor"); }
        }

        public int MidiInstrument { get; internal set; }

        public Dictionary<int, Tuple<int, MidiEvent>> LastNotesOn { get; set; }

        #region ZOOM
        #pragma warning disable S3237

        public float CellWidth
        {
            get
            {
                return (MidiManager.attachedView.Model.XZoom * int.Parse(ConfigurationManager.AppSettings["cellWidth"]));
            }
            set
            {
                RaisePropertyChanged("CellWidth");
                RaisePropertyChanged("CellHeigth");
                Ctrl.DrawNoteAppelations();
                Ctrl.DrawMidiEvents();
            }
        }

        public float CellHeigth
        {
            get
            {
                return (MidiManager.attachedView.Model.YZoom * int.Parse(ConfigurationManager.AppSettings["cellHeigth"]));
            }
            set
            {
                RaisePropertyChanged("CellHeigth");
                RaisePropertyChanged("CellWidth");
                Ctrl.DrawNoteAppelations();
                Ctrl.DrawMidiEvents();
            }
        }

        #pragma warning restore S3237
        #endregion

        public int XOffset { get; internal set; }
        public double DAWhosReso { get; } = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"]);

        #endregion

    }
}
