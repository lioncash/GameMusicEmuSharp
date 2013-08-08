using System;
using GameMusicEmuSharp;
using NAudio.Wave;

namespace GameMusicEmuSharpExample
{
	internal class Program
	{
		#region Fields

		private const string fileName = "Super Mario Bros. 3.nsf";
		private static int track = 0;
		private static readonly GmeReader reader;

		#endregion

		#region Constructor

		static Program()
		{
			reader = new GmeReader(fileName);
		}

		#endregion

		#region Methods

		internal static void Main()
		{
			// Play the track.
			IWavePlayer player = new WaveOut();
			player.Init(reader);
			player.Play();

			// Display basic track info.
			DisplayTrackInfo(false);

			Console.WriteLine("If you are playing a format that supports multiple tracks:\n\nPress the right-arrow key to change tracks.\nPress the left-arrow key to go back");
			Console.WriteLine();
			Console.WriteLine("Hit escape or enter to exit the program.");

			// Loop to check if the user hits left or right arrow keys to change tracks.
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey();
				if (keyInfo.Key == ConsoleKey.RightArrow)
				{
					if (track < reader.TrackCount-1)
					{
						reader.SetTrack(++track);
						DisplayTrackInfo(false);
					}
				}
				else if (keyInfo.Key == ConsoleKey.LeftArrow)
				{
					if (track > 0)
					{
						reader.SetTrack(--track);
						DisplayTrackInfo(false);
					}
				}
			} while (keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Enter);
		}

		// Displays information about the currently playing track.
		private static void DisplayTrackInfo(bool printExtendedTrackInfo)
		{
			// Get the track info
			GmeTrackInfo trackInfo = reader.TrackInfo;

			Console.Clear();
			Console.WriteLine("Song Title: " + ((trackInfo.song == "") ? "Unspecified" : trackInfo.song));
			Console.WriteLine("Game: " + ((trackInfo.game == "") ? "Unspecified" : trackInfo.game));
			Console.WriteLine("System: " + ((trackInfo.system == "") ? "Unspecified" : trackInfo.system));
			Console.WriteLine("Artist: " + ((trackInfo.author == "") ? "Unspecified" : trackInfo.author));
			Console.WriteLine("Copyright: " + ((trackInfo.copyright == "") ? "Unspecified" : trackInfo.copyright));
			Console.WriteLine("Comment: " + ((trackInfo.comment == "") ? "Unspecified" : trackInfo.comment));
			Console.WriteLine("Dumper: " + ((trackInfo.dumper == "") ? "Unspecified" : trackInfo.dumper));
			Console.WriteLine();
			Console.WriteLine("Track count: " + reader.TrackCount);
			Console.WriteLine();

			if (printExtendedTrackInfo)
			{
				// Get the current equalizer info for this track.
				GmeEqualizer equalizer = reader.Equalizer;

				Console.WriteLine("Length: " + ((trackInfo.length == -1) ? "Unknown" : trackInfo.length.ToString()));
				Console.WriteLine("Play Length: " + ((trackInfo.playLength == -1) ? "Unknown" : trackInfo.playLength.ToString()));
				Console.WriteLine("Loop Length: " + ((trackInfo.loopLength == -1) ? "Unknown" : trackInfo.loopLength.ToString()));
				Console.WriteLine("Intro Length: " + ((trackInfo.introLength == -1) ? "Unknown" : trackInfo.introLength.ToString()));
				Console.WriteLine();

				Console.WriteLine("Voice Count: " + reader.VoiceCount);
				Console.WriteLine();

				Console.WriteLine("Channel Names: ");
				Console.WriteLine();

				for (int i = 0; i < reader.VoiceCount; i++)
				{
					Console.WriteLine(reader.GetVoiceName(i));
				}

				Console.WriteLine();

				// Equalizer info.
				Console.WriteLine("Treble: " + equalizer.Treble);
				Console.WriteLine("Bass: " + equalizer.Bass);
				Console.WriteLine();

				// GmeType info.
				GmeType type = reader.Type;
				Console.WriteLine("Type: " + type.system);
				Console.WriteLine("Track Count: " + type.trackCount); // Zero = non-fixed track count for the format.
				Console.WriteLine("Supports multiple tracks: " + reader.SupportsMultipleTracks);
				Console.WriteLine();
			}

			// track+1 because multi-track files start with track 1 as index 0.
			Console.WriteLine("Currently playing track: " + (track+1) + "/" + reader.TrackCount);
			Console.WriteLine();
		}

		#endregion
	}
}
