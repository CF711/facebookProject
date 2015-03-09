getStockPrice = function (symbol, callback) {
    $.ajax({
        url: 'http://dev.markitondemand.com/Api/v2/Quote/jsonp?symbol=' + symbol,
        success: callback,
        dataType: 'jsonp'

    });
}