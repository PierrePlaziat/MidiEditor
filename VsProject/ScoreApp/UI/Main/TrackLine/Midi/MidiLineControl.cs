using Sanford.Multimedia.Midi;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Configuration;
using System.Windows.Input;
using TrackExtensions;

namespace ScoreApp.TrackLine.MvcMidi
{
    public class MidiLineControl
    {

        #region CTOR

        MidiLineView View { get; set; }
        MidiLineModel Model { get; set; }

        #region CONSTANTES

        readonly int cellWidth = int.Parse(ConfigurationManager.AppSettings["cellWidth"].ToString());
        readonly int cellHeigth = int.Parse(ConfigurationManager.AppSettings["cellHeigth"].ToString());
        readonly double notesQuantity = double.Parse(ConfigurationManager.AppSettings["notesQuantity"].ToString());
        readonly double DAWhosReso = double.Parse(ConfigurationManager.AppSettings["DAWhosReso"].ToString());

        #endregion

        public MidiLineControl (MidiLineModel model, MidiLineView view)
        {
            this.Model = model;
            this.View = view;
            Init();
        }

        private void Init()
        {
            Console.WriteLine("Init track : " + Model.Track.Name() + " - Length : "+  Model.Track.Length);
            View.TrackName.Content = Model.Track.Name();
            View.TrackName.Content = Model.Track.Name();
            DrawNoteAppelations();
            DrawMidiEvents();
        }

        public event EventHandler<int> TrackFocused;

        public static readonly DependencyProperty AttachedNoteOnProperty =
            DependencyProperty.RegisterAttached(
                "AttachedNoteOn",
                typeof(MidiEvent),
                typeof(MidiLineControl)
        );

        public static readonly DependencyProperty AttachedNoteOffProperty =
            DependencyProperty.RegisterAttached(
                "AttachedNoteOff",
                typeof(MidiEvent),
                typeof(MidiLineControl)
        );

        #endregion

        #region INTERACTIONS

        internal void TrackGotFocus(object sender, RoutedEventArgs e)
        {
            TrackFocused.Invoke(sender,Model.Track.Id());
        }

        internal void InsertNote(double start, double end, int noteIndex)
        {
            int channel = 0; int intensity = 100;
            // Generate Midi Note
            Tuple<MidiEvent, MidiEvent> msgs = MidiManager.CreateNote(channel, noteIndex, Model.Track,start,end,intensity);
            // Draw it on MidiRoll
            DrawNote(start,end,noteIndex, msgs.Item1, msgs.Item2);
        }

        #endregion

        #region DRAW GRID

        private void DrawNoteAppelations()
        {
            int noteWithoutOctave;
            Brush currentColor = Brushes.White;
            for (int i=0;i<128;i++)
            {
                // identify note
                noteWithoutOctave = i % 12;
                // choose note color
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
                // make rectangle
                Rectangle rec = new Rectangle
                {
                    Width = cellWidth,
                    Height = cellHeigth,
                    Fill = currentColor,
                    Stroke = Brushes.Gray,
                    StrokeThickness = .5f
                };
                // place rectangle
                Canvas.SetLeft(rec, 0);
                Canvas.SetTop(rec, (notesQuantity - i)*cellHeigth);
                // piano toucn on rectangle
                int j = i;
                rec.MouseLeftButtonDown += (s, e) => MidiManager.Playback(true, j);
                rec.MouseLeftButtonUp += (s, e) => MidiManager.Playback(false, j);
                rec.MouseLeave += (s, e) => MidiManager.Playback(false, j);
                // add it to the control
                View.TrackNotes.Children.Add(rec);
            }
        }

        #endregion

        #region DRAW MIDI EVENT

        private void DrawMidiEvents()
        {
            int i = 0;
            foreach (var midiEvent in Model.Track.Iterator())
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
                Tuple<int, MidiEvent> onPosition;
                if(Model.lastNotesOn.TryGetValue(noteIndex,out onPosition))
                {
                    DrawNote(
                        (double)onPosition.Item1/ DAWhosReso,
                        (double)position / DAWhosReso,
                        noteIndex, 
                        onPosition.Item2,
                        midiEvent
                    );
                    Model.lastNotesOn.Remove(noteIndex);
                }
            }
            // NOTE ON
            if (status >= (int)ChannelCommand.NoteOn &&
                status <= (int)ChannelCommand.NoteOn + ChannelMessage.MidiChannelMaxValue)
            {
                int noteIndex = (int)midiEvent.MidiMessage.GetBytes()[1];
                int velocity = (int)midiEvent.MidiMessage.GetBytes()[2];
                Tuple<int, MidiEvent> onPosition;
                if (velocity>0)
                {
                    Model.lastNotesOn[noteIndex] = new Tuple<int, MidiEvent>(position, midiEvent);
                }
                else
                {
                    if (Model.lastNotesOn.TryGetValue(noteIndex, out onPosition))
                    {
                        DrawNote(onPosition.Item1, position, noteIndex, onPosition.Item2, midiEvent);
                        Model.lastNotesOn.Remove(noteIndex);
                    }
                }
            }
            // ProgramChange
            if (status >= (int)ChannelCommand.ProgramChange &&
                status <= (int)ChannelCommand.ProgramChange + ChannelMessage.MidiChannelMaxValue)
            {
                Model.MidiInstrument = (int)midiEvent.MidiMessage.GetBytes()[1];
            }
        }

        private void DrawNote(double start, double end, int noteIndex, MidiEvent messageOn, MidiEvent messageOff)
        {
            Rectangle rec = new Rectangle();
            try
            {
                rec.Width = (end-start)*15;
            }
            catch
            {
                rec.Width = 1;
            }
            rec.Height = cellHeigth;
            rec.Fill = Brushes.DarkSeaGreen;
            rec.Stroke = Brushes.DarkGreen;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec,start*cellWidth);
            Canvas.SetTop(rec, ((notesQuantity - noteIndex)*5));
            rec.MouseLeftButtonDown += NoteLeftDown;
            rec.MouseRightButtonDown += NoteRightDown;
            rec.SetValue(AttachedNoteOnProperty, messageOn);
            rec.SetValue(AttachedNoteOffProperty, messageOff);
            View.TrackBody.Children.Add(rec);
        }

        private void NoteLeftDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            MidiEvent noteOn = (MidiEvent)rec.GetValue(AttachedNoteOnProperty);
            MidiEvent noteOff = (MidiEvent)rec.GetValue(AttachedNoteOffProperty);
            Console.WriteLine("(TODO) NoteClicked : " + noteOn.MidiMessage.GetBytes());
            // TODO Move and extend note
        }

        private void NoteRightDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            MidiEvent noteOn = (MidiEvent)rec.GetValue(AttachedNoteOnProperty);
            MidiEvent noteOff = (MidiEvent)rec.GetValue(AttachedNoteOffProperty);
            View.TrackBody.Children.Remove(rec);
            // TODO delete midi event   
            Model.Track.RemoveAt(noteOn.AbsoluteTicks);
            Model.Track.RemoveAt(noteOff.AbsoluteTicks);
        }

        #endregion


    }
}
