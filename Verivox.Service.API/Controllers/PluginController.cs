using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Verivox.Common;
using Verivox.Common.Plugins;
using Verivox.Service.API.Models;

namespace Verivox.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PluginController : ControllerBase
    {
        private readonly IPluginService _pluginService;
        private readonly IWebHelper _webHelper;

        public PluginController(IPluginService pluginService, IWebHelper webHelper)
        {
            this._pluginService = pluginService;
            this._webHelper = webHelper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Plugin>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            var result = new PluginCollection
            {
                Plugins = _pluginService.GetPluginDescriptors<IPlugin>().Select(s => new Plugin { Description = s.Description, Name = s.SystemName, Installed = s.Installed }),
                NeedToRestart = _pluginService.NeedToRestart
            };
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Plugin>), (int)HttpStatusCode.OK)]
        public IActionResult Install(Plugin data)
        {
            var pluginDescriptor = _pluginService.GetPluginDescriptorBySystemName<IPlugin>(data.Name);
            if (pluginDescriptor == null)
                return GetAll();

            if (data.Installed)
            {
                if (pluginDescriptor.Installed)
                    return GetAll();
                _pluginService.PreparePluginToInstall(data.Name);
                _pluginService.InstallPlugins();
            }
            else
            {
                if (!pluginDescriptor.Installed)
                    return GetAll();
                _pluginService.PreparePluginToUninstall(data.Name);
                _pluginService.UninstallPlugins();
            }
            return GetAll();
        }
    }
}
