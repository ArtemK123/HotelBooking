import React from "react";

export default class SearchString extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
  }

  render() {
    return (
      <div>
        <label>{this.props.name}&nbsp;</label>
        <input
          type="text"
          className="form-control"
          placeholder={this.props.placeholder}
          onChange={event => this.props.onChangeCheck(event)}
          name={this.props.filterName}
          value={this.props.value}
          disabled={this.props.disable}
        />
      </div>
    );
  }
}
