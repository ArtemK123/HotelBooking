import React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import HotelsOnMap from "./map/HotelsOnMap";
import About from "./about/About";
import Contacts from "./contacts/Contacts";
import Header from "./shared/Header";
import "./App.css";
import Form from "./authorization/Form";
import { Form as RegistrationForm } from "./registration/Registration";
import SideDrawer from "./sideDrawer/SideDrawer";
import Backdrop from "./backdrop/Backdrop";

import Profile from "./profile/Profile";
import HomePage from "./hotels/HomePage";
import HotelPage from "./hotels/HotelPage/HotelPage";
import Order from "./booking/BookingForm";
import config from "../config";

class App extends React.Component {
  state = {
    sideDrawerOpen: false,
    user: { email: "", role: "manager", token: null }
  };
  drawerToggleClickHandler = () => {
    this.setState(prevState => {
      return {
        sideDrawerOpen: !prevState.sideDrawerOpen
      };
    });
  };
  backdropClickHandler = () => {
    this.setState({ sideDrawerOpen: false });
  };
  loginSuccess = token => {
    localStorage.setItem("jwt", token);
    fetch(`${config.apiDomain}/api/account/get/${localStorage.getItem("jwt")}`)
      .then(res => res.json())
      .then(res => {
        if (res && res.email && res.role) {
          localStorage.setItem("name", res.name);
          localStorage.setItem("role", res.role);
          localStorage.setItem("photoPath", "/" + res.photoPath);
          this.forceUpdate();
        }
      });
  };
  logoutCallback = () => {
    fetch(`${config.apiDomain}/api/account/logout`, {
      method: "post",
      headers: {
        "Content-type": "application/json"
      },
      body: JSON.stringify({ token: localStorage.getItem("jwt") })
    });
    localStorage.removeItem("jwt");
    localStorage.removeItem("name");
    localStorage.removeItem("role");
    localStorage.removeItem("photoPath");
    this.forceUpdate();
  };

  orderSuccess = () => {
    this.setState({ type: "success" });
  };

  componentDidMount() {
    localStorage.setItem("nameFilter", "");
    localStorage.setItem("cityFilter", "");
    localStorage.setItem("priceFromFilter", "");
    localStorage.setItem("priceToFilter", "");
    localStorage.setItem("checkInFilter", "");
    localStorage.setItem("checkOutFilter", "");
    localStorage.setItem("adultsFilter", "");
  }

  render() {
    const { sideDrawerOpen, user } = this.state;

    return (
      <div className="container p-0 border" style={{ minHeight: "100%" }}>
        {sideDrawerOpen && <Backdrop click={this.backdropClickHandler} />}
        <Header
          drawerClickHandler={this.drawerToggleClickHandler}
          userProfile={user}
          logoutCallback={this.logoutCallback}
        />
        {sideDrawerOpen && <SideDrawer />}
        <div className="container pt-5">
          <Route exact path="/" component={HomePage} />
          <Route path="/map" component={HotelsOnMap} />
          <Route path="/about" component={About} />
          <Route path="/contacts" component={Contacts} />

          <Route
            path="/login"
            render={props => (
              <Form {...props} successCallback={this.loginSuccess} />
            )}
          />
          <Route
            path="/profile"
            component={() => (
              <Profile
                {...this.state.user}
                updateParentCallback={() => this.forceUpdate()}
              />
            )}
          />
          <Route
            path="/registration"
            render={props => (
              <RegistrationForm
                type={"registration"}
                submitText={"Register"}
                successCallback={this.loginSuccess}
              />
            )}
          />
          <Route path="/hotel/:id" component={HotelPage} />
          <Route
            path="/room/booking/:id"
            render={props => (
              <Order props={props} successCallback={this.orderSuccess} />
            )}
          />
        </div>
      </div>
    );
  }
}

export default App;
