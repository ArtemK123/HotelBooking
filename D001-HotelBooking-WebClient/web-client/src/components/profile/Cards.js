import React from "react";

const Cards = props => {
  const DeleteCard = event => {
    props.onRemoving(event.target.id);
  };
  const cardNumbers = props.cards.map((card, i) => (
    <li key={i}>
      <div className="card-number">{card.number}</div>{" "}
      <i
        className="deletion-button fas fa-times"
        id={card.number}
        onClick={DeleteCard}
      />
    </li>
  ));

  return (
    <div className="credit-cards">
      <label>My credit cards</label>
      <ul>{cardNumbers}</ul>
    </div>
  );
};

export default Cards;
