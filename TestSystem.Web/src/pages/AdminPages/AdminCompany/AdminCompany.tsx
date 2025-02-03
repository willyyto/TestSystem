// AdminDashboard.tsx
import React from "react";
import AdminCompanyTable from "./AdminCompanyTable/AdminCompanyTable.tsx";

const AdminCompany: React.FC = () => {
    return (
        <div>
            <h1 className="text-5xl py-8 font-bold mb-4">Company Table</h1>
            <AdminCompanyTable/>
        </div>
    );
};
export default AdminCompany;
