window.dashboardChart = {

    render: function (canvasId, labels, values) {

        const ctx = document.getElementById(canvasId);

        if (!ctx)
            return;

        if (ctx.chart)
            ctx.chart.destroy();

        ctx.chart = new Chart(ctx, {

            type: 'bar',

            data: {

                labels: labels,

                datasets: [{

                    label: 'Movimientos',

                    data: values,

                    backgroundColor: '#1976D2',

                    borderRadius: 6,

                    borderWidth: 0
                }]
            },

            options: {

                responsive: true,

                maintainAspectRatio: false,

                plugins: {

                    legend: {
                        display: false
                    }
                },

                scales: {

                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
}