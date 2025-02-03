import type {RouteObject} from 'react-router';
import {DefaultLayout, Layout} from 'layouts';
import AppRoutes from 'navigation/AppRoutes';
import Home from 'pages/CommonPages/Home';
import BlogPage from 'pages/CommonPages/Blog';
import DocsPage from 'pages/CommonPages/Docs';
import PricingPage from 'pages/CommonPages/Pricing';
import AboutPage from 'pages/CommonPages/About';
import Login from "pages/CommonPages/Login";
import Dashboard from "pages/UserPages/Dashboard";
import TestBox from "pages/UserPages/TestBox";
import Result from "pages/UserPages/Result";
import CreateTest from "pages/AdminPages/AdminTest/CreateTest";
import ProtectedRoute from "./ProtectedRoute.tsx";
import Logout from "pages/CommonPages/Logout";
import Page401 from "pages/CommonPages/AppStatus/Page401";
/*import Page404 from "pages/AppStatus/Page404";*/
import AdminDashboard from "pages/AdminPages/AdminDashboard";
import {SideLayout} from "layouts/SideLayout.tsx";
import AdminTest from "pages/AdminPages/AdminTest";
import AdminCompany from "pages/AdminPages/AdminCompany";
import AdminUser from "../pages/AdminPages/AdminUserTable";

const routes: RouteObject[] = [
    {
        path: AppRoutes.root,
        element: (
            <Layout><Home/></Layout>
        ),
    },
    {
        path: AppRoutes.dashboard,
        element: (
            <Layout>
                <ProtectedRoute><Dashboard/></ProtectedRoute>
            </Layout>
        ),
    },
    {
        path: AppRoutes.admindashboard,
        element: (
            <SideLayout>
                <ProtectedRoute roles={['admin']}><AdminDashboard/></ProtectedRoute>
            </SideLayout>
        ),
    },
    {
        path: AppRoutes.admintest,
        element: (
            <SideLayout>
                <ProtectedRoute roles={['admin']}><AdminTest/></ProtectedRoute>
            </SideLayout>
        ),
    },
    {
        path: AppRoutes.admincompany,
        element: (
            <SideLayout>
                <ProtectedRoute roles={['admin']}><AdminCompany/></ProtectedRoute>
            </SideLayout>
        ),
    },
    {
        path: AppRoutes.adminuser,
        element: (
            <SideLayout>
                <ProtectedRoute roles={['admin']}><AdminUser/></ProtectedRoute>
            </SideLayout>
        ),
    },
    {
        path: AppRoutes.adminuserview,
        element: (
            <SideLayout>
                <ProtectedRoute roles={['admin']}><Dashboard/></ProtectedRoute>
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
        path: AppRoutes.admintestcreate,
        element: (
            <Layout> 
                <ProtectedRoute><CreateTest/></ProtectedRoute>
            </Layout>
        ),
    }
];

export default routes;
