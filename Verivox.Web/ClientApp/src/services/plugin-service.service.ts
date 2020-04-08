import { Injectable } from '@angular/core';
import { AppConfig } from '../assets/config/app.config';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { PluginRequest } from '../models/plugin.request';

@Injectable({
  providedIn: 'root'
})
export class PluginService {
  private pluginEndpoint = AppConfig.settings.apiServer.pluginEndpoint;
  private pluginObserve: Observable<CommonResponse<PluginCollectionResponse>>;
  constructor(private http: HttpClient) {
    this.pluginObserve = http.get<CommonResponse<PluginCollectionResponse>>(this.pluginEndpoint);
  }
  PluginHttp() {
    return this.pluginObserve;
  }
  Install(item: PluginRequest) {
    return this.http.post<CommonResponse<PluginCollectionResponse>>(this.pluginEndpoint, item);
  }
}
