import React, {Component, Fragment} from 'react';
import axios from 'axios';
import Request from '../../common/commonRequest';
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
    }).catch(error => {console.log(error.response)});
    this.getUserInfo();
  }

  getUserInfo() {
    axios.get('/Account/CurrentUser', {withCredentials: true}).then(response => {
      console.log(response);
      let user = {username: response.data.userName};
      this.setState({currentUser: user});
    }).catch(error => {console.log(error.response)});

    Request.get('/Account/CurrentUser', "cred",
      response => {
        console.log(response);
        let user = {username: response.data.userName};
        this.setState({currentUser: user});
      },
      error => {console.log(error.response)});
  }

  logOutHandler = () => {
    axios
      .post('/Account/Logout', null, {withCredentials: true})
      .then(response => {
        window.location.reload();
      })
  }

  componentDidMount() {
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
