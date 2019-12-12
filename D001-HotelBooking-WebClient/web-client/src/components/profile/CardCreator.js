import React from "react";
import InputBlock from "./InputBlock";

class CardCreator extends React.Component {
  state = {
    errorMessage: null,
    number: null,
    dueDate: null,
    cv2: null
  };

  Submit = () => {
    let { message, ...newCard } = this.state;
    if (this.props.cards.some(card => card.number === this.state.number)) {
      this.setState({ errorMessage: "You've already added this card" });
      return;
    }
    if (this.fieldsAreValid()) {
      this.props.onCardCreation({ ...newCard });
    }
  };
  onPropertyChange = event => {
    this.setState({
      [event.target.name]: event.target.value,
      errorMessage: null
    });
  };
  fieldsAreValid = () => {
    if (
      this.state.number == null ||
      this.state.dueDate == null ||
      this.state.cv2 == null
    ) {
      this.setState({
        errorMessage: "Please, fill all fields"
      });
      return false;
    }
    if (!this.state.number.match("^[0-9]{16}$")) {
      this.setState({
        errorMessage: "Number of card should consist of 16 numbers"
      });
      return false;
    }
    let date = new Date(this.state.dueDate);
    let currentYear = new Date().getFullYear();
    let currentMonth = new Date().getMonth();
    if (
      date.getFullYear() < currentYear ||
      (date.getFullYear() === currentYear && date.getMonth() < currentMonth)
    ) {
      this.setState({
        errorMessage: "The card has already expired"
      });
      return false;
    }

    if (!this.state.cv2.match("^[0-9]{3}$")) {
      this.setState({
        errorMessage: "CV2 should consist of 3 numbers"
      });
      return false;
    }
    return true;
  };
  render() {
    return (
      <div className="adding-card-block">
        <InputBlock
          label="Number of card"
          type="text"
          pattern="^[0-9]{16}$"
          placeholder="ex: 1234567812345678"
          name="number"
          onChange={this.onPropertyChange}
          errorMessage={this.state.errorMessage}
        />
        {/* <p className="text-danger">{this.state.errorMessage}</p> */}
        <div className="col-7 d-inline-block m-0 p-0 pr-2">
          <InputBlock
            label="Date of expiration"
            type="month"
            onChange={this.onPropertyChange}
            name="dueDate"
          />
        </div>
        <div className="col-5 d-inline-block m-0 p-0 pl-2">
          <InputBlock
            label="CV2"
            type="password"
            pattern="^[0-9]{3}$"
            onChange={this.onPropertyChange}
            name="cv2"
          />
        </div>
        <button className="btn btn-info btn-block" onClick={this.Submit}>
          Add card
        </button>
      </div>
    );
  }
}

export default CardCreator;
