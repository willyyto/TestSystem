// AdminDashboard.tsx
import React from "react";
import AdminTestTable from "./AdminTestTable/AdminTestTable.tsx";
const AdminDashboard: React.FC = () => {
    return (
        <div>
            <h1  className="text-3xl font-bold mb-4">Admin Dashboard</h1>
            <AdminTestTable/>
        </div>
    );
};
export default AdminDashboard;
