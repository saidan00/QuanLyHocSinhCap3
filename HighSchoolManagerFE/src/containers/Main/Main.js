import React from 'react';
import {Route, Switch} from 'react-router-dom';
import AuthRoute from '../../hoc/AuthRoute/AuthRoute';

import APIDemo from '../APIDemo/APIDemo';
import Student from '../Student/Student';
import AddStudent from '../Student/AddStudent/AddStudent';
import ClassManager from '../Student/ClassManager/ClassManager';

const main = () => {
  //TODO: Add AuthRoute for Admin
  return (
    <AuthRoute roles={['Manager', 'Teacher']}>
      <Switch>
        <Route path="/" exact component={APIDemo} />

        <Route path="/Student/MyClass" render={() => <h1>STOP</h1>} />
        <Route path="/Student/AddStudent" component={AddStudent} />
        <Route path="/Student/ClassManager" component={ClassManager} />
        <Route path="/Student" component={Student} />

        <Route path="/Result" exact render={() => <p>Result page</p>} />
        <Route path="/Conduct" exact render={() => <p>Conduct page</p>} />
        <Route path="/Report" exact render={() => <p>Report page</p>} />
        <Route path="" render={() => <p>Not found</p>} />
      </Switch>
    </AuthRoute>
  );
};

export default main;
