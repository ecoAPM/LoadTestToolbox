var ChartJS = require('chartjs-node');

module.exports = function Chart(callback, config) {
    var chartEngine = new ChartJS(config.width, config.height);

    return chartEngine.drawChart(config).then(() => {
        return chartEngine.getImageDataUrl('image/png').then(data => {
            callback(null, data);
        });
    });
};