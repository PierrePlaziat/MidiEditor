using ScoreApp.MVC;
using System.Windows.Forms;

namespace ScoreApp.Managers
{

    public static class UiManager
    {

        public static Vue mainWindow { get; } = new Vue();

        internal static void Init()
        {
            mainWindow.Initialize();
        }

        public static void ThrowError(string message)
        {
            MessageBox.Show(
                message,
                "Error",
                MessageBoxButtons.OK
            );
        }

        // track config
        public static int TrackHeightDefault { get; set; } = 100;
        public static int TrackHeightMin { get; set; } = 100;
        public static int TrackHeightMax { get; set; } = 500;

        // input config
        public static double noteLengthDivider { get; set; } = 4;
        public static double plotDivider { get; set; } = 4;
        public static int plotVelocity { get; set; } = 100;

    }

}
