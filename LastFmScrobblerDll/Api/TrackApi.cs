﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.XPath;

namespace Lpfm.LastFmScrobbler.Api
{
    internal class TrackApi : ITrackApi
    {
        private const string UpdateNowPlayingMethodName = "track.updateNowPlaying";
        private const string ScrobbleMethodName = "track.scrobble";
        private const string LoveMethodName = "track.love";
        private const string UnLoveMethodName = "track.unlove";
        private const string BanMethodName = "track.ban";
        private const string UnBanMethodName = "track.unban";

        /// <summary>
        /// Instantiates a <see cref="TrackApi"/>
        /// </summary>
        public TrackApi()
            : this(new WebRequestRestApi())
        {
        }

        /// <summary>
        /// Instantiates a <see cref="TrackApi"/>
        /// </summary>
        public TrackApi(IRestApi restApi)
        {
            RestApi = restApi;
        }

        private IRestApi RestApi { get; set; }

        #region ITrackApi Members

        /// <summary>
        /// Notifies Last.fm that a user has started listening to a track. 
        /// </summary>
        /// <param name="track">A <see cref="Track"/> DTO containing track details</param>
        /// <param name="context">Optional. Sub-client version (not public, only enabled for certain API keys). Null if not used</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>        
        /// <returns>A <see cref="ScrobbleResponse"/>DTO containing details of Last.FM's response</returns>
        /// <remarks>It is important to not use the corrections returned by the now playing service as input for the scrobble request, 
        /// unless they have been explicitly approved by the user</remarks>
        public NowPlayingResponse UpdateNowPlaying(Track track, string context, Authentication authentication)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Notifies Last.fm that a user has started listening to a track. 
        /// </summary>
        /// <param name="track">A <see cref="Track"/> DTO containing track details</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>        
        /// <returns>A <see cref="ScrobbleResponse"/>DTO containing details of Last.FM's response</returns>
        /// <remarks>It is important to not use the corrections returned by the now playing service as input for the scrobble request, 
        /// unless they have been explicitly approved by the user</remarks>
        public NowPlayingResponse UpdateNowPlaying(Track track, Authentication authentication)
        {
            Dictionary<string, string> parameters = TrackToNameValueCollection(track);

            ApiHelper.AddRequiredParams(parameters, UpdateNowPlayingMethodName, authentication);

            // send request
            var navigator = RestApi.SendPostRequest(ApiHelper.LastFmWebServiceRootUrl, parameters);

            return GetNowPlayingResponseFromNavigator(navigator);
        }

        /// <summary>
        /// Notifies Last.FM that the user UnLoves the track
        /// </summary>
        /// <param name="track">A <see cref="Track"/> DTO containing track details</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>     
        /// <returns>int LastFM return code. 0 is Success, above 0 is failure</returns>
        public RatingResponse Love(Track track, Authentication authentication)
        {
            return RateTrack(track, authentication, LoveMethodName);
        }

        /// <summary>
        /// Notifies Last.FM that the user Loves the track
        /// </summary>
        /// <param name="track">A <see cref="Track"/> DTO containing track details</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>     
        /// <returns>int LastFM return code. 0 is Success, above 0 is failure</returns>
        public RatingResponse UnLove(Track track, Authentication authentication)
        {
            return RateTrack(track, authentication, UnLoveMethodName);
        }

        /// <summary>
        /// Notifies Last.FM that the user wants to Ban the track
        /// </summary>
        /// <param name="track">A <see cref="Track"/> DTO containing track details</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>     
        /// <returns>int LastFM return code. 0 is Success, above 0 is failure</returns>
        public RatingResponse Ban(Track track, Authentication authentication)
        {
            return RateTrack(track, authentication, BanMethodName);
        }

