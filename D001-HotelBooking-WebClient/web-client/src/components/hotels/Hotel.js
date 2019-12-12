import React from "react";
import "./style.css";
import { Link } from "react-router-dom";
import config from "../../config";

let defaultHotelImage = require("../../media/defaultPhotoHotel.jpg");

const Hotel = props => {
  const hotelPagePath = `/hotel/${props.id}`;
  let imageSrc = props.images[0] ? props.images[0] : defaultHotelImage;

  return (
    <div className="hotel d-flex flex-column align-items-center justify-content-between m-3">
      <Link to={hotelPagePath} className="hotel-link">
        <img
          src={config.apiDomain + imageSrc}
          className="rounded hotel-image"
          alt="Hotel"
          width="100%"
        />

        {props.name}
      </Link>
    </div>
  );
};

export default Hotel;
