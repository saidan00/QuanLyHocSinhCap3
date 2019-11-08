import React from 'react';
import {Route, Switch} from 'react-router-dom';
import AuthRoute from '../../hoc/AuthRoute/AuthRoute';

import APIDemo from '../APIDemo/APIDemo';

import Student from '../Student/Student';
import MyClass from '../Student/MyClass/MyClass';
import AddStudent from '../Student/AddStudent/AddStudent';
import ClassManager from '../Student/ClassManager/ClassManager';

import Assignment from '../Assignment/Assignment';

import Result from '../Result/Result';
import ResultView from '../Result/ResultView/ResultView';
import ResultManager from '../Result/ResultManager/ResultManager';

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
            <Assignment />
          </AuthRoute>
        )} />

        <Route path="/Result/View" component={ResultView} />
        <Route path="/Result/Manage" component={ResultManager} />
        <Route path="/Result" exact component={Result} />

        <Route path="/Conduct" exact render={() => <p>Conduct page</p>} />
        <Route path="/Report" exact render={() => <p>Report page</p>} />
        <Route path="" render={() => <p>Not found</p>} />
      </Switch>
    </AuthRoute>
  );
};

export default main;
