# Game Music Emu Sharp
----
## What is it?

Game Music Emu Sharp is a fully documented wrapper around almost all of the features of Game Music Emu (https://code.google.com/p/game-music-emu/)

This allows for the possibility of multiple C# based implementations of the sound playback.

## Do I have to care about freeing things.

No, this is all done in the background with method calls that would usually require this in C++. (Though if you spot anything wrong, please tell me!).

The only exception to this is closing the player. When you are done, simply call the ```GmeNative.gme_delete(IntPtr emuHandle)``` method.

## How do I use this

Sound playback isn't built in, just like the original library. You need to implement it with an audio system (I am working on NAudio, which is almost done).

For the time being, here is how to get info from the files you load.

```
string fileName = "[path to music file here]";
int track = 0;          // The track we'll get info from.
int sampleRate = 48000; // Sample rate to use.

// Get an emulator handle.
IntPtr emuHandle = GmeNative.OpenFile(fileName, sampleRate);

// Get info about the track (Note that many of the supported formats can have more than one track in them).
GmeTrackInfo trackInfo = GmeNative.GetTrackInfo(emuHandle, track);

// Print out the track info.

Console.WriteLine("Game: "   + trackInfo.game);
Console.WriteLine("System: " + trackInfo.system);
Console.WriteLine("Artist: " + trackInfo.author);
// ... etc

```