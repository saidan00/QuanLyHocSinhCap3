import Request from './commonRequest';

class CommonAuth {
  static async getCurrentUser() {
    let user;
    await Request.get('/Account/currentUser', 'cred', response => {
      user = response.data;
    });
    return user;
  }

  // alloedRoles is array
  static async isInRoles(allowedRoles) {
    const user = await this.getCurrentUser()
    const userRole = user.roles.result;
    if (!userRole)
      return false;
    return allowedRoles.filter(role => userRole.includes(role)).length > 0
    //return 1;
  }
}

export default CommonAuth;
