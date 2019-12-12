import React from "react";
import "./style.css";
import ErrorMessage from "./ErrorMessage";

const PasswordBlock = props => {
  return (
    <div className="form-group">
      <label>Please, enter your password</label>
      <input
        type="password"
        className="form-control"
        placeholder="Password"
        value={props.password}
        onChange={props.onChange}
      />
      <ErrorMessage errorMessage={props.errorMessage} />
    </div>
  );
};

export default PasswordBlock;
