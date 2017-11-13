google.charts.setOnLoadCallback(drawStatisticsCharts);

// Storing the charts globally, so we can call clearChart before redrawing the charts, to fix the memory leak issue in Google Charts.
var statisticsCharts = {
	pie: null
};


function drawStatisticsCharts() {
	loadStatisticsChart();

	setInterval(function () {
		loadStatisticsChart();
	}, 60000);
}

function loadStatisticsChart() {
	var buildConfigurationId = $('#statistics_chart').data('build-configuration-id');

	$.ajax({
		url: '/Home/GetLastStatistics',
		data: {
			buildConfigurationId: buildConfigurationId
		},
		cache: false,
		success: function (latestData) {
			drawStatisticsChart(latestData);
		},
		error: function (xhr, textStatus, errorThrown) {
			displayAjaxError(xhr, textStatus, errorThrown);
		}
	});
}

function drawStatisticsChart(latestData) {
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
			0: { color: '#FB1B45' },
			1: { color: 'green' }
		},
		legend: {
			textStyle: {
				color: '#999',
				fontSize: 20
			},
			position: 'bottom'
		}
	};

	if (charts.pie) {
		charts.pie.clearChart();
	}

	charts.pie = new google.visualization.PieChart(document.getElementById('statistics_chart'));
	charts.pie.draw(dataTable, options);
}