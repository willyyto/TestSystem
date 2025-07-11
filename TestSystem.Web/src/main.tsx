import React, {Suspense} from "react";
import ReactDOM from "react-dom/client";

import {createBrowserRouter, RouterProvider} from 'react-router-dom';
import routes from 'navigation/RouterConfig';
import "@/styles/globals.css";
import { LoadingSpinner } from "components/common/LoadingSpinner";

const router = createBrowserRouter(routes);

ReactDOM.createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
        <Suspense fallback={<LoadingSpinner />}>
            <RouterProvider router={router}/>
        </Suspense>
    </React.StrictMode>,
);
