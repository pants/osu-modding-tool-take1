# osu-modding-tool-v1

I originally planned to rewrite this one more time to be more how I wanted and  cleaner but it's unlikely I'll ever revisit this.
The code is messy, but quite a bit of reading went into this such as learning the syntax for 
[CLR](https://en.wikipedia.org/wiki/Common_Language_Runtime) and patching/minipulating it with [dnlib](https://github.com/0xd4d/dnlib). 
Seemed like a waste to not do anything with the project.

This is was a rough first take of an osu! (pre-laser) modding, automatic class mapping (across updates, finds new names for obfuscated class/method name changes), and patching tool. 

Designed for practice in mind. 
Score submitting is disabled and has a setting at startup for offline mode, if you try to get this to run I don't recommend using it online even with no submitting.

Features include:
* A GUI for modifying settings
  * Approach Rate
  * DoubleTime Speed
  * Overall Difficulty
  * Circle Size
  
* Private server switcher at start up

Video of it in action:
https://youtu.be/Q2gpteMYSQk
