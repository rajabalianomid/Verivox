using Microsoft.AspNetCore.Hosting;
using System;

namespace Verivox.Common
{
    public class WebHelper : IWebHelper
    {
        private readonly IApplicationLifetime _applicationLifetime;
        public WebHelper(IApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }
        public void RestartAppDomain()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                _applicationLifetime.StopApplication();
        }
    }
}
