import React from 'react';
import './style.css'

const ErrorMessage = (props) => {
    return (
      <small className ="text-danger">{props.errorMessage}</small>
    );
  }

  export default ErrorMessage;