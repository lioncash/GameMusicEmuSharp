using System;
using System.Runtime.InteropServices;

namespace GameMusicEmuSharp
{
	/// <summary>
	/// Class which contains all the native methods/interop for interfacing with Game Music Emu.
	/// </summary>
	public static class GmeNative
	{
		#region Interface Methods

		/// <summary>
		/// Encapsulates gme_open_file. Opens a given file and returns the required emulator handle. 
		/// </summary>
		/// <param name="fileName">Path to the file to open.</param>
		/// <param name="sampleRate">The sample rate to use during playback.</param>
		/// <returns>An emulator handle.</returns>
		public static IntPtr OpenFile(string fileName, int sampleRate)
		{
			// Get an emulator handle.
			IntPtr emulatorHandle;
			gme_open_file(fileName, out emulatorHandle, sampleRate);

			return emulatorHandle;
		}

		/// <summary>
		/// Encapsulates gme_voice_name. Gets the name of voice 'i'.
		/// <para></para>
		/// The valid indexes for voices are from 0 - gme_voice_count() - 1.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="i">The voice to get the name of.</param>
		/// <returns>The name of voice 'i'</returns>
		public static string GetVoiceName(IntPtr emuHandle, int i)
		{
			// Get the string ptr.
			IntPtr outStringPtr = gme_voice_name(emuHandle, i);

			// Convert it to an ANSI string.
			return (Marshal.PtrToStringAnsi(outStringPtr));
		}

		/// <summary>
		/// Encapsulates gme_track_info() to get a struct containing track information.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="track">The track to get information from.</param>
		/// <returns>A GmeTrackInfo struct containing information about the track.</returns>
		public static GmeTrackInfo GetTrackInfo(IntPtr emuHandle, int track)
		{
			// Get the pointer to the native struct for the track info.
			IntPtr trackInfoPtr;
			gme_track_info(emuHandle, out trackInfoPtr, track);

			// Marshal it over to the C# based struct.
			GmeTrackInfo trackInfo = (GmeTrackInfo)Marshal.PtrToStructure(trackInfoPtr, typeof(GmeTrackInfo));

			// Free the native pointer.
			gme_free_info(trackInfoPtr);

			// Return the C# based one full of info.
			return trackInfo;
		}

		/// <summary>
		/// Encapsulates gme_equalizer.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handler to a MusicEmu reference.</param>
		/// <returns>A GmeEqualizer struct that contains the current frequency equalizer parameters.</returns>
		public static GmeEqualizer GetEqualizer(IntPtr emuHandle)
		{
			// Get the equalizer
			GmeEqualizer equalizer;
			gme_equalizer(emuHandle, out equalizer);

			return equalizer;
		}

		/// <summary>
		/// Encapsulates gme_set_equalizer. Sets new equalizer values.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="treble">The desired treble level.</param>
		/// <param name="bass">The desired bass level.</param>
		public static void SetEqualizer(IntPtr emuHandle, double treble, double bass)
		{
			GmeEqualizer equalizer = new GmeEqualizer();
			equalizer.Treble = treble;
			equalizer.Bass = bass;

			gme_set_equalizer(emuHandle, ref equalizer);
		}

		/// <summary>
		/// Encapuslates gme_type. Gets the type info for the loaded file.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <returns>The type info for the loaded file.</returns>
		public static GmeType GetType(IntPtr emuHandle)
		{
			IntPtr typePtr = gme_type(emuHandle);

			return (GmeType)Marshal.PtrToStructure(typePtr, typeof(GmeType));
		}

		#endregion

		#region P/Invoke Methods

		// The name of the DLL that all these native functions are called from.
		private const string DllName = "gme";

		/* Basic operations */

		/// <summary>
		/// Opens a given music file and sets a reference to emuOut.
		/// </summary>
		/// <param name="path">The path to the music file to load.</param>
		/// <param name="emuOut">The IntPtr handle that will be set to a MusicEmu reference.</param>
		/// <param name="sampleRate">The sample rate to play this file at.</param>
		/// <returns>An IntPtr reference. Or a string error message if an error occurs.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern string gme_open_file(string path, out IntPtr emuOut, int sampleRate);

