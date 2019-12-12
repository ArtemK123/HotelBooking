import React from "react";
import "../style.css";
import Rating from "./Rating";
import config from "../../../config";

let defaultHotelImage = require("../../../media/defaultPhotoHotel.jpg");

const HotelDescription = ({ hotelInfo }) => {
  if (hotelInfo === undefined || hotelInfo.images === undefined) return null;
  console.log(hotelInfo);
  let imageSrc = hotelInfo.images[0] ? hotelInfo.images[0] : defaultHotelImage;

  return (
    <div>
      <img src={config.apiDomain + imageSrc} className="img-rounded w-100" alt="Hotel" />
      <div className="d-flex justify-content-between w-100">
        <h4 className="col-sm-6">{hotelInfo.name}</h4>
        <div className="row col-sm-6 justify-content-end mr-2">
          <div className="col-sm-8">{<Rating rating={hotelInfo.rating} />}</div>
        </div>
      </div>
      <p>{hotelInfo.city}</p>
      <p>{hotelInfo.address}</p>
      <p>{hotelInfo.description}</p>
    </div>
  );
};

export default HotelDescription;