        /// <summary>
        /// Notifies Last.FM that the user wants to UnBan the track
        /// </summary>
        /// <param name="track">A <see cref="Track"/> DTO containing track details</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>     
        /// <returns>int LastFM return code. 0 is Success, above 0 is failure</returns>
        public RatingResponse UnBan(Track track, Authentication authentication)
        {
            return RateTrack(track, authentication, UnBanMethodName);
        }

        private RatingResponse RateTrack(Track track, Authentication authentication, string method)
        {
            Dictionary<string, string> parameters = TrackToNameValueCollection(track);

            ApiHelper.AddRequiredParams(parameters, method, authentication);

            // send request
            var navigator = RestApi.SendPostRequest(ApiHelper.LastFmWebServiceRootUrl, parameters);

            return GetRatingResponseFromNavigator(navigator);
        }

        /// <summary>
        /// Add a track-play to a user's profile
        /// </summary>
        /// <param name="track">A <see cref="Track"/> DTO containing track details</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>
        /// <returns>A <see cref="ScrobbleResponse"/>DTO containing details of Last.FM's response</returns>
        public ScrobbleResponse Scrobble(Track track, Authentication authentication)
        {
            Dictionary<string, string> parameters = TrackToNameValueCollection(track);

            ApiHelper.AddRequiredParams(parameters, ScrobbleMethodName, authentication);

            // send request
            var navigator = RestApi.SendPostRequest(ApiHelper.LastFmWebServiceRootUrl, parameters);

            return GetScrobbleResponseFromNavigator(navigator);
        }

        /// <summary>
        /// Add a track-play to a user's profile
        /// </summary>
        /// <param name="track">A <see cref="Track"/> DTO containing track details</param>
        /// <param name="context">Optional. Sub-client version (not public, only enabled for certain API keys). Null if not used</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>
        /// <returns>A <see cref="ScrobbleResponse"/>DTO containing details of Last.FM's response</returns>
        public ScrobbleResponse Scrobble(Track track, string context, Authentication authentication)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add a track-play to a user's profile
        /// </summary>
        /// <param name="tracks">A list of <see cref="Track"/></param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>
        /// <returns>A <see cref="ScrobbleResponse"/>DTO containing details of Last.FM's response</returns>
        public ScrobbleResponses Scrobble(IList<Track> tracks, Authentication authentication)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add a track-play to a user's profile
        /// </summary>
        /// <param name="tracks">A list of <see cref="Track"/></param>
        /// <param name="context">Optional. Sub-client version (not public, only enabled for certain API keys). Null if not used</param>
        /// <param name="authentication"><see cref="Authentication"/> object</param>
        /// <returns>A <see cref="ScrobbleResponse"/>DTO containing details of Last.FM's response</returns>
        public ScrobbleResponses Scrobble(IList<Track> tracks, string context, Authentication authentication)
        {
            throw new NotImplementedException();
        }

        #endregion


        protected RatingResponse GetRatingResponseFromNavigator(XPathNavigator navigator)
        {
            if (ApiHelper.SelectSingleNode(navigator, "/lfm/@status").Value == "ok")
                return new RatingResponse() { ErrorCode = 0 };
            else
            {
                return new RatingResponse() { ErrorCode = ApiHelper.SelectSingleNode(navigator, "/lfm/error/@code").ValueAsInt };
            }
        }

        protected ScrobbleResponse GetScrobbleResponseFromNavigator(XPathNavigator navigator)
        {
            var responses = GetScrobbleResponsesFromNavigator(navigator);
            if (responses.Count > 1) throw new InvalidOperationException("More than one scrobble response returned. One or zero expected");
            return responses.FirstOrDefault();
        }

