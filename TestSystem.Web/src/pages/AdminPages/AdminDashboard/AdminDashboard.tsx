// AdminDashboard.tsx
import React from "react";
import {BarChart, LineChart} from './Chart';
import {Card} from "@heroui/react";

const AdminDashboard: React.FC = () => {
    return (
        <>
            <h1 className="text-5xl py-8 font-bold mb-4">Admin Dashboard</h1>
            <div className=" min-h-screen">
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-6">
                    <Card
                          className="p-4 rounded-lg border-default-100 bg-primary-400">
                        <div>
                            <h4 className="text-lg font-semibold text-gray-50">Total Users</h4>
                            <p className="text-2xl text-gray-50">1,234</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg bg-warning">
                        <div>
                            <h4 className="text-lg font-semibold text-gray-50">Total Sales</h4>
                            <p className="text-2xl text-gray-50">$5,678</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg bg-success">
                        <div>
                            <h4 className="text-lg font-semibold text-gray-50">New Orders</h4>
                            <p className="text-2xl text-gray-50">123</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg bg-default-50">
                        <div>
                            <h4 className="text-lg font-semibold">Pending Reviews</h4>
                            <p className="text-2xl">45</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg bg-default-50">
                        <div>
                            <h4 className="text-lg font-semibold">New Customers</h4>
                            <p className="text-2xl">67</p>
                        </div>
                    </Card>
                    <Card className="p-4 rounded-lg bg-default-50">
                        <div>
                            <h4 className="text-lg font-semibold">Active Sessions</h4>
                            <p className="text-2xl">89</p>
                        </div>
                    </Card>
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <Card className="p-4 rounded-lg border-1 border-default-100">
                        <BarChart/>
                    </Card>
                    <Card className="p-4 rounded-lg border-1 border-default-100">
                        <LineChart/>
                    </Card>
                </div>
            </div>
        </>
    );
}

export default AdminDashboard;
