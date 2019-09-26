import React, {Component, Fragment} from 'react';
import {Route, Switch} from 'react-router-dom';
import Layout from './hoc/Layout/Layout';
import './App.css';

import APIDemo from './containers/APIDemo/APIDemo';

class App extends Component {
  render() {
    return (
      <div className="App">
        <Layout>
          <Switch>
            <Route path="/" exact component={APIDemo} />
            <Route path="/Student" exact render={() => (<p>Student page</p>)} />
            <Route path="/Result" exact render={() => (<p>Result page</p>)} />
            <Route path="/Conduct" exact render={() => (<p>Conduct page</p>)} />
            <Route path="/Report" exact render={() => (<p>Report page</p>)} />
            <Route path="" render={() => (<p>Not found</p>)} />
          </Switch>
        </Layout>
      </div>
    );
  }
}

export default App;
