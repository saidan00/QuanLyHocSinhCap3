import React, {Component, Fragment} from 'react';
import {Redirect} from 'react-router-dom';
import Request from '../../common/commonRequest';
import AuthContext from '../../context/auth-context';

class AuthRoute extends Component {
  //props: role
  state = {
    loading: true,
    isAuthen: false,
    isAuthor: false,
    role: '',
    username: '',
    teacher: {},
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
  async getUserInfo() {
    console.log('START getUserInfo');
    await Request.get('/Account/currentUser', 'cred', response => {
      this.setState({username: response.data.userName, role: response.data.roles, teacher: response.data.teacher});
      console.log('FINISHED getUserInfo: ' + this.state.username, this.state.role);
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
    await this.getUserInfo();
    const userRole = this.state.role;
    console.log('allowedRoles: ', allowedRoles, 'currentUser role:', userRole);
    if (allowedRoles.filter(role => userRole === role).length > 0)
      this.setState({isAuthor: true});
  }

  render() {
    return (
      <AuthContext.Provider value={{
        isAuthen: this.state.isAuthen,
        isAuthor: this.state.isAuthor,
        user: {
          username: this.state.username,
          role: this.state.role,
        }
      }}>
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
      </AuthContext.Provider>
    );
  }
}

export default AuthRoute;
