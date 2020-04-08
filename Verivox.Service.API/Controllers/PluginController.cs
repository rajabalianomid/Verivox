using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public PluginController(IPluginService pluginService, IWebHelper webHelper)
        {
            _pluginService = pluginService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Plugin>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            CommonResponse<PluginCollection> model = new CommonResponse<PluginCollection>();
            try
            {
                model.Result = new PluginCollection
                {
                    Plugins = _pluginService.GetPluginDescriptors<IPlugin>().Select(s => new Plugin
                    {
                        Description = s.Description,
                        Name = s.SystemName,
                        Installed = s.Installed
                    }),
                    NeedToRestart = _pluginService.NeedToRestart
                };
            }
            catch (Exception)
            {
                model.IsError = true;
                model.Message = "Occur an error, please try later!";
            }
            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Plugin>), (int)HttpStatusCode.OK)]
        public IActionResult Install(Plugin data)
        {
            PluginDescriptor pluginDescriptor = _pluginService.GetPluginDescriptorBySystemName<IPlugin>(data.Name);
            if (pluginDescriptor == null)
            {
                return GetAll();
            }

            if (data.Installed)
            {
                if (pluginDescriptor.Installed)
                {
                    return GetAll();
                }

                _pluginService.PreparePluginToInstall(data.Name);
                _pluginService.InstallPlugins();
            }
            else
            {
                if (!pluginDescriptor.Installed)
                {
                    return GetAll();
                }

                _pluginService.PreparePluginToUninstall(data.Name);
                _pluginService.UninstallPlugins();
            }
            return GetAll();
        }
    }
}
