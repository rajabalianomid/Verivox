import { Component, Input } from '@angular/core';
import { PluginService } from '../../services/plugin-service.service';
import { PluginRequest } from '../../models/plugin.request';

@Component({
  selector: 'app-install-plugin',
  templateUrl: './install-plugin.component.html'
})
export class InstallPluginComponent {
  @Input() public pluginsmodel: CommonResponse<PluginCollectionResponse>;
  constructor(private PluginService: PluginService) {
    this.LoadAll();
  }

  LoadAll() {
    this.PluginService.PluginHttp().subscribe(result => {
      this.pluginsmodel = result;
    }, error => console.error(error));
  }

  public Install(name, flag) {
    this.PluginService.Install(new PluginRequest(name, flag)).subscribe(result => {
      this.pluginsmodel = result;
    }, error => console.error(error));
  }
}
