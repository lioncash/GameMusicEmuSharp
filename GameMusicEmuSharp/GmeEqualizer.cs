using System.Runtime.InteropServices;

namespace GameMusicEmuSharp
{
	/// <summary>
	/// Represents the gme_equalizer_t.
	/// <p/>
	/// Basically an equalizer for GME.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GmeEqualizer
	{
		/// <summary>
		/// Treble.
		/// <p/>
		/// -50.0 = muffled. <br/>
		/// 0 = flat. <br/>
		/// +5.0 = extra crisp.
		/// </summary>
		public double Treble;

		/// <summary>
		/// Bass.
		/// <p/>
		/// 1 = Full bass.<br/>
		/// 90 = Average. <br/>
		/// 16000 = Almost no bass.
		/// </summary>
		public double Bass;

		// Reserved
		private double d2, d3, d4, d5, d6, d7, d8, d9;
	}
}
