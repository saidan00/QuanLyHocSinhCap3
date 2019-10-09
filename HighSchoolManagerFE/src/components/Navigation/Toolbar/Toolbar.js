import React from 'react';
import {Link} from 'react-router-dom';
import styles from './Toolbar.module.css';
import ToolbarUtil from './ToolbarUtil/ToolbarUtil';
import logoimg from '../../../assets/images/sgh_logo.png';

const toolbar = (props) => {
  return (
    <div className={styles.Toolbar}>
      <Link className={styles.Logo} to="/" >
        <img src={logoimg} alt="" title="Sai Gon High" />
      </Link>
      <div className={styles.Search}></div>
      <div className={styles.Utils}>
        <ToolbarUtil label="Account" icon="fa-user-circle" >
          <span>Lưu Minh Hoàng</span>
        </ToolbarUtil>
      </div>
    </div>
  );
}

export default toolbar;
