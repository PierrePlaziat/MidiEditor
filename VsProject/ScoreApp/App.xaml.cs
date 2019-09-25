using ScoreApp.Managers;
using System.Windows;

namespace ScoreApp
{

    /////////////////
    // ENTRY POINT //
    /////////////////
    
    public partial class App : Application
    {

        public App()
        {
            MidiManager.Init();
            UiManager.Init();
        }

    }
}
