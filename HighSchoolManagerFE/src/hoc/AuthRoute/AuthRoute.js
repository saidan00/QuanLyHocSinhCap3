import React, {Component, Fragment} from 'react';
import {Redirect} from 'react-router-dom';
import Request from '../../common/commonRequest';

class AuthRoute extends Component {
  //props: role
  state = {
    loading: true,
    isAuthen: false,
    isAuthor: false,
    role: [],
  };

  async componentDidMount() {
    console.log('1');
    await this.doAuthenticate();
    console.log('2');
    await this.doAuthorize();
    this.setState({loading: false});
    console.log('3');
    //this.setState({isAuthen: true, isAuthor: true, role: ["Teacher"], loading: false});
  }

  async doAuthenticate() {
    console.log('START doAuthenticate');
    await Request.get('/Account/isSignedIn', 'cred', response => {
      this.setState({isAuthen: response.data});
      console.log('FINISHED doAuth', this.state.isAuthen);
    });
  }
  async getUserRole() {
    console.log('START getUserRole');
    await Request.get('/Account/currentUser', 'cred', response => {
      this.setState({role: response.data.roles.result});
      console.log('FINISHED getUserRole: ' + this.state.role);
    });
  }
  async doAuthorize() {
    console.log('START doAuthorize with isAuthen: ', this.state.isAuthen);
    if (!this.state.isAuthen) return;
    const allowedRoles = this.props.roles;
    if (typeof allowedRoles === 'undefined') {
      this.setState({isAuthor: true});
      return;
    }
    await this.getUserRole();
    const userRole = this.state.role;
    console.log('allowedRoles: ', allowedRoles, 'currentUser role:', userRole);
    if (allowedRoles.filter(role => userRole.includes(role)).length > 0)
      this.setState({isAuthor: true});
  }

  render() {
    return (
      <Fragment>
        {!this.state.loading ? (
          this.state.isAuthen ? (
            this.state.isAuthor ? (
              this.props.children
            ) : (
              <p>Access Denied</p>
            )
          ) : (
            <Redirect to="/Login" />
          )
        ) : null}
      </Fragment>
    );
  }
}

export default AuthRoute;
