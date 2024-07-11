// AdminDashboard.tsx
import React from "react";
import AdminTestTable from "./AdminTestTable/AdminTestTable.tsx";

const AdminTest: React.FC = () => {
    return (
        <div>
            <h1 className="text-5xl py-8 font-bold mb-4">Test Table</h1>
            <AdminTestTable/>
        </div>
    );
};
export default AdminTest;