		/// <summary>
		/// Starts playing a track.
		/// </summary>
		/// <param name="emuHandle">An IntPtr handle to a MusicEmu reference.</param>
		/// <param name="songIndex">The song to play (note that 0 is the first track).</param>
		/// <returns>A string error message if an error occurs.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern string gme_start_track(IntPtr emuHandle, int songIndex);

		/// <summary>
		/// Generates 'count' number of 16-bit signed sample info into 'output'.
		/// </summary>
		/// <param name="emuHandle">An IntPtr handle to a MusicEmu reference.</param>
		/// <param name="count">The number of samples to generate into output.</param>
		/// <param name="output">A buffer which contains generated sample information.</param>
		/// <returns>An error message if an error occurs.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern string gme_play(IntPtr emuHandle, int count, [In, Out] short[] output);

		/// <summary>
		/// When finished with the emulator, call this to free the memory used by it.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu to free.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gme_delete(IntPtr emuHandle);


		/* Track position/length */


		/// <summary>
		/// Sets the time for the track to start fading out.
		/// <para></para>
		/// Once the fade ends, gme_track_ended() returns true.
		/// </summary>
		/// <remarks>
		/// The fade time can also be changed while the track is playing.
		/// </remarks>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="startMSec">The duration of the fade in milliseconds.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gme_set_fade(IntPtr emuHandle, int startMSec);

		/// <summary>
		/// Checks if the currently playing track has ended.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <returns>True if the track has ended. False, otherwise.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool gme_track_ended(IntPtr emuHandle);

		/// <summary>
		/// Gets the number of milliseconds (1000ms = 1 second) played since the beginning of the track.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <returns>The number of milliseconds played since the beginning of the track.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gme_tell(IntPtr emuHandle);

		/// <summary>
		/// The number of samples generated since the beginning of the track.
		/// </summary>
		/// <param name="emuHandle">An IntPtr handle to a MusicEmu reference.</param>
		/// <returns>The number of samples generated since the beginning of the track.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gme_tell_samples(IntPtr emuHandle);

		/// <summary>
		/// Seeks to a new time in the track.
		/// </summary>
		/// <remarks>
		/// Note that seeking backwards or far forwards can take a while.
		/// </remarks>
		/// <param name="emuHandle">An IntPtr handle to a MusicEmu reference.</param>
		/// <param name="msec">The amount of milliseconds to seek.</param>
		/// <returns>An error message if an error occurs.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern string gme_seek(IntPtr emuHandle, int msec);

		/// <summary>
		/// Seeks n samples from the beginning of the track.
		/// </summary>
		/// <param name="emuHandle">An IntPtr handle to a MusicEmu reference.</param>
		/// <param name="nSamples">The number of samples to skip.</param>
		/// <returns>An error message if an error occurs.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern string gme_seek_samples(IntPtr emuHandle, int nSamples);

		/// <summary>
		/// The number of tracks contained within the music file.
		/// </summary>
		/// <param name="emuHandle">A handle to the music emulator.</param>
		/// <returns>The number of tracks contained within the music file.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gme_track_count(IntPtr emuHandle);


		/* Informational */

		/// <summary>
		/// Gets the most recent warning string.
		/// <para></para>
		/// - Clears the current warning after returning.
		/// <para></para>
		/// - Warning is also cleared when loading a file and starting a track.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <returns>The warning string, or Null if there isn't one.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern string gme_warning(IntPtr emuHandle);

		/// <summary>
		/// Loads an M3U playlist file (must be done after loading music).
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="path">The path to the M3U playlist.</param>
		/// <returns>An error message if an error occurs.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern string gme_load_m3u(IntPtr emuHandle, string path);

		/// <summary>
		/// Clears any loaded M3U playlist and any internal playlist the music format supports (NSFE for example).
		/// </summary>
		/// <param name="emuHandle">An IntPtr handle to a MusicEmu reference.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gme_clear_playlist(IntPtr emuHandle);

