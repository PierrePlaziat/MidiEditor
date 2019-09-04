using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace TrackExtensions
{

    public static class Extensions
    {

        // Track Extensions

        #region int Id

        static Dictionary<Track, int> trackIds = new Dictionary<Track, int>();

        static int nextId = 0;

        public static int Id(this Track trk)
        {
            try { return trackIds[trk]; }
            catch
            {
                trackIds.Add(trk, nextId++);
                return trackIds[trk];
            }
        }

        #endregion

        #region string Name

        static Dictionary<Track, string> trackNames = new Dictionary<Track, string>();

        public static string Name(this Track trk)
        {
            try { return trackNames[trk]; }
            catch
            {
                trackNames.Add(trk, "NewTrack");
                return trackNames[trk];
            }
        }

        #endregion

        #region Color color

        static Dictionary<Track, Color> trackColors = new Dictionary<Track, Color>();

        public static Color Color(this Track trk)
        {
            try { return trackColors[trk]; }
            catch
            {
                Random rnd = new Random();
                trackColors.Add(
                    trk, System.Windows.Media.Color.FromRgb(
                        (byte)rnd.Next(0, 255),
                        (byte)rnd.Next(0, 255),
                        (byte)rnd.Next(0, 255)
                    )
                );
                return trackColors[trk];
            }
        }

        public static void SetColor(this Track trk, Color _color)
        {
            try { trackColors[trk]=_color; }
            catch
            {
                trackColors.Add(trk,_color);
            }
        }

        #endregion

    }
}
