import React, {Component} from 'react';
import {NavLink} from 'react-router-dom';
import styles from './SidedrawerItem.module.css';

class SidedrawerItem extends Component {
  state = {
    isInActive: false,
  }

  constructor(props) {
    super(props);
    this.sidedrawerItemRef = React.createRef();
  }

  componentDidMount() {
    this.sidedrawerOnClassChangeHandler();
  }

  componentDidUpdate() {
    console.log("UPDATE!");
    this.sidedrawerOnClassChangeHandler();
  }

  sidedrawerOnClassChangeHandler = () => {
    const sidedrawerClasses = this.sidedrawerItemRef.current.classList;
    const newIsInActive = sidedrawerClasses.contains(styles.active);
    console.log(newIsInActive);
    if (newIsInActive !== this.state.isInActive)
      this.setState({isInActive: newIsInActive});
  }

  render() {
    return (
      <li className={[styles.SidedrawerWrapper, (this.state.isInActive ? styles.active : null)].join(' ')}>
        <NavLink
          to={this.props.link}
          exact={this.props.exact}
          className={styles.SidedrawerItem}
          activeClassName={styles.active}
          ref={this.sidedrawerItemRef}
          onClick={this.sidedrawerOnClassChangeHandler}
        >
          <div className={styles.label}>
            <i className={"fas fa-fw "+this.props.icon}></i>
            {this.props.label}
          </div>
          <div className={styles.indicator}></div>
        </NavLink>
        <div className={styles.SidedrawerMenu}>
          {this.props.children}
        </div>
        {/*
        <NavLink
          to={this.props.link}
          exact={this.props.exact}
          className={styles.SidedrawerItem}
          activeClassName={styles.active}
        >
          <div className={styles.SidedrawerWrapper}>
            <div className={styles.label}>
              <i className={"fas fa-fw "+this.props.icon}></i>
              {this.props.label}
            </div>
            <div className={styles.indicator}></div>
          </div>
          <div className={styles.SidedrawerMenu}>
            {this.props.children}
          </div>
        </NavLink>
        */}
      </li>
    );
  }
};

export default SidedrawerItem;
