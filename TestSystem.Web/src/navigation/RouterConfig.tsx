import type {RouteObject} from 'react-router';
import {DefaultLayout, Layout} from 'layouts';
import AppRoutes from 'navigation/AppRoutes';
import { lazy } from 'react'

import Home from 'pages/CommonPages/Home';
import BlogPage from 'pages/CommonPages/Blog';
import DocsPage from 'pages/CommonPages/Docs';
import PricingPage from 'pages/CommonPages/Pricing';
import AboutPage from 'pages/CommonPages/About';
import Login from "pages/CommonPages/Login";
import ProtectedRoute from "./ProtectedRoute.tsx";
import {SideLayout} from "layouts/SideLayout";

const Dashboard = lazy(() => import('pages/UserPages/Dashboard'))
const TestBox = lazy(() => import('pages/UserPages/TestBox'))
const Result = lazy(() => import('pages/UserPages/Result'))
const CreateTest = lazy(() => import('pages/AdminPages/AdminTest/CreateTest'))
const Logout = lazy(() => import('pages/CommonPages/Logout'))
const Page401 = lazy(() => import('pages/CommonPages/AppStatus/Page401'))
const AdminDashboard = lazy(() => import('pages/AdminPages/AdminDashboard'))
const AdminTest = lazy(() => import('pages/AdminPages/AdminTest'))
const AdminCompany = lazy(() => import('pages/AdminPages/AdminCompany'))
const AdminUser = lazy(() => import('pages/AdminPages/AdminUserTable'))

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
