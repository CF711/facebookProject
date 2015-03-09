$.get(
    'http://dev.markitondemand.com/Api/v2/Quote/jsonp',
    {symbol: 'GOOG'},
    function(data) {
        var lastPrice = Number(data.LastPrice);
        var change = Number(data.Change);
        var changeSign;
        if (change > 0) {
            changeSign = 1;
        } else if (change < 0) {
            changeSign = -1;
        } else {
            changeSign = 0;
        }
        console.log(data);
    }
);