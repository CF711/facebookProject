StockComponent = React.createClass({

    componentDidMount: function() {
        this._updateStockPrices();
    },

    render: function () {
        return (
            <ul>
                {this.props.symbols.map(function(symbol){
                    var price = 'loading...';
                    if (this.state) {
                        price = '$' + this.state['price' + symbol];
                    }
                    return (<li>{symbol} - {price}</li>);
                }.bind(this))}
            </ul>
        );
    },

    _updateStockPrices: function() {
        var component = this;
        this.props.symbols.forEach(
            function(symbol) {
                getStockPrice(symbol, function(data) {
                    var newState = {};
                    newState['price' + symbol] = data.LastPrice;
                    component.setState(newState);
                });
            }
        );
        //window.setTimeout(this._updateStockPrices, 5000);
    }
});