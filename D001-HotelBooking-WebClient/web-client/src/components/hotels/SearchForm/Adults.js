import React from "react";

export default class Adults extends React.Component {
  render() {
    const numberOfPeople = Array(10)
      .fill()
      .map((e, i) => i + 1);
    const options = numberOfPeople.map((number, i) => (
      <option key={i}>{number}</option>
    ));

    return (
      <div className="container">
        <div className="row">
          <div className="w-50">
            <div className="form-group">
              <label>{this.props.name}</label>
              <select
                className="form-control"
                name={this.props.filterName}
                value={this.props.value}
                onChange={event => this.props.onChangeCheck(event)}
              >
                <option>{"-"}</option>
                {options}
              </select>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
