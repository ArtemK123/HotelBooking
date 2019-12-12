import React from "react";

const InputBlock = props => {
  let newProps = { ...props };
  delete newProps.errorMessage;
  return (
    <div className="form-group">
      <label>{props.label}</label>
      <input className="form-control" {...newProps} />
      <p className="text-danger">{props.errorMessage}</p>
    </div>
  );
};

export default InputBlock;
