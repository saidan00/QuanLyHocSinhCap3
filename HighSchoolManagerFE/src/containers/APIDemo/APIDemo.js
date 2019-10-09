import React, {Component, Fragment} from 'react';
import moment from 'moment';
import Request from '../../common/commonRequest';
import Button from '../../components/UI/Button/Button';
import {DatePicker, Input, Select} from 'antd';
const {Option} = Select;

class APIDemo extends Component {
  state = {
    students: [],
    addingStudent: {},
    updatingStudent: {},
    updatingstudentID: null,
    loading: true,
  };

  componentDidMount() {
    this.fetchStudents();
  }

  fetchStudents() {
    this.setState({loading: true});
    Request.get(
      '/Student/Get',
      "cred",
      response => {
        let newStudents = [...response.data];
        newStudents.forEach(el => {
          el.birthday = moment(el.birthday, 'YYYY/MM/DD');
        });
        this.setState({students: newStudents, loading: false});
      },
    );
  }

  addingStudentOnChangeHandler = (event, key) => {
    const value = event ? (event.target ? event.target.value : event) : null;
    this.setState(prevState => {
      let newStudent = {...prevState.addingStudent};
      newStudent[key] = value;
      return {addingStudent: newStudent};
    });
  };

  updatingStudentOnChangeHandler = (event, key) => {
    const value = event ? (event.target ? event.target.value : event) : null;
    this.setState(prevState => {
      let newStudent = {...prevState.updatingStudent};
      newStudent[key] = value;
      return {updatingStudent: newStudent};
    });
  };

  updatingstudentIDOnChangeHandler = event => {
    const value = parseInt(event);
    const newStudent = {
      ...this.state.students.filter(el => el.studentID === value)[0],
    };
    this.setState({
      updatingStudent: {...newStudent},
      updatingstudentID: value,
    });
  };

  addStudentHandler = () => {
    const newStudent = {...this.state.addingStudent};
    newStudent.birthday = newStudent.birthday.format('YYYY-MM-DD');
    console.log(newStudent);
    Request.post(
      '/Student/Create',
      newStudent,
      "cred",
      response => {
        console.log('POST SUCCESSFUL!', response);
        this.fetchStudents();
        this.setState({addingStudent: {}});
      },
      error => {
        console.log('POST UNSUCCESSFUL :(', error.response);
      },
    );
  };

  updateStudentHandler = () => {
    const newStudent = {...this.state.updatingStudent};
    const newStudentId = this.state.updatingstudentID;
    newStudent.birthday = newStudent.birthday.format('YYYY-MM-DD');
    Request.put(
      '/Student/Edit?studentId='+newStudentId,
      newStudent,
      "cred",
      response => {
        console.log('PUT SUCCESSFUL!', response);
        this.fetchStudents();
        this.setState({updatingStudent: {}, updatingstudentID: null});
      },
      error => {
        console.log('PUT UNSUCCESSFUL :(', error);
      },
    );
  };

  render() {
    return (
      <Fragment>
        <h1>High School Manager</h1>
        <p>Now in React!</p>
        &nbsp;
        <h3>API Demo</h3>
        <hr />
        <h2>Students List</h2>
        <table>
          <thead>
            <tr>
              <td>ID</td>
              <td>Class</td>
              <td>Last Name</td>
              <td>First Name</td>
              <td>Date of Birth</td>
              <td>Home Address</td>
            </tr>
          </thead>
          <tbody>
            {this.state.loading
              ? null
              : this.state.students.map(el => (
                  <tr key={'studentsTable-' + el.studentID}>
                    <td>{el.studentID}</td>
                    <td>{el.classID}</td>
                    <td>{el.lastName}</td>
                    <td>{el.firstName}</td>
                    <td>{el.birthday.format('DD/MM/YYYY')}</td>
                    <td>{el.address}</td>
                  </tr>
                ))}
          </tbody>
        </table>
        <hr />
        <h2>Add Student</h2>
        <Input
          type="text"
          value={this.state.addingStudent.lastName}
          placeholder="Last Name"
          onChange={event =>
            this.addingStudentOnChangeHandler(event, 'lastName')
          }
        />
        <Input
          type="text"
          value={this.state.addingStudent.firstName}
          placeholder="First Name"
          onChange={event =>
            this.addingStudentOnChangeHandler(event, 'firstName')
          }
        />
        <DatePicker
          value={this.state.addingStudent.birthday}
          onChange={event =>
            this.addingStudentOnChangeHandler(event, 'birthday')
          }
        />
        <Input
          type="text"
          value={this.state.addingStudent.address}
          placeholder="Home address"
          onChange={event =>
            this.addingStudentOnChangeHandler(event, 'address')
          }
        />
        <br />
        <Button color="primary" clicked={this.addStudentHandler}>
          Confirm
        </Button>
        <hr />
        <h2>Update Student</h2>
        <Select
          value={this.state.updatingstudentID || ''}
          onChange={this.updatingstudentIDOnChangeHandler}>
          <Option value="" hidden>
            Select student...
          </Option>
          {this.state.students.map(el => (
            <Option key={'putStdnt-' + el.studentID} value={el.studentID}>
              {el.studentID + ' - ' + el.firstName}
            </Option>
          ))}
        </Select>
        <br />
        {this.state.loading || !this.state.updatingstudentID ? null : (
          <Fragment>
            <Input
              type="text"
              value={this.state.updatingStudent.lastName}
              placeholder="Last Name"
              onChange={event =>
                this.updatingStudentOnChangeHandler(event, 'lastName')
              }
            />
            <Input
              type="text"
              value={this.state.updatingStudent.firstName}
              placeholder="First Name"
              onChange={event =>
                this.updatingStudentOnChangeHandler(event, 'firstName')
              }
            />
            <DatePicker
              value={this.state.updatingStudent.birthday}
              onChange={event =>
                this.updatingStudentOnChangeHandler(event, 'birthday')
              }
            />
            <Input
              type="text"
              value={this.state.updatingStudent.address}
              placeholder="Home address"
              onChange={event =>
                this.updatingStudentOnChangeHandler(event, 'address')
              }
            />
            <br />
            <Button color="primary" clicked={this.updateStudentHandler}>
              Confirm
            </Button>
          </Fragment>
        )}
      </Fragment>
    );
  }
}

export default APIDemo;
