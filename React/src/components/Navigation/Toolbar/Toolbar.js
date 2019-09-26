import React from 'react';
import styles from './Toolbar.module.css';
import ToolbarUtil from './ToolbarUtil/ToolbarUtil';

const toolbar = (props) => {
  return (
    <div className={styles.Toolbar}>
      <div className={styles.Logo}></div>
      <div className={styles.Search}></div>
      <div className={styles.Utils}>
        <ToolbarUtil label="Settings" icon="fa-cog" />
        <ToolbarUtil label="Account" icon="fa-user-circle" />
      </div>
    </div>
  );
}

export default toolbar;
