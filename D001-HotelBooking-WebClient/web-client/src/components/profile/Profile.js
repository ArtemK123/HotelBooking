import React from "react";
import InputBlock from "./InputBlock";
import "./style.css";
import Cards from "./Cards";
import CardCreator from "./CardCreator";
import ProfilePhoto from "./ProfilePhoto";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import "react-tabs/style/react-tabs.css";
import Orders from "./Orders";
import { Redirect } from "react-router-dom";
import config from "../../config";

class Profile extends React.Component {
  state = {
    errors: {
      name: null,
      surname: null,
      email: null,
      telephone: null,
      passport: null,
      currentPassword: null,
      newPassword: null,
      newPasswordConfirmation: null
    },
    orders: [],
    clientId: "",
    name: "",
    surname: "",
    email: "",
    telephone: "",
    passport: "",
    oldPassword: "",
    newPassword: "",
    newPasswordConfirmation: "",
    photoPath: null,
    creditCards: [],
    info: ""
  };

  async loadProfileInfo() {
    if (localStorage.getItem("jwt")) {
      await fetch(
        `${config.apiDomain}/api/account/get/${localStorage.getItem("jwt")}`
      )
        .then(res => res.json())
        .then(res => {
          console.log(res);
          this.setState({ ...res });
          fetch(`${config.apiDomain}/api/order/of-client/${res.clientId}`)
            .then(result => result.json())
            .then(result => {
              this.setState({ orders: result });
            });
        });
    }
  }

  componentDidMount = async () => {
    await this.loadProfileInfo();
  };

  onCardCreation = card => {
    fetch(
      `${config.apiDomain}/api/account/card/add/${localStorage.getItem("jwt")}`,
      {
        method: "post",
        headers: { "Content-type": "application/json" },
        body: JSON.stringify(card)
      }
    )
      .then(res => res.json())
      .then(res => {
        if (res.isSuccessful) {
          this.setState({ info: "" });
          this.loadProfileInfo();
        } else {
          this.setState({ info: res.message });
        }
      });
  };
  onCardRemoving = number => {
    let card = this.state.creditCards.find(card => card.number !== number);
    if (card === null) return;
    fetch(
      `${config.apiDomain}/api/account/card/remove/${localStorage.getItem(
        "jwt"
      )}`,
      {
        method: "delete",
        headers: { "Content-type": "application/json" },
        body: JSON.stringify(card)
      }
    )
      .then(res => res.json())
      .then(res => {
        if (res.isSuccessful) {
          this.setState({ info: "" });
          this.loadProfileInfo();
        } else {
          this.setState({ info: res.message });
        }
      });
  };
  submitForm = () => {
    let errorsExist = false;
    Object.keys(this.state.errors).map(key => {
      if (this.state.errors[key] != null) {
        errorsExist = true;
      }
      return null;
    });
    if (errorsExist) {
      return;
    }
    let { errors, orders, ...user } = this.state;
    fetch(
      `${config.apiDomain}/api/account/update/${localStorage.getItem("jwt")}`,
      {
        method: "PUT",
        headers: { "Content-type": "application/json" },
        body: JSON.stringify({ ...user })
      }
    )
      .then(res => res.json())
      .then(res => {
        this.setState({
          info: res.message ? res.message : "Profile successfully updated"
        });
      });
  };
  onFieldChange = event => {
    const fieldName = event.target.name;
    const fieldValue = event.target.value;
    this.setState(prevState => ({
      [fieldName]: fieldValue,
      errors: { ...prevState.errors, [fieldName]: null }
    }));
  };
  fieldValidation = (field, value) => {
    switch (field) {
      case "name":
        if (value === "") {
          return;
        }
        if (value.length > 20) {
          this.setState(prevState => {
            let state = Object.assign({}, prevState);
            state.errors.name = "Max length of name is 20";
            return { state };
          });
        }
        break;
      case "surname":
        if (value === "") {
          return;
        }
        if (value.length > 30) {
          this.setState(prevState => {
            let state = Object.assign({}, prevState);
            state.errors.surname = "Max length of surname is 30";
            return { state };
          });
        }
        break;
      case "email":
        if (value === "") {
          this.setState(prevState => {
            let state = Object.assign({}, prevState);
            state.errors.email = "Email is required";
            return { state };
          });
          return;
        }
        if (
          !value.match("^([a-zA-Z0-9_.]+)@([a-zA-Z0-9_.]+)\\.([a-zA-Z]{2,5})$")
        ) {
          this.setState(prevState => {
            let state = Object.assign({}, prevState);
            state.errors.email = "Please, enter valid email";
            return { state };
          });
        }
        break;
      case "telephone":
        if (value === "") {
          return;
        }
        if (!value.match("^\\+380[0-9]{9}$")) {
          this.setState(prevState => {
            let state = Object.assign({}, prevState);
            state.errors.telephone = "Example: +380951234567";
            return { state };
          });
        }
        break;
      case "passport":
        if (value === "") {
          return;
        }
        if (!value.match("^[А-Я]{2}[0-9]{6}$")) {
          this.setState(prevState => {
            let state = Object.assign({}, prevState);
            state.errors.passport = "Example: ЯИ789123";
            return { state };
          });
        }
        break;
      case "oldPassword":
        if (value === "") {
          return;
        }
        if (value.length > 30 || value.length < 6) {
          this.setState(prevState => {
            let state = Object.assign({}, prevState);
            state.errors.newPassword =
              "Password should have a length from 6 to 30";
            return { state };
          });
        }
        break;
      case "newPasswordConfirmation":
        if (value !== this.state.newPassword) {
          this.setState(prevState => {
            let state = Object.assign({}, prevState);
            state.errors.newPasswordConfirmation = "Passwords don't match";
            return { state };
          });
        }
        break;
      default:
        break;
    }
  };
  onFieldBlur = event => {
    this.fieldValidation(event.target.name, event.target.value);
  };

