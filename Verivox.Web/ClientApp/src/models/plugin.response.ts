interface PluginResponse {
  Name: string;
  Description: number;
  Installed: boolean;
}
interface PluginCollectionResponse {
  Plugins: PluginResponse,
  NeedToRestart: boolean
}
