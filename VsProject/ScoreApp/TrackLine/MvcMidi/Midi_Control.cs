using Sanford.Multimedia.Midi;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ScoreApp.TrackLine.MvcMidi
{
    class Midi_Control
    {

        #region CTOR

        Midi_View view;
        Midi_Model model;

        public Midi_Control (Midi_Model model, Midi_View view)
        {
            this.model = model;
            this.view = view;
            Init();
        }

        private void Init()
        {
            Console.WriteLine("Init track : " + model.track.Name + " - Length : "+  model.track.Length);
            view.TrackName.Content = model.track.Name;
            // draw frame
            DrawGrid();
            DrawMidiEvents();
        }

        #endregion

        
        private void UpdateTrackBody()
        {
            // prevent redraw same frame // not sure if usefull
            if (model.sequencerPosition == model.sequencer.Position) return; 
            model.sequencerPosition = model.sequencer.Position;
            // draw frame
            DrawGrid();
            DrawMidiEvents();
        }

        #region DRAW GRID

        const int cellWidth = 15;
        const int cellHeigth = 5;
        const double widthMax = 800;
        const double heightMax = 100;
        private void DrawGrid()
        {
            double currentXPosition = 0;
            double currentYPosition = 0;
            while (currentXPosition<widthMax)
            {
                currentYPosition = 0;
                while (currentYPosition < heightMax)
                {
                    DrawCell(currentXPosition, currentYPosition);
                    currentYPosition += cellHeigth;
                }
                currentXPosition += cellWidth;
            }
        }
        
        private void DrawCell(double currentWidth, double currentHeight)
        {
            Rectangle rec = new Rectangle();
            rec.Width = cellWidth;
            rec.Height = cellHeigth;
            rec.Fill = Brushes.White;
            rec.Stroke = Brushes.WhiteSmoke;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec, currentWidth);
            Canvas.SetTop(rec, currentHeight);
            view.TrackBody.Children.Add(rec);
        }

        #endregion

        #region DRAW MIDI EVENT

        private void DrawMidiEvents()
        {
            foreach(var midiEvent in model.track.Iterator())
            {
                if (midiEvent.MidiMessage.MessageType == MessageType.Channel)
                {
                    DrawChannelMsg(midiEvent);
                }
                if (midiEvent.MidiMessage.MessageType == MessageType.Meta)
                {
                    DrawMetaMsg(midiEvent);
                }
                if (midiEvent.MidiMessage.MessageType == MessageType.Short)
                {
                    DrawShortMsg(midiEvent);
                }
                if (midiEvent.MidiMessage.MessageType == MessageType.SystemCommon)
                {
                    DrawSystemCommonMsg(midiEvent);
                }
                if (midiEvent.MidiMessage.MessageType == MessageType.SystemExclusive)
                {
                    DrawSystemExclusiveMsg(midiEvent);
                }
                if (midiEvent.MidiMessage.MessageType == MessageType.SystemRealtime)
                {
                    DrawSystemRealtimeMsg(midiEvent);
                }
            }
        }

        private void DrawChannelMsg(MidiEvent midiEvent)
        {
            Rectangle rec = new Rectangle();
            rec.Width = cellWidth;
            rec.Height = cellHeigth;
            rec.Fill = Brushes.BlueViolet;
            rec.Stroke = Brushes.WhiteSmoke;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec, midiEvent.DeltaTicks);
            Canvas.SetTop(rec, 0);
            view.TrackBody.Children.Add(rec);
        }

        private void DrawMetaMsg(MidiEvent midiEvent)
        {
            Rectangle rec = new Rectangle();
            rec.Width = cellWidth;
            rec.Height = cellHeigth;
            rec.Fill = Brushes.Brown;
            rec.Stroke = Brushes.WhiteSmoke;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec, midiEvent.DeltaTicks );
            Canvas.SetTop(rec, 15);
            view.TrackBody.Children.Add(rec);
        }

        private void DrawSystemRealtimeMsg(MidiEvent midiEvent)
        {
            Rectangle rec = new Rectangle();
            rec.Width = cellWidth;
            rec.Height = cellHeigth;
            rec.Fill = Brushes.MediumTurquoise;
            rec.Stroke = Brushes.WhiteSmoke;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec, midiEvent.AbsoluteTicks );
            Canvas.SetTop(rec, 75);
            view.TrackBody.Children.Add(rec);
        }

        private void DrawSystemExclusiveMsg(MidiEvent midiEvent)
        {
            Rectangle rec = new Rectangle();
            rec.Width = cellWidth;
            rec.Height = cellHeigth;
            rec.Fill = Brushes.Red;
            rec.Stroke = Brushes.WhiteSmoke;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec, midiEvent.AbsoluteTicks );
            Canvas.SetTop(rec, 60);
            view.TrackBody.Children.Add(rec);
        }

        private void DrawSystemCommonMsg(MidiEvent midiEvent)
        {
            Rectangle rec = new Rectangle();
            rec.Width = cellWidth;
            rec.Height = cellHeigth;
            rec.Fill = Brushes.Gold;
            rec.Stroke = Brushes.WhiteSmoke;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec, midiEvent.AbsoluteTicks );
            Canvas.SetTop(rec, 45);
            view.TrackBody.Children.Add(rec);
        }

        private void DrawShortMsg(MidiEvent midiEvent)
        {
            Rectangle rec = new Rectangle();
            rec.Width = cellWidth;
            rec.Height = cellHeigth;
            rec.Fill = Brushes.Chartreuse;
            rec.Stroke = Brushes.WhiteSmoke;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec, midiEvent.AbsoluteTicks );
            Canvas.SetTop(rec, 30);
            view.TrackBody.Children.Add(rec);
        }

        #endregion

    }
}
