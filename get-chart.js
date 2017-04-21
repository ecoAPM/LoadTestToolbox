var ChartJS = require('chartjs-node');

module.exports = function Chart(callback, config) {
    config.options.plugins.beforeDraw = function (chartInstance) {
        var ctx = chartInstance.chart.ctx;
        ctx.fillStyle = "white";
        ctx.fillRect(0, 0, chartInstance.chart.width, chartInstance.chart.height);
    };

    var chartEngine = new ChartJS(config.width, config.height);
    return chartEngine.drawChart(config).then(() => {
        return chartEngine.getImageDataUrl('image/png').then(data => {
            callback(null, data);
        });
    });
};