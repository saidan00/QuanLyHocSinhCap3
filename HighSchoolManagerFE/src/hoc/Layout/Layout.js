import React, {Fragment} from 'react';
import styles from './Layout.module.css';
import Toolbar from '../../components/Navigation/Toolbar/Toolbar';
import Sidedrawer from '../../components/Navigation/Sidedrawer/Sidedrawer';

const layout = (props) => {
  return (
    <Fragment>
      <Toolbar />
      <div className="d-flex">
        <Sidedrawer />
        <div className={styles.Wrapper}>
          <div className="container-fluid">
            {props.children}
          </div>
        </div>
      </div>
    </Fragment>
  );
}

export default layout;
