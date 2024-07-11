import { Bar, Line } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, BarElement, PointElement, LineElement, Title, Tooltip, Legend);

export const BarChart: React.FC = () => {
    const data = {
        labels: ['January', 'February', 'March', 'April', 'May', 'June'],
        datasets: [
            {
                label: 'Sales',
                backgroundColor: 'rgb(0, 111, 238)',
                data: [65, 59, 80, 81, 56, 55],
            },
        ],
    };

    return (
        <div>
            <h3 className="text-xl font-semibold mb-4">Monthly Sales</h3>
            <Bar
                data={data}
                options={{
                    responsive: true,
                    plugins: {
                        title: {
                            display: false,
                        },
                        legend: {
                            display: true,
                            position: 'right',
                        },
                    },
                }}
            />
        </div>
    );
};

export const LineChart: React.FC = () => {
    const data = {
        labels: ['January', 'February', 'March', 'April', 'May', 'June'],
        datasets: [
            {
                label: 'New Users',
                fill: false,
                lineTension: 0.5,
                backgroundColor: 'rgb(0, 111, 238)',
                borderColor: 'rgb(0, 111, 238)',
                borderWidth: 2,
                data: [65, 59, 80, 81, 56, 55],
            },
        ],
    };

    return (
        <div>
            <h3 className="text-xl font-semibold mb-4">New Users Over Time</h3>
            <Line
                data={data}
                options={{
                    responsive: true,
                    plugins: {
                        title: {
                            display: false,
                        },
                        legend: {
                            display: true,
                            position: 'right',
                        },
                    },
                }}
            />
        </div>
    );
};
