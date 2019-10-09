import React, {Component, Fragment} from 'react';
import axios from 'axios';
import Button from '../../../components/UI/Button/Button';
import {Input} from 'antd';

class Login extends Component {
  state = {
    loginModel: {
      userName: null,
      password: null,
    },
  };

  loginModelOnChangeHandler = (event, key) => {
    const value = event.target.value;
    this.setState(prevState => {
      let newModel = {...prevState.loginModel};
      newModel[key] = value;
      return {loginModel: newModel};
    });
  };

  submitLoginModel = () => {
    const newModel = {...this.state.loginModel};
    console.log("sending", newModel);
    axios
      .post('/Account/Login', newModel, {withCredentials: true})
      .then(response => {
        console.log('LOGIN SUCCESSFUL!', response);
        window.location.reload();
      })
      .catch(error => {
        console.log('LOGIN UNSUCCESSFUL :(', error);
      });
  }

  render() {
    return(
      <Fragment>
        <hr />
        Login
        <Input
          placeholder="Username"
          value={this.state.loginModel.userName}
          onChange={event =>
            this.loginModelOnChangeHandler(event, 'userName')
          }
        />
        <Input.Password
          placeholder="Password"
          value={this.state.loginModel.password}
          onChange={event =>
            this.loginModelOnChangeHandler(event, 'password')
          }
        />
        <Button color="primary" clicked={this.submitLoginModel}>Login</Button>
      </Fragment>
    );
  }
}

export default Login;
