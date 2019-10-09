import React, {Component, Fragment} from 'react';
import {Route, Link} from 'react-router-dom';
import styles from './ClassManager.module.css';

import Modal from '../../../components/UI/Modal/Modal';
import CreateClass from './CreateClass/CreateClass';

class ClassManager extends Component {
  modalClosedHandler = () => {
    this.props.history.push('/Student/ClassManager');
  }

  render() {
    return (
      <Fragment>
        <Route path="/Student/ClassManager/Create" render={() => (
          <Modal modalClosed={this.modalClosedHandler}>
            <CreateClass />
          </Modal>
        )} />
        <div className={styles.ClassManager}>
          <h1>Class Manager</h1>
          <Link to="/Student/ClassManager/Create">Create Class</Link>
        </div>
      </Fragment>
    );
  }
}

export default ClassManager;
