import React from "react";
import "./style.css";
import EmailBlock from "./EmailBlock";
import PasswordBlock from "./PassworBlock";
import { Redirect } from "react-router-dom";
import config from "../../config";
import { withRouter } from "react-router";

class Form extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
  }

  state = {
    email: "",
    password: "",
    emailError: "",
    passwordError: "",
    message: "",
    success: false
  };

  onEmailChange(event) {
    this.setState({ email: event.target.value });
  }
  onPasswordChange(event) {
    this.setState({ password: event.target.value });
  }

  submit(event) {
    event.preventDefault();
    fetch(`${config.apiDomain}/api/account/login`, {
      method: "post",
      body: JSON.stringify({
        email: this.state.email,
        password: this.state.password
      }),
      headers: {
        "Content-type": "application/json"
      }
    })
      .then(res => res.json())
      .then(res => {
        console.log(res);
        if (res.isSuccessful) {
          this.setState({ success: true });
          this.props.successCallback(res.token);
          this.props.history.goBack();
        } else {
          this.setState({ message: res.message });
        }
      });
  }

  render() {
    return (
      <div>
        <form className="container col-5 border border-info rounded loginform">
          <h3 className="text-center p-2">Log in</h3>
          <EmailBlock
            errorMessage={this.state.emailError}
            email={this.state.email}
            onChange={this.onEmailChange.bind(this)}
          />
          <PasswordBlock
            errorMessage={this.state.passwordError}
            password={this.state.password}
            onChange={this.onPasswordChange.bind(this)}
          />
          <p className="text-danger">{this.state.message}</p>
          <button
            className="btn btn-info btn-block"
            onClick={this.submit.bind(this)}
          >
            Submit
          </button>
        </form>
      </div>
    );
  }
}

const FormWithRouter = withRouter(Form);
export default Form;
