using System;
using GameMusicEmuSharp;
using NAudio.Wave;

// TODO: Fix .vgz playback. (needs zlib I think?)

namespace GameMusicEmuSharpExample
{
	/// <summary>
	/// Class for reading files supported by Game Music Emu
	/// </summary>
	public class GmeReader : WaveStream
	{
		private readonly IntPtr emuHandle;
		private readonly GmeTrackInfo trackInfo;
		private readonly WaveFormat waveFormat;

		private const int sampleRate = 48000;
		private const int channels = 2;

		private int track = 0;
		private bool isPlaying;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fileName">File to open using Game Music Emu.</param>
		public GmeReader(string fileName)
		{
			// Open the file.
			emuHandle = GmeNative.OpenFile(fileName, sampleRate);

			// Enable accurate sound emulation.
			GmeNative.gme_enable_accuracy(emuHandle, true);

			// Get track info
			trackInfo = GmeNative.GetTrackInfo(emuHandle, track);

			// Init the wave format.
			waveFormat = new WaveFormat(sampleRate, 16, channels);
		}

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

		public override WaveFormat WaveFormat
		{
			get { return waveFormat; }
		}

		public override long Length
		{
			get { return trackInfo.playLength; }
		}

		// TODO: Get this working correctly.
		public override long Position
		{
			get { return GmeNative.gme_tell(emuHandle); }
			set { GmeNative.gme_seek(emuHandle, (int)value / waveFormat.BlockAlign); }
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
	}
}
