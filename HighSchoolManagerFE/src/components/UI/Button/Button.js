import React from 'react';
import styles from './Button.module.css';

const button = props => {
  return (
    <button
      style={props.style}
      className={[styles.Button, styles[props.color]].join(' ')}
      onClick={props.clicked}
      disabled={props.disabled}
      >
      <div>{props.children}</div>
    </button>
  );
};

export default button;
