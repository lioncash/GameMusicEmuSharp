using System.Runtime.InteropServices;

namespace GameMusicEmuSharp
{
	/// <summary>
	/// Represents the gme_equalizer_t.
	/// <para></para>
	/// Basically an equalizer for GME.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GmeEqualizer
	{
		/// <summary>
		/// Treble.
		/// <para></para>
		/// -50.0 = muffled. <para></para>
		/// 0 = flat.        <para></para>
		/// +5.0 = extra crisp.
		/// </summary>
		public double Treble;

		/// <summary>
		/// Bass.
		/// <p/>
		/// 1 = Full bass.<para></para>
		/// 90 = Average. <para></para>
		/// 16000 = Almost no bass.
		/// </summary>
		public double Bass;

		// Reserved
		private double d2, d3, d4, d5, d6, d7, d8, d9;
	}
}
