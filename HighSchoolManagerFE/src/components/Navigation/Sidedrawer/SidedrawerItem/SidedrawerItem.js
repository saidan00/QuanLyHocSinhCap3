import React from 'react';
import {NavLink} from 'react-router-dom';
import styles from './SidedrawerItem.module.css';

const sidedrawerItem = props => {
  return (
    <li>
      <NavLink
        to={props.link}
        exact={props.exact}
        className={styles.SidedrawerItem}
        activeClassName={styles.active}
      >
        <div className={styles.itemWrapper}>
          <i className={"fas fa-fw "+props.icon}></i>
          {props.children}
        </div>
        <div className={styles.indicator}></div>
      </NavLink>
    </li>
  );
};

export default sidedrawerItem;
