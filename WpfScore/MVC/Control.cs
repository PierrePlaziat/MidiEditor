using Sanford.Multimedia.Midi;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace WpfScore.MVC
{

    class Control
    {

        Model model;
        Vue vue;

        public Control(Model model,Vue vue)
        {
            this.model = model;
            this.vue = vue;
            Load();
        }

        private void Load()
        {
            if (OutputDevice.DeviceCount == 0)
            {
                System.Windows.Forms.MessageBox.Show("No MIDI output devices available.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                vue.Close();
            }
            else
            {
                try
                {
                    model.outDevice = new OutputDevice(model.outDeviceID);

                    model.sequence.LoadProgressChanged += HandleLoadProgressChanged;
                    model.sequence.LoadCompleted += HandleLoadCompleted;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    vue.Close();
                }
            }
        }
        public void Open(string fileName)
        {
            try
            {
                model.sequencer.Stop();
                model.playing = false;
                model.sequence.LoadAsync(fileName);
                vue.Cursor = System.Windows.Input.Cursors.Wait;
                vue.startButton.IsEnabled = false;
                vue.continueButton.IsEnabled = false;
                vue.stopButton.IsEnabled = false;
                vue.OpenMenuItem.IsEnabled = false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        internal void Close()
        {
            model.sequence.Dispose();
            if (model.outDevice != null)
            {
                model.outDevice.Dispose();
            }
            model.outDialog.Dispose();
        }

        internal void HandleScroll(RoutedPropertyChangedEventArgs<double> e)
        {
            model.sequencer.Position = (int)e.NewValue;
            //if (e.Type == ScrollEventType.EndScroll)
            //{
            //    model.sequencer.Position = (int)e.NewValue;

            //    scrolling = false;
            //}
            //else
            //{
            //    scrolling = true;
            //}
        }

        internal void HandleLoadComplete(AsyncCompletedEventArgs e)
        {
            vue.Cursor = System.Windows.Input.Cursors.Arrow;
            vue.startButton.IsEnabled = true;
            vue.continueButton.IsEnabled = true;
            vue.stopButton.IsEnabled = true;
            vue.OpenMenuItem.IsEnabled = true;
            vue.ProgressionBar.Value = 0;

            if (e.Error == null)
            {
                vue.positionScrollBar.Value = 0;
                vue.positionScrollBar.Maximum = model.sequence.GetLength();
            }
            else
            {
                System.Windows.MessageBox.Show(e.Error.Message);
            }
        }

        internal void Stop()
        {
            try
            {
                model.playing = false;
                model.sequencer.Stop();
                model.timer.Stop();
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
                model.playing = true;
                model.sequencer.Continue();
                model.timer.Start();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        internal void Start()
        {
            try
            {
                model.playing = true;
                model.sequencer.Start();
                model.timer.Start();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void HandleLoadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            vue.ProgressionBar.Value = e.ProgressPercentage;
        }

        private void HandleLoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            vue.Cursor = System.Windows.Input.Cursors.Arrow;
            vue.startButton.IsEnabled = true;
            vue.continueButton.IsEnabled = true;
            vue.stopButton.IsEnabled = true;
            vue.OpenMenuItem.IsEnabled = true;
            vue.ProgressionBar.Value = 0;

            if (e.Error == null)
            {
                vue.positionScrollBar.Value = 0;
                vue.positionScrollBar.Maximum = model.sequence.GetLength();
            }
            else
            {
                System.Windows.MessageBox.Show(e.Error.Message);
            }
        }

    }
}
