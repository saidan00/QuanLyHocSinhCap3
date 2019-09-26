import React from 'react';
import styles from './ToolbarUtil.module.css';

const toolbarUtil = props => {
  return (
    <div className={styles.ToolbarUtil}>
        <i className={"fas fa-fw "+props.icon} title={props.label} ></i>
        <div className={styles.active}></div>
    </div>
  );
}

export default toolbarUtil;
