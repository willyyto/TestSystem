// Dashboard.tsx
import React from 'react';


import UserTestTable from "./UserTestTable/UserTestTable.tsx";
import UserResultTestTable from "./UserResultTestTable/UserResultTestTable.tsx";


const Dashboard = () => {
    return (
        <div className="p-4">
            <div className="mb-2 flex justify-between items-center">
                <h1 className="text-5xl py-8 font-bold mb-4">User Tests</h1>
            </div>
            <UserTestTable/>
            <div className="mt-16">
                <div className="mb-2 flex justify-between items-center">
                    <h1 className="text-5xl py-8 font-bold mb-4">User Results</h1>
                </div>
                <UserResultTestTable/>
            </div>
        </div>
    );
};

export default Dashboard;
