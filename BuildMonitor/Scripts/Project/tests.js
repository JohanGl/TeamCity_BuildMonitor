google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawCharts);

function drawCharts() {
	var historyData = [["Builds", "Ignored", "Failed", "Passed"], [0, 0, 0, 0], [1, 2, 3, 2], [2, 4, 6, 4], [3, 6, 9, 6], [4, 8, 12, 8], [5, 10, 15, 10], [6, 12, 18, 12], [7, 14, 21, 14], [8, 16, 24, 16], [9, 18, 27, 18], [10, 20, 30, 20], [11, 22, 33, 22], [12, 24, 36, 24], [13, 26, 39, 26], [14, 28, 42, 28], [15, 30, 45, 30], [16, 32, 48, 32], [17, 34, 51, 34], [18, 36, 54, 36], [19, 38, 57, 38], [20, 40, 60, 40], [21, 42, 63, 42], [22, 44, 66, 44], [23, 46, 69, 46], [24, 48, 72, 48], [25, 50, 75, 50], [26, 52, 78, 52], [27, 54, 81, 54], [28, 56, 84, 56], [29, 58, 87, 58], [30, 60, 90, 60], [31, 62, 93, 62], [32, 64, 96, 64], [33, 66, 99, 66], [34, 68, 102, 68], [35, 70, 105, 70], [36, 72, 108, 72], [37, 74, 111, 74], [38, 76, 114, 76], [39, 78, 117, 78], [40, 80, 120, 80], [41, 82, 123, 82], [42, 84, 126, 84], [43, 86, 129, 86], [44, 88, 132, 88], [45, 90, 135, 90], [46, 92, 138, 92], [47, 94, 141, 94], [48, 96, 144, 96], [49, 98, 147, 98], [50, 100, 150, 100], [51, 102, 153, 102], [52, 104, 156, 104], [53, 106, 159, 106], [54, 108, 162, 108], [55, 110, 165, 110], [56, 112, 168, 112], [57, 114, 171, 114], [58, 116, 174, 116], [59, 118, 177, 118], [60, 120, 180, 120], [61, 122, 183, 122], [62, 124, 186, 124], [63, 126, 189, 126], [64, 128, 192, 128], [65, 130, 195, 130], [66, 132, 198, 132], [67, 134, 201, 134], [68, 136, 204, 136], [69, 138, 207, 138], [70, 140, 210, 140], [71, 142, 213, 142], [72, 144, 216, 144], [73, 146, 219, 146], [74, 148, 222, 148], [75, 150, 225, 150], [76, 152, 228, 152], [77, 154, 231, 154], [78, 156, 234, 156], [79, 158, 237, 158], [80, 160, 240, 160], [81, 162, 243, 162], [82, 164, 246, 164], [83, 166, 249, 166], [84, 168, 252, 168], [85, 170, 255, 170], [86, 172, 258, 172], [87, 174, 261, 174], [88, 176, 264, 176], [89, 178, 267, 178], [90, 180, 270, 180], [91, 182, 273, 182], [92, 184, 276, 184], [93, 186, 279, 186], [94, 188, 282, 188], [95, 190, 285, 190], [96, 192, 288, 192], [97, 194, 291, 194], [98, 196, 294, 196], [99, 198, 297, 198]];
	
	drawHistoryChart(historyData);

	var latestData = [["Status", "Count"], ["Passed", 100], ["Failed", 3], ["Ignored", 61]];
	drawLatestChart(latestData);
}

function drawHistoryChart(historyData) {
	var dataTable = google.visualization.arrayToDataTable(historyData);

	var options = {
		isStacked: true,
		chartArea: { width: '90%', height: '90%' },
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
				fontSize: 30
			},
			position: 'bottom'
		}
	};

	var chart = new google.visualization.PieChart(document.getElementById('test_latest_chart'));
	chart.draw(dataTable, options);
}