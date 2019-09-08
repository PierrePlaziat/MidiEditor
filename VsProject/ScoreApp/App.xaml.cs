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
            MidiManager.Init(new MVC.Vue());
            MidiManager.attachedView.Initialize();
        }

    }
}
