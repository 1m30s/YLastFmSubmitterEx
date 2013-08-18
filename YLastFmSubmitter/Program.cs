using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YLastFmSubmitter
{
    class Program
    {

        static int Main(string[] args)
        {
            if (args.Length != 0)
            {
                Console.WriteLine("YLastFmSubmitter Usage:");

                Console.WriteLine("play \"Title\" \"Artist\" \tSubmit CurrentPlaying");
                Console.WriteLine("scrobble \tSubmit Scrobbling");
                Console.WriteLine("stop \tSubmit Stop");
                Console.WriteLine("q \tQuit Application");
                return 0;
            }
            //Console.WriteLine(args.Length);

            var scr = new YScrobbler();

            while (true)
            {
                // 行読み込み
                string line = Console.ReadLine() + "\r\n";
                if (line == null) break;
                
                string command, arg1, arg2;
                int duration;
                // 解析
                int cnt = TokenizeLine(line, out command, out arg1, out arg2, out duration);
                if (command == "q")
                {
                    break;
                }
                else if (command == "stop")
                {
                    string str = "stop";
                    Console.WriteLine(str);
                }
                else if (command == "play")
                {
                    if (cnt != 4 && arg1.Length != 0 && arg2.Length != 0) {
                        Console.WriteLine("Error: Invalid args.");
                    } else {
                        string str = "play: " + arg1 + " " + arg2;
                        Console.WriteLine(str);
                        scr.Send(0, arg1, arg2, duration);
                    }
                } else if (command == "scrobble") {
                    if (cnt != 1) {
                        Console.WriteLine("Error: Invalid args.");
                    } else {
                        string str = "scrobble last track.";
                        Console.WriteLine(str);
                        scr.Send(1, "", "", 0);
                    }
                }
                else {
                    Console.WriteLine("Error: Invalid command.");
                }
            }


            return 0;
        }


        private static int TokenizeLine(string line, out string command, out string arg1, out string arg2, out int duration) {
            command = arg1 = arg2 = "";
            duration = 0;
            int tokenCnt = 0;
            int quotation = 0; // quotation of next token (0: not processed, 1: in quotation, 2: out of quotaiton, 3: no quotation
            string buf = "";

            // Tokeninze
            int i;
            for (i = 0; i < line.Length; i++) {
                // Process as a state machine
                if ((quotation != 1) && line[i] == ' ') {
                    tokenCnt++;
                    while (line[i + 1] == ' ') i++; // skip continuous space

                    quotation = 0; // quotation of next token
                    if (line[i + 1] != '\"') quotation = 3;
                } else if (line[i] == '\n' || line[i] == '\r' || line[i] == '\0') {// end
                    tokenCnt++;
                    break;

                } else // inter.
                {
                    if (tokenCnt == 0) {
                        command += line[i];
                    } else if (tokenCnt == 1) {
                        if (quotation == 0) {
                            if (line[i] == '\"') quotation = 1;
                        } else if (quotation == 1) {
                            if (line[i] == '\"') quotation = 2;
                            else arg1 += line[i];
                        } else if (quotation == 2) {
                        } else if (quotation == 3) {
                            arg1 += line[i];
                        }
                    } else if (tokenCnt == 2) {
                        if (quotation == 0) {
                            if (line[i] == '\"') quotation = 1;
                        } else if (quotation == 1) {
                            if (line[i] == '\"') quotation = 2;
                            else arg2 += line[i];
                        } else if (quotation == 2) {
                        } else if (quotation == 3) {
                            arg2 += line[i];
                        }
                    }else if (tokenCnt == 3) {
                        buf += line[i];
                    }
                }
            }
            if (tokenCnt >= 4) {
                try {
                    duration = Int32.Parse(buf);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception.Message);
                }
            }
            return tokenCnt;
        }
    }
}
