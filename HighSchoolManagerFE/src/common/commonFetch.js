import Request from './commonRequest';
import Params from './commonParams';

class CommonFetch {
  static async fetchClasses(component, acceptedFilters) {
    const searchParams = Params.getSearchParamsFromObj(component.state.filters, acceptedFilters);
    let _classesPromise = await Request.get('/Class/Get?'+searchParams, 'cred');
    let _classes = _classesPromise.data;
    let _formattedClasses = _classesPromise.data.reduce((classesArr, cls) => {
      if (!classesArr[cls.year]) {
        classesArr[cls.year] = [];
      }
      classesArr[cls.year].push(cls);
      return classesArr;
    }, {});
    component.setState({classes: _classes, formattedClasses: _formattedClasses});
  }

  static async fetchGrades(component) {
    await Request.get('/Grade/Get', 'cred', response => {
      let _grades = response.data;
      component.setState({grades: _grades});
    });
  }

  static async fetchSubjects(component) {
    let subjectsPromise = await Request.get('/Subject/Get', 'cred');
    let _subjects = subjectsPromise.data;
    component.setState({subjects: _subjects});
  }

  static async fetchYears(component) {
    let newYears = [];
    //await Request.get('/Class/Get', 'cred', response => {
      //newYears = response.data.map(c => c.year);
      //newYears = newYears.filter((y, index, self) => self.indexOf(y) === index);
      //let newFilters = {...component.state.filters}
      //newFilters.year = newYears[newYears.length-1];
      //component.setState({years: newYears, filters: newFilters});
    //});
    await Request.get('/Semester/Get', 'cred', response => {
      newYears = response.data.map(s => s.year);
      newYears = newYears.filter((y, index, self) => self.indexOf(y) === index);
      let newFilters = {...component.state.filters}
      newFilters.year = newYears[newYears.length-1];
      component.setState({years: newYears, filters: newFilters});
    });
  }

  static async fetchSemesters(component, acceptedFilters) {
    const searchParams = Params.getSearchParamsFromObj(component.state.filters, acceptedFilters);
    let semestersPromise = await Request.get('/Semester/Get?'+searchParams, 'cred');
    let _semesters = semestersPromise.data;
    component.setState({semesters: _semesters});
    //if (_semesters.length > 0)
      //this.setState({year: _semesters.reverse()[0].year});
  }

  static async fetchMonthlyReport(component, acceptedFilters) {
    const searchParams = Params.getSearchParamsFromObj(component.state.filters, acceptedFilters);
    let _reportPromise = await Request.get('/Report/MonthlyReport?'+searchParams, 'cred');
    let _report = _reportPromise.data;
    _report = _report.map(rRow => {return {...rRow, key: rRow.student.studentID}});
    component.setState({report: _report});
  }

  static async fetchSemesterReport(component, acceptedFilters) {
    const searchParams = Params.getSearchParamsFromObj(component.state.filters, acceptedFilters);
    let _reportPromise = await Request.get('/Report/SemesterReport?'+searchParams, 'cred');
    let _report = _reportPromise.data;
    _report = _report.map(rRow => {return {...rRow, key: rRow.student.studentID}});
    component.setState({report: _report});
  }

  static async fetchPerformanceReport(component, acceptedFilters) {
    const searchParams = Params.getSearchParamsFromObj(component.state.filters, acceptedFilters);
    let _reportPromise = await Request.get('/Report/PerformanceReport?'+searchParams, 'cred');
    let _report = _reportPromise.data;
    _report = _report.map(rRow => {return {...rRow, key: rRow.aClass.classID}});
    component.setState({report: _report});
  }

}

export default CommonFetch;
