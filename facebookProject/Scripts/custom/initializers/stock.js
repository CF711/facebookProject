$(function () {
    React.render(React.createElement(StockComponent, { symbols: ['GOOG', 'AAPL', 'PATK'] }), $('#stock')[0]);
});