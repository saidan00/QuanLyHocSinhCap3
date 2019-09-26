import React from 'react';
import styles from './Sidedrawer.module.css';
import SidedrawerItem from './SidedrawerItem/SidedrawerItem';

const sidedrawer = props => {
  return (
    <div className={styles.Sidedrawer}>
      <ul>
        <SidedrawerItem link="/" icon="fa-school" exact>
          Home
        </SidedrawerItem>
        <SidedrawerItem link="/Student" icon="fa-users">
          Students
        </SidedrawerItem>
        <SidedrawerItem link="/Result" icon="fa-graduation-cap">
          Results
        </SidedrawerItem>
        <SidedrawerItem link="/Conduct" icon="fa-clipboard-list">
          Conduct
        </SidedrawerItem>
        <SidedrawerItem link="/Report" icon="fa-poll">
          Reports
        </SidedrawerItem>
      </ul>
    </div>
  );
};

export default sidedrawer;
