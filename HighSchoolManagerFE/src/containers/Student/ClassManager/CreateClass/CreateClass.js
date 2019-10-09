import React, {Component, Fragment} from 'react';
import {Input, InputNumber, Select, Checkbox, message} from 'antd';
import styles from './CreateClass.module.css';

import Request from '../../../../common/commonRequest';
import Validation from '../../../../common/commonValidation';
import Params from '../../../../common/commonParams';
import Button from '../../../../components/UI/Button/Button';
import InvalidBox from '../../../../components/Partial/InvalidBox/InvalidBox';
import LoadScreen from '../../../../components/UI/LoadScreen/LoadScreen';

const {Option} = Select;

class CreateClass extends Component {
  state = {
    loading: true,
    step: 1,
    step1_grades: [],
    step1_additionalInfos: {
      availableStudents: null,
      existingClasses: null,
    },
    classModel: {
      classYear: null,
      classGrade: null,
      classQuantity: null,
      // classes: [{name: '', studentIDs: []}]
      classes: [],
    },
    invalidFields1: [],
    invalidFields2: [],
  }
  constructor(props) {
    super(props);
    this.currentDate = new Date();
    this.state.classModel.classYear = this.currentDate.getFullYear();
    this.state.classModel.classGrade = 1;
    Validation.initFormFields(this.formFields1);
  }

  stepTitles = ['', 'Step 1: Choose Grade, Set Classes quantity', 'Step 2: Pick students to add'];
  formFields1 = {
    classGrade: {
      event: {},
      label: 'Grade',
      validates: [{name: 'NotNull', params: []}],
      errorMessage: '',
    },
    classQuantity: {
      event: {},
      label: 'Quantity',
      validates: [
        {name: 'NotNull', params: []},
        {name: 'Int', params: []},
        {name: 'RangeFrom', params: [1]}
      ],
      errorMessage: '',
    },
  };

  form1OnChangeHandler = (event, key) => {
    const value = event ? (event.target ? event.target.value : event) : null;
    this.setState(prevState => {
      let newModel = {...prevState.classModel};
      newModel[key] = value;
      return {classModel: newModel};
    });
  };

  async fetchGrades() {
    await Request.get('/Grade/Get', 'cred', response => {
      let _grades = response.data;
      this.setState({step1_grades: _grades});
    });
  }

  async fetchAdditionalInfos() {
    //Available students
    //Existing classes
    let _existingClasses = null;
    let _additionalInfo = {...this.state.additionalInfos}
    const searchParams = Params.getSearchParamsFromObj(this.state.classModel, ['gradeID']);
    await Request.get('/Class/Get?'+searchParams, 'cred', response => {
      _existingClasses = response.data.length;
      console.log(_existingClasses);
      _additionalInfo['existingClasses'] = _existingClasses;
      this.setState({step1_additionalInfos: _additionalInfo});
    });
  }

  validateField1OnBlurHandler = (event, key) => {
    let _invalidFields = [...this.state.invalidFields1].filter(fieldId => fieldId !== key);
    const value = event ? (event.target ? event.target.value : event) : null;
    if (!Validation.validateField(this.formFields1, key, value)) {
      _invalidFields.push(key);
    }
    this.setState({invalidFields1: _invalidFields});
  }

  submitForm1Handler = () => {
    if (!this.validateForm(this.formFields1, 'invalidFields1', this)) {
      message.error('Create new classes failed. Check for invalid fields');
      return;
    }
    // Go to step 2
    message.success('Proceed!');
  }

  async componentDidMount() {
    this.validateForm = Validation.validateForm;
    await Promise.all([this.fetchGrades(), this.fetchAdditionalInfos()]);
    this.setState({loading: false});
  }

  render() {
    const inputMargin = {margin: '0 16px'};
    const inputMarginSibling = {margin: '8px 16px 0'};
    const inputWidth = {width: '200px'};
    const spanWidth = {display: 'inline-block', width: '100px'};
    const spanWidthBig = {display: 'inline-block', width: '200px'};
    const errorProps1 = Validation.getErrorProps(this.formFields1, this.state.invalidFields1);

    return(
      <Fragment>
        <h1>Creating Class Wizard</h1>
        <hr />
        <h2>{this.stepTitles[this.state.step]}</h2>
        <div className={styles.CreateClass}>
          {/* STEP 1*/}
          {this.state.step !== 1 ? null : (this.state.loading ? <LoadScreen /> : 
            <div className={styles.FormWrapper}>
              <div className={styles.FieldWrapper}>
                <span style={{...inputMargin, ...spanWidth}}>Year:</span>
                <Input style={{...inputMargin, ...inputWidth}} value={this.state.classModel.classYear+'-'+parseInt(this.state.classModel.classYear+1)} disabled />
              </div>
              <div className={styles.FieldWrapper}>
                <span style={{...inputMarginSibling, ...spanWidth}}>{this.formFields1['classGrade'].label}:</span>
                <Select 
                  style={{...inputMarginSibling, ...inputWidth, ...errorProps1.classGrade.style}}
                  placeholder="Select grade"
                  ref={this.formFields1.classGrade.event}
                  value={this.state.classModel.classGrade}
                  onChange={e => {this.form1OnChangeHandler(e, 'classGrade'); this.validateField1OnBlurHandler(e, 'classGrade')}}
                >
                    {this.state.step1_grades.map(e => {
                      return (
                        <Option key={`gradeSelect-${e.gradeID}`} value={e.gradeID}>
                          {e.name}
                        </Option>
                      );
                    })}
                </Select>
                <InvalidBox margin="5px 16px 0 148px" width="200px">{errorProps1.classGrade.message}</InvalidBox>
                <span style={{...inputMarginSibling, ...spanWidth}}>&nbsp;</span>
                <span style={{...inputMarginSibling, ...spanWidthBig}}><b>Available students: </b>?</span>
                <span style={{...inputMarginSibling, ...spanWidth}}>&nbsp;</span>
                <span style={{...inputMarginSibling, ...spanWidthBig}}>
                  <b>Existing classes: </b>{(this.state.step1_additionalInfos.existingClasses!==null || this.state.loading) ?
                    this.state.step1_additionalInfos.existingClasses : '...'}
                </span>
              </div>
              <div className={styles.FieldWrapper}>
                <span style={{...inputMarginSibling, ...spanWidth}}>{this.formFields1['classQuantity'].label}:</span>
                <Input
                  style={{...inputMarginSibling, ...inputWidth, ...errorProps1.classQuantity.style}}
                  ref={this.formFields1.classQuantity.event}
                  value={this.state.classModel.classQuantity}
                  onChange={e => this.form1OnChangeHandler(e, 'classQuantity')}
                  onBlur={e => this.validateField1OnBlurHandler(e, 'classQuantity')}
                />
                <InvalidBox margin="5px 16px 0 148px" width="200px">{errorProps1.classQuantity.message}</InvalidBox>
                <span style={{...inputMarginSibling, ...spanWidth}}>&nbsp;</span>
                <Checkbox style={{...inputMarginSibling, ...inputWidth}} >Recommended quantity</Checkbox>
              </div>
              <div
                className={styles.FieldWrapper}
                style={{display: 'flex', justifyContent: 'center'}}>
                <Button color="primary" clicked={this.submitForm1Handler}>Continue >></Button>
              </div>
            </div>
          )}
        </div>
      </Fragment>
    );
  }
}

export default CreateClass;
