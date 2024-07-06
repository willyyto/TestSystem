import type {RouteObject} from 'react-router';
import {Layout} from 'pages';
import AppRoutes from 'navigation/AppRoutes';
import Dashboard from 'pages/Dashboard';

const routes: RouteObject[] = [
    {
        path: AppRoutes.root,
        element: (
            <Layout> <Dashboard/></Layout>
        ),
    }
];

export default routes;
