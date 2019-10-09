import React, {Component, Fragment} from 'react';
import {Link} from 'react-router-dom';
import {Input, Select, Table} from 'antd';
import moment from 'moment';
import styles from './Student.module.css';

import Request from '../../common/commonRequest';
import Params from '../../common/commonParams';
import LoadScreen from '../../components/UI/LoadScreen/LoadScreen';
import Card from '../../components/UI/Card/Card';

const {Option} = Select;

class Student extends Component {
  state = {
    loading: true,
    updating: false,
    students: [],
    grades: [],
    classes: [],
    filters: {
      name: '',
      gradeID: '',
      //classID: null,
    },
  };
  tableColumns = [ 
    {title: 'Class', dataIndex: 'classLabel', width: 75},
    {title: 'Year', dataIndex: 'classYear', width: 75},
    {title: 'Last Name', dataIndex: 'lastName', width: 200, sorter: (a,b) => a.lastName.localeCompare(b.lastName), sortDirections: ['ascend', 'descend']},
    {title: 'First Name', dataIndex: 'firstName', width: 200, sorter: (a,b) => a.firstName.localeCompare(b.firstName), sortDirections: ['ascend', 'descend']},
    {title: 'DOB', dataIndex: 'birthdayFormatted', width: 200, sorter: (a, b) => a.birthday.isAfter(b.birthday), sortDirections: ['descend']},
    {title: 'Address', dataIndex: 'address'},
    {title: 'Actions', width: 200, fixed: 'right', render: (text, record, index) => {
      return (
        <Fragment>
          <Link to={`/Result/Edit/${record.key}`}>Results</Link> |&nbsp;
          <Link to={`/Conduct/Edit/${record.key}`}>Conducts</Link> |&nbsp;
            <Link to={`/Student/Edit/${record.key}`}><b>Edit</b></Link>
        </Fragment>
      );
    }},
  ]

  async componentDidMount() {
    await Promise.all([this.fetchStudents(), this.fetchGrades(), this.fetchClasses()]);
    this.setState({loading: false});
  }

  async componentDidUpdate() {
    if (this.state.updating) {
      this.setState({updating: false});
      await this.fetchStudents();
    }
  }

  async fetchGrades() {
    await Request.get('/Grade/Get', 'cred', response => {
      let _grades = response.data;
      this.setState({grades: _grades});
    });
  }

  //TODO: filter by YEAR
  async fetchClasses() {
    const searchParams = Params.getSearchParamsFromObj(this.state.filters, ['gradeID']);
    await Request.get('/Class/Get?'+searchParams, 'cred', response => {
      let _classes = response.data;
      this.setState({classes: _classes});
    });
  }

  async fetchStudents() {
    const searchParams = Params.getSearchParamsFromObj(this.state.filters);
    await Request.get('/Student/Get?'+searchParams, 'cred', response => {
      let newStudents = response.data.map(_student => {
        let newStudent = {};
        newStudent.key = _student.studentID;
        newStudent.classLabel = (_student.class) ? _student.class.name : <i>None</i>;
        newStudent.classYear = (_student.class) ? _student.class.year : <i>None</i>;
        newStudent.lastName = _student.lastName;
        newStudent.firstName = _student.firstName;
        //newStudent.gender = _student.gender; //TODO: Add Gender
        newStudent.birthday = moment(_student.birthday, 'YYYY/MM/DD');
        newStudent.birthdayFormatted = newStudent.birthday.format('DD/MM/YYYY');
        newStudent.address = _student.address;
        return newStudent;
      });
      this.setState({students: newStudents, classes: []});
    });
  }

  filterOnChangeHandler = (event, key) => {
    const value = event ? (event.target ? event.target.value : event) : null;
    this.setState(prevState => {
      let _filters = {...prevState.filters};
      _filters[key] = value;
      return {filters: _filters, updating: true};
    });
  };

  render() {
    return (
      <Fragment>
        {this.state.loading ? (
          <LoadScreen style={{position: 'fixed', top: '64px', left: '225px'}} />
        ) : (
          <div className={styles.Student}>
            <h1>Students</h1>
            <p>{this.state.students.length} Students, ? Classes</p>
            <Card style={{height: 'calc(100vh - 232px)', flexDirection: 'column'}} >
              <div className={styles.FilterWrapper}>
                <div>
                  <span>Name: </span>
                  <Input
                    className={styles.FilterInput}
                    placeholder="Student name"
                    value={this.state.filters.name}
                    onChange={event => this.filterOnChangeHandler(event, 'name')}
                  />
                </div>
                <div>
                  <span>Grade: </span>
                  <Select
                    className={styles.FilterSelect}
                    placeholder="Select grade..."
                    value={this.state.filters.gradeID}
                    onChange={event => this.filterOnChangeHandler(event, 'gradeID')}
                  >
                    <Option key="gradeFilter-None" value="">None</Option>
                    {this.state.grades.map(e => {
                      return (
                        <Option key={`gradeFilter-${e.gradeID}`} value={e.gradeID}>
                          {e.name}
                        </Option>
                      );
                    })}
                  </Select>
                </div>
                <div>
                  <span>Class: </span>
                  <Select
                    className={styles.FilterSelect}
                    placeholder="Select class..."
                    value={this.state.filters.classID}
                    onChange={event => this.filterOnChangeHandler(event, 'classID')}
                    disabled={!this.state.filters.gradeID}
                  >
                    {this.state.classes.map(e => {
                      return (
                        <Option key={`classFilter-${e.classID}`} value={e.classID}>
                          {e.name}
                        </Option>
                      );
                    })}
                  </Select>
                </div>
              </div>
              <div className={styles.TableWrapper}>
                <Table columns={this.tableColumns} dataSource={this.state.students} scroll={{x: 1300, y: 'calc(100vh - 362px)'}} pagination={false} />
              </div>
            </Card>
          </div>
        )}
      </Fragment>
    );
  }
}

export default Student;
