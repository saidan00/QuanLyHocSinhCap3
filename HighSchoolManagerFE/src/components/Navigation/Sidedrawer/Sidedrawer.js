import React, {Component, Fragment} from 'react';
import styles from './Sidedrawer.module.css';
import Request from '../../../common/commonRequest';
import Auth from '../../../common/commonAuth';
import LoadScreen from '../../UI/LoadScreen/LoadScreen';
import SidedrawerItem from './SidedrawerItem/SidedrawerItem';
import SidedrawerMenuItem from './SidedrawerMenuItem/SidedrawerMenuItem';

class Sidedrawer extends Component {
  state = {
    loading: true,
    sidedrawerType: null,
  }

  async componentDidMount() {
    const sidedrawerType1 = await Auth.isInRoles(["Manager", "Teacher"]);
    if (sidedrawerType1)
      this.setState({sidedrawerType: 1});
      this.setState({loading: false});
  }

  render() {
    return (
      <div className={styles.Sidedrawer}>
        {this.state.loading ? <LoadScreen /> :
          <ul>
            <SidedrawerItem link="/" label="Home" icon="fa-home" exact>
            </SidedrawerItem>
            {this.state.sidedrawerType===1 ? (
              <Fragment>
                <SidedrawerItem link="/Student" label="Students" icon="fa-users">
                  <SidedrawerMenuItem link="/Student/MyClass" label="My Class" />
                  <SidedrawerMenuItem link="/Student/AddStudent" label="Add Student" />
                  <SidedrawerMenuItem link="/Student/ClassManager" label="Class Manager" />
                </SidedrawerItem>
                <SidedrawerItem link="/Result" label="Results" icon="fa-graduation-cap">
                  <SidedrawerMenuItem link="/Result/View" label="View" />
                  <SidedrawerMenuItem link="/Result/Manage" label="Manage" />
                </SidedrawerItem>
                <SidedrawerItem link="/Conduct" label="Conduct" icon="fa-clipboard-list">
                  <SidedrawerMenuItem link="/Conduct/Violations" label="Violation Record" />
                  <SidedrawerMenuItem link="/Conduct/RulesManager" label="Rules Manager" />
                </SidedrawerItem>
                <SidedrawerItem link="/Report" label="Report" icon="fa-chart-bar">
                  <SidedrawerMenuItem link="/Report/Create" label="Create Report" />
                </SidedrawerItem>
              </Fragment>
            ) : null}
          </ul>
        }
      </div>
    );
  }

};

export default Sidedrawer;
