google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawCharts);

function drawCharts() {
	loadHistoryChart();
	loadLatestChart();

	setInterval(function () {
		loadHistoryChart();
		loadLatestChart();
	}, 60000);
}

function loadHistoryChart() {
	$.ajax({
		url: '/Tests/History',
		cache: false,
		success: function (historyData) {
			drawHistoryChart(historyData);
		},
		error: function (xhr, textStatus, errorThrown) {
			displayAjaxError(xhr, textStatus, errorThrown);
		}
	});
}

function loadLatestChart() {
	$.ajax({
		url: '/Tests/Latest',
		cache: false,
		success: function (latestData) {
			drawLatestChart(latestData);
		},
		error: function (xhr, textStatus, errorThrown) {
			displayAjaxError(xhr, textStatus, errorThrown);
		}
	});
}

function drawHistoryChart(historyData) {
	var dataTable = google.visualization.arrayToDataTable(historyData);

	var options = {
		isStacked: true,
		chartArea: { width: '90%', height: '80%' },
		backgroundColor: {
			fill: '#1C1E22'
		},
		areaOpacity: 0.6,
		hAxis: {
			textStyle: {
				color: '#999'
			}
		},
		vAxis: {
			minValue: 0,
			textStyle: {
				color: '#999'
			},
			gridlines: {
				color: '#666'
			}
		},
		series: {
			0: { color: '#bbb' },
			1: { color: '#FB1B45' },
			2: { color: 'green' }
		},
		legend: {
			position: 'none'
		}
	};

	var chart = new google.visualization.AreaChart(document.getElementById('test_history_chart'));
	chart.draw(dataTable, options);
}

function drawLatestChart(latestData) {
	var dataTable = google.visualization.arrayToDataTable(latestData);

	var options = {
		chartArea: { width: '100%', height: '80%' },
		backgroundColor: {
			fill: '#1c1e22'
		},
		pieSliceBorderColor: '#272b30',
		pieSliceText: 'value',
		pieSliceTextStyle: {
			fontSize: 20
		},
		slices: {
			0: { color: 'green' },
			1: { color: '#FB1B45' },
			2: { color: '#bbb' }
		},
		legend: {
			textStyle: {
				color: '#999',
				fontSize: 20
			},
			position: 'bottom'
		}
	};

	var chart = new google.visualization.PieChart(document.getElementById('test_latest_chart'));
	chart.draw(dataTable, options);
}