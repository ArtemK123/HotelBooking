import React from "react";
import { Redirect } from "react-router-dom";
import config from "../../config";

function Form(props) {
  const firstNameLabelSmall =
    "Please give us a name of one of the people staying in this room";
  const emailLabelSmall = "We'll send your confirmation email to this address";
  const phoneLabelSmall = "We'll only contact you in an emergency";

  const client = props.client();

  const state = props.state();

  let creditCards = [];

  if (client) {
    for (var creditCard in client.creditCards) {
      creditCards.push({
        value: client.creditCards[creditCard].id,
        text: client.creditCards[creditCard].number
      });
    }
  }

  return (
    <div className="">
      <h2>Book a room</h2>
      <div className="border border-info rounded p-4">
        <form method="post">
          <h2>Details</h2>

          <Input
            labelBig={"First name"}
            labelSmall={firstNameLabelSmall}
            name={"firstName"}
            id={"firstNameId"}
            class={"form-control col-6"}
            ph={"e.g.Ivan"}
            onChange={event => {
              props.onFieldChange(event);
            }}
            value={state !== undefined ? state.firstName : ""}
          />

          <Input
            labelBig={"Second name"}
            name={"secondName"}
            id={"secondNameId"}
            class={"form-control col-6"}
            ph={"e.g.Ivanov"}
            onChange={event => {
              props.this.setState({ secondName: event.target.value });
            }}
            value={state !== undefined ? state.secondName : ""}
          />

          <Input
            labelBig={"Email"}
            name={"email"}
            id={"emailId"}
            labelSmall={emailLabelSmall}
            class={"form-control col-6"}
            ph={"expamle@address.com"}
            onChange={event => {
              props.this.setState({ email: event.target.value });
            }}
            value={state !== undefined ? state.email : ""}
          />

          <Input
            labelBig={"Phone number"}
            name={"phone"}
            id={"phoneId"}
            labelSmall={phoneLabelSmall}
            class={"form-control col-6"}
            ph={"e.g.+380501234567"}
            onChange={event => {
              props.this.setState({ telephone: event.target.value });
            }}
            value={
              state !== undefined
                ? state.telephone !== null
                  ? state.telephone
                  : ""
                : ""
            }
          />

          <h2 className="mt-2">Payment details</h2>
          <div>
            <Select
              options={creditCards}
              onChange={event => {
                props.this.setState({ creditCardId: event.target.value });
              }}
            />

            <div className="row pl-3">
              <DatePicker
                label={"Check In"}
                name={"checkIn"}
                id={"checkInId"}
                class={"form-control "}
                onChange={event => {
                  props.this.setState({ checkIn: event.target.value });
                }}
              />

              <DatePicker
                label={"Check Out"}
                name={"checkOut"}
                id={"checkOutId"}
                class={"form-control "}
                onChange={event => {
                  props.this.setState({ checkOut: event.target.value });
                }}
              />
            </div>
          </div>
          <p>{props.this.state.info}</p>

          <button
            className="btn btn-info form-control mt-4"
            onClick={props.this.submit.bind(props.this)}
          >
            Complete booking
          </button>
        </form>
      </div>
    </div>
  );
}

function Input(props) {
  let secondLabel;

  if (props.labelSmall) {
    secondLabel = <label htmlFor={props.id}>{props.labelSmall}</label>;
  }

  return (
    <div>
      <label htmlFor={props.id} className="font-weight-bold">
        {props.labelBig}
      </label>
      <br />
      {secondLabel}
      <input
        type={props.type}
        id={props.id}
        className={props.class}
        name={props.name}
        placeholder={props.ph}
        value={props.value}
        onChange={props.onChange}
      />
    </div>
  );
}

function Option(props) {
  return <option value={props.value}>{props.text}</option>;
}

function Select(props) {
  let options = [<Option value={null} text={"Choose credit card"} key={0} />];
  props.options.forEach(option => {
    options.push(
      <Option value={option.value} text={option.text} key={option.value} />
    );
  });

  return (
    <select
      className="mdb-select md-form"
      defaultValue
      onChange={props.onChange}
    >
      {options}
    </select>
  );
}

function DatePicker(props) {
  return (
    <div className="mt-4">
      <label htmlFor={props.id} className="font-weight-bold ml-1">
        {props.label}
      </label>
      <input
        type="date"
        className={props.class}
        name={props.name}
        onChange={props.onChange}
      />
    </div>
  );
}

