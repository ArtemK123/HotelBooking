import React from "react";
import { NavLink } from "react-router-dom";
import { Link } from "react-router-dom";
import "./style.css";


const SideDrawer = (props) => {
  const activeStyle = { color: "#004c5f" };
  let authorizationDependedLinks;

  if (localStorage.getItem("jwt") !== null) {
    authorizationDependedLinks = (
      <li>
        <Link to="/logout">Log out</Link>
      </li>
    );
  } else {
    authorizationDependedLinks = (
      [<li>
          <Link to="/login">Log in</Link>
        </li>,
        <li>
          <Link to="/registration">Register</Link>
        </li>
      ]
    );
  }
  return (
    <nav className="side-drawer">
      <ul>
        <li>
          <NavLink to="/" activeStyle={activeStyle} exact>
            Hotels
          </NavLink>
        </li>
        <li>
          <NavLink to="/map" activeStyle={activeStyle}>
            Map
          </NavLink>
        </li>
        <li>
          <NavLink to="/about" activeStyle={activeStyle}>
            About
          </NavLink>
        </li>
        <li>
          <NavLink to="/contacts" activeStyle={activeStyle}>
            Contacts
          </NavLink>
        </li>
        {authorizationDependedLinks}
      </ul>
    </nav>
  );
};

export default SideDrawer;
