// AdminDashboard.tsx
import React from "react";
import {Card, Navbar} from "@nextui-org/react";
const AdminDashboard: React.FC = () => {
    return (
        <>
            <div className="flex-1 flex flex-col">
                {/* Navbar */}
                <Navbar className="shadow p-4">
                    <h1 className="text-3xl font-bold">Admin Dashboard</h1>
                </Navbar>

                {/* Content Area */}
                <div className="p-6 flex-1 overflow-y-auto">
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                        {/* Sample Cards */}
                        <Card>
                            <h3 className="font-semibold mb-2">
                                Total Users
                            </h3>
                            <p className="text-lg">1,234</p>
                        </Card>
                        <Card>
                            <h3 className="font-semibold mb-2">
                                New Orders
                            </h3>
                            <p className="text-lg">567</p>
                        </Card>
                        <Card>
                            <h3 className="font-semibold mb-2">
                                Revenue
                            </h3>
                            <p className="text-lg">$12,345</p>
                        </Card>
                    </div>
                </div>
            </div>
        </>
    );
}

export default AdminDashboard;
