using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreApp.Utils
{
    class Snippets
    {

        // CHANGE INSTRUMENT :
        //outDevice.Send(new ChannelMessage(ChannelCommand.ProgramChange, 0, (int) GeneralMidiInstrument.AltoSax));
        // Leslie version
        /*sing System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;

namespace ProgramListDemo
{
    public partial class Form1 : Form
    {
        private Sanford.Multimedia.Midi.OutputDevice outDevice = null;

        private Sanford.Multimedia.Midi.ChannelMessageBuilder builder = new ChannelMessageBuilder();
            
        public Form1()
        {
            builder.Command = ChannelCommand.ProgramChange;
            builder.MidiChannel = 0;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if(OutputDevice.DeviceCount == 0)
            {
                MessageBox.Show("No MIDI output devices available.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);

                Close();
            }
            else
            {
                try
                {
                    outDevice = new OutputDevice(0);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    Close();
                }
            }

            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if(outDevice != null)
            {
                outDevice.Dispose();
            }

            base.OnClosed(e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {           
            if(outDevice != null)
            {
                builder.Data1 = comboBox1.SelectedIndex;

                builder.Build();

                outDevice.Send(builder.Result);
            }

        }
    }
}*/

// Calculate Mesures :
//int TotalMeasures = (int)(((float)this.Sequencer.Position / this.Sequence.Division) * ((float)this.TimeSign_Denom) / 4.0f);
//int BeatPosInBar = (TotalMeasures % this.TimeSign_Num) + 1;

// TrackName 
/* The toolkit is easy to extend to store tracknames.Trackname is a meta message, so it is read under TrackReader's ParseMetaMessage() method. There you can add:
 Hide Copy Code
else if (type == MetaType.TrackName)
 {
     byte[] data = new byte[ReadVariableLengthValue()];
     Array.Copy(trackData, trackIndex, data, 0, data.Length);
     newTrack.Insert(ticks, new MetaMessage(type, data));


     track.Name = System.Text.ASCIIEncoding.ASCII.GetString(data);
     trackIndex += data.Length;
 }
 and ta-daa*/

// PITCH WHEEL
/*int pitchBend = 8192; // Pitch wheel center.<br />
int mask = 127;<br />
 <br />
ChannelMessageBuilder builder = new ChannelMessageBuilder();<br />
 <br />
// Build pitch bend message;<br />
builder.Command = ChannelCommand.PitchWheel;<br />
 <br />
// Unpack pitch bend value into two data bytes.<br />
builder.Data1 = pitchBend & mask;<br />
builder.Data2 = pitchBend >> 7;<br />
 <br />
// Build message.<br />
builder.Build();<br />
 <br />
ChannelMessage pitchBendMessage = builder.Result;<br />
 <br />
// Send message (assumes we've created an output device).<br />
outDevice.Send(pitchBendMessage);<br />*/

// PANIC, all note off 
/*public void Panic()
{
    for (int i = 0; i < 16; i++)
    {
        //Sending all-notes-off command to channel.
        m_outDevice.Send(new ChannelMessage(ChannelCommand.Controller, i, 120, 0));
    }
}*/





// 
/*In my PianoRoll-editor, I can draw Notes on a grid. I have to have a reference to the NoteOn and NoteOff MidiEvents (for drawing and editing the notes as lines with a certain note-length etc).

To get a MidiEvent in a Track, you'll have to pass the Index of the event. But, if a Track is changed, the indexes of the events are changed too, of course.

So, storing the indexes of the NoteOn and NoteOff MidiEvents as properties of the Note isn't working ('cause they're changing).

I've accomplished to create references to the actual NoteOn and NoteOff MidiEvents and store them as properties of type MidiEvent. But, when I change the AbsoluteTicks-values of these MidiEvents, they're not updated in the Track they're in.

Marco*/

/*At this time, you can't alter a MidiEvent directly. You're best option is removing the MidiEvent from the track and reinserting the MIDI data at a new position. A bit of a hassel, but I wanted to make MidiEvents immutable to the outside world.

Hmm, I could probably add a Move method to the MidiEvent class. This would take a ticks value. The MidiEvent would then move itself forward or backwards in the Track depending on the ticks value you've given it. What do you think about that?
*/


}
}
