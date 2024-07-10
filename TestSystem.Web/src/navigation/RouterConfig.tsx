import type {RouteObject} from 'react-router';
import {DefaultLayout, Layout} from 'layouts';
import AppRoutes from 'navigation/AppRoutes';
import Home from 'pages/Home';
import BlogPage from 'pages/Blog';
import DocsPage from 'pages/Docs';
import PricingPage from 'pages/Pricing';
import AboutPage from 'pages/About';
import Login from "pages/Login";
import Dashboard from "pages/Dashboard";
import TestBox from "pages/TestBox";
import Result from "pages/Result";
import CreateTest from "pages/CreateTest";
import ProtectedRoute from "./ProtectedRoute.tsx";
import Logout from "pages/Logout";
import Page401 from "pages/AppStatus/Page401";
/*import Page404 from "pages/AppStatus/Page404";*/
import AdminDashboard from "pages/AdminDashboard";
import {SideLayout} from "layouts/SideLayout.tsx";
import AdminTest from "pages/AdminTest";

const routes: RouteObject[] = [
    {
        path: AppRoutes.root,
        element: (
            <Layout> <Home/></Layout>
        ),
    },
    {
        path: AppRoutes.dashboard,
        element: (
            <SideLayout>
                <ProtectedRoute roles={['user']}><Dashboard/></ProtectedRoute>
            </SideLayout>
        ),
    },
    {
        path: AppRoutes.admindashboard,
        element: (
            <SideLayout>
                <ProtectedRoute><AdminDashboard/></ProtectedRoute>
            </SideLayout>
        ),
    },
    {
        path: AppRoutes.admintest,
        element: (
            <SideLayout>
                <ProtectedRoute><AdminTest/></ProtectedRoute>
            </SideLayout>
        ),
    },
    {
        path: AppRoutes.blog,
        element: (
            <Layout>
                <BlogPage/>
            </Layout>
        ),
    },
    {
        path: AppRoutes.docs,
        element: (
            <Layout> 
                <DocsPage/>
            </Layout>
        ),
    },
    {
        path: AppRoutes.pricing,
        element: (
            <Layout> 
                <PricingPage/>
            </Layout>
        ),
    },
    {
        path: AppRoutes.about,
        element: (
            <Layout>
                <AboutPage/>
            </Layout>
        ),
    },
    {
        path: AppRoutes.login,
        element: (
            <DefaultLayout> 
                <Login/>
            </DefaultLayout>
        ),
    },
    {
        path: AppRoutes.logout,
        element: (
            <DefaultLayout>
                <Logout/>
            </DefaultLayout>
        ),
    },
    {
        path: AppRoutes.unauthorised,
        element: (
            <Layout>
                <Page401/>
            </Layout>
        ),
    },
    {
        path: AppRoutes.quiz,
        element: (
            <Layout> 
                <ProtectedRoute><TestBox/></ProtectedRoute>
            </Layout>
        ),
    },
    {
        path: AppRoutes.result,
        element: (
            <Layout> 
                <ProtectedRoute><Result/></ProtectedRoute>
            </Layout>
        ),
    },
    {
        path: AppRoutes.createtest,
        element: (
            <Layout> 
                <ProtectedRoute><CreateTest/></ProtectedRoute>
            </Layout>
        ),
    }
];

export default routes;
