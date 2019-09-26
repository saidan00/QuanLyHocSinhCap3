import React, {Component, Fragment} from 'react';
import styles from './APIDemo.module.css';
import axios from 'axios';
import moment from 'moment';
import Button from '../../components/UI/Button/Button';
import {DatePicker, Input, Select} from 'antd';
const {Option} = Select;

class APIDemo extends Component {
  state = {
    students: [],
    addingStudent: {},
    updatingStudent: {},
    updatingStudentID: null,
    loading: true,
  };

  componentDidMount() {
    this.fetchStudents();
  }

  fetchStudents() {
    this.setState({loading: true});
    axios.get('/StudentAPITest').then(response => {
      let newStudents = [...response.data];
      newStudents.forEach(el => {
        el.Birthday = moment(el.Birthday, 'YYYY/MM/DD');
      });
      this.setState({students: newStudents, loading: false});
    });
  }

  addingStudentOnChangeHandler = (event, key) => {
    const value = event ?
      ( (event.target) ? event.target.value : event )
      : null;
    this.setState(prevState => {
      let newStudent = {...prevState.addingStudent};
      newStudent[key] = value;
      return {addingStudent: newStudent};
    });
  };

  updatingStudentOnChangeHandler = (event, key) => {
    const value = event ?
      ( (event.target) ? event.target.value : event )
      : null;
    this.setState(prevState => {
      let newStudent = {...prevState.updatingStudent};
      newStudent[key] = value;
      return {updatingStudent: newStudent};
    });
  };

  updatingStudentIDOnChangeHandler = event => {
    const value = parseInt(event);
    const newStudent = {
      ...this.state.students.filter(el => el.StudentID === value)[0],
    };
    this.setState({
      updatingStudent: {...newStudent},
      updatingStudentID: value,
    });
  };

  addStudentHandler = () => {
    const newStudent = {...this.state.addingStudent};
    newStudent.Birthday = newStudent.Birthday.format('YYYY/MM/DD');
    axios
      .post('/StudentAPITest', newStudent)
      .then(response => {
        console.log('POST SUCCESSFUL!', response);
        this.fetchStudents();
        this.setState({addingStudent: {}});
      })
      .catch(error => {
        console.log('POST UNSUCCESSFUL :(', error);
      });
  };

  updateStudentHandler = () => {
    const newStudent = {...this.state.updatingStudent};
    const newStudentId = this.state.updatingStudentID;
    newStudent.Birthday = newStudent.Birthday.format('YYYY/MM/DD');
    axios
      .put('/StudentAPITest/' + newStudentId, newStudent)
      .then(response => {
        console.log('PUT SUCCESSFUL!', response);
        this.fetchStudents();
        this.setState({updatingStudent: {}, updatingStudentID: null});
      })
      .catch(error => {
        console.log('PUT UNSUCCESSFUL :(', error);
      });
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
                  <tr key={'studentsTable-' + el.StudentID}>
                    <td>{el.StudentID}</td>
                    <td>{el.ClassID}</td>
                    <td>{el.LastName}</td>
                    <td>{el.FirstName}</td>
                    <td>{el.Birthday.format("DD/MM/YYYY")}</td>
                    <td>{el.Address}</td>
                  </tr>
                ))}
          </tbody>
        </table>
        <hr />
        <h2>Add Student</h2>
        <Input
          type="text"
          value={this.state.addingStudent.LastName}
          placeholder="Last Name"
          onChange={event =>
            this.addingStudentOnChangeHandler(event, 'LastName')
          }
        />
        <Input
          type="text"
          value={this.state.addingStudent.FirstName}
          placeholder="First Name"
          onChange={event =>
            this.addingStudentOnChangeHandler(event, 'FirstName')
          }
        />
        <DatePicker
          value={this.state.addingStudent.Birthday}
          onChange={event =>
            this.addingStudentOnChangeHandler(event, 'Birthday')
          }
        />
        <Input
          type="text"
          value={this.state.addingStudent.Address}
          placeholder="Home Address"
          onChange={event =>
            this.addingStudentOnChangeHandler(event, 'Address')
          }
        />
        <br />
        <Button color="primary" clicked={this.addStudentHandler}>
          Confirm
        </Button>
        <hr />
        <h2>Update Student</h2>
        <Select
          value={this.state.updatingStudentID || ''}
          onChange={this.updatingStudentIDOnChangeHandler}>
          <Option value="" hidden>
            Select student...
          </Option>
          {this.state.students.map(el => (
            <Option key={'putStdnt-' + el.StudentID} value={el.StudentID}>
              {el.StudentID + ' - ' + el.FirstName}
            </Option>
          ))}
        </Select>
        <br />
        {this.state.loading || !this.state.updatingStudentID ? null : (
          <Fragment>
            <Input
              type="text"
              value={this.state.updatingStudent.LastName}
              placeholder="Last Name"
              onChange={event =>
                this.updatingStudentOnChangeHandler(event, 'LastName')
              }
            />
            <Input
              type="text"
              value={this.state.updatingStudent.FirstName}
              placeholder="First Name"
              onChange={event =>
                this.updatingStudentOnChangeHandler(event, 'FirstName')
              }
            />
            <DatePicker
              value={this.state.updatingStudent.Birthday}
              onChange={event =>
                this.updatingStudentOnChangeHandler(event, 'Birthday')
              }
            />
            <Input
              type="text"
              value={this.state.updatingStudent.Address}
              placeholder="Home Address"
              onChange={event =>
                this.updatingStudentOnChangeHandler(event, 'Address')
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
