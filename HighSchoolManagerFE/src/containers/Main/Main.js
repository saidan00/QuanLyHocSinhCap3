import React from 'react';
import {Route, Switch} from 'react-router-dom';
import AuthRoute from '../../hoc/AuthRoute/AuthRoute';

import APIDemo from '../APIDemo/APIDemo';
import Student from '../Student/Student';
import AddStudent from '../Student/AddStudent/AddStudent';
import ClassManager from '../Student/ClassManager/ClassManager';
import MyClass from '../Student/MyClass/MyClass';

const main = () => {
  //TODO: Add AuthRoute for Admin
  return (
    <AuthRoute roles={['Manager', 'Teacher']}>
      <Switch>
        <Route path="/" exact component={APIDemo} />

        <Route path="/Student/MyClass" component={MyClass} />
        <Route path="/Student/AddStudent" render={() => (
          <AuthRoute roles={['Manager']}>
            <AddStudent />
          </AuthRoute>
        )} />
        <Route path="/Student/ClassManager" render={props => (
          <AuthRoute roles={['Manager']}>
            <ClassManager history={{...props.history}}/>
          </AuthRoute>
        )} />
        <Route path="/Student" component={Student} />

        <Route path="/Assignment" render={() => (
          <AuthRoute roles={['Manager']}>
            <p>Teaching Assignments Page</p>
          </AuthRoute>
        )} />

        <Route path="/Result" exact render={() => <p>Result page</p>} />
        <Route path="/Conduct" exact render={() => <p>Conduct page</p>} />
        <Route path="/Report" exact render={() => <p>Report page</p>} />
        <Route path="" render={() => <p>Not found</p>} />
      </Switch>
    </AuthRoute>
  );
};

export default main;
