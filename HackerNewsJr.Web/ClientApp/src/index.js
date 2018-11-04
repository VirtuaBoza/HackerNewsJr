import './polyfills';
import 'react-app-polyfill/ie11';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import buildSignalRConnection from './helpers/buildSignalRConnection';

const rootElement = document.getElementById('root');

ReactDOM.render(
  <App buildSignalRConnection={buildSignalRConnection} />,
  rootElement,
);

registerServiceWorker();
