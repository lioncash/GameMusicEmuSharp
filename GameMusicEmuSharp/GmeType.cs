using System;
using System.Runtime.InteropServices;

namespace GameMusicEmuSharp
{
	/// <summary>
	/// Represents a GME compatible file type.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct GmeType
	{
		/// <summary>Name of the system this music file type is usually for.</summary>>
		[MarshalAs(UnmanagedType.LPStr)]
		public string system;

		/// <summary>Non-zero for formats with a fixed number of tracks.</summary>
		public int trackCount;

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr newemu();  // C++ only
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate IntPtr newInfo(); // C++ only

		// Internal
		private string extension;
		private int flags;
	}
}
