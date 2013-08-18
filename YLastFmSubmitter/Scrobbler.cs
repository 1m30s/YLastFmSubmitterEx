using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection;
using Lpfm.LastFmScrobbler;
using Lpfm.LastFmScrobbler.Api;
using Microsoft.Win32;

namespace YLastFmSubmitter {
    class YScrobbler {
        //private const string LpfmRegistryNameSpace = "HKEY_CURRENT_USER\\Software\\LastFmScrobbler.SampleApplication";
        private const string SettingFileName = "LASSettings.xml";
        private Settings appSettings = new Settings();

        //TODO: Go to http://www.last.fm/api/account and apply for an API account. Then paste the key and secret into these constants
        private const string ApiKey = "2f0cf15a0e28a9bf580744b4861c4f01";
        private const string ApiSecret = "d6de39d1df05473074cb27d0f44c9eb4";

        private readonly Scrobbler _scrobbler;
        private Track CurrentTrack { get; set; }

        public YScrobbler() {
            try {
                /*
                // Set up the Scrobbler
                if (string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(ApiSecret))
                {
                    throw new InvalidOperationException(
                        "ApiKey and ApiSecret have not been set. Go to http://www.last.fm/api/account and apply for an API account. Then paste the key and secret into the constants on PlayerForm.cs");
                }*/

                string sessionKey = GetSessionKey();

                // instantiate the async scrobbler
                _scrobbler = new Scrobbler(ApiKey, ApiSecret, sessionKey);
                CurrentTrack = new Track();
                Console.WriteLine("Ready.");
            }
            catch (Exception exception) {
                Console.WriteLine(exception.Message);
            }
        }
        /*
        private string GetRegistrySetting() {
            if (string.IsNullOrEmpty(valueName)) throw new ArgumentException("valueName cannot be empty or null", "valueName");
            valueName = valueName.Trim();

            object regValue = Registry.GetValue(LpfmRegistryNameSpace, valueName, defaultValue);

            if (regValue == null) {
                // Key does not exist
                return defaultValue;
            } else {
                return regValue.ToString();
            }
        }*/
        private void SaveSettings(){
            //＜XMLファイルに書き込む＞

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(Settings));

            Assembly myAssembly = Assembly.GetEntryAssembly();
            string myPath = System.IO.Path.GetDirectoryName(myAssembly.Location);

            //ファイルを開く
            try {
                System.IO.FileStream fs1 =
                    new System.IO.FileStream(myPath + '\\' + SettingFileName, System.IO.FileMode.Create);

                serializer.Serialize(fs1, appSettings);

                fs1.Close();
            }
            catch (Exception exception) {
                //    Console.WriteLine(exception.Message);
            }
        }
        private void LoadSettings() {
            //＜XMLファイルから読み込む＞

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(Settings));

            Assembly myAssembly = Assembly.GetEntryAssembly();
            string myPath = System.IO.Path.GetDirectoryName(myAssembly.Location);

            //ファイルを開く
            try {
                System.IO.FileStream fs2 =
                    new System.IO.FileStream(myPath + '\\' + SettingFileName, System.IO.FileMode.Open);

                appSettings = (Settings)serializer.Deserialize(fs2);

                fs2.Close();
            }
            catch (Exception exception) {
            //    Console.WriteLine(exception.Message);
            }
        }

        private string GetSessionKey() {

            LoadSettings();

            // try get the session key from the registry
            string sessionKey = appSettings.Key;

            if (string.IsNullOrEmpty(sessionKey)) {
                // instantiate a new scrobbler
                var scrobbler = new Scrobbler(ApiKey, ApiSecret);

                //NOTE: This is demo code. You would not normally do this in a production application
                while (string.IsNullOrEmpty(sessionKey)) {
                    // Try get session key from Last.fm
                    try {
                        sessionKey = scrobbler.GetSession();

                        // successfully got a key. Save it to the registry for next time
                        appSettings.Key = sessionKey;
                        SaveSettings();
                        //SetRegistrySetting(sessionKeyRegistryKeyName, sessionKey);
                    }
                    catch (LastFmApiException exception) {
                        // get session key from Last.fm failed
                    //    Console.WriteLine(exception.Message);

                        // get a url to authenticate this application
                        string url = scrobbler.GetAuthorisationUri();

                        // open the URL in the default browser
                        Process.Start(url);

                        // To enable easy authorisation controlled by other application, this must be the first response.
                        // An external application can check whether authentication is needed by checking first responce to the message below
                        Console.WriteLine("[Authenticates] Press RETURN when application authenticated."); 

                        Console.ReadLine();
                    }
                }
            }

            return sessionKey;
        }
        /*
        public static void SetRegistrySetting(string valueName, string value) {
            if (string.IsNullOrEmpty(valueName)) throw new ArgumentException("valueName cannot be empty or null", "valueName");
            valueName = valueName.Trim();

            Registry.SetValue(LpfmRegistryNameSpace, valueName, value);
        }*/

        public void Send(int type, string title, string artist, int duration) {
            try {

                if (type == 0) { // NowPlaying
                    // Send
                    CurrentTrack.TrackName = title;
                    CurrentTrack.ArtistName = artist;
                    CurrentTrack.WhenStartedPlaying = DateTime.Now;
                    CurrentTrack.Duration = new TimeSpan(0,0,duration);

                    _scrobbler.NowPlaying(CurrentTrack);
                } else if (type == 1) { // NowPlaying
                    _scrobbler.Scrobble(CurrentTrack);
                }
            }
            catch (Exception exception) {
                Console.WriteLine(exception.Message);
            }
        }

        private TimeSpan TimeSpan(int p) {
            throw new NotImplementedException();
        }
    }
}
