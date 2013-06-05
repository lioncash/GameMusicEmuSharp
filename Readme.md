# Game Music Emu Sharp
----
## What is it?

Game Music Emu Sharp is a fully documented wrapper around almost all of the features of Game Music Emu (https://code.google.com/p/game-music-emu/)

This allows for the possibility of multiple C# based implementations of the sound playback.

## Do I have to care about freeing things?

No, this is all done in the background with method calls that would usually require this in C++. (Though if you spot anything wrong, please tell me!).

The only exception to this is closing the player. When you are done, simply call the ```GmeNative.gme_delete(IntPtr emuHandle)``` method.

## How do I use this?

Sound playback isn't built in, just like the original library. You need to implement it with an audio system (See the GameMusicEmuSharpExample folder for an example NAudio implementation).
