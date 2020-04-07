import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PluginService } from '../../services/plugin-service.service';
import { PluginRequest } from '../../models/plugin.request';

@Component({
  selector: 'app-install-plugin',
  templateUrl: './install-plugin.component.html'
})
export class InstallPluginComponent {
  public pluginsmodel: PluginCollectionResponse;
  public PluginService: PluginService;
  constructor(private pluginService: PluginService) {
    this.PluginService = pluginService;
    this.LoadAll();
  }

  LoadAll() {
    this.PluginService.PluginHttp().subscribe(result => {
      debugger;
      this.pluginsmodel = result;
    }, error => console.error(error));
  }

  public Install(name, flag) {
    this.PluginService.Install(new PluginRequest(name, flag)).subscribe(result => {
      debugger;
      this.pluginsmodel = result;
    }, error => console.error(error));
  }
}
