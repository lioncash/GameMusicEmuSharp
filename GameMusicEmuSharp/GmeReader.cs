using System;
using NAudio.Wave;

// TODO: Fix .vgz playback. (needs zlib I think?)

namespace GameMusicEmuSharp
{
	/// <summary>
	/// Class for reading files supported by Game Music Emu
	/// </summary>
	public class GmeReader : WaveStream
	{
		#region Fields

		private readonly IntPtr emuHandle;
		private readonly WaveFormat waveFormat;

		private int track = 0;
		private bool isPlaying;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fileName">File to open using Game Music Emu.</param>
		/// <param name="sampleRate">Desired sample rate. Defaults to 44100Hz if not specified.</param>
		/// <param name="channels">Number of channels to use. Defaults to two if not specified.</param>
		public GmeReader(string fileName, int sampleRate=44100, int channels=2)
		{
			// Open the file.
			this.emuHandle = GmeNative.OpenFile(fileName, sampleRate);

			// Enable accurate sound emulation.
			GmeNative.gme_enable_accuracy(emuHandle, true);

			// Get track info
			this.TrackInfo = GmeNative.GetTrackInfo(emuHandle, track);
			this.TrackCount = GmeNative.gme_track_count(emuHandle);
			this.VoiceCount = GmeNative.gme_voice_count(emuHandle);
			this.Equalizer = GmeNative.GetEqualizer(emuHandle);
			this.Type = GmeNative.GetType(emuHandle);

			GmeType tempType = this.Type; // Since properties can't be used in ref arguments.
			this.SupportsMultipleTracks = GmeNative.gme_type_multitrack(ref tempType);

			// Init the wave format.
			this.waveFormat = new WaveFormat(sampleRate, 16, channels);
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~GmeReader()
		{
			// Free the handle acquired for GameMusicEmu.
			GmeNative.gme_delete(emuHandle);
		}

		#endregion

		#region Properties

		/// <summary>
		/// The number of tracks in the currently loaded file.
		/// </summary>
		public int TrackCount
		{
			get;
			private set;
		}

		/// <summary>
		/// Track info for the currently loaded track.
		/// </summary>
		public GmeTrackInfo TrackInfo
		{
			get;
			private set;
		}

		/// <summary>
		/// The number of voices in this track.
		/// </summary>
		public int VoiceCount
		{
			get;
			private set;
		}

		/// <summary>
		/// The current equalizer for this track.
		/// </summary>
		public GmeEqualizer Equalizer
		{
			get;
			private set;
		}

		/// <summary>
		/// The type of file format being played/read.
		/// </summary>
		public GmeType Type
		{
			get;
			private set;
		}

		/// <summary>
		/// Whether or not the loaded format supports
		/// multiple tracks within a single file.
		/// </summary>
		public bool SupportsMultipleTracks
		{
			get;
			private set;
		}

		public override WaveFormat WaveFormat
		{
			get { return waveFormat; }
		}

		public override long Length
		{
			get { return TrackInfo.playLength; }
		}

		public override long Position
		{
			get { return (GmeNative.gme_tell_samples(emuHandle) / waveFormat.SampleRate) * waveFormat.AverageBytesPerSecond; }
			set { GmeNative.gme_seek_samples(emuHandle, (int)value / waveFormat.BlockAlign); }
		}

		#endregion

		#region Methods

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!isPlaying)
			{
				GmeNative.gme_start_track(emuHandle, track++);
				isPlaying = true;
			}

			// Make a buffer and fill it.
			short[] sBuffer = new short[count / 2];
			GmeNative.gme_play(emuHandle, count / 2, sBuffer);

			// Convert the short samples to byte samples and place them
			// in the NAudio byte sample buffer.
			Buffer.BlockCopy(sBuffer, 0, buffer, 0, buffer.Length);

			return buffer.Length;
		}

		/// <summary>
		/// Sets the track to play
		/// </summary>
		/// <param name="trackNum">The track to play</param>
		public void SetTrack(int trackNum)
		{
			// Set that we aren't playing.
			this.isPlaying = false;
			this.track = trackNum;
		}

		/// <summary>
		/// Gets the name of a voice in the track.
		/// </summary>
		/// <param name="voiceIndex">Voice index of the voice to get the name of.</param>
		/// <returns>The name of the voice specified by the given index.</returns>
		public string GetVoiceName(int voiceIndex)
		{
			return GmeNative.GetVoiceName(emuHandle, voiceIndex);
		}

		#endregion
	}
}
