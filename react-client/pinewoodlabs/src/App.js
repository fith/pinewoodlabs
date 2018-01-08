import React, { Component } from 'react';
import Socket from './PinewoodLabs/Client/Socket';
import Editor from './PinewoodLabs/Client/Editor';
import Stats from './PinewoodLabs/Client/Stats';
import Unity from 'react-unity-webgl'

import './App.css';

class App extends Component {
  constructor(props) {
    super(props)

    this.socket = new Socket();

    this.state = {
      showEditor: true,
      showUnity: true,
      showStats: true
    };
  }
  componentDidMount() {
  }

  render() {
    return (
      <div className="App">
        <header className="App-header">
          <img src="/images/logo.png" alt="Pinewood Labs - Pinewood Derby Simulator" />
        </header>
        <Unity
          height={640}
          src='/Build/pine wood.json'
          loader='/Build/UnityLoader.js'
        />
        <div className="App-sections">
          <div className="App-section">
            <Editor socket={this.socket} />
          </div>
          <div className="App-section">
            <Stats socket={this.socket} />
          </div>
        </div>
        <footer className="App-footer">
          Thanks for viewing.
        </footer>
      </div>
    );
  }
}

export default App;
