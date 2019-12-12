import React from "react";
import "./style.css";
import ErrorMessage from "./ErrorMessage";

const EmailBlock = ({ email, errorMessage, onChange }) => {
  return (
    <div className="form-group">
      <label>Please, enter your email</label>
      <input
        type="email"
        className="form-control"
        placeholder="e.g. user1@gmail.com"
        value={email}
        onChange={onChange}
      />
      <ErrorMessage errorMessage={errorMessage} />
    </div>
  );
};

export default EmailBlock;
