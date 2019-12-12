import React from "react";
import "./style.css";
import { Redirect } from "react-router-dom";
import config from "../../config";

function FormHeader(props) {
  return (
    <div>
      <div className="mt-2">{props.step}</div>
      <h2 className="text-center text-capitalize">{props.header}</h2>
    </div>
  );
}

function Input(props) {
  switch (props.type) {
    case "text":
    case "password":
      return (
        <div className="mt-1">
          <label htmlFor={props.id}>{props.value}</label>
          <input
            type={props.type}
            id={props.id}
            name={props.name}
            className={props.class}
            placeholder={props.ph}
            onChange={props.onChange}
          />
        </div>
      );
    case "radio":
      return (
        <div className="radio mt-2">
          <input
            type={props.type}
            name={props.name}
            id={props.id}
            onChange={props.onChange}
          />
          <label htmlFor={props.id}>{props.label}</label>
        </div>
      );
    case "submit":
      return (
        <div className="">
          <input
            type={props.type}
            className={props.class}
            value={props.value}
          />
        </div>
      );
    default:
      break;
  }
}

class RegistrationForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      email: "",
      name: "",
      surname: "",
      password: "",
      passwordAgain: "",
      role: "",
      info: ""
    };
  }

  isEmptyField(field) {
    if (field === "" || field === null || field === undefined) {
      return true;
    }
    return false;
  }

  validate() {
    // validation of current state - if all right, successful result will be returned
    let result = {
      message: "",
      successful: false
    };
    if (
      this.isEmptyField(this.state.email) ||
      this.isEmptyField(this.state.name) ||
      this.isEmptyField(this.state.surname) ||
      this.isEmptyField(this.state.password) ||
      this.isEmptyField(this.state.passwordAgain)
    ) {
      result.message = "Empty fields are not allowed";
    } else if (this.state.password !== this.state.passwordAgain) {
      result.message = "Passwords should match";
    } else {
      result.message = "Validation complited";
      result.successful = true;
    }
    return result;
  }

  // fetching to backend
  submit(event) {
    event.preventDefault();
    let validation = this.validate();
    if (!validation.successful) {
      this.setState({ info: validation.message });
      return;
    }
    let dto = {
      email: this.state.email,
      username: this.state.name,
      surname: this.state.surname,
      role: this.state.role,
      password: this.state.password
    };
    console.log(dto);
    fetch(config.apiDomain + "/api/account/register", {
      method: "post",
      headers: {
        "Content-type": "application/json"
      },
      body: JSON.stringify(dto)
    })
      .then(res => res.json())
      .then(res => {
        console.log(res);
        if (res.isSuccessful) {
          this.props.callback(true, res.token);
        } else {
          this.setState({ info: res.message });
        }
      });
  }

  render() {
    return (
      <div>
        <Input
          type={"text"}
          id={"emailInput"}
          value={"Email address"}
          class={"form-control"}
          name={"email"}
          ph={"example@address.com"}
          onChange={event => {
            this.setState({ email: event.target.value });
          }}
        />

        <Input
          type={"text"}
          id={"nameInput"}
          value={"Name"}
          class={"form-control"}
          name={"Name"}
          ph={"e.g. Ivan"}
          onChange={event => {
            this.setState({ name: event.target.value });
          }}
        />

        <Input
          type={"text"}
          id={"surnameInput"}
          value={"Surname"}
          class={"form-control"}
          name={"surname"}
          ph={"e.g. Ivanov"}
          onChange={event => {
            this.setState({ surname: event.target.value });
          }}
        />

        <Input
          type={"password"}
          id={"passwdInput"}
          value={"Password"}
          class={"form-control"}
          name={"passwd"}
          ph={"Enter your password"}
          onChange={event => {
            this.setState({ password: event.target.value });
          }}
        />

        <Input
          type={"password"}
          id={"confirmPasswdInput"}
          value={"Confirm password"}
          class={"form-control"}
          name={"confirmPasswd"}
          ph={"Confirm your password"}
          onChange={event => {
            this.setState({ passwordAgain: event.target.value });
          }}
        />

        <div className="mt-2">
          <span>Register as:</span>
          <Input
            type="radio"
            id={"asClient"}
            name={"role"}
            label={"Client"}
            onChange={event => {
              this.setState({ role: "Client" });
            }}
          />
          <Input
            type="radio"
            id={"asManager"}
            name={"role"}
            label={"Manager"}
            onChange={event => {
              this.setState({ role: "Manager" });
            }}
          />
        </div>
        <p className="text-danger">{this.state.info}</p>
        <button
          className="btn btn-info form-control"
          onClick={this.submit.bind(this)}
        >
          Submit
        </button>
      </div>
    );
  }
}

class ConfirmationForm extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
  }

  render() {
    return (
      <div>
        <p>Registration completed successfully</p>
        <button
          className="btn btn-info form-control"
          onClick={this.props.callback}
        >
          Go to home page
        </button>
      </div>
    );
  }
}

export class Form extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
    this.state = {
      type: props.type
    };
  }

  componentToRender() {
    console.log(this.state.type);
    switch (this.state.type) {
      case "registration":
        return (
          <RegistrationForm callback={this.registrationCallback.bind(this)} />
        );
      case "confirmation":
        return (
          <ConfirmationForm callback={this.confirmationCallback.bind(this)} />
        );
      case "homeRedirect":
        return <Redirect to="/" />;
      default:
        return (
          <RegistrationForm callback={this.registrationCallback.bind(this)} />
        );
    }
  }

  confirmationCallback() {
    this.setState({ type: "homeRedirect" });
  }

  registrationCallback(successful, token = null) {
    if (successful) {
      this.props.successCallback(token);
      this.setState({
        type: "confirmation"
      });
    }
  }

  render() {
    return (
      <div className="container col-5 col-xs-10 col-sm-9 col-md-7 col-lg-5 border rounded border-info registration-form">
        <FormHeader header={this.props.type} />
        {this.componentToRender()}
      </div>
    );
  }
}
