// AdminDashboard.tsx
import React from "react";
import AdminTestTable from "./AdminTestTable/AdminTestTable.tsx";
const AdminTest: React.FC = () => {
    return (
        <div>
            <h1 className="text-3xl font-bold mb-4">Test Viewer</h1>
            <AdminTestTable/>
        </div>
    );
};
export default AdminTest;
