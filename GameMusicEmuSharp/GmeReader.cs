﻿using System;
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
		private readonly GmeTrackInfo trackInfo;
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
			emuHandle = GmeNative.OpenFile(fileName, sampleRate);

			// Enable accurate sound emulation.
			GmeNative.gme_enable_accuracy(emuHandle, true);

			// Get track info
			trackInfo = GmeNative.GetTrackInfo(emuHandle, track);

			// Init the wave format.
			waveFormat = new WaveFormat(sampleRate, 16, channels);
		}

		#endregion

		#region Properties

		public override WaveFormat WaveFormat
		{
			get { return waveFormat; }
		}

		public override long Length
		{
			get { return trackInfo.playLength; }
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

		#endregion
	}
}