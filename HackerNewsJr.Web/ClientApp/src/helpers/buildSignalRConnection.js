import { HubConnectionBuilder } from '@aspnet/signalr';

const buildSignalRConnection = url => {
  return new HubConnectionBuilder().withUrl(url).build();
};

export default buildSignalRConnection;
