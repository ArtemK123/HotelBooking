import React from "react";

export default class Calendar extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
  }
  render() {
    return (
      <div className="w-50">
        <div className="form-group">
          <label htmlFor="inputDate">{this.props.name}</label>
          <input
            type="date"
            className="form-control"
            name={this.props.filterName}
            onChange={event => this.props.onChangeCheck(event)}
            value={this.props.value ? this.props.value : undefined}
          />
        </div>
      </div>
    );
  }
}
