using System.Runtime.InteropServices;

namespace GameMusicEmuSharp
{
	/// <summary>
	/// Represents a gme_info_t struct.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct GmeTrackInfo
	{
		/* Times are in milliseconds. -1 if unknown. */

		/// <summary>Total length, if the file specifies it.</summary>
		public int length;

		/// <summary>Length of the song up to the looping section.</summary>
		public int introLength;

		/// <summary>The length of the looping section.</summary>
		public int loopLength;

		/// <summary>
		/// Length if available, otherwise this is equal to (introLength + loopLength * 2) if available.
		/// Otherwise this is set to a default of 150000 (2.5 minutes).
		/// </summary>
		public int playLength;

		// Reserved
		private readonly int i4, i5, i6, i7, i8, i9, i10, i11, i12, i13, i14, i15;

		// The following are set to empty strings if they are not available.
		
		/// <summary>The game system this track is from.</summary>
		public string system;

		/// <summary>The game this track is from.</summary>
		public string game;

		/// <summary>The name of this track.</summary>
		public string song;

		/// <summary>The author of this track.</summary>
		public string author;

		/// <summary>The copyright of this track.</summary>
		public string copyright;

		/// <summary>The comment in this track.</summary>
		public string comment;

		/// <summary>The person that dumped this music file.</summary>
		public string dumper;

		// Reserved.
		private readonly string s7, s8, s9, s10, s11, s12, s13, s14, s15;
	}
}
