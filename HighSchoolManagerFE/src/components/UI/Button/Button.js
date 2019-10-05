import React from 'react';
import styles from './Button.module.css';

const button = props => {
  return (
    <a
      className={[styles.Button, styles[props.color]].join(' ')}
      onClick={props.clicked}>
      <div>{props.children}</div>
    </a>
  );
};

export default button;
