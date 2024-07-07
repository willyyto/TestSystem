import type {RouteObject} from 'react-router';
import {DefaultLayout, Layout} from 'layouts';
import AppRoutes from 'navigation/AppRoutes';
import Home from 'pages/Home';
import BlogPage from 'pages/Blog';
import DocsPage from 'pages/Docs';
import PricingPage from 'pages/Pricing';
import AboutPage from 'pages/About';
import Login from "../pages/Login";
import Dashboard from "../pages/Dashboard";
import QuizAttempt from "../pages/QuizAttempt";

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
            <Layout> <Dashboard/></Layout>
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
    },
    {
        path: AppRoutes.quiz,
        element: (
            <Layout> <QuizAttempt/></Layout>
        ),
    }
];

export default routes;
