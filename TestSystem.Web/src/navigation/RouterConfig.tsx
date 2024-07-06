import type {RouteObject} from 'react-router';
import AppRoutes from 'navigation/AppRoutes';
import PrivateRoute from 'navigation/PrivateRoute';
import Dashboard from 'pages/Dashboard';

const routes: RouteObject[] = [
    {
        path: AppRoutes.root,
        element: (
            <Dashboard/>
        ),
    },
    {
        path: AppRoutes.dashboard,
        element: (
            <PrivateRoute element={<Dashboard/>}/>
        ),
    },
];

export default routes;
