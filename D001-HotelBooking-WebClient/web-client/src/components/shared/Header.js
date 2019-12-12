import React from "react";
import { NavLink } from "react-router-dom";
import "./style.css";
import { Link } from "react-router-dom";
import DrawerToggleButton from "../sideDrawer/DrawerButton";
import ProfileIcon from "./ProfileIcon";

const Header = props => {
  const activeStyle = { color: "#004c5f" };
  const picture = "default/picture.jpg";

  let authenticationButtons;
  let profile;

  if (localStorage.getItem("jwt") !== null) {
    profile = (
      <ProfileIcon
        email={localStorage.getItem("name")}
        role={localStorage.getItem("role")}
        logoutCallback={props.logoutCallback}
      />
    );
  } else {
    authenticationButtons = (
      <div className="container" id="links">
        <Link to="/login" className="link">
          Log in
        </Link>
        <Link to="/registration" className="link">
          Register
        </Link>
      </div>
    );
  }
  return (
    <div>
      <div className="container p-0 header-image-buttons">
        {authenticationButtons}
        {profile}
        <picture>
          <source srcSet={picture} />
          <img
            className="header-image rounded"
            src={picture}
            alt="header hotel"
          />
        </picture>
      </div>
      <div className="toolbar">
        <nav className="bg-light border toolbar-navigation">
          <div className="toggle-button">
            <DrawerToggleButton click={props.drawerClickHandler} />
          </div>
          <div className="toolbar-items">
            <div className="toolbar-item">
              <NavLink to="/" activeStyle={activeStyle} exact>
                Hotels
              </NavLink>
            </div>
            <div className="toolbar-item">
              <NavLink to="/map" activeStyle={activeStyle}>
                Map
              </NavLink>
            </div>
            <div className="toolbar-item">
              <NavLink to="/about" activeStyle={activeStyle}>
                About
              </NavLink>
            </div>
            <div className="toolbar-item">
              <NavLink to="/contacts" activeStyle={activeStyle}>
                Contacts
              </NavLink>
            </div>
          </div>
        </nav>
      </div>
    </div>
  );
};

export default Header;
