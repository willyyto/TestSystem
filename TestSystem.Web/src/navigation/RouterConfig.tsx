import type {RouteObject} from 'react-router';
import {Layout} from 'layouts';
import AppRoutes from 'navigation/AppRoutes';
import Home from 'pages/Home';
import BlogPage from 'pages/Blog';
import DocsPage from 'pages/Docs';
import PricingPage from 'pages/Pricing';
import AboutPage from 'pages/About';
import Login from "../pages/Login";
import DefaultLayout from "../layouts/DefaultLayout.tsx";

const routes: RouteObject[] = [
    {
        path: AppRoutes.root,
        element: (
            <Layout> <Home/></Layout>
        ),
    },
    {
        path: AppRoutes.blog,
        element: (
            <Layout> <BlogPage/></Layout>
        ),
    },
    {
        path: AppRoutes.docs,
        element: (
            <Layout> <DocsPage/></Layout>
        ),
    },
    {
        path: AppRoutes.pricing,
        element: (
            <Layout> <PricingPage/></Layout>
        ),
    },
    {
        path: AppRoutes.about,
        element: (
            <Layout> <AboutPage/></Layout>
        ),
    },
    {
        path: AppRoutes.login,
        element: (
            <DefaultLayout> <Login/></DefaultLayout>
        ),
    }
];

export default routes;
