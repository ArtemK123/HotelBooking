import React from "react";
import { Link } from "react-router-dom";
import config from "../../config";

const isPathValid = path => {
  return path !== null && path !== undefined && path !== "";
};

const ProfileBlock = props => {
  const photoPath = isPathValid(localStorage.getItem("photoPath"))
    ? "\\" + localStorage.getItem("photoPath")
    : "default/profile.png";

  return (
    <div className="container profile-icon text-center p-2 mr-2">
      <Link to="/profile" className="btn p-0">
        <img
          src={config.apiDomain + photoPath}
          className="profileIconImage"
          alt="Icon"
        />
      </Link>
      <button className="btn logout-button" onClick={props.logoutCallback}>
        Log out
      </button>
      <div className="user-welcome">
        Hello, {props.role} <Link to="/profile">{props.email}</Link>!
      </div>
    </div>
  );
};

export default ProfileBlock;