        protected ScrobbleResponses GetScrobbleResponsesFromNavigator(XPathNavigator navigator)
        {
            var responses = new ScrobbleResponses
                                {
                                    AcceptedCount = ApiHelper.SelectSingleNode(navigator, "/lfm/scrobbles/@accepted").ValueAsInt,
                                    IgnoredCount = ApiHelper.SelectSingleNode(navigator, "/lfm/scrobbles/@ignored").ValueAsInt
                                };

            foreach (XPathNavigator item in navigator.Select("/lfm/scrobbles/scrobble"))
            {
                var response = new ScrobbleResponse
                                   {
                                       IgnoredMessageCode = ApiHelper.SelectSingleNode(item, "ignoredMessage/@code").ValueAsInt,
                                       IgnoredMessage = ApiHelper.SelectSingleNode(item, "ignoredMessage").Value,
                                       Track = GetCorrectedTrack(item)
                                   };

                responses.Add(response);
            }

            return responses;
        }

        protected NowPlayingResponse GetNowPlayingResponseFromNavigator(XPathNavigator navigator)
        {
            var item = navigator.SelectSingleNode("/lfm/nowplaying");

            var response = new NowPlayingResponse
                               {
                                   IgnoredMessageCode = ApiHelper.SelectSingleNode(item, "ignoredMessage/@code").ValueAsInt,
                                   IgnoredMessage = ApiHelper.SelectSingleNode(item, "ignoredMessage").Value,
                                   Track = GetCorrectedTrack(item)
                               };

            return response;
        }

        private static CorrectedTrack GetCorrectedTrack(XPathNavigator item)
        {
            var track = new CorrectedTrack();
            track.TrackNameCorrected = ApiHelper.SelectSingleNode(item, "track/@corrected").ValueAsBoolean;
            track.TrackName = ApiHelper.SelectSingleNode(item, "track").Value;

            track.ArtistNameCorrected = ApiHelper.SelectSingleNode(item, "artist/@corrected").ValueAsBoolean;
            track.ArtistName = ApiHelper.SelectSingleNode(item, "artist").Value;

            track.AlbumNameCorrected = ApiHelper.SelectSingleNode(item, "album/@corrected").ValueAsBoolean;
            track.AlbumName = ApiHelper.SelectSingleNode(item, "album").Value;

            track.AlbumArtistCorrected = ApiHelper.SelectSingleNode(item, "albumArtist/@corrected").ValueAsBoolean;
            track.AlbumArtist = ApiHelper.SelectSingleNode(item, "albumArtist").Value;

            return track;
        }

        protected virtual Dictionary<string, string> TrackToNameValueCollection(Track track)
        {
            var nameValues = new Dictionary<string, string>();

            Validator.ValidateObject(track, new ValidationContext(track, null, null));

            nameValues.Add(Track.ArtistNameParamName, track.ArtistName);
            nameValues.Add(Track.TrackNameParamName, track.TrackName);

            if (!string.IsNullOrEmpty(track.AlbumArtist)) nameValues.Add(Track.AlbumArtistParamName, track.AlbumArtist);
            if (!string.IsNullOrEmpty(track.AlbumName)) nameValues.Add(Track.AlbumNameParamName, track.AlbumName);
            //nameValues.Add(Track.DurationParamName, ((int)track.Duration.TotalSeconds).ToString());
            if (track.MusicBrainzId > 0) nameValues.Add(Track.MusicBrainzIdParamName, track.MusicBrainzId.ToString());
            if (track.TrackNumber > 0) nameValues.Add(Track.TrackNumberParamName, track.TrackNumber.ToString());

            if (track.WhenStartedPlaying.HasValue)
            {
                nameValues.Add(Track.WhenStartedPlayingParamName, DateTimeToTimestamp(track.WhenStartedPlaying.Value).ToString());
            }

            return nameValues;
        }

        /// <summary>
        /// Returns the local time value of a DateTime object to a UTC UNIX timestamp format (integer number of seconds since 00:00:00, January 1st 1970 UTC).           
        /// </summary>
        protected virtual int DateTimeToTimestamp(DateTime dateTime)
        {
            DateTime utcDateTime = dateTime.ToUniversalTime();
            var jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0);

            TimeSpan timeSinceJan1St1970 = utcDateTime.Subtract(jan1St1970);
            return (int) timeSinceJan1St1970.TotalSeconds;
        }
    }
}