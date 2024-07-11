// AdminDashboard.tsx
import React from "react";
import {BarChart, LineChart} from './Chart';
import {Card} from "@nextui-org/react";

const AdminDashboard: React.FC = () => {
    return (
        <>
            <h1 className="text-5xl py-8 font-bold mb-4">Admin Dashboard</h1>
            <div className=" min-h-screen">
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-6">
                    <Card
                          className="p-4 rounded-lg">
                        <div>
                            <h4 className="text-lg font-semibold">Total Users</h4>
                            <p className="text-2xl">1,234</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg">
                        <div>
                            <h4 className="text-lg font-semibold">Total Sales</h4>
                            <p className="text-2xl">$5,678</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg">
                        <div>
                            <h4 className="text-lg font-semibold">New Orders</h4>
                            <p className="text-2xl">123</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg">
                        <div>
                            <h4 className="text-lg font-semibold">Pending Reviews</h4>
                            <p className="text-2xl">45</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg">
                        <div>
                            <h4 className="text-lg font-semibold">New Customers</h4>
                            <p className="text-2xl">67</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg">
                        <div>
                            <h4 className="text-lg font-semibold">Active Sessions</h4>
                            <p className="text-2xl">89</p>
                        </div>
                    </Card>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <Card className="p-4 rounded-lg">
                        <BarChart/>
                    </Card>
                    <Card className="p-4 rounded-lg">
                        <LineChart/>
                    </Card>
                </div>
            </div>
        </>
    );
}

export default AdminDashboard;
