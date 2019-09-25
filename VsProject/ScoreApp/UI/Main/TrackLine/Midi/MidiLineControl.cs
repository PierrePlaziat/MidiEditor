using Sanford.Multimedia.Midi;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using TrackExtensions;
using ScoreApp.Managers;

namespace ScoreApp.TrackLine.MvcMidi
{
    public class MidiLineControl
    {

        #region CTOR

        public MidiLineView view { get; set; }
        MidiLineModel model { get; set; }
        
        public MidiLineControl (MidiLineModel model, MidiLineView view)
        {
            this.model = model;
            this.view = view;
            Init();
        }

        private void Init()
        {
            // track header
            FillInstrumentBox();
            view.TrackName.Content = model.Track.Name();
            // track body
            DrawPianoRoll();
            DrawMidiEvents();
        }

        private void FillInstrumentBox()
        {
            foreach (GeneralMidiInstrument instrument in Enum.GetValues(typeof(GeneralMidiInstrument)))
            {
                view.ComboInstruments.Items.Add(
                    new ComboBoxItem()
                    {
                        Content = instrument.ToString("G")
                    }
                );
            }
            view.ComboInstruments.SelectedIndex = 0;
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
            TrackFocused.Invoke(sender,model.Track.Id());
        }

        internal void InsertNote(double start, double end, int noteIndex)
        {
            if (MidiManager.IsPlaying) return;
            // Generate Midi Note
            int channel = 0;
            int velocity = UiManager.plotVelocity;
            var msgs = MidiManager.CreateNote(
                channel, 
                noteIndex,
                model.Track,
                start,
                end,
                velocity);
            // Draw it on MidiRoll
            DrawNote(start,end,noteIndex, msgs.Item1, msgs.Item2);
        }

        #endregion

        #region DRAW GRID

        public void DrawPianoRoll()
        {
            view.TrackNotes.Children.Clear();
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
                    Width = 15,
                    Height = model.CellHeigth,
                    Fill = currentColor,
                    Stroke = Brushes.Gray,
                    StrokeThickness = .5f
                };
                // place rectangle
                Canvas.SetLeft(rec, 0);
                Canvas.SetTop(rec, (127 - i)*model.CellHeigth);
                // piano toucn on rectangle
                int j = i;
                rec.MouseLeftButtonDown += (s, e) => MidiManager.Playback(true, j);
                rec.MouseLeftButtonUp += (s, e) => MidiManager.Playback(false, j);
                rec.MouseLeave += (s, e) => MidiManager.Playback(false, j);
                // add it to the control
                view.TrackNotes.Children.Add(rec);
                view.TrackNotes.Height = 127 * model.CellHeigth;
                view.TrackBody.Height = 127 * model.CellHeigth;
            }
        }

        #endregion

        #region DRAW MIDI EVENT

        public void DrawMidiEvents()
        {
            view.TrackBody.Children.Clear();
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
                if (model.LastNotesOn.TryGetValue(noteIndex, out Tuple<int, MidiEvent> onPosition))
                {
                    DrawNote(
                        (double)onPosition.Item1 / model.DAWhosReso,
                        (double)position / model.DAWhosReso,
                        noteIndex,
                        onPosition.Item2,
                        midiEvent
                    );
                    model.LastNotesOn.Remove(noteIndex);
                }
            }
            // NOTE ON
            if (status >= (int)ChannelCommand.NoteOn &&
                status <= (int)ChannelCommand.NoteOn + ChannelMessage.MidiChannelMaxValue)
            {
                int noteIndex = (int)midiEvent.MidiMessage.GetBytes()[1];
                int velocity = (int)midiEvent.MidiMessage.GetBytes()[2];
                if (velocity > 0)
                {
                    model.LastNotesOn[noteIndex] = new Tuple<int, MidiEvent>(position, midiEvent);
                }
                else
                {
                    if (model.LastNotesOn.TryGetValue(noteIndex, out Tuple<int, MidiEvent> onPosition))
                    {
                        DrawNote(onPosition.Item1, position, noteIndex, onPosition.Item2, midiEvent);
                        model.LastNotesOn.Remove(noteIndex);
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

        private void DrawNote(double start, double end, int noteIndex, MidiEvent messageOn, MidiEvent messageOff)
        {
            Rectangle rec = new Rectangle();
            try
            {
                rec.Width = (end-start)* model.CellWidth;
            }
            catch
            {
                rec.Width = 1;
            }
            rec.Height = model.CellHeigth;
            rec.Fill = Brushes.DarkSeaGreen;
            rec.Stroke = Brushes.DarkGreen;
            rec.StrokeThickness = .5f;
            Canvas.SetLeft(rec,start*model.CellWidth);
            Canvas.SetTop(rec, ((127 - noteIndex)*model.CellHeigth));
            rec.MouseLeftButtonDown += NoteLeftDown;
            rec.MouseRightButtonDown += NoteRightDown;
            rec.SetValue(AttachedNoteOnProperty, messageOn);
            rec.SetValue(AttachedNoteOffProperty, messageOff);
            view.TrackBody.Children.Add(rec);
        }

        private void NoteLeftDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.ClickCount>1)
            {
                if (MidiManager.IsPlaying) return;
                Rectangle rec = (Rectangle)sender;
                MidiEvent noteOn = (MidiEvent)rec.GetValue(AttachedNoteOnProperty);
                MidiEvent noteOff = (MidiEvent)rec.GetValue(AttachedNoteOffProperty);
                view.TrackBody.Children.Remove(rec);
                model.Track.Remove(noteOn);
                model.Track.Remove(noteOff);
            }
        }

        private void NoteRightDown(object sender, MouseButtonEventArgs e)
        {
        }

        #endregion
        

    }
}
