using Sanford.Multimedia.Midi;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Configuration;
using System.Windows.Input;

namespace ScoreApp.TrackLine.MvcMidi
{
    public class MidiLineControl
    {

        #region CTOR

        Midi_View view;
        MidiLineModel model;

        public MidiLineControl (MidiLineModel model, Midi_View view)
        {
            this.model = model;
            this.view = view;
            Init();
        }

        private void Init()
        {
            Console.WriteLine("Init track : " + model.Track.Name + " - Length : "+  model.Track.Length);
            view.TrackName.Content = model.Track.Name;
            view.TrackName.Content = model.Track.Name;
            DrawNoteAppelations();
            DrawMidiEvents();
        }

        #endregion


        #region CONSTANTES

        readonly int cellWidth = int.Parse(ConfigurationManager.AppSettings["cellWidth"].ToString());
        readonly int cellHeigth = int.Parse(ConfigurationManager.AppSettings["cellHeigth"].ToString());
        readonly double notesQuantity = double.Parse(ConfigurationManager.AppSettings["notesQuantity"].ToString());
        readonly double DAWhosReso = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"].ToString());

        #endregion
        
        public event EventHandler<int> TrackFocused;
        internal void TrackGotFocus(object sender, RoutedEventArgs e)
        {
            TrackFocused.Invoke(sender,model.Track.id);
        }

        #region DRAW GRID

        private void DrawNoteAppelations()
        {
            int noteWithoutOctave;
            Brush currentColor = Brushes.White;
            for (int i=0;i<128;i++)
            {
                noteWithoutOctave = i % 12;
                switch (noteWithoutOctave)
                {
                    case 0: currentColor = Brushes.White; break;
                    case 1: currentColor = Brushes.Black; break;
                    case 2: currentColor = Brushes.White; break;
                    case 3: currentColor = Brushes.Black; break;
                    case 4: currentColor = Brushes.White; break;
                    case 5: currentColor = Brushes.White; break;
                    case 6: currentColor = Brushes.Black; break;
                    case 7: currentColor = Brushes.White; break;
                    case 8: currentColor = Brushes.Black; break;
                    case 9: currentColor = Brushes.White; break;
                    case 10: currentColor = Brushes.Black; break;
                    case 11: currentColor = Brushes.White; break;
                }
                if (i == 60) currentColor = Brushes.Yellow;
                if (i == 69) currentColor = Brushes.Cyan;
                Rectangle rec = new Rectangle
                {
                    Width = cellWidth,
                    Height = cellHeigth,
                    Fill = currentColor,
                    Stroke = Brushes.Gray,
                    StrokeThickness = .5f
                };
                Canvas.SetLeft(rec, 0);
                Canvas.SetTop(rec, (notesQuantity - i)*cellHeigth);
                view.TrackNotes.Children.Add(rec);
            }
        }

        #endregion

        #region DRAW MIDI EVENT

        private void DrawMidiEvents()
        {
            int i = 0;
            foreach (var midiEvent in model.Track.Iterator())
            {
                if (midiEvent.MidiMessage.MessageType == MessageType.Channel)
                {
                    DrawChannelMsg(midiEvent);
                }
                i++;
            }
        }

        private void DrawChannelMsg(MidiEvent midiEvent)
        {
            int status = midiEvent.MidiMessage.Status;
            int position = midiEvent.AbsoluteTicks;
            // NOTE OFF
            if (status >= (int)ChannelCommand.NoteOff &&
                status <= (int)ChannelCommand.NoteOff + ChannelMessage.MidiChannelMaxValue)
            {
                int noteIndex = (int)midiEvent.MidiMessage.GetBytes()[1];
                int onPosition;
                if(model.lastNotesOn.TryGetValue(noteIndex,out onPosition))
                {
                    DrawNote((double)onPosition/ DAWhosReso, (double)position / DAWhosReso, noteIndex,midiEvent);
                    model.lastNotesOn.Remove(noteIndex);
                }
            }
            // NOTE ON
            if (status >= (int)ChannelCommand.NoteOn &&
                status <= (int)ChannelCommand.NoteOn + ChannelMessage.MidiChannelMaxValue)
            {
                int noteIndex = (int)midiEvent.MidiMessage.GetBytes()[1];
                int velocity = (int)midiEvent.MidiMessage.GetBytes()[2];
                int onPosition;
                if (velocity>0)
                {
                    model.lastNotesOn[noteIndex] = position;
                }
                else
                {
                    if (model.lastNotesOn.TryGetValue(noteIndex, out onPosition))
                    {
                        DrawNote(onPosition, position, noteIndex, midiEvent);
                        model.lastNotesOn.Remove(noteIndex);
                    }
                }
            }
            // ProgramChange
            if (status >= (int)ChannelCommand.ProgramChange &&
                status <= (int)ChannelCommand.ProgramChange + ChannelMessage.MidiChannelMaxValue)
            {
                model.MidiInstrument = (int)midiEvent.MidiMessage.GetBytes()[1];
            }
        }

        private void DrawNote(double start, double end, int noteIndex, MidiEvent midievent)
        {
            Rectangle rec = new Rectangle();
            rec.Width = (end-start)*15;
            rec.Height = cellHeigth;
            rec.Fill = Brushes.DarkSeaGreen;
            rec.Stroke = Brushes.DarkGreen;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec,start*cellWidth);
            Canvas.SetTop(rec, ((notesQuantity - noteIndex)*5));
            rec.MouseLeftButtonDown += NoteLeftDown;
            rec.MouseRightButtonDown += NoteRightDown;
            rec.SetValue(AttachedMidiEventProperty, midievent);
            view.TrackBody.Children.Add(rec);
        }

        public static readonly DependencyProperty AttachedMidiEventProperty = 
            DependencyProperty.RegisterAttached(
                "AttachedMidiEvent",
                typeof(MidiEvent),
                typeof(MidiLineControl)
        );

        private void NoteLeftDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            MidiEvent mid = (MidiEvent)rec.GetValue(AttachedMidiEventProperty);
            Console.WriteLine("NoteClicked : " + mid.MidiMessage.GetBytes());
            // TODO Move and extend note
        }

        private void NoteRightDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            MidiEvent mid = (MidiEvent)rec.GetValue(AttachedMidiEventProperty);
            Console.WriteLine("NoteClicked : " + mid.MidiMessage.GetBytes());
            view.TrackBody.Children.Remove(rec);
            // TODO delete midi event            
        }

        #endregion


    }
}
