﻿using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CodeValue.SuiteValue.UI.Metro.TwitterAuthentication;

namespace LinqToTwitter.OAuth
{
    public class WinRtAuthorizer : OAuthAuthorizer, ITwitterAuthorizer
    {

        /// <summary>
        /// Url that Twitter redirects to after user authorizes your app
        /// </summary>
        public Uri Callback { get; set; }


        /// <summary>
        /// Perform authorization
        /// </summary>
        public void Authorize()
        {
        }

        public async Task<WinRtAuthorizer> AuthorizeAsync()
        {
            dynamic configuration = new ExpandoObject();
            configuration.TwitterClientId = this.Credentials.ConsumerKey;
            configuration.TwitterClientSecret = this.Credentials.ConsumerSecret;
            configuration.TwitterRedirectUrl = Callback.ToString();
            var twitAuthentication = new TwitterAuthProvider();
            twitAuthentication.Configure(configuration);
            var user = await twitAuthentication.Authenticate();
            OAuthTwitter.OAuthToken = twitAuthentication.OAuthToken;
            OAuthTwitter.OAuthTokenSecret = twitAuthentication.OAuthTokenSecret;
            Credentials.ScreenName = user.UserName;
            Credentials.UserId = user.Id;
            Credentials.OAuthToken = twitAuthentication.OAuthToken;
            Credentials.AccessToken = OAuthTwitter.OAuthTokenSecret;
            return this;
        }

    }
}
