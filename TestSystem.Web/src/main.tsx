import React from "react";
import ReactDOM from "react-dom/client";
import {PrimeReactProvider} from "primereact/api";
import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import {AuthProvider} from 'services/AuthContext';
import routes from 'navigation/RouterConfig';
import "./index.css";
import "primeicons/primeicons.css";

const router = createBrowserRouter(routes);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
    <React.StrictMode>
        <PrimeReactProvider value={{unstyled: false}}>
            <AuthProvider>
                <RouterProvider router={router}/>
            </AuthProvider>
        </PrimeReactProvider>
    </React.StrictMode>
);