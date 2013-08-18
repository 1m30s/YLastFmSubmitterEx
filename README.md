YLastFmSubmitter
======================

This software submits now playing and scrobble information to your last.fm account through command-line.

Command-line based feature makes it easy that your programs written by any programming language use last.fm APIs.

This software utilizes LPFM Last.fm Scrobbler DLL. Since this dll contains some bugs, I fixed those and included all the source code in this project file.


## Supported Environment
Windows XP or later.

## Compiler
Microsoft Visual C# 2010

# Usage
* "YLastFmSubmitter help"
* For more information, read source code.

## Command
* 1) send current playing song info
    * play "[SongTitle]" "[Artist]" [duration(sec)]
* 2) send scrobbling info (same song to last "play" command)
    * scrobble
* 3) quit application
    * q


# License

The MIT License (MIT)

Copyright (c) 2011 - 2012 Daniel Larsen, Adam Haile

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in 
the Software without restriction, including without limitation the rights to 
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
of the Software, and to permit persons to whom the Software is furnished to do 
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