  pushImageToServer = async image => {
    let formData = new FormData();
    formData.append("token", localStorage.getItem("jwt"));
    formData.append("image", image);
    fetch(config.apiDomain + "/api/images/account/update", {
      method: "post",
      body: formData
    })
      .then(res => res.json())
      .then(res => {
        console.log(res);
        if (res.isSuccessful) {
          let newPath = res.message;
          if (newPath) {
            localStorage.setItem("photoPath", newPath);
            this.setState({ photoPath: newPath });
            this.props.updateParentCallback();
          }
        } else {
          console.log(res.errorMessage);
        }
      });
  };

  render() {
    let toRender;
    if (localStorage.getItem("jwt")) {
      toRender = (
        <Tabs>
          <TabList>
            <Tab>Personal information</Tab>
            <Tab>Orders</Tab>
          </TabList>

          <TabPanel>
            <div className="container col-10 border border-info rounded mb-4 p-0 personal-info-form">
              <h2 className="text-center p-2">Personal information</h2>
              <div className="d-flex flex-row justify-content-between flex-wrap">
                <div className="personal-info">
                  <InputBlock
                    label="Name"
                    type="text"
                    placeholder="Enter your name"
                    value={this.state.name ? this.state.name : ""}
                    name="name"
                    onChange={this.onFieldChange}
                    errorMessage={this.state.errors.name}
                    onBlur={this.onFieldBlur}
                  />
                  <InputBlock
                    label="Surname"
                    type="text"
                    placeholder="Enter your surname"
                    value={this.state.surname ? this.state.surname : ""}
                    name="surname"
                    onChange={this.onFieldChange}
                    errorMessage={this.state.errors.surname}
                    onBlur={this.onFieldBlur}
                  />
                  <InputBlock
                    label="Email address"
                    type="email"
                    placeholder=""
                    value={this.state.email ? this.state.email : ""}
                    name="email"
                    readOnly
                    // onChange={this.onFieldChange}
                    // errorMessage={this.state.errors.email}
                    // onBlur={this.onFieldBlur}
                  />
                  <InputBlock
                    label="Mobile telephone"
                    type="tel"
                    pattern="^\+380[0-9]{9}$"
                    placeholder="ex: +380501234567"
                    value={this.state.telephone ? this.state.telephone : ""}
                    name="telephone"
                    onChange={this.onFieldChange}
                    errorMessage={this.state.errors.telephone}
                    onBlur={this.onFieldBlur}
                  />
                  <InputBlock
                    label="Passport"
                    type="text"
                    placeholder="ex: МИ123456"
                    pattern="[A-Z]{2}[0-9]{6}"
                    value={this.state.passport ? this.state.passport : ""}
                    name="passport"
                    onChange={this.onFieldChange}
                    errorMessage={this.state.errors.passport}
                    onBlur={this.onFieldBlur}
                  />
                  <InputBlock
                    label="Current password"
                    type="password"
                    name="oldPassword"
                    onChange={this.onFieldChange}
                    errorMessage={this.state.errors.oldPassword}
                    onBlur={this.onFieldBlur}
                  />
                  <InputBlock
                    label="New password"
                    type="password"
                    placeholder="From 6 to 30 symbols"
                    name="newPassword"
                    onChange={this.onFieldChange}
                    errorMessage={this.state.errors.newPassword}
                    onBlur={this.onFieldBlur}
                  />
                  <InputBlock
                    label="Confirm new password"
                    type="password"
                    name="newPasswordConfirmation"
                    onChange={this.onFieldChange}
                    errorMessage={this.state.errors.newPasswordConfirmation}
                    onBlur={this.onFieldBlur}
                  />
                </div>
                <div className="photo-and-cards">
                  <div className="h-100 d-flex flex-column justify-content-between">
                    <ProfilePhoto callbackWithImage={this.pushImageToServer} />
                    <Cards
                      cards={this.state.creditCards}
                      onRemoving={this.onCardRemoving}
                    />
                    <CardCreator
                      onCardCreation={this.onCardCreation}
                      cards={this.state.creditCards}
                    />
                  </div>
                </div>
              </div>
              {this.state.info ? (
                <p className="pl-3">{this.state.info}</p>
              ) : null}
              <div className="submit-button-block">
                <button
                  type="submit"
                  className="btn btn-info btn-block save-changes-button"
                  onClick={this.submitForm}
                >
                  Save changes
                </button>
              </div>
            </div>
          </TabPanel>
          <TabPanel>
            <Orders orders={this.state.orders} />
          </TabPanel>
        </Tabs>
      );
    } else {
      toRender = <Redirect to="/" />;
    }
    return toRender;
  }
}

export default Profile;
