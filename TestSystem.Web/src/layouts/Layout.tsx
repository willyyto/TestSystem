import {Footer} from "../components/Footer.tsx";
import {Header} from "../components/Header.tsx";
import DefaultLayout from "./DefaultLayout.tsx";

export default function Layout({children}: { children: React.ReactNode; }) {
    return ( <>
        <DefaultLayout>
            <Header/>
            <main className="container mx-auto max-w-7xl px-6 flex-grow pt-16">
                {children}
            </main>
            <Footer/>
        </DefaultLayout>
    </>);
}
