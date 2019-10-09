import axios from 'axios';

class CommonRequest {
  static optionsList = {
    "cred": {
      withCredentials: true
    },
  };
  //opts="opt1,opt2"
  static getOpts(opts) {
    return opts.split(',').map(key => {return this.optionsList[key]})
      .reduce((optsObj, opt) => {
        return optsObj = {...optsObj, ...opt}
      });
  }

  // 'options' is an string of multiple options, reflect with optionsList
  static get(url, options, thenFunc, catchFunc) {
    const req = axios.get(url, this.getOpts(options))
      .then(response => thenFunc(response));
    if (typeof catchFunc !== "undefined")
      req.catch(error => catchFunc(error));
    return req;
  }
  static post(url, data, options, thenFunc, catchFunc) {
    const req = axios.post(url, data, this.getOpts(options))
      .then(response => thenFunc(response));
    if (typeof catchFunc !== "undefined")
      req.catch(error => catchFunc(error));
    return req;
  }
  static put(url, data, options, thenFunc, catchFunc) {
    const req = axios.put(url, data, this.getOpts(options))
      .then(response => thenFunc(response));
    if (typeof catchFunc !== "undefined")
      req.catch(error => catchFunc(error));
    return req;
  }
  static delete(url, id, data, options, thenFunc, catchFunc) {
    const req = axios.delete(url+'/'+id, data, this.getOpts(options))
      .then(response => thenFunc(response));
    if (typeof catchFunc !== "undefined")
      req.catch(error => catchFunc(error));
    return req;
  }
}

export default CommonRequest;
