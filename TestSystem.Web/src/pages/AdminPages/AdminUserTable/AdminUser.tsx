// AdminDashboard.tsx
import React from "react";
import AdminUserTable from "./AdminUserTable/AdminUserTable.tsx";
const AdminUser: React.FC = () => {
    return (
        <div>
            <h1 className="text-5xl py-8 font-bold mb-4">Account Management</h1>
            <AdminUserTable/>
        </div>
    );
};
export default AdminUser;