		/// <summary>
		/// Gets information for a particular track, and sets its info to 'trackInfo'
		/// </summary>
		/// <remarks>
		/// Must be freed after using it.
		/// </remarks>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="trackInfo">The struct that will hold the track info.</param>
		/// <param name="track">The track to get info about.</param>
		/// <returns>The GmeTrackInfo struct that contains info about the track. Or an error message if an error occurs.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern string gme_track_info(IntPtr emuHandle, out IntPtr trackInfo, int track);

		/// <summary>
		/// Frees the memory used by a GmeTrackInfo struct.
		/// </summary>
		/// <param name="trackInfo">The IntPtr representing track info to free.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern void gme_free_info(IntPtr trackInfo);


		/* Advanced Playback */

		/// <summary>
		/// Adjusts the stereo echo depth, where 0.0 = off, and 1.0 = maximum.
		/// <para></para>
		/// Has no effect for GYM, SPC, and Sega Genesis VGM music.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="depth">The desired stereo echo depth.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gme_set_stereo_depth(IntPtr emuHandle, double depth);


		/// <summary>
		/// Disable automatic end-of-track detection and skips the silence at the beginning if true.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="ignore">Whether or not to disable the silence.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gme_ignore_silence(IntPtr emuHandle, bool ignore);

		/// <summary>
		/// Adjusts the song tempo, where 1.0 = normal, 0.5 = half-speed, 2.0 = double-speed.
		/// <para></para>
		/// Value returned by gme_track_length() assumes tempo = 1.0.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="tempo">The desired tempo to set the song to.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gme_set_tempo(IntPtr emuHandle, double tempo);

		/// <summary>
		/// Gets the number of voices being used by the currently loaded file.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <returns>The number of voices being used by the currently loaded file.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gme_voice_count(IntPtr emuHandle);

		/// <summary>
		/// Gets the name of voice 'i'.
		/// <para></para>
		/// Valid values of i are from 0 - gme_voice_count() - 1
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="i">The voice to get the name of.</param>
		/// <returns>The name of the voice specified by i.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr gme_voice_name(IntPtr emuHandle, int i);

		/// <summary>
		/// Sets the muting state of all voices at once using a bitmask; 
		/// where -1 mutes all voices. <para></para>
		/// 0 unmutes them all.        <para></para>
		/// 0x01 mutes just the first voice, etc.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="mutingMask">The bitmask to mute the voices based off of.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gme_mute_voices(IntPtr emuHandle, int mutingMask);

		/// <summary>
		/// Gets the current frequency equalizer parameters.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="output">The output that provides the current frequency equalizer parameters.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern void gme_equalizer(IntPtr emuHandle, out GmeEqualizer output);

		/// <summary>
		/// Sets the equalizer parameters to ones defines by the new equalizer reference.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="equalizer">The new equalizer to set.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern void gme_set_equalizer(IntPtr emuHandle, ref GmeEqualizer equalizer);

		/// <summary>
		/// Enables/Disables the most accurate sound emulation options.
		/// </summary>
		/// <param name="emuHandle">The IntPtr handle to a MusicEmu reference.</param>
		/// <param name="enabled">Whether or not to enable accurate sound emulation.</param>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gme_enable_accuracy(IntPtr emuHandle, bool enabled);


		// Game Music Types


		/// <summary>
		/// Gets the type of emulator.
		/// </summary>
		/// <param name="emuHandle">An IntPtr handle to a MusicEmu reference.</param>
		/// <returns>The type of emulator.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr gme_type(IntPtr emuHandle);

		/// <summary>
		/// Gets the name of the game system for this music file type.
		/// </summary>
		/// <param name="type">The music file type.</param>
		/// <returns>The name of the game system for this music file type.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		public static extern string gme_type_system(ref GmeType type);

		/// <summary>
		/// Checks if this music file type supports multiple tracks.
		/// </summary>
		/// <param name="type">The music file type.</param>
		/// <returns>True if this music file type supports multiple tracks. False otherwise.</returns>
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool gme_type_multitrack(ref GmeType type);

		#endregion
	}
}
