import React from "react";
import config from "../../config";

const isPathValid = path => {
  return path !== null && path !== undefined && path !== "";
};

const ProfilePhoto = props => {
  const photoPath = isPathValid(localStorage.getItem("photoPath"))
    ? "\\" + localStorage.getItem("photoPath")
    : "profile/profile.png";
  return (
    <div className="container profile-photo">
      <picture>
        <source srcSet={config.apiDomain + photoPath} />
        <img className="profile-image rounded" src={photoPath} alt="profile" />
      </picture>
      <div className="mt-4 mb-4">
        <label className="btn d-block mx-auto btn-info edit-photo-button">
          Edit{" "}
          <input
            type="file"
            className="w-100"
            hidden
            onChange={event => props.callbackWithImage(event.target.files[0])}
          />
        </label>
      </div>
    </div>
  );
};

export default ProfilePhoto;
