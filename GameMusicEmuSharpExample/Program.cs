using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using GameMusicEmuSharp;

namespace GameMusicEmuSharpExample
{
	internal class Program
	{
		internal static void Main()
		{
			const string fileName = "Super Mario Bros. 3.nsf";
			const int sampleRate = 48000;

			// Get the emulator handle.
			IntPtr emulatorHandle = GmeNative.OpenFile(fileName, sampleRate);

			// Get track info for track 0 (technically track 1, just 0-based).
			GmeTrackInfo trackInfo = GmeNative.GetTrackInfo(emulatorHandle, 0);

			// Get the current equalizer info for this track.
			GmeEqualizer equalizer = GmeNative.GetEqualizer(emulatorHandle);

			// Print out various info about the file.

			Console.WriteLine("Track count: " + GmeNative.gme_track_count(emulatorHandle));
			Console.WriteLine();

			Console.WriteLine("Song Title: " + ((trackInfo.song      == "")     ? "Unspecified" : trackInfo.song));
			Console.WriteLine("Game: "       + ((trackInfo.game      == "")     ? "Unspecified" : trackInfo.game));
			Console.WriteLine("System: "     + ((trackInfo.system    == "")     ? "Unspecified" : trackInfo.system));
			Console.WriteLine("Artist: "     + ((trackInfo.author    == "")     ? "Unspecified" : trackInfo.author));
			Console.WriteLine("Copyright: "  + ((trackInfo.copyright == "")     ? "Unspecified" : trackInfo.copyright));
			Console.WriteLine("Comment: "    + ((trackInfo.comment   == "")     ? "Unspecified" : trackInfo.comment));
			Console.WriteLine("Dumper: "     + ((trackInfo.dumper    == "")     ? "Unspecified" : trackInfo.dumper));
			Console.WriteLine();

			Console.WriteLine("Length: "       + ((trackInfo.length      == -1) ? "Unknown" : trackInfo.length.ToString()));
			Console.WriteLine("Play Length: "  + ((trackInfo.playLength  == -1) ? "Unknown" : trackInfo.playLength.ToString()));
			Console.WriteLine("Loop Length: "  + ((trackInfo.loopLength  == -1) ? "Unknown" : trackInfo.loopLength.ToString()));
			Console.WriteLine("Intro Length: " + ((trackInfo.introLength == -1) ? "Unknown" : trackInfo.introLength.ToString()));
			Console.WriteLine();

			Console.WriteLine("Voice Count: "  + GmeNative.gme_voice_count(emulatorHandle));
			Console.WriteLine();

			Console.WriteLine("Channel Names: ");
			Console.WriteLine();

			for (int i = 0; i < GmeNative.gme_voice_count(emulatorHandle); i++)
			{
				Console.WriteLine(GmeNative.GetVoiceName(emulatorHandle, i));
			}

			Console.WriteLine();

			// Equalizer info.
			Console.WriteLine("Treble: " + equalizer.Treble);
			Console.WriteLine("Bass: "   + equalizer.Bass);
			Console.WriteLine();

			// GmeType info.
			GmeType type = GmeNative.GetType(emulatorHandle);
			Console.WriteLine("Type: "        + type.system);
			Console.WriteLine("Track Count: " + type.trackCount); // Zero = non-fixed track count for the format.
			Console.WriteLine("Supports multitrack: " + GmeNative.gme_type_multitrack(ref type));

			// Keep the console window up.
			Console.ReadLine();
		}
	}
}
