using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Web.Http;
using Thinktecture.IdentityModel.Tokens.Http;

namespace DBI.Mobile.EMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            var authConfig = new AuthenticationConfiguration
            {
                SendWwwAuthenticateResponseHeaders = false,
                RequireSsl = true,
                EnableSessionToken = true
            };

            // setup authentication against membership
            authConfig.AddBasicAuthentication((userName, password) => DBI.Core.Security.Authentication.Authenticate(userName, password));

            config.MessageHandlers.Add(new AuthenticationHandler(authConfig));

        }
    }
}
