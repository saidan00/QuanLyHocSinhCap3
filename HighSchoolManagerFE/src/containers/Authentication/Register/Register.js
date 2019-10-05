import React, {Component, Fragment} from 'react';
import axios from 'axios';
import styles from './Register.module.css';
import Button from '../../../components/UI/Button/Button';
import {Input} from 'antd';

class Register extends Component {
  state = {
    registerModel: {
      userName: null,
      password: null,
    },
  };

  registerModelOnChangeHandler = (event, key) => {
    const value = event.target.value;
    this.setState(prevState => {
      let newModel = {...prevState.registerModel};
      newModel[key] = value;
      return {registerModel: newModel};
    });
  };

  submitRegisterModel = () => {
    const newModel = {...this.state.registerModel};
    console.log("sending", newModel);
    axios
      .post('/Account/Register', newModel, {withCredentials: true})
      .then(response => {
        console.log('REGISTER SUCCESSFUL!', response);
        window.location.reload();
      })
      .catch(error => {
        console.log('REGISTER UNSUCCESSFUL :(', error);
      });
  }

  render() {
    return (
      <Fragment>
        <hr />
        Register
        <Input
          placeholder="Username"
          value={this.state.registerModel.userName}
          onChange={event =>
            this.registerModelOnChangeHandler(event, 'userName')
          }
        />
        <Input.Password
          placeholder="Password"
          value={this.state.registerModel.password}
          onChange={event =>
            this.registerModelOnChangeHandler(event, 'password')
          }
        />
        <Button color="primary" clicked={this.submitRegisterModel}>Register</Button>
      </Fragment>
    );
  }
}

export default Register;
