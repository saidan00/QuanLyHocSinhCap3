import React, {Component, Fragment} from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import styles from './Authentication.module.css';
import Button from '../../components/UI/Button/Button';
import Login from './Login/Login';
import Register from './Register/Register';

class Authentication extends Component {
  state = {
    auth: false,
    currentUser: {
      username: null,
    },
  };

  checkIsSignedIn() {
    axios.get('/Account/isSignedIn', {withCredentials: true}).then(response => {
      console.log(response);
      this.setState({auth: response.data});
    });
    this.getUserInfo();
  }

  getUserInfo() {
    axios.get('/Account/User', {withCredentials: true}).then(response => {
      console.log(response);
      let user = {username: response.data};
      this.setState({currentUser: user});
    });
  }

  logOutHandler = () => {
    axios
      .post('/Account/Logout', null, {withCredentials: true})
      .then(response => {
        window.location.reload();
      })
  }

  componentDidMount() {
    console.log(Cookies.get());
    this.checkIsSignedIn();
  }

  render() {
    return (
      <Fragment>
        <h1>Auth Test</h1>
        <p>You have {this.state.auth ? 'signed in' : 'not signed in'}</p>
        {!this.state.auth ? null : (
          <Fragment>
            <p>Welcome back, {this.state.currentUser.username}</p>
            <Button color="primary" clicked={this.logOutHandler} >Log Out</Button>
          </Fragment>
        )}
        {this.state.auth ? null : (
          <Login />
        )}
        <Register />
      </Fragment>
    );
  }
}

export default Authentication;
