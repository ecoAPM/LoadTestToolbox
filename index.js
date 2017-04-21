var Chart = require('./get-chart');
var config = {
    width: 1280,
    height: 720,
    options: {
        type: 'line'
    }
};
var log = function(err, data) {
    console.log(data);
};
var chart = new Chart(log, config);