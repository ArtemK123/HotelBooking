import React from "react";
import "../style.css";
import config from "../../../config";
let filledStar = "default/filledStar_1.jpg";
let emptyStar = "default/emptyStar_1.jpg";

const Star = props => {
  switch (props.type) {
    case "empty":
      return (
        <div className="w-20">
          <img
            src={config.apiDomain + emptyStar}
            className="img-rounded w-100"
            alt="empty-star"
          />
        </div>
      );
    case "filled":
      return (
        <div className="w-20">
          <img
            src={config.apiDomain + filledStar}
            className="img-rounded w-100"
            alt="filled-star"
          />
        </div>
      );
    default:
      return null;
  }
};

const Rating = ({ rating }) => {
  if (rating < 0 || rating > 5) return null;
  let stars = [];
  for (let i = 0; i < 5; i++) {
    let type = i < rating ? "filled" : "empty";
    stars[i] = <Star type={type} key={i} />;
  }
  return <div className="d-flex">{stars}</div>;
};

export default Rating;
