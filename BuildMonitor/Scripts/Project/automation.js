$(document).ready(function () {
	$('body').css('display', 'none');
	$('body').fadeIn(1000);

	google.charts.load('current', { 'packages': ['corechart'] });
	google.charts.setOnLoadCallback(drawCharts);

	// Storing the charts globally, so we can call clearChart before redrawing the charts, to fix the memory leak issue in Google Charts.
	var charts = {};

	var buildConfigurationIds = [];

	function drawCharts() {
		var elements = document.getElementsByClassName('pie-chart');

		// Load all charts for the first time.
		for (var i = 0; i < elements.length; i++) {
			var element = elements[i];
			var buildConfigurationId = $(element).data('build-configuration-id');
			buildConfigurationIds.push(buildConfigurationId);
			charts[buildConfigurationId] = {
				element: element
			};
			loadChart(buildConfigurationId);
		}
		
		// Refresh the charts in every X seconds.
		setInterval(function () {
			for (var i = 0; i < buildConfigurationIds.length; i++) {
				var buildConfigurationId = buildConfigurationIds[i];
				loadChart(buildConfigurationId);
			}
		}, 60000);
	}

	function loadChart(buildConfigurationId) {
		$.ajax({
			url: '/Automation/TestRunResults',
			data: {
				buildConfigurationId: buildConfigurationId
			},
			cache: false,
			success: function (response) {
				drawChart(response.TestResults, buildConfigurationId);
				$("#last-updated").text(response.UpdatedText).removeClass("update-failure");
			},
			error: function (xhr, textStatus, errorThrown) {
				displayAjaxError(xhr, textStatus, errorThrown);
			}
		});
	}


	function drawChart(testRunResults, buildConfigurationId) {
		var dataTable = google.visualization.arrayToDataTable(testRunResults);

		var options = {
			chartArea: { width: '100%', height: '100%' },
			backgroundColor: {
				fill: '#1c1e22'
			},
			pieSliceBorderColor: '#272b30',
			pieSliceText: 'value',
			pieSliceTextStyle: {
				fontSize: 30
			},
			slices: {
				0: { color: 'green' },
				1: { color: '#FB1B45' },
				2: { color: '#bbb' }
			},
			legend: {
				textStyle: {
					color: '#999',
					fontSize: 30
				},
				position: 'none'
			},
			pieHole: 0.4
		};

		if (charts[buildConfigurationId] && charts[buildConfigurationId].chart) {
			charts[buildConfigurationId].chart.clearChart();
		}

		var element = charts[buildConfigurationId].element;
		charts[buildConfigurationId].chart = new google.visualization.PieChart(element);
		charts[buildConfigurationId].chart.draw(dataTable, options);
	}
});