class BookingForm extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
    console.log(this.props);
    this.locationCallback = props.locationCallback;
    this.state = {
      firstName: "",
      secondName: "",
      email: "",
      telephone: "",
      creditCardId: 0,
      checkIn: null,
      checkOut: null,
      info: ""
    };
  }

  isEmptyField(field) {
    if (field === "" || field === null || field === undefined || field === 0) {
      return true;
    }
    return false;
  }

  isValid(field, pattern) {
    return pattern.test(String(field).toLowerCase());
  }

  isValidDate(checkIn, checkOut) {
    let checkInDate = new Date(checkIn);
    let checkOutDate = new Date(checkOut);
    let isCheckInFuture = checkInDate - Date.now() > 0;
    let isCheckOutFuture = checkOutDate - Date.now() > 0;
    let diff = checkOutDate - checkInDate > 0;

    return isCheckInFuture && isCheckOutFuture && diff;
  }

  validate(dto) {
    // validation of current state - if all right, successful result will be
    // returned
    let result = {
      message: "",
      successful: false
    };

    let isEmpty = false;
    let isValid = true;
    let namePattern = /[a-zA-Z ]{2,30}$/;
    let emailPattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    let phonePattern = /^\+380[0-9]{9}$/;

    for (let prop in dto) {
      isEmpty = isEmpty || this.isEmptyField(dto[prop]);
    }

    isValid = isValid && this.isValid(dto.firstName, namePattern);
    isValid = isValid && this.isValid(dto.secondName, namePattern);
    isValid = isValid && this.isValid(dto.email, emailPattern);
    isValid = isValid && this.isValid(dto.telephone, phonePattern);
    isValid = isValid && this.isValidDate(dto.checkIn, dto.checkOut);

    if (isEmpty) {
      result.message = "Empty fields are not allowed";
    } else if (!isValid) {
      result.message = "Fields are not valid";
    } else {
      result.message = "Validation complited";
      result.successful = true;
    }
    return result;
  }

  componentDidMount = () => {
    if (localStorage.getItem("jwt")) {
      fetch(
        `${config.apiDomain}/api/account/get/${localStorage.getItem("jwt")}`
      )
        .then(res => res.json())
        .then(res => {
          console.log(res);
          this.setState({
            client: res,
            firstName: res.name,
            secondName: res.surname,
            email: res.email,
            telephone: res.telephone
          });
        });
    }
  };

  submit(event) {
    event.preventDefault();
    let dto = {
      name: this.state.firstName,
      surname: this.state.secondName,
      email: this.state.email,
      telephone: this.state.telephone,
      creditCardId: this.state.creditCardId,
      checkIn: this.state.checkIn,
      checkOut: this.state.checkOut,
      roomId: Object.values(this.props)[1].props.match.params.id,
      token: localStorage.getItem("jwt")
    };
    let validation = this.validate(dto);
    if (!validation.successful) {
      this.setState({ info: validation.message });
      return;
    }
    console.log(dto);
    fetch(`${config.apiDomain}/api/order/add`, {
      method: "post",
      headers: {
        "Content-type": "application/json"
      },
      body: JSON.stringify(dto)
    })
      .then(res => res.json())
      .then(res => {
        console.log(res);
        if (res && res.isSuccessful) {
          this.props.callback();
        } else {
          this.setState({ info: res.message });
        }
      });
  }

  clientCallback = () => {
    return this.state.client;
  };

  stateCallback = () => {
    return this.state;
  };

  onFieldChange = event => {
    const fieldName = event.target.name;
    const fieldValue = event.target.value;
    this.setState(prevState => ({
      [fieldName]: fieldValue,
      errors: { ...prevState.errors, [fieldName]: null }
    }));
  };

  render() {
    return localStorage.getItem("jwt") ? (
      <Form
        this={this}
        client={this.clientCallback}
        state={this.stateCallback}
        onFieldChange={this.onFieldChange}
      />
    ) : (
      <Redirect to="/" />
    );
  }
}

class OrderCompleted extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
  }

  render() {
    return (
      <div>
        <p>Order completed successfully</p>
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

export class Order extends React.Component {
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
      case "booking":
        return (
          <BookingForm
            callback={this.registrationCallback.bind(this)}
            props={this.props}
          />
        );
      case "success":
        return (
          <OrderCompleted callback={this.confirmationCallback.bind(this)} />
        );
      case "homeRedirect":
        return <Redirect to="/" />;
      default:
        return (
          <BookingForm
            callback={this.registrationCallback.bind(this)}
            props={this.props}
          />
        );
    }
  }

  confirmationCallback() {
    this.setState({ type: "homeRedirect" });
  }

  registrationCallback() {
    this.props.successCallback();
    this.setState({
      type: "success"
    });
  }

  render() {
    return (
      <div className="container col-5 col-xs-10 col-sm-9 col-md-7 col-lg-5">
        {this.componentToRender()}
      </div>
    );
  }
}

export default Order;
